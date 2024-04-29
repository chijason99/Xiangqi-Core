using XiangqiCore.Pieces.PieceTypes;
using XiangqiCore.Pieces.ValidationStrategy;

namespace xiangqi_core_test.XiangqiCore.PieceTest;
public static class PieceFactoryTests
{
    [Fact]
    public static void ShouldCreateKing_WhenCreatingKingInPieceFactory()
    {
        // Arrange

        // Act
        var redKing = PieceFactory.Create(PieceType.King, Side.Red, new Coordinate(1, 4));
        var blackKing = PieceFactory.Create(PieceType.King, Side.Black, new Coordinate(5, 10));

        // Assert
        redKing.Should().NotBeNull();
        blackKing.Should().NotBeNull();

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
        var redRook = PieceFactory.Create(PieceType.Rook, Side.Red, new Coordinate(3, 7));
        var blackRook = PieceFactory.Create(PieceType.Rook, Side.Black, new Coordinate(4, 1));

        // Assert
        redRook.Should().NotBeNull();
        blackRook.Should().NotBeNull();

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
        var redKnight = PieceFactory.Create(PieceType.Knight, Side.Red, new Coordinate(9, 7));
        var blackKnight = PieceFactory.Create(PieceType.Knight, Side.Black, new Coordinate(4, 10));

        // Assert
        redKnight.Should().NotBeNull();
        blackKnight.Should().NotBeNull();

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
        var redCannon = PieceFactory.Create(PieceType.Cannon, Side.Red, new Coordinate(5, 5));
        var blackCannon = PieceFactory.Create(PieceType.Cannon, Side.Black, new Coordinate(2, 1));

        // Assert
        redCannon.Should().NotBeNull();
        blackCannon.Should().NotBeNull();

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
        var redAdvisor = PieceFactory.Create(PieceType.Advisor, Side.Red, new Coordinate(5, 5));
        var blackAdvisor = PieceFactory.Create(PieceType.Advisor, Side.Black, new Coordinate(2, 1));

        // Assert
        redAdvisor.Should().NotBeNull();
        blackAdvisor.Should().NotBeNull();

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
        var redBishop = PieceFactory.Create(PieceType.Bishop, Side.Red, new Coordinate(5, 5));
        var blackBishop = PieceFactory.Create(PieceType.Bishop, Side.Black, new Coordinate(2, 1));

        // Assert
        redBishop.Should().NotBeNull();
        blackBishop.Should().NotBeNull();

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
        var redPawn = PieceFactory.Create(PieceType.Pawn, Side.Red, new Coordinate(5, 5));
        var blackPawn = PieceFactory.Create(PieceType.Pawn, Side.Black, new Coordinate(2, 1));

        // Assert
        redPawn.Should().NotBeNull();
        blackPawn.Should().NotBeNull();

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

    [Fact]
    public static void ShouldCreateRandomPiece_WhenCreatingRandomPieceInPieceFactory()
    {
        // Arrange
        Coordinate randomCoordinate = new Coordinate(5, 5);
        Piece randomPiece = PieceFactory.CreateRandomPiece(randomCoordinate);

        // Assert
        randomPiece.PieceType.Should().NotBe(PieceType.None);
        randomPiece.Side.Should().NotBe(Side.None);
    }
}
