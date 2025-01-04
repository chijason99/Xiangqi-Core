using XiangqiCore.Extension;
using XiangqiCore.Misc;
using XiangqiCore.Move.MoveObject;
using XiangqiCore.Move.NotationParsers;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Move;

public abstract class MoveNotationParserBase : INotationParser
{
	private static Dictionary<Type, MoveNotationParserBase> _instances = [];

	protected virtual Dictionary<char, PieceType> SymbolToPieceTypeMap { get; set; } = [];
	protected virtual Dictionary<char, MoveDirection> SymbolToMoveDirectionMap { get; set; } = [];
	protected virtual Dictionary<char, PieceOrder> SymbolToPieceOrderMap { get; set; } = [];


	public Language Language { get; }

	protected MoveNotationParserBase(Language langauge = Language.NotSpecified)
	{
		Language = langauge;

		InitializePieceTypeSymbolMap();
		InitializeMoveDirectionSymbolMap();
		InitializePieceOrderSymbolMap();
	}

	public static T GetMoveNotationParserInstance<T>() where T : MoveNotationParserBase, new()
	{
		if (!_instances.ContainsKey(typeof(T)))
			_instances[typeof(T)] = new T();

		return (T)_instances[typeof(T)];
	}

	public abstract ParsedMoveObject Parse(string notation);

	protected virtual void InitializeMoveDirectionSymbolMap()
	{
		if (Language == Language.NotSpecified)
			return;

		foreach (MoveDirection moveDirection in Enum.GetValues<MoveDirection>())
		{
			string[] symbols = EnumHelper<MoveDirection>.GetSymbols(moveDirection, Language);

			foreach (string symbol in symbols)
			{
				char symbolChar = symbol[0];

				SymbolToMoveDirectionMap[symbolChar] = moveDirection;
			}
		}
	}

	protected virtual void InitializePieceOrderSymbolMap()
	{
		if (Language == Language.NotSpecified)
			return;

		foreach (PieceOrder pieceOrder in Enum.GetValues<PieceOrder>().Where(po => po != PieceOrder.Unknown))
		{
			string[] symbols = EnumHelper<PieceOrder>.GetSymbols(pieceOrder, Language);

			foreach (string symbol in symbols)
			{
				char symbolChar = symbol[0];

				SymbolToPieceOrderMap[symbolChar] = pieceOrder;
			}
		}
	}

	protected virtual void InitializePieceTypeSymbolMap()
	{
		if (Language == Language.NotSpecified)
			return;

		foreach (PieceType pieceType in Enum.GetValues<PieceType>().Where(pt => pt != PieceType.None))
		{
			foreach (Side side in Enum.GetValues<Side>().Where(s => s != Side.None))
			{
				string[] symbols = EnumHelper<PieceType>.GetSymbols(pieceType, Language, side);

				foreach (string symbol in symbols)
				{
					char symbolChar = symbol[0];

					SymbolToPieceTypeMap[symbolChar] = pieceType;
				}
			}
		}
	}

	protected virtual PieceType ParsePieceType(string notation)
	{
		if (SymbolToPieceTypeMap.TryGetValue(notation[0], out PieceType pieceType))
			return pieceType;
		// The piece name would be in the second character if there are more than one pieces of the same type on the same column
		else if (SymbolToPieceTypeMap.TryGetValue(notation[1], out PieceType pieceType2))
			return pieceType2;
		// The piece type would be pawn if the piece name is not found in the first two characters
		else
			return PieceType.Pawn;
	}

	protected virtual MoveDirection ParseMoveDirection(string notation)
		=> SymbolToMoveDirectionMap.TryGetValue(notation[2], out MoveDirection moveDirection) ? moveDirection : throw new ArgumentException("Unknown move direction");

	protected virtual PieceOrder ParsePieceOrder(string notation)
	{
		if (SymbolToPieceOrderMap.TryGetValue(notation[0], out PieceOrder pieceOrder))
			return pieceOrder;

		return PieceOrder.Unknown;
	}

	// Multi Column Pawn situation
	// Meaning that there are more than one columns holding two or more pawns of the same color
	protected virtual bool IsMultiColumnPawn(string notation) => ParsePieceType(notation) == PieceType.Pawn &&
		notation.IndexOfAny(['兵', '卒', 'p', 'P']) != 0;

	protected virtual int GetMinNumberOfPawnsOnColumn(string notation)
	{
		const int defaultMinNumberOfPawnsOnColumn = 2;

		if (SymbolToPieceOrderMap.TryGetValue(notation[0], out PieceOrder pieceOrder)
			&& pieceOrder != PieceOrder.First
			&& pieceOrder != PieceOrder.Last)
		{
			return (int)pieceOrder;
		}

		return defaultMinNumberOfPawnsOnColumn;
	}
}

public readonly struct PieceTypeSideKey(PieceType pieceType, Side side)
{
    public readonly PieceType PieceType { get; } = pieceType;
    public readonly Side Side { get; } = side;
}