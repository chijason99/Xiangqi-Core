using XiangqiCore.Extension;
using XiangqiCore.Game;
using XiangqiCore.Misc;
using XiangqiCore.Pieces.PieceTypes;

namespace xiangqi_core_test.XiangqiCore.ValidationStrategyTests;
public static class PawnValidationStrategyTests
{
    public static IEnumerable<object[]> RedPawnCoordinates
    {
        get
        {
            // Red Pawn With Valid Move
            yield return new object[] { new PawnValidationTestData(Side.Red, new(5, 4), new(5, 5), ExpectedResult: true) };
            yield return new object[] { new PawnValidationTestData(Side.Red, new(7, 5), new(7, 6), ExpectedResult: true) };
            yield return new object[] { new PawnValidationTestData(Side.Red, new(5, 7), new(4, 7), ExpectedResult: true) };
            yield return new object[] { new PawnValidationTestData(Side.Red, new(5, 7), new(6, 7), ExpectedResult: true) };
            yield return new object[] { new PawnValidationTestData(Side.Red, new(5, 7), new(5, 8), ExpectedResult: true) };


            // Red Pawn With Invalid Move
            yield return new object[] { new PawnValidationTestData(Side.Red, new(5, 4), new(5, 6), ExpectedResult: false) };
            yield return new object[] { new PawnValidationTestData(Side.Red, new(5, 4), new(4, 4), ExpectedResult: false) };
            yield return new object[] { new PawnValidationTestData(Side.Red, new(5, 4), new(6, 4), ExpectedResult: false) };
            yield return new object[] { new PawnValidationTestData(Side.Red, new(5, 4), new(5, 3), ExpectedResult: false) };
            yield return new object[] { new PawnValidationTestData(Side.Red, new(3, 7), new(3, 6), ExpectedResult: false) };
            yield return new object[] { new PawnValidationTestData(Side.Red, new(3, 7), new(1, 7), ExpectedResult: false) };
            yield return new object[] { new PawnValidationTestData(Side.Red, new(3, 7), new(5, 7), ExpectedResult: false) };

            // Black Pawn with Valid Move
            yield return new object[] { new PawnValidationTestData(Side.Black, new(7, 6), new(7, 5), ExpectedResult: true) };
            yield return new object[] { new PawnValidationTestData(Side.Black, new(7, 5), new(7, 4), ExpectedResult: true) };
            yield return new object[] { new PawnValidationTestData(Side.Black, new(7, 4), new(6, 4), ExpectedResult: true) };
            yield return new object[] { new PawnValidationTestData(Side.Black, new(7, 4), new(8, 4), ExpectedResult: true) };
            yield return new object[] { new PawnValidationTestData(Side.Black, new(7, 4), new(7, 3), ExpectedResult: true) };


            // Black Pawn with Invalid Move
            yield return new object[] { new PawnValidationTestData(Side.Black, new(7, 6), new(7, 7), ExpectedResult: false) };
            yield return new object[] { new PawnValidationTestData(Side.Black, new(7, 5), new(7, 5), ExpectedResult: false) };
            yield return new object[] { new PawnValidationTestData(Side.Black, new(7, 7), new(8, 7), ExpectedResult: false) };
            yield return new object[] { new PawnValidationTestData(Side.Black, new(7, 7), new(6, 7), ExpectedResult: false) };
            yield return new object[] { new PawnValidationTestData(Side.Black, new(7, 3), new(5, 3), ExpectedResult: false) };
            yield return new object[] { new PawnValidationTestData(Side.Black, new(7, 3), new(9, 3), ExpectedResult: false) };
            yield return new object[] { new PawnValidationTestData(Side.Black, new(7, 3), new(7, 4), ExpectedResult: false) };
        }
    }

    [Theory]
    [MemberData(nameof(RedPawnCoordinates))]
    public static async Task ValidateMoveLogicForPieceShouldReturnTrue_WhenGivenValidMoves_ForRedPawnAsync(PawnValidationTestData pawnValidationTestData)
    {
        // Arrange
        XiangqiBuilder builder = new();

        Coordinate pawnCoordinate = pawnValidationTestData.StartingPosition;
        Coordinate destination = pawnValidationTestData.Destination;
        Side side = pawnValidationTestData.Side;

        XiangqiGame game =  await builder
                            .WithEmptyBoard()
                            .WithBoardConfig(config => config.AddPiece(PieceType.Pawn, side, pawnCoordinate))
                            .BuildAsync();
        // Act
        Pawn pawn = (Pawn)game.BoardPosition.GetPieceAtPosition(pawnCoordinate);

        bool isMoveValid = pawn.ValidationStrategy.ValidateMoveLogicForPiece(game.BoardPosition, pawnCoordinate, destination);
        bool expectedResult = pawnValidationTestData.ExpectedResult;
        // Assert
        isMoveValid.Should().Be(expectedResult);
    }
}

public record PawnValidationTestData(Side Side, Coordinate StartingPosition, Coordinate Destination, bool ExpectedResult);
