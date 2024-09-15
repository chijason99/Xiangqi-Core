using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xiangqi_core_test.XiangqiCore.ExtensionTest;
using XiangqiCore.Extension;
using XiangqiCore.Game;
using XiangqiCore.Misc;

namespace xiangqi_core_test.XiangqiCore.ValidationStrategyTests;
public static class DefaultValidationStrategyTests
{
    private const string _startingFen1 = "2b1kab1r/2R1n4/5a3/p2r2n1p/2p1C4/1N3p3/P3P3P/4B4/3R5/3AKAB2 b - - 1 1";
    private const string _startingFen2 = "1r2kabC1/3na4/4b1c2/p1p3r1p/9/2P1C4/Pc2P3P/2N1B4/9/1RBAKA1R1 w - - 2 15";
    private const string _startingFen3 = "9/3k5/9/9/9/9/3R5/9/9/4K4 w - - 0 0";
    private const string _startingFen4 = "3k5/9/9/9/9/9/9/9/3pp2n1/4K4 w - - 0 0";

    public static IEnumerable<object []> IsProposedMoveValidTestData
    {
        get
        {
            yield return new object[] { new IsProposedMoveValidTestData(_startingFen1, new Coordinate(7, 7), new Coordinate(5, 6), ExpectedResult: false) };
            yield return new object[] { new IsProposedMoveValidTestData(_startingFen1, new Coordinate(4, 7), new Coordinate(5, 7), ExpectedResult: false) };
            yield return new object[] { new IsProposedMoveValidTestData(_startingFen1, new Coordinate(3, 10), new Coordinate(5, 8), ExpectedResult: false) };
            yield return new object[] { new IsProposedMoveValidTestData(_startingFen1, new Coordinate(5, 9), new Coordinate(6, 7), ExpectedResult: false) };
            yield return new object[] { new IsProposedMoveValidTestData(_startingFen1, new Coordinate(5, 10), new Coordinate(4, 10), ExpectedResult: false) };

            yield return new object[] { new IsProposedMoveValidTestData(_startingFen2, new Coordinate(5, 8), new Coordinate(3, 10), ExpectedResult: true) };
            yield return new object[] { new IsProposedMoveValidTestData(_startingFen2, new Coordinate(5, 9), new Coordinate(6, 8), ExpectedResult: true) };
            yield return new object[] { new IsProposedMoveValidTestData(_startingFen2, new Coordinate(7, 10), new Coordinate(9, 8), ExpectedResult: true) };

            yield return new object[] { new IsProposedMoveValidTestData(_startingFen3, new Coordinate(4, 9), new Coordinate(4, 10), ExpectedResult: true) };
            yield return new object[] { new IsProposedMoveValidTestData(_startingFen3, new Coordinate(4, 9), new Coordinate(4, 8), ExpectedResult: true) };
            yield return new object[] { new IsProposedMoveValidTestData(_startingFen3, new Coordinate(4, 9), new Coordinate(5, 9), ExpectedResult: true) };

            yield return new object[] { new IsProposedMoveValidTestData(_startingFen4, new Coordinate(5, 1), new Coordinate(5, 2), ExpectedResult: true) };
            yield return new object[] { new IsProposedMoveValidTestData(_startingFen4, new Coordinate(5, 1), new Coordinate(4, 1), ExpectedResult: true) };
            yield return new object[] { new IsProposedMoveValidTestData(_startingFen4, new Coordinate(5, 1), new Coordinate(6, 1), ExpectedResult: true) };
        }
    }

    [Theory]
    [MemberData(nameof(IsProposedMoveValidTestData))]
    public static async Task IsProposedMoveValid_ShouldReturnExpectedResultAsync(IsProposedMoveValidTestData testData)
    {
        // Arrange
        XiangqiBuilder builder = new();
        XiangqiGame game = await builder.UseCustomFen(testData.StartingFen).BuildAsync();

        Coordinate staringPosition = testData.StartingPosition;
        Coordinate destination = testData.Destination;
        bool expectedResult = testData.ExpectedResult;

        Piece[,] boardPosition = game.BoardPosition;
        Piece pieceToMove = boardPosition.GetPieceAtPosition(staringPosition);

        // Act
        bool actualResult = pieceToMove.ValidationStrategy.IsProposedMoveValid(boardPosition, staringPosition, destination);

        // Assert
        actualResult.Should().Be(actualResult);
    }
}

public record IsProposedMoveValidTestData(string StartingFen, Coordinate StartingPosition, Coordinate Destination, bool ExpectedResult);
