# Xiangqi-Core Nuget Package

## Description
Xiangqi-Core is a comprehensive library designed to facilitate the development of applications related to Xiangqi (Chinese Chess). It provides a robust set of functionalities including move generation, move validation, game state management, etc. Built with flexibility and performance in mind, XiangqiCore aims to be the go-to solution for developers looking to integrate Xiangqi mechanics into their software.

## Features

- **Fluent API**: Provides a fluent API for easy configuration and initialization of game instances.
- **Game State Management**: Easily manage game states, including piece positions, turn tracking, and game outcome detection.
- **Parsing of Move Notations**: Supports parsing of move notations in UCCI, Chinese, and English, allowing for versatile game command inputs.
- **Move Validation**: Validate player moves, ensuring moves adhere to the rules of Xiangqi.
- **Utility Functions**: A collection of utility functions for piece and board management, including piece movement simulation and position checking.


## Installation

Xiangqi-Core is available as a NuGet package. You can install it using the NuGet Package Manager or the dotnet CLI.

`dotnet add package Xiangqi-Core`

## Usage

To get started with Xiangqi-Core, first import the package into your project:

```using XiangqiCore;```

Here's a simple example of setting up a game board and making a move:

```
// Create a new game instance with the help of the XiangqiBuilder using fluent API
XiangqiBuilder builder = new (); 
XiangqiGame game = builder.UseDefaultConfiguration().Build();
	
// Make a move
game.MakeMove("ÅÚ¶þÆ½Îå", MoveNotationType.Chinese);
```
Refer to the documentation for more detailed examples and usage instructions.

## Contributing

Contributions to Xiangqi-Core are welcome! If you have suggestions for improvements or bug fixes, please feel free to fork the repository and submit a pull request.

## License

Xiangqi-Core is licensed under the MIT License. See the LICENSE file for more details.

## Contact

For questions or support, please contact chijason99@gmail.com