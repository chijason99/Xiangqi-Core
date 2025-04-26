# Dependency Injection

The Xiangqi-Core library provides built-in support for dependency injection, making it easy to integrate its services into your .NET applications. By using the `AddXiangqiCore` extension method, you can register all the core services required for Xiangqi application development with a single line of code.

## Overview

The `AddXiangqiCore` method is an extension method for `IServiceCollection` that registers the following services with their default implementations:

### Registered Services

| Service Interface                  | Default Implementation            | Description                                                                 |
|------------------------------------|------------------------------------|-----------------------------------------------------------------------------|
| `IImageGenerationService`          | `DefaultImageGenerationService`   | Generates images of the Xiangqi game board.                                |
| `IPgnGenerationService`            | `DefaultPgnGenerationService`     | Generates PGN strings for game records.                                    |
| `IMoveParsingService`              | `DefaultMoveParsingService`       | Parses move notations and game records.                                    |
| `IMoveTranslationService`          | `DefaultMoveTranslationService`   | Translates move notations between different languages and formats.         |
| `IImageSavingService`              | `DefaultImageSavingService`       | Saves generated images to files.                                           |
| `IGifSavingService`                | `DefaultGifSavingService`         | Saves generated GIFs to files.                                             |
| `IGifGenerationService`            | `DefaultGifGenerationService`     | Generates animated GIFs of the game board.                                 |
| `IPgnSavingService`                | `DefaultPgnSavingService`         | Saves PGN strings to files.                                                |
| `IXiangqiBuilder`                  | `XiangqiBuilder`                  | Provides a fluent API for configuring and initializing Xiangqi game instances. |

---

## How to Use

To use Xiangqi-Core with dependency injection, call the `AddXiangqiCore` method on your `IServiceCollection` during application startup. This will register all the necessary services with their default implementations.

### Example: ASP.NET Core
```c#
var builder = WebApplication.CreateBuilder(args);

// Add Xiangqi-Core services to the DI container 
builder.Services.AddXiangqiCore();

var app = builder.Build();

app.Run();
```

---