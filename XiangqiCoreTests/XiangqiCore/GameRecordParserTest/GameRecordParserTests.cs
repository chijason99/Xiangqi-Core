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
            string gameRecordString = "1. �R���M��  �䣳�M��    2. �����Mһ  �R���M��\r\n  3. �R���M��  ܇���M��";

            // Act
            List<string> result = GameRecordParser.Parse(gameRecordString);

            // Assert
            Assert.Equal(6, result.Count);
        }

        [Fact]
        public static void Parse_ShouldReturnMoveSets_WhenGivenGameRecordStringWithExtraWhitespace()
        {
            // Arrange
            string gameRecordString = "  1. �R���M��  �䣳�M��    2. �����Mһ  �R���M��\r\n  3. �R���M��  ܇���M��  ";

            // Act
            List<string> result = GameRecordParser.Parse(gameRecordString);

            // Assert
            Assert.Equal(6, result.Count);
        }

        [Fact]
        public static void Parse_ShouldReturnMoveSets_WhenGivenGameRecordStringWithDifferentNotationStyles()
        {
            // Arrange
            string gameRecordString = "1. �R���M��  �䣳�M��    2. ��������  ������\r\n  3. �������  ��������";

            // Act
            List<string> result = GameRecordParser.Parse(gameRecordString);

            // Assert
            Assert.Equal(6, result.Count);
        }
    }
}
