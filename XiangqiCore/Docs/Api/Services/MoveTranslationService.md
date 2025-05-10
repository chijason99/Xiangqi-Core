### Move Translation Service

The Move Translation Service in Xiangqi-Core provides tools for translating move notations between different languages and formats. It supports multiple notation types, including Traditional Chinese, Simplified Chinese, English, and UCCI, making it versatile for applications that need to handle multilingual or cross-format game data.

## Overview

The Move Translation Service includes:
- **`IMoveTranslationService`**: An interface that defines the core method for translating move notations.
- **`DefaultMoveTranslationService`**: The default implementation of the `IMoveTranslationService` interface, which uses specialized translators for each notation type.

This service is essential for applications that need to display or process move notations in different languages or formats.

---

## `IMoveTranslationService`

The `IMoveTranslationService` interface provides a set of APIs for translating move notations between different languages and formats.

### Public Methods

#### `TranslateMove(MoveHistoryObject move, MoveNotationType notationType)`
Translates a move notation to the specified notation type.

**Parameters**:
- `move` (MoveHistoryObject): The move history object containing the details of the move to be translated.
- `notationType` (MoveNotationType): The target notation type to which the move should be translated (e.g., Traditional Chinese, Simplified Chinese, English, or UCCI).

**Return Value**:
- Returns a `string` containing the translated move notation.

**Example**:
```c#
XiangqiBuilder builder = new (); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();

game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);

IMoveTranslationService moveTranslationService = new DefaultMoveTranslationService();
string translatedMove = moveTranslationService.TranslateMove(game.MoveHistory[0], MoveNotationType.English);

Console.WriteLine(translatedMove);

// Output: 
// C2=5
```

---

## Default Implementation

The `DefaultMoveTranslationService` is the default implementation of the `IMoveTranslationService` interface. It uses specialized translators for each notation type:

- **Traditional Chinese**: `TraditionalChineseNotationTranslator`
- **Simplified Chinese**: `SimplifiedChineseNotationTranslator`
- **English**: `EnglishNotationTranslator`
- **UCCI**: `UcciNotationTranslator`

### How It Works

1. **`TranslateMove`**:
   - Delegates the translation task to the appropriate translator based on the `MoveNotationType`.
   - Returns the translated move notation as a string.

### Example
```c#
XiangqiBuilder builder = new (); 
XiangqiGame game = builder.WithDefaultConfiguration().Build();

// Make a move in Traditional Chinese notation 
game.MakeMove("炮二平五", MoveNotationType.TraditionalChinese);

// Initialize the move translation service 
IMoveTranslationService moveTranslationService = new DefaultMoveTranslationService();

// Translate the move to English notation 
string translatedMove = moveTranslationService.TranslateMove(game.MoveHistory[0], MoveNotationType.English);

Console.WriteLine($"Translated Move: {translatedMove}");

// Output: 
// Translated Move: C2=5
```


---

### Summary

The Move Translation Service in Xiangqi-Core provides robust tools for translating move notations between different languages and formats. It supports multiple notation types and ensures accurate translations, making it an essential component for applications that need to handle multilingual or cross-format game data.