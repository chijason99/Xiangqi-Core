using XiangqiCore.Game;
using XiangqiCore.Move;

namespace xiangqi_core_test.XiangqiCore.MoveRecordTreeTests;

public static class MoveNavigationTest
{
    [Fact]
    public static void NavigateToMove_ShouldSetBoardStateCorrectly()
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

        // Act
        game.NavigateToMove(moveNumber: 1);

        // Assert
        game.CurrentFen.Should().Be("rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C2C4/9/RNBAKABNR b - - 1 1");
    }
    
    [Fact]
    public static void NavigateToPreviousMove_ShouldSetBoardStateCorrectly()
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

        // Act
        game.NavigateToPreviousMove();

        // Assert
        game.CurrentMove.MoveHistoryObject.MoveNotation
            .Should()
            .Be("c8=5");
    }
    
    [Fact]
    public static void NavigateToStart_ShouldSetBoardStateCorrectly()
    {
        // Arrange
        XiangqiBuilder builder = new();
        
        XiangqiGame game = builder
            .WithStartingFen("4kab2/2c1a4/n3bc3/2p1C2rp/4PNpn1/2P6/1R2N3P/4B4/4C4/2BAKA3 b - - 0 1")
            .Build();

        // Make some initial moves
        game.MakeMove("马８进９", MoveNotationType.SimplifiedChinese);
        game.MakeMove("后炮平三 ", MoveNotationType.SimplifiedChinese);
        game.MakeMove("将５平４", MoveNotationType.SimplifiedChinese);

        // Act
        game.NavigateToStart();

        // Assert
        game.CurrentFen.Should()
            .Be("4kab2/2c1a4/n3bc3/2p1C2rp/4PNpn1/2P6/1R2N3P/4B4/4C4/2BAKA3 b - - 0 1");
    }
    
    [Fact]
    public static void NavigateToNextMove_ShouldSetBoardStateCorrectly()
    {
        // Arrange
        XiangqiBuilder builder = new();
        
        XiangqiGame game = builder
            .WithStartingFen("4kab2/2c1a4/n3bc3/2p1C2rp/4PNpn1/2P6/1R2N3P/4B4/4C4/2BAKA3 b - - 0 1")
            .Build();

        // Make some initial moves
        game.MakeMove("马８进９", MoveNotationType.SimplifiedChinese);
        game.MakeMove("后炮平三 ", MoveNotationType.SimplifiedChinese);
        game.MakeMove("将５平４", MoveNotationType.SimplifiedChinese);

        game.NavigateToStart();
        
        // Act
        game.NavigateToNextMove();
        game.NavigateToNextMove();

        // Assert
        game.CurrentFen.Should()
            .Be("4kab2/2c1a4/n3bc3/2p1C2rp/4PNp2/2P6/1R2N3n/4B4/6C2/2BAKA3 b - - 1 2");
    }
    
    [Fact]
    public static void NavigateToEnd_ShouldSetBoardStateCorrectly()
    {
        // Arrange
        XiangqiBuilder builder = new();
        
        XiangqiGame game = builder
            .WithMoveRecord(@"  1.兵七进一      卒７进１  
  2.马八进七      马８进７  
  3.炮二平五      车９平８  
  4.马二进三      炮８平９  
  5.车一进一      士４进５  
  6.车一平六      车８进６  
  7.炮八进一      马２进１  
  8.兵五进一      车８退２  
  9.马七进五      马７进６  
 10.炮八进三      象３进５  
 11.炮八平五      炮２进５  
 12.兵五进一      马６进７  
 13.后炮退一      车１平２  
 14.兵九进一      炮９平７  
 15.兵九进一      卒１进１  
 16.车九进五      车８退１  
 17.车六进二      炮２退３  
 18.车九进一      马７退８  
 19.相三进五      炮２退３  
 20.马五进四      炮７平６  
 21.车九平八      炮２平３  
 22.车八进三      马１退２  
 23.车六平八      马２进１  
 24.马三进五      马８进９  
 25.后炮平三      将５平４  
 26.炮三平六      将４平５  
 27.马五进六      马９进８  
 28.仕六进五      车８平６  
 29.炮六平二      炮６进２  
 30.炮二平三      将５平４  
 31.马六进七      士５进４  
 32.车八进四      马１进２  
 33.兵五平四      车６平５  
 34.车八退二      将４平５  
 35.车八进四      炮３退１  
 36.炮三进八      象５退７  
 37.车八平七  ", MoveNotationType.SimplifiedChinese)
            .Build();
        
        // Act
        game.NavigateToEnd();

        // Assert
        // Verify that the current move is the first move
        game.CurrentFen.Should().Be("2R1kab2/9/2Na5/2p1r3p/5Pp2/2P6/9/4B4/4A4/2B1KA3 b - - 0 37");
    }

    [Fact]
    public static void NavigateToMoveWithVariationPath_ShouldSetBoardStateCorrectly()
    {
        // Arrange
        XiangqiGame game = CreateGameInstanceWithVariations();

        // Act
        // Assert
        // Navigate to the end of the first variation
        Assert.Multiple(() =>
        {
            game.NavigateToEnd(new VariationPath()
            {
                {13, 0}
            });

            game.CurrentFen.Should().Be("2bakab2/2r6/1cn1c1n2/p1p1p1p1p/7R1/2P2NP2/P3P3P/1CN1C4/3r5/R1BAKAB2 w - - 16 8");

            // Navigate to the end of the second variation
            game.NavigateToEnd(new VariationPath()
            {
                {13, 1}
            });

            game.CurrentFen.Should().Be("2bakab2/r8/1cn1c1n2/p1p3p1p/4p4/2P3P2/P2rP3P/1CN2CN2/4A4/R1B1KABR1 w - - 16 8");

            // Navigate to the end of the third variation
            game.NavigateToEnd(new VariationPath()
            {
                {13, 2}
            });

            game.CurrentFen.Should().Be("2bakab2/3r5/1cn1c1n2/2p1p1p1p/7R1/r1P3P2/4P3P/BCN1C1N2/9/R2AKAB2 w - - 0 9");
        });
    }

    [Fact]
    public static void NavigateToMoveWithMultipleVariationPath_ShouldRenderBoardStateCorrectly()
    {
        // Arrange
        var game = CreateGameInstanceWithVariations();

        // Navigate to the second variation of move 13
        game.NavigateToMove(15, new VariationPath()
        {
            { 13, 1 }
        });
        
        game.MakeMove("r1=6", MoveNotationType.English);
        game.MakeMove("B7+5", MoveNotationType.English);
        game.MakeMove("p5+1", MoveNotationType.English);
        game.MakeMove("C8+2", MoveNotationType.English);
        game.MakeMove("r6+5", MoveNotationType.English);

        game.NavigateToPreviousMove();
        game.NavigateToPreviousMove();
        
        game.MakeMove("R9=6", MoveNotationType.English);
        game.MakeMove("r4+3", MoveNotationType.English);
        game.MakeMove("K5=6", MoveNotationType.English);

        // Act
        game.NavigateToEnd(new VariationPath()
        {
            { 13, 1 },
            { 16, 1 },
            { 19, 0 }
        });

        // Assert
        game.CurrentFen.Should().Be("2bakab2/9/1cn1c1n2/p1p3p1p/4p4/1CP3P2/P2rPr2P/2N1BCN2/4A4/R3KABR1 w - - 20 10");
    }

    public static XiangqiGame CreateGameInstanceWithVariations()
    {
        XiangqiBuilder builder = new();

        XiangqiGame game = builder
            .WithDefaultConfiguration()
            .Build();
        
        // 順炮直車對橫車
        game.MakeMove("C2=5", MoveNotationType.English);
        game.MakeMove("c8=5", MoveNotationType.English);
        game.MakeMove("H2+3", MoveNotationType.English);
        game.MakeMove("h8+7", MoveNotationType.English);
        game.MakeMove("R1=2", MoveNotationType.English);
        game.MakeMove("r9+1", MoveNotationType.English);
        game.MakeMove("P3+1", MoveNotationType.English);
        game.MakeMove("r9=4", MoveNotationType.English);
        game.MakeMove("H8+7", MoveNotationType.English);
        game.MakeMove("h2+3", MoveNotationType.English);
        game.MakeMove("P7+1", MoveNotationType.English);
        game.MakeMove("r1+1", MoveNotationType.English);
        
        // Add the first common variation
        game.MakeMove("H3+4", MoveNotationType.English);
        game.MakeMove("r1=3", MoveNotationType.English);
        game.MakeMove("R2+5", MoveNotationType.English);
        game.MakeMove("r4+7", MoveNotationType.English);

        game.NavigateToMove(12);
        
        // Add the second variation
        game.MakeMove("A6+5", MoveNotationType.English);
        game.MakeMove("r4+5", MoveNotationType.English);
        game.MakeMove("C5=4", MoveNotationType.English);
        game.MakeMove("p5+1", MoveNotationType.English);
        
        game.NavigateToMove(12);
        
        // Add the third variation
        game.MakeMove("E7+9", MoveNotationType.English);
        game.MakeMove("p1+1", MoveNotationType.English);
        game.MakeMove("R2+5", MoveNotationType.English);
        game.MakeMove("p1+1", MoveNotationType.English);
        game.MakeMove("P9+1", MoveNotationType.English);
        game.MakeMove("r1+4", MoveNotationType.English);

        return game;
    }
}