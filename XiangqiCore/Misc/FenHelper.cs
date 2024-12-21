using System.Text.RegularExpressions;
using XiangqiCore.Boards;
using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Misc;

public static partial class FenHelper
{
    private static readonly Regex _fenRegex = FenRegex();

    public static bool Validate(string fenInput)
    {
        Match match = _fenRegex.Match(fenInput);

        // 1 overall group, 10 groups for 10 rows on the board, 1 group for turnorder, 1 group for round number, 1 group for turns without capturing
        const int expectedNumberOfGroups = 14;
        
        if (match.Groups.Count != expectedNumberOfGroups) return false;

        // Validate each FEN row
        return Enumerable.Range(1, 10).All(index => ValidateFenRow(match.Groups[index].Value));
    }

    public static bool ValidateFenRowColumns(string fenRow)
    {
        int totalNumberOfColumnsInRow = fenRow
                                        .Select(c => char.IsDigit(c) ? int.Parse(c.ToString()) : 1)
                                        .Sum();

        return totalNumberOfColumnsInRow == 9;
    }

    public static bool ValidateFenRowCharacters(string fenRow)
    {
        var flattenedFenRow = FlattenFenRow(fenRow);

        return flattenedFenRow
                .Where(x => !char.IsDigit(x))
                .All(piece => FenCharacterMap.ContainsKey(char.ToLower(piece)));
    }

    public static bool ValidateFenRow(string fenRow)
        => ValidateFenRowCharacters(fenRow) && ValidateFenRowColumns(fenRow);

    // Tried splitting the FEN string before, so try with regex this time
    // Regex Key
    // ([1-9rbaknpc]+)... : capturing the group of string composed of 1-9 and rbaknpc, this is for all the rows.
    // \s([bw]) : separated by a space, then capture the turn order of the FEN, indicating by b/w
    // .{5}:  ignore 5 characters, they are not useful in xiangqi fen
    // (\d+): capture the the number of moves without capturing here
    // \s(\d+) : separated by a white space, then capture the round number
    [GeneratedRegex(@"^([1-9rbaknpc]+)[\/]([1-9rbaknpc]+)[\/]([1-9rbaknpc]+)[\/]([1-9rbaknpc]+)[\/]([1-9rbaknpc]+)[\/]([1-9rbaknpc]+)[\/]([1-9rbaknpc]+)[\/]([1-9rbaknpc]+)[\/]([1-9rbaknpc]+)[\/]([1-9rbaknpc]+)\s([bw]).{5}(\d+)\s(\d+)", RegexOptions.IgnoreCase, "en-GB")]
    private static partial Regex FenRegex();

    // Getting all the consecutive ones in the row
    [GeneratedRegex(@"1+")]
    private static partial Regex GetAllConsecutiveOnesRegex();

    public static Piece[,] CreatePositionFromFen(string fenInput)
    {
        Match match = _fenRegex.Match(fenInput);

        Piece[,] result = new Piece[10, 9];

        for(int i = 10; i > 0; i--)
        {
            var fenRow = match.Groups[i].Value;
            var rowNumberInResult = 10 - i;
            var actualRowNumber = rowNumberInResult + 1;

            Piece[] parsedFen = ParseFenRow(fenRow, actualRowNumber);
            
            for(int j = 0; j < 9; j++)
            {
                result[rowNumberInResult, j] = parsedFen[j];
            }
        }

        return result;
    }

    public static Piece[] ParseFenRow(string fenRow, int rowNumber)
    {
        var flattenedFenRow = FlattenFenRow(fenRow);

        return flattenedFenRow
                    .Select((x, index) =>
                    {
                        if (char.IsDigit(x))
                            return PieceFactory.CreateEmptyPiece();

                        PieceType targetPieceType = FenCharacterMap.GetValueOrDefault(char.ToLower(x));
                        Side targetSide = GetSideFromFenChar(x);
                        Coordinate targetCoordinate = new (column: index + 1, row: rowNumber);

                        return PieceFactory.Create(targetPieceType, targetSide, targetCoordinate);
                    })
                    .ToArray();
    }

    public static Dictionary<char, PieceType> FenCharacterMap
        => new ()
        {
            { 'k', PieceType.King },
            { 'c', PieceType.Cannon },
            { 'r', PieceType.Rook },
            { 'b', PieceType.Bishop },
            { 'p', PieceType.Pawn },
            { 'n', PieceType.Knight },
            { 'a', PieceType.Advisor },
            { '1', PieceType.None }
        };

    public static string SplitCharDigitToStringOfOne(char num)
    {
        int parsedNum = int.Parse(num.ToString());

        return string.Join("", Enumerable.Repeat("1", parsedNum));
    }

    public static string FlattenFenRow(string fenRow)
        => string.Join("", fenRow.Select(x => char.IsDigit(x) ? SplitCharDigitToStringOfOne(x) : x.ToString()));

    public static Side GetSideFromFenChar(char piece) => char.IsDigit(piece) ? Side.None : char.IsUpper(piece) ? Side.Red : Side.Black;

    public static string GetFenFromPosition(Piece[,] position)
    {
        var groupedFenRows = position
                                .Cast<Piece>()
                                .Select((piece, index) => new { character = piece.GetFenCharacter, index })
                                .GroupBy(x => x.index / 9);

        var groupedFenRowsString = groupedFenRows
                                    .Select(group => string.Join("", group.Select(x => x.character)));

        var cleanedFenRowsString = groupedFenRowsString
                                    .Select(ConcatOnesInFenRow)
                                    .Reverse();

        return string.Join("/", cleanedFenRowsString);
    }

    private static string ConcatOnesInFenRow(string fenRow)
    {
        Regex getAllConsecutiveOnesRegex = GetAllConsecutiveOnesRegex();

        MatchEvaluator getLengthOfConsecutiveOnesEvaluator = new(GetLengthOfConsecutiveOnes);

        return getAllConsecutiveOnesRegex.Replace(fenRow, getLengthOfConsecutiveOnesEvaluator);
    }

    private static string GetLengthOfConsecutiveOnes(Match match) => match.Length.ToString();

    public static Side GetSideToMoveFromFen(string fenString)
    {
        Match match = _fenRegex.Match(fenString);
        const int sideToMoveIndex = 11;
        const Side defaultSideToMove = Side.Red;

        if (match.Success && IsNumberOfRegexMatchAboveTargetIndex(match, sideToMoveIndex))
            return match.Groups[sideToMoveIndex].Value is "w" or "r" ? Side.Red : Side.Black;

        return defaultSideToMove;
    }

    public static int GetRoundNumber(string fenString)
    {
        Match match = _fenRegex.Match(fenString);
        const int roundNumberIndex = 13;
        const int defaultRoundNumber = 0;

        if (match.Success && IsNumberOfRegexMatchAboveTargetIndex(match, roundNumberIndex))
            return int.Parse(match.Groups[roundNumberIndex].Value);

        return defaultRoundNumber;
    }

    public static int GetNumberOfMovesWithoutCapture(string fenString)
    {
        Match match = _fenRegex.Match(fenString);
        const int numberOfMovesWithoutCaptureIndex = 12;
        const int defaultNumberOfMovesWithoutCapture = 0;

        if (match.Success && IsNumberOfRegexMatchAboveTargetIndex(match, numberOfMovesWithoutCaptureIndex))
            return int.Parse(match.Groups[numberOfMovesWithoutCaptureIndex].Value);

        return defaultNumberOfMovesWithoutCapture;
    }

    private static bool IsNumberOfRegexMatchAboveTargetIndex(Match match, int targetIndex)
        => match.Groups.Count > targetIndex;

    public static string AppendGameInfoToFen(this string boardFen, Side sideToMove, int roundNumber, int numberOfMovesWithoutCapture)
    {
        string sideToMoveFen = sideToMove == Side.Red ? "w" : "b";

        return $"{boardFen} {sideToMoveFen} - - {numberOfMovesWithoutCapture} {roundNumber}";
    }

	public static PieceCounts ExtractPieceCounts(string fenString)
	{
		Piece[,] position = CreatePositionFromFen(fenString);

		Dictionary<PieceType, int> redPieces = [];
		Dictionary<PieceType, int> blackPieces = [];

		var groupedPieces = position.Cast<Piece>()
			.Where(x => x is not EmptyPiece)
			.GroupBy(x => x.PieceType)
			.Select(x => new { 
                PieceType = x.Key, 
                RedCount = x.Count(p => p.Side == Side.Red), 
                BlackCount = x.Count(p => p.Side == Side.Black) });

		foreach (var piece in groupedPieces)
		{
			redPieces[piece.PieceType] = piece.RedCount;
			blackPieces[piece.PieceType] = piece.BlackCount;
		}

		return new PieceCounts(redPieces, blackPieces);
	}
}