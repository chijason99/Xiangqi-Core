using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Move;
using XiangqiCore.Pieces.PieceTypes;

namespace xiangqi_core_test.XiangqiCore.MoveParserTest;
public static class MoveParserTestHelper
{
    // useful links:
    // 1. https://www.xqbase.com/protocol/cchess_move.htm
    // 2. https://www.wxf-xiangqi.org/images/wxf-rules/2018_World_Xiangqi_Rules_Chinese_2018.pdf

    public const string StartingFen1 = "1r2kabr1/4a4/2n1b1n1c/p1p1p3p/6p2/2PN1R3/P3P1c1P/2CCB1N2/9/R2AKAB2 b - - 3 11";
    public const string StartingFen2 = "2baka1r1/7r1/2n1b1n2/p1p1p1p1p/9/2P2NP2/P3P2cP/2N1C4/9/R1BAKABR1 b - - 5 9";
    public const string StartingFen3 = "2baka3/9/2c1b3n/C3p3p/2pn2c2/1R3r1N1/P3P3P/2N1BC3/4A4/4KAB2 w - - 1 17";
    
    public const string MultiPawnFen1 = "4k4/3P1P3/4P4/3P1P3/9/9/9/9/9/5K3 w - - 0 0";
    public const string MultiPawnFen2 = "5k3/9/2P6/9/2P6/9/2P6/9/9/4K4 w - - 0 0";
    public const string MultiPawnFen3 = "5k3/2P6/2P6/2P1P4/2P6/9/9/9/9/4K4 w - - 0 0";
    public const string MultiPawnFen1Black = "3k5/9/9/9/9/9/3p1p3/4p4/3p1p3/4K4 b - - 0 0";
    public const string MultiPawnFen2Black = "4k4/9/9/6p2/9/6p2/9/6p2/9/3K5 b - - 0 0";
    public const string MultiPawnFen3Black = "4k4/9/9/9/9/6p2/4p1p2/6p2/6p2/3K5 b - - 0 0";


    public async static Task AssertMoveWithNotationMethod(MoveNotationMethodTestData testData)
    {
        // Arrange
        XiangqiBuilder builder = new();

        XiangqiGame game = await builder
                            .WithCustomFen(testData.StartingFen)
                            .BuildAsync();

        Piece pieceToMove = game.Board.GetPieceAtPosition(testData.StartingPosition);

        pieceToMove.PieceType.Should().NotBe(PieceType.None);

        // Act
        bool moveResult = await game.Move(testData.MoveNotation, testData.NotationType);

        // Assert
        moveResult.Should().Be(testData.ExpectedResult);
        game.Board.GetPieceAtPosition(testData.StartingPosition).PieceType.Should().Be(PieceType.None);
        game.Board.GetPieceAtPosition(testData.Destination).PieceType.Should().Be(testData.MovingPieceType);
    }
}

public record MoveNotationMethodTestData(string StartingFen, string MoveNotation, MoveNotationType NotationType, Coordinate StartingPosition, Coordinate Destination, PieceType MovingPieceType, bool ExpectedResult);