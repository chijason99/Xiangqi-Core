using XiangqiCore.Move.Move;
using XiangqiCore.Pieces.PieceTypes;

namespace xiangqi_core_test.XiangqiCore.XiangqiGameTest;
public static class XiangqiGameTests
{
    // useful links:
    // 1. https://www.xqbase.com/protocol/cchess_move.htm
    // 2. https://www.wxf-xiangqi.org/images/wxf-rules/2018_World_Xiangqi_Rules_Chinese_2018.pdf

    private const string _startingFen1 = "1r2kabr1/4a4/2n1b1n1c/p1p1p3p/6p2/2PN1R3/P3P1c1P/2CCB1N2/9/R2AKAB2 b - - 3 11";
    private const string _startingFen2 = "2baka1r1/7r1/2n1b1n2/p1p1p1p1p/9/2P2NP2/P3P2cP/2N1C4/9/R1BAKABR1 b - - 5 9";
    private const string _startingFen3 = "2baka3/9/2c1b3n/C3p3p/2pn2c2/1R3r1N1/P3P3P/2N1BC3/4A4/4KAB2 w - - 1 17";
    private const string _multiPawnFen1 = "4k4/3P1P3/4P4/3P1P3/9/9/9/9/9/5K3 w - - 0 0";
    private const string _multiPawnFen2 = "5k3/9/2P6/9/2P6/9/2P6/9/9/4K4 w - - 0 0";
    private const string _multiPawnFen3 = "5k3/2P6/2P6/2P1P4/2P6/9/9/9/9/4K4 w - - 0 0";
    private const string _multiPawnFen1Black = "3k5/9/9/9/9/9/3p1p3/4p4/3p1p3/4K4 b - - 0 0";
    private const string _multiPawnFen2Black = "4k4/9/9/6p2/9/6p2/9/6p2/9/3K5 b - - 0 0";
    private const string _multiPawnFen3Black = "4k4/9/9/9/9/6p2/4p1p2/6p2/6p2/3K5 b - - 0 0";

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

    public static IEnumerable<object[]> MoveMethodWithChineseNotationTestData
    {
        get
        {
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "前車進4", MoveNotationType.Chinese, new(8, 9), new(8, 5), PieceType.Rook, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "後車平9", MoveNotationType.Chinese, new(8, 10), new(9, 10), PieceType.Rook, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "馬7退5", MoveNotationType.Chinese, new(7, 8), new(5, 9), PieceType.Knight, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "炮8平1", MoveNotationType.Chinese, new(8, 4), new(1, 4), PieceType.Cannon, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "卒3進1", MoveNotationType.Chinese, new(3, 7), new(3, 6), PieceType.Pawn, ExpectedResult: true) };


            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "車八平四", MoveNotationType.Chinese, new(2, 5), new(6, 5), PieceType.Rook, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "馬二進一", MoveNotationType.Chinese, new(8, 5), new(9, 7), PieceType.Knight, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "相五進七", MoveNotationType.Chinese, new(5, 3), new(3, 5), PieceType.Bishop, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "炮九退二", MoveNotationType.Chinese, new(1, 7), new(1, 5), PieceType.Cannon, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "帥五平六", MoveNotationType.Chinese, new(5, 1), new(4, 1), PieceType.King, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "士五進六", MoveNotationType.Chinese, new(5, 2), new(4, 3), PieceType.Advisor, ExpectedResult: true) };
        }
    }

    public static IEnumerable<object[]> MoveMethodWithEnglishNotationTestData
    {
        get
        {
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "+r+4", MoveNotationType.English, new(8, 9), new(8, 5), PieceType.Rook, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "-r=9", MoveNotationType.English, new(8, 10), new(9, 10), PieceType.Rook, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "h7-5", MoveNotationType.English, new(7, 8), new(5, 9), PieceType.Knight, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "c8=1", MoveNotationType.English, new(8, 4), new(1, 4), PieceType.Cannon, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "p3+1", MoveNotationType.English, new(3, 7), new(3, 6), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "R8=4", MoveNotationType.English, new(2, 5), new(6, 5), PieceType.Rook, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "H2+1", MoveNotationType.English, new(8, 5), new(9, 7), PieceType.Knight, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "E5+7", MoveNotationType.English, new(5, 3), new(3, 5), PieceType.Bishop, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "C9-2", MoveNotationType.English, new(1, 7), new(1, 5), PieceType.Cannon, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "K5=6", MoveNotationType.English, new(5, 1), new(4, 1), PieceType.King, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "A5+6", MoveNotationType.English, new(5, 2), new(4, 3), PieceType.Advisor, ExpectedResult: true) };
        }
    }

    public static IEnumerable<object[]> MoveMethodWithUcciNotationTestData
    {
        get
        {
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "H8H4", MoveNotationType.UCCI, new(8, 9), new(8, 5), PieceType.Rook, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "H9I9", MoveNotationType.UCCI, new(8, 10), new(9, 10), PieceType.Rook, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "G7E8", MoveNotationType.UCCI, new(7, 8), new(5, 9), PieceType.Knight, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "H3A3", MoveNotationType.UCCI, new(8, 4), new(1, 4), PieceType.Cannon, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "C6C5", MoveNotationType.UCCI, new(3, 7), new(3, 6), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "B4F4", MoveNotationType.UCCI, new(2, 5), new(6, 5), PieceType.Rook, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "H4I6", MoveNotationType.UCCI, new(8, 5), new(9, 7), PieceType.Knight, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "E2C4", MoveNotationType.UCCI, new(5, 3), new(3, 5), PieceType.Bishop, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "A6A4", MoveNotationType.UCCI, new(1, 7), new(1, 5), PieceType.Cannon, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "E0D0", MoveNotationType.UCCI, new(5, 1), new(4, 1), PieceType.King, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "E1D2", MoveNotationType.UCCI, new(5, 2), new(4, 3), PieceType.Advisor, ExpectedResult: true) };
        }
    }

    public static IEnumerable<object []> MoveMethodWithRedChineseNotationForMultiPawnTestData
    {
        get
        {
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "前六平五", MoveNotationType.Chinese, new(4, 9), new(5, 9), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "後六平五", MoveNotationType.Chinese, new(4, 7), new(5, 7), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "前四平五", MoveNotationType.Chinese, new(6, 9), new(5, 9), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "後四平五", MoveNotationType.Chinese, new(6, 7), new(5, 7), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "前六進一", MoveNotationType.Chinese, new(4, 9), new(4, 10), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "後六進一", MoveNotationType.Chinese, new(4, 7), new(4, 8), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "前四進一", MoveNotationType.Chinese, new(6, 9), new(6, 10), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "後四進一", MoveNotationType.Chinese, new(6, 7), new(6, 8), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "前六平七", MoveNotationType.Chinese, new(4, 9), new(3, 9), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "後六平七", MoveNotationType.Chinese, new(4, 7), new(3, 7), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "前四平三", MoveNotationType.Chinese, new(6, 9), new(7, 9), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "後四平三", MoveNotationType.Chinese, new(6, 7), new(7, 7), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "兵五進一", MoveNotationType.Chinese, new(5, 8), new(5, 9), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "兵五平四", MoveNotationType.Chinese, new(5, 8), new(6, 8), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "兵五平六", MoveNotationType.Chinese, new(5, 8), new(4, 8), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen2, "前兵進一", MoveNotationType.Chinese, new(3, 8), new(3, 9), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen2, "中兵平六", MoveNotationType.Chinese, new(3, 6), new(4, 6), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen2, "後兵進一", MoveNotationType.Chinese, new(3, 4), new(3, 5), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen3, "前兵進一", MoveNotationType.Chinese, new(3, 9), new(3, 10), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen3, "二兵平六", MoveNotationType.Chinese, new(3, 8), new(4, 8), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen3, "三兵平八", MoveNotationType.Chinese, new(3, 7), new(2, 7), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen3, "後兵平六", MoveNotationType.Chinese, new(3, 6), new(4, 6), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen3, "兵五進一", MoveNotationType.Chinese, new(5, 7), new(5, 8), PieceType.Pawn, ExpectedResult: true) };
        }
    }

    public static IEnumerable<object[]> MoveMethodWithBlackChineseNotationForMultiPawnTestData
    {
        get
        {
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "前4平5", MoveNotationType.Chinese, new(4, 2), new(5, 2), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "後4平5", MoveNotationType.Chinese, new(4, 4), new(5, 4), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "前6平5", MoveNotationType.Chinese, new(6, 2), new(5, 2), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "後6平5", MoveNotationType.Chinese, new(6, 4), new(5, 4), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "前4進1", MoveNotationType.Chinese, new(4, 2), new(4, 1), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "後4進1", MoveNotationType.Chinese, new(4, 4), new(4, 3), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "前6進1", MoveNotationType.Chinese, new(6, 2), new(6, 1), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "後6進1", MoveNotationType.Chinese, new(6, 4), new(6, 3), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "前4平3", MoveNotationType.Chinese, new(4, 2), new(3, 2), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "後4平3", MoveNotationType.Chinese, new(4, 4), new(3, 4), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "前6平7", MoveNotationType.Chinese, new(6, 2), new(7, 2), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "後6平7", MoveNotationType.Chinese, new(6, 4), new(7, 4), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "卒5進1", MoveNotationType.Chinese, new(5, 3), new(5, 2), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "卒5平6", MoveNotationType.Chinese, new(5, 3), new(6, 3), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "卒5平4", MoveNotationType.Chinese, new(5, 3), new(4, 3), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen2Black, "前卒進1", MoveNotationType.Chinese, new(7, 3), new(7, 2), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen2Black, "中卒平6", MoveNotationType.Chinese, new(7, 5), new(6, 5), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen2Black, "後卒進1", MoveNotationType.Chinese, new(7, 7), new(7, 6), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen3Black, "前卒進1", MoveNotationType.Chinese, new(7, 2), new(7, 1), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen3Black, "二卒平6", MoveNotationType.Chinese, new(7, 3), new(6, 3), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen3Black, "三卒平8", MoveNotationType.Chinese, new(7, 4), new(8, 4), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen3Black, "後卒平6", MoveNotationType.Chinese, new(7, 5), new(6, 5), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen3Black, "卒5進1", MoveNotationType.Chinese, new(5, 4), new(5, 3), PieceType.Pawn, ExpectedResult: true) };
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
    [MemberData(nameof(MoveMethodWithChineseNotationTestData))]
    public static void MoveMethod_ShouldAlterTheBoardCorrectly_WhenUsingChineseMoveNotation(MoveNotationMethodTestData testData)
        => AssertMoveWithNotationMethod(testData);

    [Theory]
    [MemberData(nameof(MoveMethodWithEnglishNotationTestData))]
    public static void MoveMethod_ShouldAlterTheBoardCorrectly_WhenUsingEnglishMoveNotation(MoveNotationMethodTestData testData)
       => AssertMoveWithNotationMethod(testData);

    [Theory]
    [MemberData(nameof(MoveMethodWithUcciNotationTestData))]
    public static void MoveMethod_ShouldAlterTheBoardCorrectly_WhenUsingUcciMoveNotation(MoveNotationMethodTestData testData)
       => AssertMoveWithNotationMethod(testData);    
    
    
    [Theory]
    [MemberData(nameof(MoveMethodWithRedChineseNotationForMultiPawnTestData))]
    public static void MoveMethod_ShouldAlterTheBoardCorrectly_WithRedMultiPawnTestData(MoveNotationMethodTestData testData)
       => AssertMoveWithNotationMethod(testData);

    [Theory]
    [MemberData(nameof(MoveMethodWithBlackChineseNotationForMultiPawnTestData))]
    public static void MoveMethod_ShouldAlterTheBoardCorrectly_WituhBlackMultiPawnTestData(MoveNotationMethodTestData testData)
   => AssertMoveWithNotationMethod(testData);

    private static void AssertMoveWithNotationMethod(MoveNotationMethodTestData testData)
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