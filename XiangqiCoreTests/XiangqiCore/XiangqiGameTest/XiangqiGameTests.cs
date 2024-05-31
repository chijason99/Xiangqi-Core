using XiangqiCore.Move.Move;
using XiangqiCore.Pieces.PieceTypes;

namespace xiangqi_core_test.XiangqiCore.XiangqiGameTest;
public static class XiangqiGameTests
{
    private const string _startingFen1 = "1r2kabr1/4a4/2n1b1n1c/p1p1p3p/6p2/2PN1R3/P3P1c1P/2CCB1N2/9/R2AKAB2 b - - 3 11";
    private const string _startingFen2 = "2baka1r1/7r1/2n1b1n2/p1p1p1p1p/9/2P2NP2/P3P2cP/2N1C4/9/R1BAKABR1 b - - 5 9";
    private const string _startingFen3 = "2baka3/9/2c1b3n/C3p3p/2pn2c2/1R3r1N1/P3P3P/2N1BC3/4A4/4KAB2 w - - 1 17";

    public static IEnumerable<object []> MoveMethodWithCoordinatesTestData
    {
        get
        {
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(2, 10), new(2, 4), ExpectedResult: true) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(7, 4), new(7, 1), ExpectedResult: true) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(7, 8), new(6, 6), ExpectedResult: true) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(3, 7), new(3, 6), ExpectedResult: true) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(5, 8), new(3, 10), ExpectedResult: true) };

            yield return new object[] { new MoveMethodTestData(_startingFen1, new(5, 10), new(4, 10), ExpectedResult: false) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(8, 10), new(7, 9), ExpectedResult: false) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(5, 9), new(6, 9), ExpectedResult: false) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(7, 4), new(7, 8), ExpectedResult: false) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(4, 5), new(3, 7), ExpectedResult: false) };
        }
    }

    public static IEnumerable<object[]> MoveMethodWithNotationTestData
    {
        get
        {
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "前車進4", MoveNotationType.Chinese, new(8, 9), new(8, 5), PieceType.Rook, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "後車平9", MoveNotationType.Chinese, new(8, 10), new(9, 10), PieceType.Rook, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "馬7退5", MoveNotationType.Chinese, new(7, 8), new(5, 9), PieceType.Knight, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "炮8平1", MoveNotationType.Chinese, new(8, 4), new(1, 4), PieceType.Cannon, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "卒3進1", MoveNotationType.Chinese, new(3, 7), new(3, 6), PieceType.Pawn, ExpectedResult: true) };


            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "車八平二", MoveNotationType.Chinese, new(2, 5), new(8, 5), PieceType.Rook, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "馬二進一", MoveNotationType.Chinese, new(8, 5), new(9, 7), PieceType.Knight, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "相五進七", MoveNotationType.Chinese, new(5, 3), new(3, 5), PieceType.Bishop, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "炮九退二", MoveNotationType.Chinese, new(1, 7), new(1, 5), PieceType.Cannon, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "帥五平六", MoveNotationType.Chinese, new(5, 1), new(4, 1), PieceType.King, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "士五進六", MoveNotationType.Chinese, new(5, 2), new(4, 3), PieceType.Advisor, ExpectedResult: true) };

        }
    }

    [Theory]
    [MemberData(nameof(MoveMethodWithCoordinatesTestData))]
    public static void MoveMethod_ShouldAlterTheBoardCorrectly(MoveMethodTestData testData)
    {
        // Arrange
        XiangqiBuilder builder = new ();

        XiangqiGame game = builder
                            .UseCustomFen(testData.StartingFen)
                            .Build();

        bool expectedResult = testData.ExpectedResult;
        
        // Act
        bool actualResult = game.Move(testData.StartingPosition, testData.Destination);

        // Assert
        actualResult.Should().Be(expectedResult);
    }

    [Theory]
    [MemberData(nameof(MoveMethodWithNotationTestData))]
    public static void MoveMethod_ShouldAlterTheBoardCorrectly_WhenUsingChineseMoveNotation(MoveNotationMethodTestData testData)
    {
        // Arrange
        XiangqiBuilder builder = new();

        XiangqiGame game = builder
                            .UseCustomFen(testData.StartingFen)
                            .Build();

        Piece pieceToMove = game.Board.GetPieceAtPosition(testData.StartingPosition);

        pieceToMove.PieceType.Should().NotBe(PieceType.None);

        // Act
        bool moveResult = game.Move(testData.MoveNotation, testData.NotationType);

        // Assert
        moveResult.Should().Be(testData.ExpectedResult);
        game.Board.GetPieceAtPosition(testData.StartingPosition).PieceType.Should().Be(PieceType.None);
        game.Board.GetPieceAtPosition(testData.Destination).PieceType.Should().Be(testData.MovingPieceType);
    }
}

public record MoveMethodTestData(string StartingFen, Coordinate StartingPosition, Coordinate Destination, bool ExpectedResult);
public record MoveNotationMethodTestData(string StartingFen, string MoveNotation, MoveNotationType NotationType, Coordinate StartingPosition, Coordinate Destination, PieceType MovingPieceType, bool ExpectedResult);