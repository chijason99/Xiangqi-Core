using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using XiangqiCore.Misc;
using XiangqiCore.Misc.Images;
using XiangqiCore.Services.GifSaving;
using XiangqiCore.Services.ImageGeneration;

namespace xiangqi_core_test.XiangqiCore.FileGenerationTest;

public class CustomThemeExportTests
{
    private static readonly string[] PieceFileNames =
    [
        "black_advisor.png",
        "black_bishop.png",
        "black_cannon.png",
        "black_king.png",
        "black_knight.png",
        "black_pawn.png",
        "black_rook.png",
        "red_advisor.png",
        "red_bishop.png",
        "red_cannon.png",
        "red_king.png",
        "red_knight.png",
        "red_pawn.png",
        "red_rook.png",
    ];

    [Fact]
    public void GenerateImage_UsesCustomThemeAssetsAndLayout()
    {
        string tempDirectory = FileHelper.CreateTempDirectory("CustomThemeImageTests");

        try
        {
            (ImageConfig imageConfig, _, _) = CreateThemeFiles(tempDirectory);

            IImageGenerationService imageGenerationService = new DefaultImageGenerationService();
            byte[] imageBytes = imageGenerationService.GenerateImage("4k4/9/9/9/9/9/9/9/9/4K4 w - - 0 1", imageConfig: imageConfig);

            using Image<Rgba32> image = Image.Load<Rgba32>(imageBytes);
            Assert.Equal(90, image.Width);
            Assert.Equal(100, image.Height);
            Assert.Equal(new Rgba32(245, 222, 179), image[5, 5]);
            Assert.Equal(new Rgba32(20, 20, 20), image[45, 5]);
            Assert.Equal(new Rgba32(200, 30, 30), image[45, 95]);
        }
        finally
        {
            Directory.Delete(tempDirectory, true);
        }
    }

    [Fact]
    public void GenerateGif_UsesCustomThemeBoardSize()
    {
        string tempDirectory = FileHelper.CreateTempDirectory("CustomThemeGifTests");

        try
        {
            (ImageConfig imageConfig, _, string gifPath) = CreateThemeFiles(tempDirectory);

            IGifSavingService gifSavingService = new DefaultGifSavingService();
            gifSavingService.Save(
                gifPath,
                new[]
                {
                    "4k4/9/9/9/9/9/9/9/9/4K4 w - - 0 1",
                    "9/4k4/9/9/9/9/9/9/9/4K4 w - - 0 1",
                },
                imageConfig);

            using Image<Rgba32> image = Image.Load<Rgba32>(gifPath);
            Assert.Equal(90, image.Width);
            Assert.Equal(100, image.Height);
        }
        finally
        {
            Directory.Delete(tempDirectory, true);
        }
    }

    private static (ImageConfig ImageConfig, string BoardPath, string GifPath) CreateThemeFiles(string tempDirectory)
    {
        string boardPath = Path.Combine(tempDirectory, "board.png");
        string indicatorPath = Path.Combine(tempDirectory, "move_indicator.png");
        string gifPath = Path.Combine(tempDirectory, "themed.gif");

        CreateSolidPng(boardPath, 90, 100, new Rgba32(245, 222, 179));
        CreateSolidPng(indicatorPath, 10, 10, new Rgba32(0, 180, 0));

        Dictionary<string, string> piecePaths = new(StringComparer.OrdinalIgnoreCase);
        foreach (string pieceFileName in PieceFileNames)
        {
            bool isRedPiece = pieceFileName.StartsWith("red_", StringComparison.OrdinalIgnoreCase);
            string piecePath = Path.Combine(tempDirectory, pieceFileName);
            CreateSolidPng(piecePath, 10, 10, isRedPiece ? new Rgba32(200, 30, 30) : new Rgba32(20, 20, 20));
            piecePaths[Path.GetFileNameWithoutExtension(pieceFileName)] = piecePath;
        }

        ImageConfig imageConfig = new()
        {
            BoardWidth = 90,
            BoardHeight = 100,
            SquareOriginX = 0,
            SquareOriginY = 0,
            SquareStepX = 10,
            SquareStepY = 10,
            PieceWidth = 10,
            PieceHeight = 10,
            MoveIndicatorWidth = 10,
            MoveIndicatorHeight = 10,
            CustomBoardImagePath = boardPath,
            CustomMoveIndicatorImagePath = indicatorPath,
            CustomPieceImagePaths = piecePaths,
            UseMoveIndicator = true,
            FrameDelayInSecond = 1,
        };

        return (imageConfig, boardPath, gifPath);
    }

    private static void CreateSolidPng(string filePath, int width, int height, Rgba32 color)
    {
        using Image<Rgba32> image = new(width, height);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                image[x, y] = color;
            }
        }

        image.SaveAsPng(filePath);
    }
}
