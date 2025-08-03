using XiangqiCore.Move.Commands;
using XiangqiCore.Move.MoveObjects;

namespace XiangqiCore.Move;

/// <summary>
/// A move node in the Xiangqi game tree.
/// </summary>
public class MoveNode
{
    /// <summary>
    /// Constructor for normal move nodes with a move command.
    /// </summary>
    /// <param name="moveCommand"></param>
    public MoveNode(IMoveCommand moveCommand, int moveNumber)
    {
        if (moveNumber <= 0)
            throw new ArgumentOutOfRangeException(nameof(moveNumber), 
                "Depth must be greater than 0 for non-root move nodes.");
        
        MoveCommand = moveCommand ?? 
                      throw new ArgumentNullException(nameof(moveCommand), 
                          "Move command cannot be null.");
        
        MoveHistoryObject = moveCommand.MoveHistoryObject ?? 
                            throw new ArgumentNullException(nameof(moveCommand), 
                                "Move history object cannot be null.");
        
        MoveNumber = moveNumber;
    }

    /// <summary>
    /// Constructor for the root move node, which does not have a move command but has a move history object.
    /// </summary>
    /// <param name="moveHistoryObject"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public MoveNode(MoveHistoryObject moveHistoryObject)
    {
        MoveHistoryObject = moveHistoryObject ?? 
                              throw new ArgumentNullException(nameof(moveHistoryObject), "Move history object cannot be null.");

        MoveNumber = 0; // The root move node has a move number of 0.
    }
    
    public MoveHistoryObject MoveHistoryObject { get; private set; }
    
    public IMoveCommand? MoveCommand { get; private set; }
    
    /// <summary>
    /// The move number or number of moves made from the root node.
    /// The root move node has a move number of 0, and each variation increases the move number by 1.
    /// This is useful for determining the level of the move in the game history,
    /// or you can treat it as the depth of the move node in the game tree.
    /// </summary>
    public int MoveNumber { get; private set; }
    
    private readonly List<MoveNode> _variations = [];
    
    public IReadOnlyCollection<MoveNode> Variations => _variations.AsReadOnly();

    /// <summary>
    /// This property is nullable because the root move does not have a parent.
    /// </summary>
    public MoveNode? Parent { get; private set; }

    /// <summary>
    /// Adds a variation to this move node.
    /// </summary>
    /// <param name="variation"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public MoveNode AddVariation(MoveNode variation)
    {
        if (variation.MoveCommand is null)
            throw new ArgumentNullException(nameof(variation), 
                "Move node must have a move command except for root node.");
        
        variation.Parent = this;
        _variations.Add(variation);
       
        return variation;
    }
    
    /// <summary>
    ///  Removes a variation from this move node.
    /// </summary>
    /// <param name="variation"></param>
    /// <exception cref="InvalidOperationException">Thrown when the variation to remove does not exist in this move node.</exception>
    public void RemoveVariation(MoveNode variation)
    {
        if (!_variations.Contains(variation))
            throw new InvalidOperationException("The variation to remove does not exist in this move node.");
        
        Queue<MoveNode> nodesToRemove = [];
        nodesToRemove.Enqueue(variation);
        
        while (nodesToRemove.Count > 0)
        {
            var node = nodesToRemove.Dequeue();
            
            // Sever the parent-child relationship to avoid memory leaks
            // and unexpected behavior when another place is holding a reference to this node.
            node.Parent = null;
            _variations.Remove(node);

            if (node._variations.Count == 0) 
                continue;
            
            foreach (var childNode in node._variations)
                nodesToRemove.Enqueue(childNode);
        }
    }
    
    /// <summary>
    /// Removes all variations from this move node.
    /// </summary>
    public void RemoveAllVariations()
    {
        foreach (var variation in _variations)
            RemoveVariation(variation);
    }
}