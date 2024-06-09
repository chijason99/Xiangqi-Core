using XiangqiCore.Move.Move;
using XiangqiCore.Pieces.PieceTypes;

namespace xiangqi_core_test.XiangqiCore.MoveParserTest;
public static class EnglishMoveNotationParserTests
{
    private const string _startingFen2 = MoveParserTestHelper.StartingFen2;
    private const string _startingFen3 = MoveParserTestHelper.StartingFen3;

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

    [Theory]
    [MemberData(nameof(MoveMethodWithEnglishNotationTestData))]
    public static void MoveMethod_ShouldAlterTheBoardCorrectly_WhenUsingEnglishMoveNotation(MoveNotationMethodTestData testData)
   => MoveParserTestHelper.AssertMoveWithNotationMethod(testData);
}
