using xiangqi_core_test.XiangqiCore.MoveParserTest;
using XiangqiCore.Move.Move;

namespace xiangqi_core_test.XiangqiCore.XiangqiGameTest;
public static class XiangqiGameTests
{
    private const string _startingFen1 = MoveParserTestHelper.StartingFen1;

    public static IEnumerable<object []> MoveMethodWithCoordinatesTestData
    {
        get
        {
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(2, 10), new(2, 4), ExpectedResult: true) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(7, 4), new(7, 1), ExpectedResult: true) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(7, 8), new(6, 6), ExpectedResult: true) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(3, 7), new(3, 6), ExpectedResult: true) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(5, 8), new(3, 10), ExpectedResult: true) };

            yield return new object[] { new MoveMethodTestData(_startingFen1, new(5, 10), new(4, 10), ExpectedResult: false) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(8, 10), new(7, 9), ExpectedResult: false) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(5, 9), new(6, 9), ExpectedResult: false) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(7, 4), new(7, 8), ExpectedResult: false) };
            yield return new object[] { new MoveMethodTestData(_startingFen1, new(4, 5), new(3, 7), ExpectedResult: false) };
        }
    }


    [Theory]
    [MemberData(nameof(MoveMethodWithCoordinatesTestData))]
    public static void MoveMethod_ShouldAlterTheBoardCorrectly(MoveMethodTestData testData)
    {
        // Arrange
        XiangqiBuilder builder = new ();

        XiangqiGame game = builder
                            .UseCustomFen(testData.StartingFen)
                            .Build();

        bool expectedResult = testData.ExpectedResult;
        
        // Act
        bool actualResult = game.Move(testData.StartingPosition, testData.Destination);

        // Assert
        actualResult.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 0", "2baka3/9/1cn1bc1n1/pC2p1p1N/2p6/6P2/P1P1P3P/2N5C/9/2BAKAB2 w - - 0 12", "1. 馬八進七  卒３進１    2. 兵三進一  馬２進３\r\n  3. 馬二進三  車１進１    4. 炮二平一  象７進５\r\n  5. 車一平二  炮８平６    6. 車九進一  車１平７\r\n  7. 馬三進二  車７平８    8. 車九平二  馬８進６\r\n  9. 馬二進一  車８進７   10. 車二進一  車９進２\r\n 11. 炮八進四  車９平８   12. 車二進六  馬６進８")]
    [InlineData("2baka2r/5n3/1cn1bc3/p3p1p1N/2p6/6P2/P1P1P3P/1CN5C/7R1/2BAKAB2 b - - 0 10", "2bak4/4a4/1c2b4/4C3N/C5P2/2p3n2/P1n1Pc2P/2N1B4/4A4/4KAB2 b - - 4 20", "10. 車二進一  車９進２\r\n 11. 炮八進四  車９平８   12. 車二進六  馬６進８\r\n 13. 炮八平三  馬３進４   14. 炮三平九  炮６進４\r\n 15. 炮九退一  馬４進３   16. 兵三進一  馬８進６\r\n 17. 炮一平五  馬６進７   18. 炮五進四  士６進５\r\n 19. 仕六進五  卒３進１   20. 相七進五")]
    [InlineData("2bak4/4a4/1c2b4/4C1N2/C5P2/3p1c3/P1n1P3P/2N1B2n1/4A4/4KAB2 w - - 9 22", "2bak4/4a4/c3b4/C8/5P3/2B6/Pp6P/3A5/N5n2/3K1AB2 w - - 0 33", "22. 馬二進三  馬７進８\r\n 23. 兵三平四  馬８進９   24. 馬三退四  馬９退７\r\n 25. 帥五平六  炮２平４   26. 仕五進六  卒４平５\r\n 27. 炮五平六  卒５平６   28. 相五進七  卒６進１\r\n 29. 炮九平七  卒６平５   30. 炮七退二  卒５平４\r\n 31. 炮七平八  卒４平３   32. 馬七退九  炮４平１\r\n 33. 炮六平九  卒３平２")]
    public static void MoveMethodShouldCreateCorrectMoveHistory_WhenProvidedGameNotation(string startingFen, string finalFen, string gameRecord)
    {
        // Arrange
        XiangqiBuilder builder = new();

        XiangqiGame game = builder
                            .UseCustomFen(startingFen)
                            .Build();

        List<string> moves = GameRecordParser.Parse(gameRecord);

        // Act
        foreach (string move in moves)
            game.Move(move, MoveNotationType.Chinese);

        // Assert
        game.CurrentFen.Should().Be(finalFen);
    }
}

public record MoveMethodTestData(string StartingFen, Coordinate StartingPosition, Coordinate Destination, bool ExpectedResult);