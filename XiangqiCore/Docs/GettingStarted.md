## Getting Started

To get started with Xiangqi-Core, first import the package into your project:

Here's a simple example of setting up a game board and making a move:

```c#
using XiangqiCore.Game;

// Create a new game instance with the help of the XiangqiBuilder
XiangqiBuilder builder = new (); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();
	
// Make a move
game.MakeMove("ÅÚ¶þÆ½Îå", MoveNotationType.Chinese);
```