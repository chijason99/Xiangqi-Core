using XiangqiCore.Extension;

namespace xiangqi_core_test.XiangqiCore.ExtensionTest;
public static class PieceExtensionTests
{
    [Theory]
    [InlineData("rnbakab1r/9/1c4nc1/p1p1p1p1p/9/9/P1P1P1P1P/1C2C4/9/RNBAKABNR w - - 2 1", 1, 9)]
    [InlineData("4kab2/cR2a2r1/2n1b2c1/p1p1p2Rp/5np2/3r5/P1P1P3P/N2C2N2/4AC3/2BAK1B2 w - - 12 14", 8, 3)]
    public static void ShouldReturnCorrectPiecesInOrder_WhenCallingGetPiecesOnRow(string fen, int rowNumber, int piecesCountOnRow)
    {
        // Arrange
        XiangqiBuilder builder = new ();
        XiangqiGame game = builder.UseCustomFen(fen).Build();

        Piece[,] position = game.GetBoardPosition;

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
}
