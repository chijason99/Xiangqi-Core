using XiangqiCore.FenHelper;
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
}
