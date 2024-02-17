namespace xiangqi_core_test.Xiangqi_Core.Methods;

public static class Create_Xiangqi_Game_Tests
{
    [Fact]
    public static void ShouldCreateDefaultXiangqiGame_WhenCallingCreateWithDefaultMethod()
    {
        // Arrange
        IXianqgiBuilder builder = new XiangqiBuilder();

        // Act
        XiangqiGame xiangqiGame = builder.UseDefaultConfiguration().Build();

        // Assert
        xiangqiGame.InitialFenString.Should().Be("rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w");

        xiangqiGame.SideToMove.Should().Be(Side.Red);
    }

    [Theory]
    [InlineData("3akab2/5r3/4b1cCn/p3p3p/2p6/6N2/PrP1N3P/4BC3/9/2BAKA1R1 b - - 2 15")]
    [InlineData("r3kabr1/4a4/1cn1b1n2/p1p1p3p/6p2/1CP6/P3P1PcP/2N1C1N2/3R5/2BAKABR1 b - - 15 8")]
    [InlineData("1C2kabr1/4a4/r3b1n2/p1p1p3p/6pc1/2P1P4/PR4PR1/BCN6/4A4/4KAB1c b - - 5 22")]
    [InlineData("2bakn3/3ca4/2n1b4/p1p2R1Np/1r4p2/2P1p2c1/P5r1P/R3C1N2/3C5/2BAKAB2 b - - 11 22")]
    public static void ShouldCreateDefaultXiangqiGameWithCustomFenString_WhenCallingUseCustomFenMethod(string customFen)
    {
        // Arrange
        IXianqgiBuilder builder = new XiangqiBuilder();

        // Act
        XiangqiGame xiangqiGame = builder.UseDefaultConfiguration().UseCustomFen(customFen).Build();

        // Assert
        xiangqiGame.InitialFenString.Should().Be(customFen);
    }

}
