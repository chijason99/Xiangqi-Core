using XiangqiCore.Move.Move;
using XiangqiCore.Pieces.PieceTypes;

namespace xiangqi_core_test.XiangqiCore.MoveParserTest;
public static class EnglishMoveNotationParserTests
{
    private const string _startingFen2 = MoveParserTestHelper.StartingFen2;
    private const string _startingFen3 = MoveParserTestHelper.StartingFen3;
    private const string _multiPawnFen1 = MoveParserTestHelper.MultiPawnFen1;
    private const string _multiPawnFen2 = MoveParserTestHelper.MultiPawnFen2;
    private const string _multiPawnFen3 = MoveParserTestHelper.MultiPawnFen3;
    private const string _multiPawnFen1Black = MoveParserTestHelper.MultiPawnFen1Black;
    private const string _multiPawnFen2Black = MoveParserTestHelper.MultiPawnFen2Black;
    private const string _multiPawnFen3Black = MoveParserTestHelper.MultiPawnFen3Black;

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

    public static IEnumerable<object[]> MoveMethodWithRedEnglishNotationForMultiPawnTestData
    {
        get
        {
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "+6=5", MoveNotationType.English, new(4, 9), new(5, 9), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "-6=5", MoveNotationType.English, new(4, 7), new(5, 7), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "+4=5", MoveNotationType.English, new(6, 9), new(5, 9), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "-4=5", MoveNotationType.English, new(6, 7), new(5, 7), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "+6+1", MoveNotationType.English, new(4, 9), new(4, 10), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "-6+1", MoveNotationType.English, new(4, 7), new(4, 8), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "+4+1", MoveNotationType.English, new(6, 9), new(6, 10), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "-4+1", MoveNotationType.English, new(6, 7), new(6, 8), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "+6=7", MoveNotationType.English, new(4, 9), new(3, 9), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "-6=7", MoveNotationType.English, new(4, 7), new(3, 7), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "+4=3", MoveNotationType.English, new(6, 9), new(7, 9), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "-4=3", MoveNotationType.English, new(6, 7), new(7, 7), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "p5+1", MoveNotationType.English, new(5, 8), new(5, 9), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "p5=4", MoveNotationType.English, new(5, 8), new(6, 8), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1, "p5=6", MoveNotationType.English, new(5, 8), new(4, 8), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen2, "17+1", MoveNotationType.English, new(3, 8), new(3, 9), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen2, "27=6", MoveNotationType.English, new(3, 6), new(4, 6), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen2, "37+1", MoveNotationType.English, new(3, 4), new(3, 5), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen3, "17+1", MoveNotationType.English, new(3, 9), new(3, 10), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen3, "27=6", MoveNotationType.English, new(3, 8), new(4, 8), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen3, "37=8", MoveNotationType.English, new(3, 7), new(2, 7), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen3, "47=6", MoveNotationType.English, new(3, 6), new(4, 6), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen3, "p5+1", MoveNotationType.English, new(5, 7), new(5, 8), PieceType.Pawn, ExpectedResult: true) };
        }
    }

    public static IEnumerable<object[]> MoveMethodWithBlackEnglishNotationForMultiPawnTestData
    {
        get
        {
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "+4=5", MoveNotationType.English, new(4, 2), new(5, 2), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "-4=5", MoveNotationType.English, new(4, 4), new(5, 4), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "+6=5", MoveNotationType.English, new(6, 2), new(5, 2), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "-6=5", MoveNotationType.English, new(6, 4), new(5, 4), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "+4+1", MoveNotationType.English, new(4, 2), new(4, 1), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "-4+1", MoveNotationType.English, new(4, 4), new(4, 3), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "+6+1", MoveNotationType.English, new(6, 2), new(6, 1), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "-6+1", MoveNotationType.English, new(6, 4), new(6, 3), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "+4=3", MoveNotationType.English, new(4, 2), new(3, 2), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "-4=3", MoveNotationType.English, new(4, 4), new(3, 4), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "+6=7", MoveNotationType.English, new(6, 2), new(7, 2), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "-6=7", MoveNotationType.English, new(6, 4), new(7, 4), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "p5+1", MoveNotationType.English, new(5, 3), new(5, 2), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "p5=6", MoveNotationType.English, new(5, 3), new(6, 3), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen1Black, "p5=4", MoveNotationType.English, new(5, 3), new(4, 3), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen2Black, "17+1", MoveNotationType.English, new(7, 3), new(7, 2), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen2Black, "27=6", MoveNotationType.English, new(7, 5), new(6, 5), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen2Black, "-7+1", MoveNotationType.English, new(7, 7), new(7, 6), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen3Black, "+7+1", MoveNotationType.English, new(7, 2), new(7, 1), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen3Black, "27=6", MoveNotationType.English, new(7, 3), new(6, 3), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen3Black, "37=8", MoveNotationType.English, new(7, 4), new(8, 4), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen3Black, "47=6", MoveNotationType.English, new(7, 5), new(6, 5), PieceType.Pawn, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_multiPawnFen3Black, "p5+1", MoveNotationType.English, new(5, 4), new(5, 3), PieceType.Pawn, ExpectedResult: true) };
        }
    }

    [Theory]
    [MemberData(nameof(MoveMethodWithEnglishNotationTestData))]
    public static void MoveMethod_ShouldAlterTheBoardCorrectly_WhenUsingEnglishMoveNotation(MoveNotationMethodTestData testData)
        => MoveParserTestHelper.AssertMoveWithNotationMethod(testData);

    [Theory]
    [MemberData(nameof(MoveMethodWithRedEnglishNotationForMultiPawnTestData))]
    public static void MoveMethod_ShouldAlterTheBoardCorrectly_WhenUsingEnglishMoveNotation_ForRedMultiPawn(MoveNotationMethodTestData testData)
        => MoveParserTestHelper.AssertMoveWithNotationMethod(testData);

    [Theory]
    [MemberData(nameof(MoveMethodWithBlackEnglishNotationForMultiPawnTestData))]
    public static void MoveMethod_ShouldAlterTheBoardCorrectly_WhenUsingEnglishMoveNotation_ForBlackMultiPawn(MoveNotationMethodTestData testData)
    => MoveParserTestHelper.AssertMoveWithNotationMethod(testData);
}
