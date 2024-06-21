namespace XiangqiCoreTests.XiangqiCore
{
    public static class GameRecordParserTests
    {
        [Fact]
        public static void Parse_ShouldReturnEmptyList_WhenGivenEmptyString()
        {
            // Arrange
            string gameRecordString = "";

            // Act
            List<string> result = GameRecordParser.Parse(gameRecordString);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public static void Parse_ShouldReturnMoveSets_WhenGivenValidGameRecordString()
        {
            // Arrange
            string gameRecordString = "1. 馬八進七  卒３進１    2. 兵三進一  馬２進３\r\n  3. 馬二進三  車１進１";

            // Act
            List<string> result = GameRecordParser.Parse(gameRecordString);

            // Assert
            Assert.Equal(6, result.Count);
        }

        [Fact]
        public static void Parse_ShouldReturnMoveSets_WhenGivenGameRecordStringWithExtraWhitespace()
        {
            // Arrange
            string gameRecordString = "  1. 馬八進七  卒３進１    2. 兵三進一  馬２進３\r\n  3. 馬二進三  車１進１  ";

            // Act
            List<string> result = GameRecordParser.Parse(gameRecordString);

            // Assert
            Assert.Equal(6, result.Count);
        }

        [Fact]
        public static void Parse_ShouldReturnMoveSets_WhenGivenGameRecordStringWithDifferentNotationStyles()
        {
            // Arrange
            string gameRecordString = "1. 馬八進七  卒３進１    2. 兵３进１  马２进３\r\n  3. 马二进三  车１进１";

            // Act
            List<string> result = GameRecordParser.Parse(gameRecordString);

            // Assert
            Assert.Equal(6, result.Count);
        }
    }
}
