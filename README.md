# Xiangqi-Core NuGet Package

## Why Xiangqi-Core?

Xiangqi-Core is the ultimate library for building Xiangqi (Chinese Chess) applications in C#. Whether you're creating a learning tool, or a visualization platform, Xiangqi-Core provides everything you need to bring your ideas to life. Designed with performance, flexibility, and developer experience in mind, it’s the go-to solution for Xiangqi enthusiasts and professionals alike.

---

## Key Features

- **Fluent API**: Effortlessly configure and initialize game instances with a developer-friendly API.
- **Game State Management**: Manage piece positions, turn tracking, and game outcomes with ease.
- **Move Parsing & Validation**: Parse and validate moves in UCCI, Traditional Chinese, Simplified Chinese, and English notations.
- **Image & GIF Generation**: Create stunning, customizable images and GIFs of the game board for visual representation.
- **PGN Support**: Generate and save PGN files for game records, or import games from dpxq.com.
- **Move Translation**: Seamlessly translate moves between different notations.
- **Randomized Board Positions**: Generate random board setups for training or analysis.
- **Dependency Injection Ready**: Integrate seamlessly into modern .NET applications.

---

## Why Developers Love Xiangqi-Core

- **Comprehensive**: Covers all aspects of Xiangqi, from move generation to game visualization.
- **Customizable**: Tailor the library to your needs with flexible configurations and extensible APIs.
- **Battle-Tested**: Built with .NET 9, ensuring top-notch performance and compatibility.
- **Developer-Friendly**: Clear documentation, intuitive APIs, and support for dependency injection make development a breeze.

---

## Get Started

Install Xiangqi-Core via NuGet:
```bash
dotnet add package Xiangqi-Core
```

### Example Usage
```c#
XiangqiBuilder builder = new(); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();

game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese); 
game.MakeMove("馬8進7", MoveNotationType.TraditionalChinese);

Console.WriteLine(game.CurrentFen); 

// Outputs the current board state in FEN notation
// rnbakab1r/9/1c4nc1/p1p1p1p1p/9/9/P1P1P1P1P/1C2C4/9/RNBAKABNR w - - 2 1
```

---

For more examples/documentations for the API, please refer to the Docs folder

## Contributing

Contributions are very welcome! If you have ideas for improvements or bug fixes, feel free to fork the repository and submit a pull request.

---

## License

Xiangqi-Core is licensed under the MIT License. See the [LICENSE](./LICENSE) file for more details.

---

## Contact

Have questions or need support? Reach out to me at **chijason99@gmail.com**.

---

Start building your Xiangqi application today with Xiangqi-Core – the most comprehensive and developer-friendly library for Chinese Chess in C#.