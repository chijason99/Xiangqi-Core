using xiangqi_core_test.XiangqiCore.MoveParserTest;

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
}

public record MoveMethodTestData(string StartingFen, Coordinate StartingPosition, Coordinate Destination, bool ExpectedResult);