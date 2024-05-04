using XiangqiCore.Extension;

namespace xiangqi_core_test.XiangqiCore.ExtensionTest;
public static class PieceExtensionTests
{
    private const string _startingFen1 = "1n2kab2/4a4/2c1b1c1n/p5p2/9/2C3P2/r3P2RP/N3BCN2/4A4/4KAB2 b - - 0 16";
    private const string _startingFen2 = "4kab2/3na4/2c1b1c1n/p5p2/9/2C2NP2/r3P2RP/N3BC3/4A4/4KAB2 b - - 2 17";
    private const string _startingFen3 = "2b1kab1r/2R1n4/5a3/p2r2n1p/2p1C4/1N3p3/P3P3P/4B4/3R5/3AKAB2 b - - 1 1";
    private const string _startingFen4 = "1r2kabC1/3na4/4b1c2/p1p3r1p/9/2P1C4/Pc2P3P/2N1B4/9/1RBAKA1R1 w - - 2 15";
    private const string _startingFen5 = "9/3k5/9/9/9/9/3R5/9/9/4K4 w - - 0 0";
    private const string _startingFen6 = "3k5/9/9/9/9/9/9/9/3pp2n1/4K4 w - - 0 0";
    public static IEnumerable<object []> CountPiecesBetweenTwoCoordinatesOnRowTestData
    {
        get
        {
            yield return new object[] { new CountPiecesBetweenTwoCoordinatesTestData(_startingFen1, new Coordinate(8, 4), new Coordinate(1, 4), ExpectedResult: 1) };
            yield return new object[] { new CountPiecesBetweenTwoCoordinatesTestData(_startingFen1, new Coordinate(7, 3), new Coordinate(1, 3), ExpectedResult: 2) };
            yield return new object[] { new CountPiecesBetweenTwoCoordinatesTestData(_startingFen1, new Coordinate(3, 5), new Coordinate(5, 5), ExpectedResult: 0) };
            yield return new object[] { new CountPiecesBetweenTwoCoordinatesTestData(_startingFen1, new Coordinate(7, 10), new Coordinate(1, 10), ExpectedResult: 3) };
            yield return new object[] { new CountPiecesBetweenTwoCoordinatesTestData(_startingFen1, new Coordinate(3, 8), new Coordinate(6, 8), ExpectedResult: 1) };
        }
    }

    public static IEnumerable<object[]> CountPiecesBetweenTwoCoordinatesOnColumnTestData
    {
        get
        {
            yield return new object[] { new CountPiecesBetweenTwoCoordinatesTestData(_startingFen2, new Coordinate(6, 3), new Coordinate(6, 10), ExpectedResult: 1) };
            yield return new object[] { new CountPiecesBetweenTwoCoordinatesTestData(_startingFen2, new Coordinate(5, 1), new Coordinate(5, 10), ExpectedResult: 5) };
            yield return new object[] { new CountPiecesBetweenTwoCoordinatesTestData(_startingFen2, new Coordinate(7, 8), new Coordinate(7, 5), ExpectedResult: 1) };
            yield return new object[] { new CountPiecesBetweenTwoCoordinatesTestData(_startingFen2, new Coordinate(7, 8), new Coordinate(7, 1), ExpectedResult: 2) };
            yield return new object[] { new CountPiecesBetweenTwoCoordinatesTestData(_startingFen2, new Coordinate(3, 5), new Coordinate(3, 7), ExpectedResult: 0) };
        }
    }

    public static IEnumerable<object []> WillExposeKingToDangerTestData
    {
        get
        {
            yield return new object[] { new WillExposeKingToDangerTestData(_startingFen3, new Coordinate(7, 7), new Coordinate(5, 6), ExpectedResult: false) };
            yield return new object[] { new WillExposeKingToDangerTestData(_startingFen3, new Coordinate(4, 7), new Coordinate(5, 7), ExpectedResult: false) };
            yield return new object[] { new WillExposeKingToDangerTestData(_startingFen3, new Coordinate(3, 10), new Coordinate(5, 8), ExpectedResult: false) };
            yield return new object[] { new WillExposeKingToDangerTestData(_startingFen3, new Coordinate(5, 9), new Coordinate(6, 7), ExpectedResult: false) };
            yield return new object[] { new WillExposeKingToDangerTestData(_startingFen3, new Coordinate(5, 10), new Coordinate(4, 10), ExpectedResult: false) };

            yield return new object[] { new WillExposeKingToDangerTestData(_startingFen4, new Coordinate(5, 8), new Coordinate(3, 10), ExpectedResult: true) };
            yield return new object[] { new WillExposeKingToDangerTestData(_startingFen4, new Coordinate(5, 9), new Coordinate(6, 8), ExpectedResult: true) };
            yield return new object[] { new WillExposeKingToDangerTestData(_startingFen4, new Coordinate(7, 10), new Coordinate(9, 8), ExpectedResult: true) };

            yield return new object[] { new WillExposeKingToDangerTestData(_startingFen5, new Coordinate(4, 9), new Coordinate(4, 10), ExpectedResult: true) };
            yield return new object[] { new WillExposeKingToDangerTestData(_startingFen5, new Coordinate(4, 9), new Coordinate(4, 8), ExpectedResult: true) };
            yield return new object[] { new WillExposeKingToDangerTestData(_startingFen5, new Coordinate(4, 9), new Coordinate(5, 9), ExpectedResult: true) };

            yield return new object[] { new WillExposeKingToDangerTestData(_startingFen6, new Coordinate(5, 1), new Coordinate(5, 2), ExpectedResult: true) };
            yield return new object[] { new WillExposeKingToDangerTestData(_startingFen6, new Coordinate(5, 1), new Coordinate(4, 1), ExpectedResult: true) };
            yield return new object[] { new WillExposeKingToDangerTestData(_startingFen6, new Coordinate(5, 1), new Coordinate(6, 1), ExpectedResult: true) };
        }
    }

    public static IEnumerable<object[]> IsKingAttackedByRookTestData
    {
        get
        {
            yield return new object[] { new IsKingAttackedTestData("2R1kab2/4a4/8c/p3n3p/6p2/2PN1RP2/P2rc1r1P/C5N2/9/2BAKAB2 b - - 0 16", new Coordinate(5, 10), ExpectedResult: true) };
            yield return new object[] { new IsKingAttackedTestData("3R1kb2/9/8c/p3n3p/6p2/2PN2P2/P2rc1r1P/C5N2/9/2BAKAB2 b - - 0 18", new Coordinate(6, 10), ExpectedResult: true) };
            yield return new object[] { new IsKingAttackedTestData("6b2/5k3/4c1N2/R5P1p/9/2P6/P2r5/C2K5/9/2Bc1AB2 w - - 5 32", new Coordinate(4, 3), ExpectedResult: true) };
            yield return new object[] { new IsKingAttackedTestData("6b2/5k3/4c1N2/R5P1p/9/2P6/P3r4/C3K4/9/2Bc1AB2 w - - 7 33", new Coordinate(5, 3), ExpectedResult: true) };

            yield return new object[] { new IsKingAttackedTestData("2b1kab2/4a4/2n1c1n1c/pRp1p3p/6p2/2PN1RP2/P2rP2rP/C3C1N2/9/2BAKAB2 w - - 24 12", new Coordinate(5, 10), ExpectedResult: false) };
            yield return new object[] { new IsKingAttackedTestData("2b1kab2/4a4/2n1c1n1c/pRp1p3p/6p2/2PN1RP2/P2rP2rP/C3C1N2/9/2BAKAB2 w - - 24 12", new Coordinate(5, 1), ExpectedResult: false) };
            yield return new object[] { new IsKingAttackedTestData("5kb2/3R5/4c4/p3N3p/6p2/2P3P2/P3c4/C3K4/5r3/2BA1AB2 w - - 6 24", new Coordinate(5, 3), ExpectedResult: false) };
            yield return new object[] { new IsKingAttackedTestData("5kb2/3R5/4c4/p3N3p/6p2/2P3P2/P3c4/C3K4/5r3/2BA1AB2 w - - 6 24", new Coordinate(6, 10), ExpectedResult: false) };
        }
    }

    public static IEnumerable<object[]> IsKingExposedToOpponentKingTestData
    {
        get
        {
            yield return new object[] { new IsKingAttackedTestData("9/9/4k4/9/9/9/2P6/9/4K4/4N4 w - - 0 0", new Coordinate(5, 2), ExpectedResult: true) };
            yield return new object[] { new IsKingAttackedTestData("9/9/4k4/9/9/9/2P6/9/4K4/4N4 w - - 0 0", new Coordinate(5, 8), ExpectedResult: true) };

            yield return new object[] { new IsKingAttackedTestData("9/9/4k4/9/9/9/9/3K5/9/9 w - - 0 0", new Coordinate(4, 3), ExpectedResult: false) };
            yield return new object[] { new IsKingAttackedTestData("4k4/4a4/1r7/9/9/1C7/9/2N6/4K4/9 w - - 0 0", new Coordinate(5, 2), ExpectedResult: false) };
        }
    }

    public static IEnumerable<object[]> IsKingAttackedByCannonTestData
    {
        get
        {
            yield return new object[] { new IsKingAttackedTestData("2bakab2/c8/2n3n2/p1p2P2p/6p2/2P6/3rN1P1P/4C4/R5C2/2BAKAB2 b - - 2 21", new Coordinate(5, 10), ExpectedResult: true) };
            yield return new object[] { new IsKingAttackedTestData("2baka3/c8/2n1b1n2/p1p2P2p/6p2/2P6/3r2P1P/4C1N2/R5C2/2BAKAB2 b - - 4 22", new Coordinate(5, 10), ExpectedResult: true) };
            yield return new object[] { new IsKingAttackedTestData("1r2kabr1/4a4/2n1b4/C1R5p/4C1pn1/2P6/Pc2P3P/2N3N2/9/1RBAKAc2 w - - 0 14", new Coordinate(5, 1), ExpectedResult: true) };
           
            yield return new object[] { new IsKingAttackedTestData("2baka3/c8/2n1b1n2/p1p2P2p/6p2/2P6/3rN1P1P/4C4/R5C2/2BAKAB2 w - - 3 21", new Coordinate(5, 10), ExpectedResult: false) };
            yield return new object[] { new IsKingAttackedTestData("2baka3/4c4/2n1b1n2/p1p2P2p/6p2/2P6/3r2P1P/4C1N2/R5C2/2BAKAB2 w - - 5 22", new Coordinate(5, 1), ExpectedResult: false) };
            yield return new object[] { new IsKingAttackedTestData("2baka3/4c4/2n1b1n2/p1p2P2p/6p2/2P6/3r2P1P/4C1N2/R5C2/2BAKAB2 w - - 5 22", new Coordinate(5, 10), ExpectedResult: false) };
           
        }
    }

    public static IEnumerable<object[]> IsKingAttackedByKnightTestData
    {
        get
        {
            yield return new object[] { new IsKingAttackedTestData("3ak1b2/4a4/4b4/p1p6/7P1/2P1C1B2/Pc2P1P2/2N1B1n1N/4A3c/3A1K3 w - - 3 28", new Coordinate(6, 1), ExpectedResult: true) };
            yield return new object[] { new IsKingAttackedTestData("1rbakabr1/9/n4Nn2/p1p1p3p/9/6P2/P1P1c3P/c4C2N/9/1RBAKAB1R b - - 1 10", new Coordinate(5, 10), ExpectedResult: true) };
            yield return new object[] { new IsKingAttackedTestData("2ba1abr1/4k4/n5n2/p1pNp3p/9/6P2/P1P1c3P/B4C2N/9/1r1AKAB1R b - - 1 12", new Coordinate(5, 9), ExpectedResult: true) };
            yield return new object[] { new IsKingAttackedTestData("1R3Cb2/3ka4/2c1b4/p1p3p1p/3r2nc1/2P1P2C1/P5P1P/N3BR3/3rA1n2/2BAK2N1 w - - 1 20", new Coordinate(5, 1), ExpectedResult: true) };
            yield return new object[] { new IsKingAttackedTestData("3k5/9/9/9/9/9/9/4K4/9/3n5 w - - 0 0", new Coordinate(5, 3), ExpectedResult: true) };
            yield return new object[] { new IsKingAttackedTestData("3k5/9/9/9/9/4n4/9/5K3/9/9 w - - 0 0", new Coordinate(6, 3), ExpectedResult: true) };
            yield return new object[] { new IsKingAttackedTestData("3k5/9/9/9/9/9/6n2/9/5K3/9 w - - 0 0", new Coordinate(6, 2), ExpectedResult: true) };

            yield return new object[] { new IsKingAttackedTestData("2baka3/c8/2n1b1n2/p1p2P2p/6p2/2P6/3rN1P1P/4C4/R5C2/2BAKAB2 w - - 3 21", new Coordinate(5, 10), ExpectedResult: false) };
            yield return new object[] { new IsKingAttackedTestData("2baka3/4c4/2n1b1n2/p1p2P2p/6p2/2P6/3r2P1P/4C1N2/R5C2/2BAKAB2 w - - 5 22", new Coordinate(5, 1), ExpectedResult: false) };
            yield return new object[] { new IsKingAttackedTestData("2baka3/4c4/2n1b1n2/p1p2P2p/6p2/2P6/3r2P1P/4C1N2/R5C2/2BAKAB2 w - - 5 22", new Coordinate(5, 10), ExpectedResult: false) };
            yield return new object[] { new IsKingAttackedTestData("1R3Cb2/3ka4/2c1b4/p1p3p1p/3r2nc1/2P1P2C1/P5P1P/N3BR3/3rANn2/2BAK4 b - - 2 21", new Coordinate(5, 1), ExpectedResult: false) };
            yield return new object[] { new IsKingAttackedTestData("3k5/9/9/9/9/4n4/4C4/5K3/9/9 w - - 0 0", new Coordinate(6, 3), ExpectedResult: false) };

        }
    }

    public static IEnumerable<object[]> IsKingAttackedByPawnTestData
    {
        get
        {
            yield return new object[] { new IsKingAttackedTestData("4k4/9/9/9/9/9/9/9/3p5/3K5 w - - 0 0", new Coordinate(4, 1), ExpectedResult: true) };
            yield return new object[] { new IsKingAttackedTestData("4k4/9/9/9/9/9/9/9/2pK5/9 w - - 0 0", new Coordinate(4, 2), ExpectedResult: true) };
            yield return new object[] { new IsKingAttackedTestData("4kP3/9/9/9/9/9/9/9/5K3/9 w - - 0 0", new Coordinate(5, 10), ExpectedResult: true) };

            yield return new object[] { new IsKingAttackedTestData("9/9/3k5/9/9/9/9/4K4/4p4/9 w - - 0 0", new Coordinate(5, 3), ExpectedResult: false) };
            yield return new object[] { new IsKingAttackedTestData("9/3P5/3k5/9/9/9/9/9/9/5K3 w - - 0 0", new Coordinate(4, 8), ExpectedResult: false) };
            yield return new object[] { new IsKingAttackedTestData("3k5/9/9/9/9/9/9/4p4/1C1p1p3/4K2C1 w - - 0 0", new Coordinate(5, 1), ExpectedResult: false) };
        }
    }

    [Theory]
    [InlineData("rnbakab1r/9/1c4nc1/p1p1p1p1p/9/9/P1P1P1P1P/1C2C4/9/RNBAKABNR w - - 2 1", 1, 9)]
    [InlineData("4kab2/cR2a2r1/2n1b2c1/p1p1p2Rp/5np2/3r5/P1P1P3P/N2C2N2/4AC3/2BAK1B2 w - - 12 14", 8, 3)]
    public static void ShouldReturnCorrectPiecesInOrder_WhenCallingGetPiecesOnRow(string fen, int rowNumber, int piecesCountOnRow)
    {
        // Arrange
        XiangqiBuilder builder = new ();
        XiangqiGame game = builder.UseCustomFen(fen).Build();

        Piece[,] position = game.BoardPosition;

        // Act
        IEnumerable<Piece> result = position.GetPiecesOnRow(rowNumber);

        // Assert
        result.Count().Should().Be(piecesCountOnRow);

        foreach (Piece piece in result)
        {
            piece.Should().NotBeNull();
            
            Coordinate pieceCoordinate = piece.Coordinate;

            position.GetPieceAtPosition(pieceCoordinate).Should().BeEquivalentTo(piece);
        }
    }

    [Theory]
    [MemberData(nameof(CountPiecesBetweenTwoCoordinatesOnRowTestData))]
    public static void ShouldReturnCorrectNumberOfPiecesBetweenTwoCoordinates_WhenProviingTwoCoordinatesOnTheSameRow(CountPiecesBetweenTwoCoordinatesTestData testData)
    {
        // Arrange
        string startingFen = testData.StartingFen;
        Coordinate startingPosition = testData.StartingPosition;
        Coordinate destination = testData.Destination;
        XiangqiBuilder builder = new ();

        XiangqiGame game = builder
                            .UseCustomFen(startingFen)
                            .Build();

        Piece[,] position = game.BoardPosition;
        // Act
        int result = position.CountPiecesBetweenOnRow(startingPosition, destination);

        int expectedNumberOfPieces = testData.ExpectedResult;

        // Assert
        result.Should().Be(expectedNumberOfPieces);
    }

    [Theory]
    [MemberData(nameof(CountPiecesBetweenTwoCoordinatesOnColumnTestData))]
    public static void ShouldReturnCorrectNumberOfPiecesBetweenTwoCoordinates_WhenProviingTwoCoordinatesOnTheSameColumn (CountPiecesBetweenTwoCoordinatesTestData testData)
    {
        // Arrange
        string startingFen = testData.StartingFen;
        Coordinate startingPosition = testData.StartingPosition;
        Coordinate destination = testData.Destination;
        XiangqiBuilder builder = new ();

        XiangqiGame game = builder
                            .UseCustomFen(startingFen)
                            .Build();

        Piece[,] position = game.BoardPosition;
        // Act
        int result = position.CountPiecesBetweenOnColumn(startingPosition, destination);

        int expectedNumberOfPieces = testData.ExpectedResult;

        // Assert
        result.Should().Be(expectedNumberOfPieces);
    }

    [Theory]
    [MemberData(nameof(WillExposeKingToDangerTestData))]
    public static void WillExposeKingToDanger_ShouldReturnCorrectResult(WillExposeKingToDangerTestData willExposeKingToDangerTestData)
    {
        // Arrange
        XiangqiBuilder builder = new();
        string startingFen = willExposeKingToDangerTestData.StartingFen;
        Coordinate startingPosition = willExposeKingToDangerTestData.StartingPosition;
        Coordinate destination = willExposeKingToDangerTestData.Destination;
        bool expectedResult = willExposeKingToDangerTestData.ExpectedResult;
        XiangqiGame game = builder
                            .UseCustomFen(startingFen)
                            .Build();

        Piece[,] boardPosition = game.BoardPosition;
        // Act
        bool actualResult = boardPosition.WillExposeKingToDanger(startingPosition, destination);

        // Assert
        actualResult.Should().Be(expectedResult);
    }

    [Theory]
    [MemberData(nameof(IsKingAttackedByRookTestData))]
    public static void IsKingAttackedByRook_ShouldReturnExpectedResult(IsKingAttackedTestData testData)
    {
        // Arrange
        XiangqiBuilder builder = new();
        string startingFen = testData.StartingFen;
        bool expectedResult = testData.ExpectedResult;
        Coordinate kingCoordinate = testData.KingCoordinate;

        XiangqiGame game = builder
                            .UseCustomFen(startingFen)
                            .Build();

        Piece[,] boardPosition = game.BoardPosition;

        // Act
        bool actualResult = boardPosition.IsKingAttackedBy<Rook>(kingCoordinate);

        // Assert
        actualResult.Should().Be(expectedResult);
    }

    [Theory]
    [MemberData(nameof(IsKingExposedToOpponentKingTestData))]
    public static void IsKingExposedToOpponentKing_ShouldReturnExpectedResult(IsKingAttackedTestData testData)
    {
        // Arrange
        XiangqiBuilder builder = new();
        string startingFen = testData.StartingFen;
        bool expectedResult = testData.ExpectedResult;
        Coordinate kingCoordinate = testData.KingCoordinate;

        XiangqiGame game = builder
                            .UseCustomFen(startingFen)
                            .Build();

        Piece[,] boardPosition = game.BoardPosition;

        // Act
        bool actualResult = boardPosition.IsKingExposedDirectlyToEnemyKing(kingCoordinate);

        // Assert
        actualResult.Should().Be(expectedResult);
    }

    [Theory]
    [MemberData(nameof(IsKingAttackedByCannonTestData))]
    public static void IsKingAttackedByCannon_ShouldReturnExpectedResult(IsKingAttackedTestData testData)
    {
        // Arrange
        XiangqiBuilder builder = new();
        string startingFen = testData.StartingFen;
        bool expectedResult = testData.ExpectedResult;
        Coordinate kingCoordinate = testData.KingCoordinate;

        XiangqiGame game = builder
                            .UseCustomFen(startingFen)
                            .Build();

        Piece[,] boardPosition = game.BoardPosition;

        // Act
        bool actualResult = boardPosition.IsKingAttackedBy<Cannon>(kingCoordinate);

        // Assert
        actualResult.Should().Be(expectedResult);
    }

    [Theory]
    [MemberData(nameof(IsKingAttackedByKnightTestData))]
    public static void IsKingAttackedByKnight_ShouldReturnExpectedResult(IsKingAttackedTestData testData)
    {
        // Arrange
        XiangqiBuilder builder = new();
        string startingFen = testData.StartingFen;
        bool expectedResult = testData.ExpectedResult;
        Coordinate kingCoordinate = testData.KingCoordinate;

        XiangqiGame game = builder
                            .UseCustomFen(startingFen)
                            .Build();

        Piece[,] boardPosition = game.BoardPosition;

        // Act
        bool actualResult = boardPosition.IsKingAttackedBy<Knight>(kingCoordinate);

        // Assert
        actualResult.Should().Be(expectedResult);
    }

    [Theory]
    [MemberData(nameof(IsKingAttackedByPawnTestData))]
    public static void IsKingAttackedByPawn_ShouldReturnExpectedResult(IsKingAttackedTestData testData)
    {
        // Arrange
        XiangqiBuilder builder = new();
        string startingFen = testData.StartingFen;
        bool expectedResult = testData.ExpectedResult;
        Coordinate kingCoordinate = testData.KingCoordinate;

        XiangqiGame game = builder
                            .UseCustomFen(startingFen)
                            .Build();

        Piece[,] boardPosition = game.BoardPosition;

        // Act
        bool actualResult = boardPosition.IsKingAttackedBy<Pawn>(kingCoordinate);

        // Assert
        actualResult.Should().Be(expectedResult);
    }
}

public record CountPiecesBetweenTwoCoordinatesTestData(string StartingFen, Coordinate StartingPosition, Coordinate Destination, int ExpectedResult);

public record WillExposeKingToDangerTestData(string StartingFen, Coordinate StartingPosition, Coordinate Destination, bool ExpectedResult);

public record IsKingAttackedTestData(string StartingFen, Coordinate KingCoordinate, bool ExpectedResult);
