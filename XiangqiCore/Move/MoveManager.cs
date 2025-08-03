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
    /// <param name="includeRootNode">
    /// If true, the root move will be included in the results.
    /// For example, when you want to show the initial position in the GIF of the game.
    /// </param>
    /// <param name="variationsPath">
    /// <see cref="VariationPath"/>
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if moveNumber or any variation path values are invalid.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the requested path does not exist.</exception>
    public List<MoveHistoryObject> GetMoveHistory(
        bool includeRootNode = false, 
        VariationPath? variationsPath = null)
    {
        var nodesOnPath = GetNodesOnPath(variationsPath);

        if (!includeRootNode)
            nodesOnPath = nodesOnPath.Skip(1); // Skip the root move if not included.
        
        return nodesOnPath
            .Select(node => node.MoveHistoryObject)
            .ToList();
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
    /// Remove all moves after the current move in the game
    /// </summary>
    public void DeleteSubsequentMoves()
    {
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
    public void NavigateToMove(int moveNumber, VariationPath? variationsPath = null)
    {
        if (moveNumber < 0)
            throw new ArgumentOutOfRangeException(
                nameof(moveNumber), 
                "Move number must be non-negative.");
		
        var targetMove = GetNodesOnPath(variationsPath)
            .FirstOrDefault(node => node.MoveNumber == moveNumber)
            ?? throw new InvalidOperationException(
                $"No move found with the specified move number: {moveNumber} and variations path: {variationsPath}.");
        
        NavigateToMove(targetMove);
    }

    /// <summary>
    /// Get the last move in the game history.
    /// </summary>
    /// <param name="variationsPath">
    /// <see cref="VariationPath"/>
    /// </param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Throw when the last move is null which should never happen</exception>
    public MoveNode GetLastMove(VariationPath? variationsPath = null)
        => GetNodesOnPath(variationsPath).LastOrDefault() 
               ?? throw new InvalidOperationException("No moves found in the history.");
    
    /// <summary>
    /// Get all variations of a specific move node.
    /// </summary>
    /// <param name="moveNodeToCheck"></param>
    /// <returns></returns>
    public IReadOnlyCollection<MoveNode> GetAllVariations(MoveNode moveNodeToCheck)
        => moveNodeToCheck.Variations?.ToArray() ?? [];
    
    /// <summary>
    /// Get an IEnumerable of MoveNodes on the path.
    /// </summary>
    private IEnumerable<MoveNode> GetNodesOnPath(VariationPath? variationsPath = null)
    {
        if (variationsPath is not null && variationsPath.Any(kvp => kvp.Key < 0 || kvp.Value < 0))
            throw new ArgumentOutOfRangeException(
                nameof(variationsPath), 
                "Variation number must be non-negative.");

        variationsPath ??= [];
        var currentNode = RootMove;

        while (currentNode is not null)
        {
            yield return currentNode;
            
            // Get the variation number for the next move.
            int variationNumber = variationsPath.GetValueOrDefault(currentNode.MoveNumber + 1, 0);
            
            currentNode = currentNode.Variations.ElementAtOrDefault(variationNumber);
        }
    }
}