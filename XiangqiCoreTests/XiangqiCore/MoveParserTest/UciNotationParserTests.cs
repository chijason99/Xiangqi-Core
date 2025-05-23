﻿using XiangqiCore.Extension;
using XiangqiCore.Game;
using XiangqiCore.Move;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Pieces.PieceTypes;

namespace xiangqi_core_test.XiangqiCore.MoveParserTest;
public static class UciNotationParserTests
{
    private const string _startingFen2 = MoveParserTestHelper.StartingFen2;
    private const string _startingFen3 = MoveParserTestHelper.StartingFen3;

    public static IEnumerable<object[]> MoveMethodWithUcciNotationTestData
    {
        get
        {
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "h9h5", MoveNotationType.UCI, new(8, 9), new(8, 5), PieceType.Rook, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "h10i10", MoveNotationType.UCI, new(8, 10), new(9, 10), PieceType.Rook, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "g8e9", MoveNotationType.UCI, new(7, 8), new(5, 9), PieceType.Knight, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "h4a4", MoveNotationType.UCI, new(8, 4), new(1, 4), PieceType.Cannon, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen2, "c7c6", MoveNotationType.UCI, new(3, 7), new(3, 6), PieceType.Pawn, ExpectedResult: true) };

            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "b5f5", MoveNotationType.UCI, new(2, 5), new(6, 5), PieceType.Rook, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "h5i7", MoveNotationType.UCI, new(8, 5), new(9, 7), PieceType.Knight, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "e3c5", MoveNotationType.UCI, new(5, 3), new(3, 5), PieceType.Bishop, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "a7a5", MoveNotationType.UCI, new(1, 7), new(1, 5), PieceType.Cannon, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "e1d1", MoveNotationType.UCI, new(5, 1), new(4, 1), PieceType.King, ExpectedResult: true) };
            yield return new object[] { new MoveNotationMethodTestData(_startingFen3, "e2d3", MoveNotationType.UCI, new(5, 2), new(4, 3), PieceType.Advisor, ExpectedResult: true) };
        }
    }

    [Theory]
    [MemberData(nameof(MoveMethodWithUcciNotationTestData))]
    public static void MoveMethod_ShouldAlterTheBoardCorrectly_WhenUsingUciMoveNotation(MoveNotationMethodTestData testData)
        => MoveParserTestHelper.AssertMoveWithNotationMethod(testData);

	[Theory]
	[InlineData("3akab2/9/1cn1b1nrc/pC2p1R1p/2p6/6P2/P1P1P3P/N3C1N2/9/2BAKAB2 b - - 2 11", "炮9退1", "i8i9")]
	[InlineData("3akab2/9/2n1b4/p3p2Cp/2p2P3/5N3/PcP1P3P/N4c3/9/2BAKAB2 w - - 1 17", "炮二進一", "h7h8")]
	[InlineData("3k1abC1/4a1N2/9/4N3p/2p2P3/P8/5c2P/1c2Bn3/5K3/3A1AB2 w - - 3 32", "帥四平五", "f2e2")]
	[InlineData("3k1abC1/4a1N2/9/4N3p/2p2P3/P8/4c3P/1c3n3/4K4/2BA1AB2 b - - 6 34", "馬6退5", "f3e5")]
	public static void TranslateToUciNotation_ShouldReturnCorrectUciNotation_WhenGivenMoveObject(string startingFen, string moveNotation, string expectedResult)
	{
		// Arrange
		XiangqiBuilder builder = new();

		XiangqiGame game = builder
							.WithStartingFen(startingFen)
							.Build();

		bool moveResult = game.MakeMove(moveNotation, MoveNotationType.TraditionalChinese);

		moveResult.Should().BeTrue();
		game.MoveHistory.Count.Should().Be(1);

		MoveHistoryObject latestMoveHistoryObject = game.MoveHistory.Last();

		// Act
		string result = latestMoveHistoryObject.TranslateTo(MoveNotationType.UCI);

		// Assert
		result.Should().Be(expectedResult);
	}
}
