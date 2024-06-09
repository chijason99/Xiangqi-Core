﻿using System.CodeDom;
using System.Reflection;
using XiangqiCore.Attributes;
using XiangqiCore.Extension;
using XiangqiCore.Move;
using XiangqiCore.Pieces;

namespace XiangqiCore.Boards;
public class Board
{
    private const string _emptyBoardFen = "9/9/9/9/9/9/9/9/9/9 w - - 0 0";

    public Board()
    {
        Position = FenHelper.CreatePositionFromFen(_emptyBoardFen);
    }

    public Board(string fenString)
    {
        Position = FenHelper.CreatePositionFromFen(fenString);
    }

    /// <summary>
    /// Use the BoardConfig to override existing pieces on board
    /// </summary>
    /// <param name="config"></param>
    public Board(string fenString, BoardConfig config) : this(fenString)
    {
        foreach (var keyValuePair in config.PiecesToAdd)
            SetPieceAtPosition(keyValuePair.Key, keyValuePair.Value);
    }

    public Piece[,] Position { get; private set; }

    public void SetPieceAtPosition(Coordinate targetCoordinates, Piece targetPiece) => Position.SetPieceAtPosition(targetCoordinates, targetPiece);

    public Piece GetPieceAtPosition(Coordinate targetCoordinates) => Position.GetPieceAtPosition(targetCoordinates);

    public string GetFenFromPosition => FenHelper.GetFenFromPosition(Position);

    public static int[] GetAllRows() => [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

    public static int[] GetAllColumns() => [1, 2, 3, 4, 5, 6, 7, 8, 9];

    public static int[] GetPalaceRows(Side color)
        => color == Side.Red ? [1, 2, 3] : color == Side.Black ? [8, 9, 10] : throw new ArgumentException("Please provide the correct Side that you are looking for");

    public static int[] GetPalaceColumns() => [4, 5, 6];

    public void MakeMove(Coordinate startingPosition, Coordinate destination, Side sideToMove)
    {
        if (!Position.HasPieceAtPosition(startingPosition))
            throw new InvalidOperationException("There must be a piece on the starting position");

        Piece pieceToMove = GetPieceAtPosition(startingPosition);

        if (pieceToMove.Side != sideToMove)
            throw new InvalidOperationException($"The side to move now should be {EnumHelper<Side>.GetDisplayName(sideToMove)}");

        if (!pieceToMove.ValidateMove(Position, startingPosition, destination))
            throw new InvalidOperationException($"The proposed move violates the game logic"); ;

        Position.MakeMove(startingPosition, destination);
    }

    public void MakeMove(ParsedMoveObject moveObject, Side sideToMove)
    {
        Coordinate startingPosition = FindStartingPosition(moveObject, sideToMove);
        Coordinate destination = FindDestination(moveObject, startingPosition);

        MakeMove(startingPosition, destination, sideToMove);
    }

    private Coordinate FindStartingPosition(ParsedMoveObject moveObject, Side sideToMove)
    {
        if (moveObject.IsFromUcciNotation)
            return moveObject.StartingPosition.Value;

        Piece[] piecesToMove = GetPiecesToMove(moveObject.PieceType, sideToMove);
        Piece pieceToMove;

        // If the starting column is provided, then find the piece that has the same column as the starting column;
        // Otherwise, i.e. there are more than one piece of the same type and side in the column, pick the one following the order
        if(moveObject is MultiColumnPawnParsedMoveObject multiColumnPawnObject)
            pieceToMove = FindPieceToMoveForMultiColumnPawn(multiColumnPawnObject, piecesToMove, sideToMove);
        else
            pieceToMove = piecesToMove.SingleOrDefault(p => p.Coordinate.Column == moveObject.StartingColumn) ??
                          piecesToMove[moveObject.PieceOrderIndex];
        
        return pieceToMove.Coordinate;
    }

    private Piece[] GetPiecesToMove (Type pieceType, Side sideToMove)
    {
        MethodInfo method = typeof(PieceExtension).GetMethod(nameof(PieceExtension.GetPiecesOfType));
        MethodInfo genericMethod = method.MakeGenericMethod(pieceType);

        IEnumerable<Piece> allPiecesOfType = ((IEnumerable<Piece>)genericMethod.Invoke(obj: null, parameters: [Position, sideToMove]));

        if (!allPiecesOfType.Any()) throw new InvalidOperationException($"Cannot find any columns containing more than one {EnumHelper<Side>.GetDisplayName(sideToMove)} {pieceType.Name}");

        Piece[] piecesToMove = allPiecesOfType
                                    .OrderByRowWithSide(sideToMove)
                                    .ToArray();

        return piecesToMove;
    }

    private Piece FindPieceToMoveForMultiColumnPawn(MultiColumnPawnParsedMoveObject moveObject, Piece[] piecesToMove, Side sideToMove)
    {
        Piece[] pawnsOnColumn;

        if (moveObject.StartingColumn != ParsedMoveObject.UnknownStartingColumn)
        {
            pawnsOnColumn = piecesToMove
                                .Where(p => p.Coordinate.Column == moveObject.StartingColumn)
                                .OrderByRowWithSide(sideToMove)
                                .ToArray();
        }
        else
        {
            pawnsOnColumn = piecesToMove
                                .GroupBy(p => p.Coordinate.Column)
                                .Where(group => group.Count() >= moveObject.MinNumberOfPawnsOnColumn)
                                .SelectMany(group => group)
                                .OrderByRowWithSide(sideToMove)
                                .ToArray();
        }

        if (moveObject.PieceOrderIndex == MultiColumnPawnParsedMoveObject.LastPawnIndex)
            return pawnsOnColumn.Last();
        else
            return pawnsOnColumn[moveObject.PieceOrderIndex];
    }

    private Coordinate FindDestination(ParsedMoveObject moveObject, Coordinate startingCoordinate)
    {
        if (moveObject.IsFromUcciNotation)
            return moveObject.Destination.Value;

        Piece pieceToMove = Position.GetPieceAtPosition(startingCoordinate);

        MoveDirection moveDirection = moveObject.MoveDirection;

        if (pieceToMove.GetType().GetCustomAttribute<MoveInDiagonalsAttribute>() is not null && moveDirection == MoveDirection.Horizontal)
            throw new ArgumentException($"Piece type {moveObject.PieceType.Name} cannot move horizontally");

        Coordinate destination = pieceToMove.GetDestinationCoordinateFromNotation(moveObject.MoveDirection, moveObject.FourthCharacter);

        return destination;
    }
}
