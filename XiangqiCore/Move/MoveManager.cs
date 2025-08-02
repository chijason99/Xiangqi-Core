using XiangqiCore.Boards;
using XiangqiCore.Move.Commands;
using XiangqiCore.Move.MoveObjects;

namespace XiangqiCore.Move;

/// <summary>
/// A class responsible for managing moves in the game of Xiangqi (Chinese Chess).
/// </summary>
public class MoveManager 
{
    private readonly MoveCommandInvoker _moveCommandInvoker;
    
    // Inject in the board to allow changing the board state during moves navigation.
    private readonly Board _board;
    
    public MoveManager(
        MoveCommandInvoker moveCommandInvoker, 
        MoveHistoryObject rootMoveHistoryObject,
        Board board) 
    {
        RootMove = new MoveNode(rootMoveHistoryObject);
        CurrentMove = RootMove;
        _moveCommandInvoker = moveCommandInvoker 
            ?? throw new ArgumentNullException(nameof(moveCommandInvoker), "MoveCommandInvoker cannot be null");
        _board = board 
            ?? throw new ArgumentNullException(nameof(board), "Board cannot be null");
    }
    
    /// <summary>
    /// The root move of the game, representing the initial state before any moves are made.
    /// </summary>
    public MoveNode RootMove { get; init; }
    
    /// <summary>
    /// The current move being played in the game.
    /// </summary>
    public MoveNode CurrentMove { get; private set; }
    
    /// <summary>
    ///  Retrieves the list of move histories in the game.
    ///  By default, it returns the main line (the first variation) if any variations exists.
    /// </summary>
    public List<MoveHistoryObject> GetMoveHistory()
    {
        List<MoveHistoryObject> results = [];
        var currentNode = RootMove;
        
        while (currentNode is not null)
        {
            results.Add(currentNode.MoveHistoryObject);
            currentNode = currentNode.Variations.FirstOrDefault();
        }

        return results;
    }
    
    /// <summary>
    /// Adds a move to the move history. If the move is a variation of the current move, it will be added as a variation.
    /// </summary>
    public MoveHistoryObject AddMove(IMoveCommand moveCommand)
    {
        var moveHistoryObject = _moveCommandInvoker.ExecuteCommand(moveCommand);
        MoveNode newMoveNode = new(moveCommand, moveNumber: CurrentMove.MoveNumber + 1);
        
        CurrentMove.AddVariation(newMoveNode);
        CurrentMove = newMoveNode;
        
        return moveHistoryObject;
    }
    
    /// <summary>
    /// Undo the current move in the game
    /// NOTE: Any variations of the current move will be lost when undoing a move.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when trying to undo the RootMove </exception>
    public void UndoMove()
    {
        _moveCommandInvoker.UndoCommand(CurrentMove.MoveCommand ?? 
                                        throw new InvalidOperationException("Cannot undo a move without a command."));
        
        NavigateToMove(CurrentMove.Parent ?? 
                       throw new InvalidOperationException("Cannot undo the root move."));
        
        CurrentMove.RemoveAllVariations();
    }
    
    /// <summary>
    /// A shortcut to undo all moves in the game, resetting the current move to the root move.
    /// This effectively clears the move history and starts a new game.
    /// Note that this will discard all variations and the current move.
    /// </summary>
    public void UndoAllMoves()
    {
        NavigateToMove(RootMove);
        CurrentMove.RemoveAllVariations();
    }
    
    /// <summary>
    ///  Navigates to a specific move in the game.
    /// </summary>
    /// <param name="moveNode"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void NavigateToMove(MoveNode moveNode)
    {
        CurrentMove = moveNode ?? 
                      throw new ArgumentNullException(nameof(moveNode), "Move node cannot be null.");
        
        _board.LoadPositionFromMoveHistoryObject(moveNode.MoveHistoryObject);
    }
    
    /// <summary>
    /// Navigate to a specific move in the game history.
    /// </summary>
    /// <param name="moveNumber">The move number to navigate to (For the starting move, this would be 0).</param>
    /// <param name="variationsPath">
    /// An optional dictionary to provide guidance on the variation path to the target move.
    /// The key is the move number (e.g., 1 for the first move) at which a choice is made,
    /// and the value is the variation index to follow.
    /// If a move number is not in the dictionary, the main line (index 0) is used by default.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if moveNumber or any variation path values are invalid.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the requested path does not exist.</exception>
    public void NavigateToMove(int moveNumber, Dictionary<int, int>? variationsPath = null)
    {
        if (moveNumber < 0)
            throw new ArgumentOutOfRangeException(
                nameof(moveNumber), 
                "Move number must be non-negative.");
		
        if (variationsPath is not null && variationsPath.Any(kvp => kvp.Key < 0 || kvp.Value < 0))
            throw new ArgumentOutOfRangeException(
                nameof(variationsPath), 
                "Variation number must be non-negative.");

        var targetMove = RootMove;

        while (targetMove.MoveNumber < moveNumber)
        {
            var variationNumber = 0;
            
            var _ = variationsPath is not null && 
                    variationsPath.TryGetValue(targetMove.MoveNumber, out variationNumber);
            
            targetMove = targetMove.Variations
                .ElementAtOrDefault(variationNumber) 
                ?? throw new InvalidOperationException(
                    $"No variation found for move number {targetMove.MoveNumber} with variation number {variationNumber}.");
        }
        
        NavigateToMove(targetMove);
    }
}