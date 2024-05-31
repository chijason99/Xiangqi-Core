using XiangqiCore.Move;
using XiangqiCore.Pieces.PieceTypes;

namespace xiangqi_core_test.XiangqiCore.MoveParsetTest;
public static class MoveParserTests
{
    [Theory]
    [InlineData("c2=5", typeof(Cannon))]
    [InlineData("h8+7", typeof(Knight))]
    [InlineData("r8-6", typeof(Rook))]
    [InlineData("k5=6", typeof(King))]
    [InlineData("a5+6", typeof(Advisor))]
    [InlineData("b5+3", typeof(Bishop))]
    [InlineData("p7=6", typeof(Pawn))]
    [InlineData("+2=3", typeof(Pawn))]
    [InlineData("-r+3", typeof(Rook))]
    public static void MoveParseShouldReturnCorrectPieceType_WithEnglishNotation(string moveNotation, Type pieceType)
    {
        // Arrange
        // Act
        Type result = EnglishNotationParser.ParsePieceTypeFromEnglishNotation(moveNotation);

        // Assert
        result.Should().Be(pieceType);
    }

    [Theory]
    [InlineData("炮二平五", typeof(Cannon))]
    [InlineData("馬8進7", typeof(Knight))]
    [InlineData("車八退六", typeof(Rook))]
    [InlineData("將五平六", typeof(King))]
    [InlineData("仕五進六", typeof(Advisor))]
    [InlineData("象5進3", typeof(Bishop))]
    [InlineData("兵七平六", typeof(Pawn))]
    [InlineData("前二平三", typeof(Pawn))]
    [InlineData("後車進3", typeof(Rook))]
    [InlineData("前馬進六", typeof(Knight))]
    public static void MoveParseShouldReturnCorrectPieceType_WithChineseNotation(string moveNotation, Type pieceType)
    {
        // Arrange
        // Act
        Type result = ChineseNotationParser.ParsePieceType(moveNotation);

        // Assert
        result.Should().Be(pieceType);
    }
}
