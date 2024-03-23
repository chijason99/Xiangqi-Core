using XiangqiCore.Pieces.PieceTypes;
using static XiangqiCore.Pieces.PieceTypes.PieceType;

namespace xiangqi_core_test.XiangqiCore.FenTest;
public static class FenHelperTests
{
    [Theory]
    [InlineData("2bakabn1/3C1C3/5rc2/p3N3p/2p3p2/9/P1P1n3P/4B4/4A4/1RBAK4 w - - 0 18")]
    [InlineData("3a1kb2/4a1c2/4b1n2/p1P2C2p/6Nn1/P8/5C2P/4B4/4A4/2BAK4 b - - 14 31")]
    [InlineData("5kb2/4a4/4b4/p1P5p/9/P7P/1nN6/4B4/4A4/2BAK4 w - - 8 40")]
    [InlineData("2bakabn1/5C3/8c/p3N3p/3C1np2/2P6/P7P/4B4/4A4/2BAK4 b - - 2 23")]
    [InlineData("2b1k4/4a1P2/2N6/5P3/2b1n4/9/3p5/4BA3/4A4/3K5 b - - 38 70")]
    public static void ShouldReturnTrue_WhenGivenValidFen(string sampleFen)
    {
        // Arrange
        // Act
        bool result = FenHelper.Validate(sampleFen);
        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("/3C1C3/5rc2/p3N3p/2p3p2/9/P1P1n3P/4B4/4A4/1RBAK4 w - - 0 18")]
    [InlineData("rCnCnBcNrN/3C1C3/5rc2/p3N3p/2p3p2/9/P1P1n3P/4B4/4A4/1RBAK4 w - - 0 18")]
    [InlineData("1c5ak9/3C1C3/5rc2/p3N3p/2p3p2/9/P1P1n3P/4B4/4A4/1RBAK4 w - - 0 18")]
    [InlineData("2bakabn1/3C1C3/5rc2/p3N3p/2p3p2/9/P1P1n3P/4B4/4A4/1RBAK4 j - - 0 18")]
    public static void ShouldReturnFalse_WhenGivenInValidFen(string sampleFen)
    {
        // Arrange
        // Act
        bool result = FenHelper.Validate(sampleFen);
        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("rnbakabnr", new PieceType[]{ Rook, Knight, Bishop, Advisor, PieceType.King, Advisor, Bishop, Knight, Rook})]
    [InlineData("2PN5", new PieceType[]{ None, None, Pawn, Knight, None, None, None, None, None })]
    [InlineData("1C2BC3", new PieceType[]{ None, Cannon, None, None, Bishop, Cannon, None, None, None })]
    [InlineData("n2cb1n2", new PieceType[]{ Knight, None, None, Cannon, Bishop, None, Knight, None, None })]
    public static void ShouldParseFenRowAndReturnCorrectPieces_WhenGivenAValidFenRowString(string fenRow, PieceType[] expectedResult)
    {
        // Arrange
        // Act
        var result = FenHelper.ParseSingleRow(fenRow);

        // Assert
        var actualResult = result.Select(x => x.GetPieceType);

        Assert.Equal(actualResult, expectedResult);
    }

    [Theory]
    [InlineData("rnbakabnr", new Side[] { Side.Black, Side.Black, Side.Black, Side.Black, Side.Black, Side.Black, Side.Black, Side.Black, Side.Black })]
    [InlineData("2PN5", new Side[] { Side.None, Side.None, Side.Red, Side.Red, Side.None, Side.None, Side.None, Side.None, Side.None })]
    [InlineData("1C2BC3", new Side[] { Side.None, Side.Red, Side.None, Side.None, Side.Red, Side.Red, Side.None, Side.None, Side.None })]
    [InlineData("n2cb1N2", new Side[] { Side.Black, Side.None, Side.None, Side.Black, Side.Black, Side.None, Side.Red, Side.None, Side.None })]
    public static void ShouldParseFenRowAndReturnCorrectSides_WhenGivenAValidFenRowString(string fenRow, Side[] expectedResult)
    {
        // Arrange
        // Act
        var result = FenHelper.ParseSingleRow(fenRow);

        // Assert
        var actualResult = result.Select(x => x.Side);

        Assert.Equal(actualResult, expectedResult);
    }

    [Theory]
    [InlineData('1', "1")]
    [InlineData('2', "11")]
    [InlineData('3', "111")]
    [InlineData('4', "1111")]
    [InlineData('5', "11111")]
    [InlineData('6', "111111")]
    [InlineData('7', "1111111")]
    [InlineData('8', "11111111")]
    [InlineData('9', "111111111")]
    public static void ShouldTurnCharDigitIntoStringOfOne_WhenGivenACharDigit(char digit, string expectedResult)
    {
        // Arrange
        // Act
        var actualResult = FenHelper.SplitCharDigitToStringOfOne(digit);

        // Assert
        actualResult.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData('c', Side.Black)]
    [InlineData('K', Side.Red)]
    [InlineData('r', Side.Black)]
    [InlineData('P', Side.Red)]
    [InlineData('1', Side.None)]
    public static void ShouldReturnCorrectSides_WhenGivenACharOfPiece(char piece, Side expectedResult)
    {
        // Arrange
        // Act
        var actualResult = FenHelper.GetSideFromFenChar(piece);

        // Assert
        actualResult.Should().Be(expectedResult);
    }
}
