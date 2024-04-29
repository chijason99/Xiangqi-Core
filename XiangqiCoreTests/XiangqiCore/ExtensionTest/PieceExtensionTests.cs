using XiangqiCore.Extension;

namespace xiangqi_core_test.XiangqiCore.ExtensionTest;
public static class PieceExtensionTests
{
    private const string _startingFen1 = "1n2kab2/4a4/2c1b1c1n/p5p2/9/2C3P2/r3P2RP/N3BCN2/4A4/4KAB2 b - - 0 16";
    private const string _startingFen2 = "4kab2/3na4/2c1b1c1n/p5p2/9/2C2NP2/r3P2RP/N3BC3/4A4/4KAB2 b - - 2 17";

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
}

public record CountPiecesBetweenTwoCoordinatesTestData(string StartingFen, Coordinate StartingPosition, Coordinate Destination, int ExpectedResult);
