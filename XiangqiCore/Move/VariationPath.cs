namespace XiangqiCore.Move;

/// <summary>
/// Provides guidance on the variation path to a target move.
/// The key is the move number (e.g., 1 for the first move) at which a choice is made,
/// and the value is the variation index to follow.
/// If a move number is not in the dictionary, the main line (index 0) is used by default.
/// </summary>
public class VariationPath : Dictionary<int, int>
{
    public VariationPath() : base() { }
    public VariationPath(Dictionary<int, int> dictionary) : base(dictionary) { }
}