﻿using XiangqiCore.Extension;
using XiangqiCore.Misc;
using XiangqiCore.Move.MoveObjects;
using XiangqiCore.Pieces.PieceTypes;

namespace XiangqiCore.Move.NotationTranslators;

public abstract class BaseNotationTranslator : INotationTranslator
{
	protected virtual Dictionary<PieceTypeSideKey, char[]> PieceTypeSymbolCache { get; set; } = [];
	protected virtual Dictionary<MoveDirection, char[]> MoveDirectionSymbolCache { get; set; } = [];
	protected virtual Dictionary<PieceOrder, char[]> PieceOrderSymbolCache { get; set; } = [];

	public Language Language { get; init; }

	protected BaseNotationTranslator(Language language = Language.NotSpecified)
	{
		Language = language;

		InitializePieceTypeSymbolMap();
		InitializeMoveDirectionSymbolMap();
		InitializePieceOrderSymbolMap();
	}

	public abstract string Translate(MoveHistoryObject moveHistoryObject);

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

				MoveDirectionSymbolCache[moveDirection] = [symbolChar];
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

				PieceOrderSymbolCache[pieceOrder] = [symbolChar];
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

					PieceTypeSymbolCache[new PieceTypeSideKey(pieceType, side)] = [symbolChar];
				}
			}
		}
	}

	protected virtual char GetPieceTypeSymbol(PieceType pieceType, Side side)
		=> PieceTypeSymbolCache[new PieceTypeSideKey(pieceType, side)].First();

	protected virtual char GetMoveDirectionSymbol(MoveDirection moveDirection)
		=> MoveDirectionSymbolCache[moveDirection].First();

	protected virtual char GetPieceOrderSymbol(PieceOrder pieceOrder)
		=> PieceOrderSymbolCache[pieceOrder].First();

	protected virtual int GetFourthCharacter(MoveHistoryObject moveHistoryObject)
	{
		// If the piece is moving in diagonals or if the piece is moving horizontally, the fourth character is the destination column
		if (moveHistoryObject.PieceMoved.IsMovingInDiagonals() || moveHistoryObject.MoveDirection == MoveDirection.Horizontal)
		{
			return moveHistoryObject.Destination
				.Column
				.ConvertToColumnBasedOnSide(moveHistoryObject.MovingSide);
		}
		// If the piece is moving vertically, the fourth character is the number of rows moved
		else
		{
			return Math.Abs(moveHistoryObject.Destination.Row - moveHistoryObject.StartingPosition.Row);
		}
	}

	protected virtual int GetStartingColumn(MoveHistoryObject moveHistoryObject)
		=> moveHistoryObject.StartingPosition
			.Column
			.ConvertToColumnBasedOnSide(moveHistoryObject.MovingSide);
}
