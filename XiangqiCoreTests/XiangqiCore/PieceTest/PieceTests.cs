using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;
using XiangqiCore.Pieces.ValidationStrategy;

namespace xiangqi_core_test.XiangqiCore.PieceTest;
public static class PieceTests
{
    [Fact]
    public static void ShouldCreateKing_WhenCreatingKingInPieceFactory()
    {
        // Arrange

        // Act
        var redKingResult = PieceFactory.Create(PieceType.King, Side.Red, new Coordinate(1, 4));
        var blackKingResult = PieceFactory.Create(PieceType.King, Side.Black, new Coordinate(5, 10));

        // Assert
        redKingResult.Should().NotBeNull();
        blackKingResult.Should().NotBeNull();

        redKingResult.IsSuccess.Should().BeTrue();
        blackKingResult.IsSuccess.Should().BeTrue();

        var redKing = redKingResult.Value;
        var blackKing = blackKingResult.Value;

        Assert.IsType<King>(redKing);
        Assert.IsType<King>(blackKing);

        Assert.IsType<KingValidationStrategy>(redKing.ValidationStrategy);
        Assert.IsType<KingValidationStrategy>(blackKing.ValidationStrategy);

        redKing.Side.Should().Be(Side.Red);
        blackKing.Side.Should().Be(Side.Black);

        redKing.Coordinate.Row.Should().Be(4);
        redKing.Coordinate.Column.Should().Be(1);

        blackKing.Coordinate.Row.Should().Be(10);
        blackKing.Coordinate.Column.Should().Be(5);
    }

    [Fact]
    public static void ShouldCreateRook_WhenCreatingRookInPieceFactory()
    {
        // Arrange

        // Act
        var redRookResult = PieceFactory.Create(PieceType.Rook, Side.Red, new Coordinate(3, 7));
        var blackRookResult = PieceFactory.Create(PieceType.Rook, Side.Black, new Coordinate(4, 1));

        // Assert
        redRookResult.Should().NotBeNull();
        blackRookResult.Should().NotBeNull();

        redRookResult.IsSuccess.Should().BeTrue();
        blackRookResult.IsSuccess.Should().BeTrue();

        var redRook = redRookResult.Value;
        var blackRook = blackRookResult.Value;

        Assert.IsType<Rook>(redRook);
        Assert.IsType<Rook>(blackRook);

        Assert.IsType<RookValidationStrategy>(redRook.ValidationStrategy);
        Assert.IsType<RookValidationStrategy>(blackRook.ValidationStrategy);

        redRook.Side.Should().Be(Side.Red);
        blackRook.Side.Should().Be(Side.Black);

        redRook.Coordinate.Row.Should().Be(7);
        redRook.Coordinate.Column.Should().Be(3);

        blackRook.Coordinate.Row.Should().Be(1);
        blackRook.Coordinate.Column.Should().Be(4);
    }

    [Fact]
    public static void ShouldCreateKnight_WhenCreatingKnightInPieceFactory()
    {
        // Arrange

        // Act
        var redKnightResult = PieceFactory.Create(PieceType.Knight, Side.Red, new Coordinate(9, 7));
        var blackKnightResult = PieceFactory.Create(PieceType.Knight, Side.Black, new Coordinate(4, 10));

        // Assert
        redKnightResult.Should().NotBeNull();
        blackKnightResult.Should().NotBeNull();

        redKnightResult.IsSuccess.Should().BeTrue();
        blackKnightResult.IsSuccess.Should().BeTrue();

        var redKnight = redKnightResult.Value;
        var blackKnight = blackKnightResult.Value;

        Assert.IsType<Knight>(redKnight);
        Assert.IsType<Knight>(blackKnight);

        Assert.IsType<KnightValidationStrategy>(redKnight.ValidationStrategy);
        Assert.IsType<KnightValidationStrategy>(blackKnight.ValidationStrategy);

        redKnight.Side.Should().Be(Side.Red);
        blackKnight.Side.Should().Be(Side.Black);

        redKnight.Coordinate.Row.Should().Be(7);
        redKnight.Coordinate.Column.Should().Be(9);

        blackKnight.Coordinate.Row.Should().Be(10);
        blackKnight.Coordinate.Column.Should().Be(4);
    }

    [Fact]
    public static void ShouldCreateCannon_WhenCreatingCannonInPieceFactory()
    {
        // Arrange

        // Act
        var redCannonResult = PieceFactory.Create(PieceType.Cannon, Side.Red, new Coordinate(5, 5));
        var blackCannonResult = PieceFactory.Create(PieceType.Cannon, Side.Black, new Coordinate(2, 1));

        // Assert
        redCannonResult.Should().NotBeNull();
        blackCannonResult.Should().NotBeNull();

        redCannonResult.IsSuccess.Should().BeTrue();
        blackCannonResult.IsSuccess.Should().BeTrue();

        var redCannon = redCannonResult.Value;
        var blackCannon = blackCannonResult.Value;

        Assert.IsType<Cannon>(redCannon);
        Assert.IsType<Cannon>(blackCannon);

        Assert.IsType<CannonValidationStrategy>(redCannon.ValidationStrategy);
        Assert.IsType<CannonValidationStrategy>(blackCannon.ValidationStrategy);

        redCannon.Side.Should().Be(Side.Red);
        blackCannon.Side.Should().Be(Side.Black);

        redCannon.Coordinate.Row.Should().Be(5);
        redCannon.Coordinate.Column.Should().Be(5);

        blackCannon.Coordinate.Row.Should().Be(1);
        blackCannon.Coordinate.Column.Should().Be(2);
    }

    [Fact]
    public static void ShouldCreateAdvisor_WhenCreatingAdvisorInPieceFactory()
    {
        // Arrange

        // Act
        var redAdvisorResult = PieceFactory.Create(PieceType.Advisor, Side.Red, new Coordinate(5, 5));
        var blackAdvisorResult = PieceFactory.Create(PieceType.Advisor, Side.Black, new Coordinate(2, 1));

        // Assert
        redAdvisorResult.Should().NotBeNull();
        blackAdvisorResult.Should().NotBeNull();

        redAdvisorResult.IsSuccess.Should().BeTrue();
        blackAdvisorResult.IsSuccess.Should().BeTrue();

        var redAdvisor = redAdvisorResult.Value;
        var blackAdvisor = blackAdvisorResult.Value;

        Assert.IsType<Advisor>(redAdvisor);
        Assert.IsType<Advisor>(blackAdvisor);

        Assert.IsType<AdvisorValidationStrategy>(redAdvisor.ValidationStrategy);
        Assert.IsType<AdvisorValidationStrategy>(blackAdvisor.ValidationStrategy);

        redAdvisor.Side.Should().Be(Side.Red);
        blackAdvisor.Side.Should().Be(Side.Black);

        redAdvisor.Coordinate.Row.Should().Be(5);
        redAdvisor.Coordinate.Column.Should().Be(5);

        blackAdvisor.Coordinate.Row.Should().Be(1);
        blackAdvisor.Coordinate.Column.Should().Be(2);
    }

    [Fact]
    public static void ShouldCreateBishop_WhenCreatingBishopInPieceFactory()
    {
        // Arrange

        // Act
        var redBishopResult = PieceFactory.Create(PieceType.Bishop, Side.Red, new Coordinate(5, 5));
        var blackBishopResult = PieceFactory.Create(PieceType.Bishop, Side.Black, new Coordinate(2, 1));

        // Assert
        redBishopResult.Should().NotBeNull();
        blackBishopResult.Should().NotBeNull();

        redBishopResult.IsSuccess.Should().BeTrue();
        blackBishopResult.IsSuccess.Should().BeTrue();

        var redBishop = redBishopResult.Value;
        var blackBishop = blackBishopResult.Value;

        Assert.IsType<Bishop>(redBishop);
        Assert.IsType<Bishop>(blackBishop);

        Assert.IsType<BishopValidationStrategy>(redBishop.ValidationStrategy);
        Assert.IsType<BishopValidationStrategy>(blackBishop.ValidationStrategy);

        redBishop.Side.Should().Be(Side.Red);
        blackBishop.Side.Should().Be(Side.Black);

        redBishop.Coordinate.Row.Should().Be(5);
        redBishop.Coordinate.Column.Should().Be(5);

        blackBishop.Coordinate.Row.Should().Be(1);
        blackBishop.Coordinate.Column.Should().Be(2);
    }

    [Fact]
    public static void ShouldCreatePawn_WhenCreatingPawnInPieceFactory()
    {
        // Arrange

        // Act
        var redPawnResult = PieceFactory.Create(PieceType.Pawn, Side.Red, new Coordinate(5, 5));
        var blackPawnResult = PieceFactory.Create(PieceType.Pawn, Side.Black, new Coordinate(2, 1));

        // Assert
        redPawnResult.Should().NotBeNull();
        blackPawnResult.Should().NotBeNull();

        redPawnResult.IsSuccess.Should().BeTrue();
        blackPawnResult.IsSuccess.Should().BeTrue();

        var redPawn = redPawnResult.Value;
        var blackPawn = blackPawnResult.Value;

        Assert.IsType<Pawn>(redPawn);
        Assert.IsType<Pawn>(blackPawn);

        Assert.IsType<PawnValidationStrategy>(redPawn.ValidationStrategy);
        Assert.IsType<PawnValidationStrategy>(blackPawn.ValidationStrategy);

        redPawn.Side.Should().Be(Side.Red);
        blackPawn.Side.Should().Be(Side.Black);

        redPawn.Coordinate.Row.Should().Be(5);
        redPawn.Coordinate.Column.Should().Be(5);

        blackPawn.Coordinate.Row.Should().Be(1);
        blackPawn.Coordinate.Column.Should().Be(2);
    }
}
