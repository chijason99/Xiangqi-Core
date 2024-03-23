using XiangqiCore.Pieces;
using XiangqiCore.Pieces.PieceTypes;

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
    [InlineData("rnbakabnr")]
    [InlineData("n2cb1n2")]
    [InlineData("2PN5")]
    [InlineData("4kabr1")]
    public static void ShouldReturnTrue_WhenCallingValidateFenRowCharacters_WithValidFenRow(string fenRow)
    {
        // Arrange
        // Act
        bool result = FenHelper.ValidateFenRowCharacters(fenRow);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("rnbjkabnr")]
    [InlineData("n2cb1I2")]
    [InlineData("1mPN5")]
    [InlineData("z4kabr1")]
    public static void ShouldReturnFalse_WhenCallingValidateFenRowCharacters_WithInvalidFenRow(string fenRow)
    {
        // Arrange
        // Act
        bool result = FenHelper.ValidateFenRowCharacters(fenRow);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("rnbakabnr")]
    [InlineData("n2cb1n2")]
    [InlineData("2PN5")]
    [InlineData("4kabr1")]
    [InlineData("9")]
    public static void ShouldReturnTrue_WhenCallingValidateFenRowColumns_WithValidFenRow(string fenRow)
    {
        // Arrange
        // Act
        bool result = FenHelper.ValidateFenRowColumns(fenRow);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("rnbdskabnr")]
    [InlineData("n3cb1c2")]
    [InlineData("1n1PN5")]
    [InlineData("6n1PN6")]
    public static void ShouldReturnFalse_WhenCallingValidateFenRowColumns_WithInvalidFenRow(string fenRow)
    {
        // Arrange
        // Act
        bool result = FenHelper.ValidateFenRowColumns(fenRow);

        // Assert
        result.Should().BeFalse();
    }


    [Theory]
    [InlineData("rnbakabnr", new PieceType[] { PieceType.Rook, PieceType.Knight, PieceType.Bishop, PieceType.Advisor, PieceType.King, PieceType.Advisor, PieceType.Bishop, PieceType.Knight, PieceType.Rook })]
    [InlineData("2PN5", new PieceType[] { PieceType.None, PieceType.None, PieceType.Pawn, PieceType.Knight, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None })]
    [InlineData("1C2BC3", new PieceType[] { PieceType.None, PieceType.Cannon, PieceType.None, PieceType.None, PieceType.Bishop, PieceType.Cannon, PieceType.None, PieceType.None, PieceType.None })]
    [InlineData("n2cb1n2", new PieceType[] { PieceType.Knight, PieceType.None, PieceType.None, PieceType.Cannon, PieceType.Bishop, PieceType.None, PieceType.Knight, PieceType.None, PieceType.None })]
    public static void ShouldParseFenRowAndReturnCorrectPieces_WhenGivenAValidFenRowString(string fenRow, PieceType[] expectedResult)
    {
        // Arrange
        const int rowNumberPlaceHolder = 1;

        // Act
        var result = FenHelper.ParseFenRow(fenRow, rowNumberPlaceHolder);

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
        const int rowNumberPlaceHolder = 1;

        // Act
        var result = FenHelper.ParseFenRow(fenRow, rowNumberPlaceHolder);

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

    [Theory]
    [InlineData("n2cb1N2", 8, new int[] { 1, 2, 5, 7, 8 })]
    [InlineData("2PN5", 10, new int[] { 0, 1, 4, 5, 6, 7, 8 })]
    [InlineData("rnbakabnr", 10, new int[] { })]
    [InlineData("1C2BC3", 10, new int[] { 0, 2, 3, 6, 7, 8 })]
    public static void ShouldParseFenRowAndReturnCorrectCoordinates_WhenGivenAValidFenRowString(string fenRow, int rowNumber, int[] emptyCoordinatesIndex)
    {
        // Arrange
        Coordinate[] expectedResult = Enumerable.Range(1, 9)
                                                .Select((x, index) => emptyCoordinatesIndex.Any(x => x == index) ?
                                                                      Coordinate.Empty :
                                                                      new Coordinate(x, rowNumber))
                                                .ToArray();

        // Act
        var actualResult = FenHelper.ParseFenRow(fenRow, rowNumber)
                                    .Select(x => x.Coordinate);

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public static void ShouldCreateValidBoard_WhenCreateBoardFromValidFen()
    {
        // Arrange 
        const string sampleFen = "4kabr1/4a4/2n1b4/p1p1p1R1p/6p2/2P1P2c1/Pcr3P1P/1CN1C4/4N4/R1BAKAB2 w - - 7 11";

        Piece[,] expectedResult = {
                                    { new Rook(new(1, 1), Side.Red), new EmptyPiece(), new Bishop(new(3, 1), Side.Red), new Advisor(new(4,1), Side.Red), new King(new(5, 1), Side.Red), new Advisor(new (6, 1), Side.Red), new Bishop(new(7, 1), Side.Red), new EmptyPiece(), new EmptyPiece()  },
                                    { new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new Knight(new(5,2), Side.Red), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece()  },
                                    { new EmptyPiece(), new Cannon(new(2, 3), Side.Red), new Knight(new(3,3), Side.Red), new EmptyPiece(), new Cannon(new(5,3), Side.Red), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece()  },
                                    { new Pawn(new(1, 4), Side.Red), new Cannon(new(2, 4), Side.Black), new Rook(new(3, 4), Side.Black), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new Pawn(new(7,4), Side.Red), new EmptyPiece(), new Pawn(new(9,4), Side.Red)  },
                                    { new EmptyPiece(), new EmptyPiece(), new Pawn(new(3, 5), Side.Red), new EmptyPiece(), new Pawn(new(5,5), Side.Red), new EmptyPiece(), new EmptyPiece(), new Cannon(new(8, 5), Side.Black), new EmptyPiece()  },
                                    { new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new Pawn(new(7,6), Side.Black), new EmptyPiece(), new EmptyPiece()  },
                                    { new Pawn(new(1, 7), Side.Black), new EmptyPiece(), new Pawn(new(3, 7), Side.Black), new EmptyPiece(), new Pawn(new(5, 7), Side.Black), new EmptyPiece(), new Rook(new(7,7), Side.Red), new EmptyPiece(), new Pawn(new(9,7), Side.Black)  },
                                    { new EmptyPiece(), new EmptyPiece(), new Knight(new(3, 8), Side.Black), new EmptyPiece(), new Bishop(new(5,8), Side.Black), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece()  },
                                    { new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new Advisor(new(5,9), Side.Black), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece()  },
                                    { new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new EmptyPiece(), new King(new(5,10), Side.Black), new Advisor(new (6,10), Side.Black), new Bishop(new(7,10), Side.Black), new Rook(new(8,10), Side.Black), new EmptyPiece()  },
                                  };
        // Act
        var actualResult = FenHelper.CreatePositionFromFen(sampleFen);

        // Assert

        // Asserting they have the same dimension
        Assert.Equal(expectedResult.GetLength(0), actualResult.GetLength(0));
        Assert.Equal(expectedResult.GetLength(1), actualResult.GetLength(1));

        Assert.True(Enumerable.Range(0, 10).All(i => Enumerable.Range(0, 9).All(j => expectedResult[i, j].Equals(actualResult[i, j]))));
    }

    [Theory]
    [InlineData("2bak1b2/r3a4/nc1c2n2/p1p1p1p1p/3R5/2P6/P3P1PrP/1CN1C1N2/R8/2BAKAB2 w - - 0 0")]
    [InlineData("2bak4/r3a4/n3c1n1b/p1p3p1p/4P4/2P3P2/P2Rc2rP/1C2C1N2/4AR3/2BAK1B2 w - - 4 15")]
    [InlineData("r1bakab1r/9/nc2c1n2/p1p1p1p1p/9/9/P1P1P1P1P/1C2C1N2/3R5/RNBAKAB2 b - - 7 4")]
    [InlineData("3akn3/4aP3/nr2b3b/p1p3p1p/5R3/2P3P2/P4R2P/4B4/4A4/3AK1B2 b - - 2 26")]
    public static void ShouldReturnCorrectFen_WhenCallingGetFenFromPosition(string sampleFen)
    {
        // Arrange
        string positionFen = sampleFen.Split(" ").First();
        XiangqiBuilder builder = new();

        var gameInstance = builder
                            .UseDefaultConfiguration()
                            .UseCustomFen(sampleFen)
                            .Build();

        // Act
        string resultFen = gameInstance.Board.GetFenFromPosition;

        // Assert
        resultFen.Should().BeEquivalentTo(positionFen);
    }
}
