﻿using XiangqiCore.Extension;
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

    public Board(BoardConfig config) : this()
    {
        foreach (var keyValuePair in config.PiecesToAdd)
            SetPieceAtPosition(keyValuePair.Key, keyValuePair.Value);
    }

    public Piece[,] Position { get; private set; }

    public void SetPieceAtPosition(Coordinate targetCoordinates, Piece targetPiece)
    {
        int row = targetCoordinates.Row - 1;
        int column = targetCoordinates.Column - 1;

        Position[row, column] = targetPiece;
    }

    public Piece GetPieceAtPosition(Coordinate targetCoordinates) => Position.GetPieceAtPosition(targetCoordinates);

    public string GetFenFromPosition => FenHelper.GetFenFromPosition(Position);

    public static int[] GetAllRows => [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
    public static int[] GetAllColumns => [1, 2, 3, 4, 5, 6, 7, 8, 9];
    public static int[] GetPalaceRows(Side color)
        => color == Side.Red ? [1, 2, 3] : color == Side.Black ? [ 8, 9, 10] : throw new ArgumentException("Please provide the correct Side that you are looking for");
    public static int[] GetPalaceColumns => [4, 5, 6]; 

    public bool WillMoveExposeKingToDanger()
    {
        return false;
    }
}
