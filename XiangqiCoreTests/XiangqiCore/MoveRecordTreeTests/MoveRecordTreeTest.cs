using XiangqiCore.Game;
using XiangqiCore.Move;

namespace xiangqi_core_test.XiangqiCore.MoveRecordTreeTests;

public static class MoveRecordTreeTest
{
    [Fact]
    public static void VariationShouldBeAdded_WhenANextMoveAlreadyExists()
    {
        // Arrange
        XiangqiBuilder builder = new();
        
        XiangqiGame game = builder
            .WithDefaultConfiguration()
            .Build();

        // Make some initial moves
        game.MakeMove("C2=5", MoveNotationType.English);
        game.MakeMove("c8=5", MoveNotationType.English);
        game.MakeMove("H2+3", MoveNotationType.English);

        // Navigate to the first move
        game.NavigateToMove(moveNumber: 1);

        // Act
        game.MakeMove("h8+7", MoveNotationType.English);
        
        // Assert
        game.NavigateToPreviousMove();
        
        // Verify that the variation is added correctly
        game.CurrentMove.Variations.Count
            .Should()
            .Be(2);
    }
}