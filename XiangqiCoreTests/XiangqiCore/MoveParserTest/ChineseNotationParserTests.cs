using XiangqiCore.Move;
using XiangqiCore.Pieces.PieceTypes;

namespace xiangqi_core_test.XiangqiCore.MoveParserTest;

public static class ChineseNotationParserTests
{
    private const string _startingFen2 = MoveParserTestHelper.StartingFen2;
    private const string _startingFen3 = MoveParserTestHelper.StartingFen3;
    private const string _multiPawnFen1 = MoveParserTestHelper.MultiPawnFen1;
    private const string _multiPawnFen2 = MoveParserTestHelper.MultiPawnFen2;
    private const string _multiPawnFen3 = MoveParserTestHelper.MultiPawnFen3;
    private const string _multiPawnFen1Black = MoveParserTestHelper.MultiPawnFen1Black;
    private const string _multiPawnFen2Black = MoveParserTestHelper.MultiPawnFen2Black;
    private const string _multiPawnFen3Black = MoveParserTestHelper.MultiPawnFen3Black;

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

    public static IEnumerable<object[]> MoveMethodWithRedChineseNotationForMultiPawnTestData
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
    [MemberData(nameof(MoveMethodWithChineseNotationTestData))]
    public static void MoveMethod_ShouldAlterTheBoardCorrectly_WhenUsingChineseMoveNotation(MoveNotationMethodTestData testData)
       => MoveParserTestHelper.AssertMoveWithNotationMethod(testData);

    [Theory]
    [MemberData(nameof(MoveMethodWithRedChineseNotationForMultiPawnTestData))]
    public static void MoveMethod_ShouldAlterTheBoardCorrectly_WithRedMultiPawnTestData(MoveNotationMethodTestData testData)
   => MoveParserTestHelper.AssertMoveWithNotationMethod(testData);

    [Theory]
    [MemberData(nameof(MoveMethodWithBlackChineseNotationForMultiPawnTestData))]
    public static void MoveMethod_ShouldAlterTheBoardCorrectly_WituhBlackMultiPawnTestData(MoveNotationMethodTestData testData)
   => MoveParserTestHelper.AssertMoveWithNotationMethod(testData);
}