using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using XiangqiCore.Game;
using XiangqiCore.Move;

namespace XiangqiCore.Services.PgnLoading;

public sealed partial class DefaultPgnLoadingService : IPgnLoadingService
{
	private const string DefaultStartingFen = "rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w - - 0 1";
	private const string UnknownResultToken = "*";

	/// <inheritdoc />
	public XiangqiGame LoadFromFile(string filePath)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

		if (!File.Exists(filePath))
			throw new FileNotFoundException("The PGN file could not be found.", filePath);

		return LoadFromString(ReadPgnContent(filePath));
	}

	/// <inheritdoc />
	public XiangqiGame LoadFromString(string pgnContent)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(pgnContent);

		PgnDocument document = ParseDocument(pgnContent);

		if (TryParseMoveNotationType(GetNotationTag(document.Tags), out MoveNotationType taggedMoveNotationType))
		{
			XiangqiGame taggedGame = CreateGame(document);
			ApplyMoves(taggedGame, document.Moves, taggedMoveNotationType);
			return taggedGame;
		}

		foreach (MoveNotationType candidateMoveNotationType in GetCandidateMoveNotationTypes(document.Moves))
		{
			XiangqiGame candidateGame = CreateGame(document);

			if (TryApplyMoves(candidateGame, document.Moves, candidateMoveNotationType))
				return candidateGame;
		}

		throw new NotSupportedException(
			document.Moves.Count == 0
				? "The PGN does not contain any supported move data."
				: $"The PGN move notation is not supported: '{document.Moves[0]}'.");
	}

	private static XiangqiGame CreateGame(PgnDocument document)
	{
		XiangqiBuilder builder = new();

		builder.WithStartingFen(GetTagValue(document.Tags, "FEN") ?? DefaultStartingFen);

		string? eventName = GetTagValue(document.Tags, "Event");
		string? site = GetTagValue(document.Tags, "Site");
		string? round = GetTagValue(document.Tags, "Round");
		DateTime? gameDate = ParseGameDate(GetTagValue(document.Tags, "Date"));

		builder.WithCompetition(competition =>
		{
			if (!string.IsNullOrWhiteSpace(eventName))
				competition.WithName(eventName);

			if (!string.IsNullOrWhiteSpace(site))
				competition.WithLocation(site);

			if (!string.IsNullOrWhiteSpace(round))
				competition.WithRound(round);

			if (gameDate.HasValue)
				competition.WithGameDate(gameDate.Value);
		});

		string? redPlayer = GetTagValue(document.Tags, "Red");
		string? redTeam = GetTagValue(document.Tags, "RedTeam");
		string? blackPlayer = GetTagValue(document.Tags, "Black");
		string? blackTeam = GetTagValue(document.Tags, "BlackTeam");

		builder.WithRedPlayer(player =>
		{
			if (!string.IsNullOrWhiteSpace(redPlayer))
				player.Name = redPlayer;

			if (!string.IsNullOrWhiteSpace(redTeam))
				player.Team = redTeam;
		});

		builder.WithBlackPlayer(player =>
		{
			if (!string.IsNullOrWhiteSpace(blackPlayer))
				player.Name = blackPlayer;

			if (!string.IsNullOrWhiteSpace(blackTeam))
				player.Team = blackTeam;
		});

		builder.WithGameResult(ParseGameResult(GetTagValue(document.Tags, "Result")));

		return builder.Build();
	}

	private void ApplyMoves(XiangqiGame game, IReadOnlyList<string> moves, MoveNotationType moveNotationType)
	{
		for (int index = 0; index < moves.Count; index++)
		{
			string move = moves[index];

			if (!game.MakeMove(move, moveNotationType))
			{
				throw new InvalidDataException(
					$"The PGN contains a move that cannot be applied at ply {index + 1}: '{move}'.");
			}
		}
	}

	private static bool TryApplyMoves(XiangqiGame game, IReadOnlyList<string> moves, MoveNotationType moveNotationType)
	{
		foreach (string move in moves)
		{
			if (!game.MakeMove(move, moveNotationType))
				return false;
		}

		return true;
	}

	private static PgnDocument ParseDocument(string pgnContent)
	{
		string normalizedContent = pgnContent.Trim().TrimStart('\uFEFF');
		Dictionary<string, string> tags = ParseTags(normalizedContent);
		string moveSection = ExtractMoveSection(normalizedContent);
		List<string> moves = TokenizeMoves(moveSection);

		return new PgnDocument(tags, moves);
	}

	private static Dictionary<string, string> ParseTags(string pgnContent)
	{
		Dictionary<string, string> tags = new(StringComparer.OrdinalIgnoreCase);

		foreach (Match match in PgnTagRegex().Matches(pgnContent))
		{
			string key = match.Groups["key"].Value;
			string value = Regex.Unescape(match.Groups["value"].Value);
			tags[key] = value;
		}

		return tags;
	}

	private static string ExtractMoveSection(string pgnContent)
	{
		List<string> moveLines = [];

		foreach (string rawLine in pgnContent.Split(["\r\n", "\n"], StringSplitOptions.None))
		{
			string line = rawLine.Trim();

			if (line.Length == 0 || PgnTagRegex().IsMatch(line))
				continue;

			moveLines.Add(line);
		}

		return string.Join("\n", moveLines);
	}

	private static List<string> TokenizeMoves(string moveSection)
	{
		List<string> moves = [];
		string sanitizedMoveSection = StripPgnAnnotations(moveSection);

		foreach (string rawToken in sanitizedMoveSection.Split((char[])null!, StringSplitOptions.RemoveEmptyEntries))
		{
			string token = SanitizeMoveToken(rawToken);

			if (token.Length == 0 || IsNonMoveToken(token))
				continue;

			moves.Add(token);
		}

		return moves;
	}

	private static string StripPgnAnnotations(string moveSection)
	{
		StringBuilder builder = new(moveSection.Length);
		int variationDepth = 0;
		int commentDepth = 0;
		bool inLineComment = false;

		foreach (char character in moveSection)
		{
			if (inLineComment)
			{
				if (character is '\r' or '\n')
				{
					inLineComment = false;
					builder.Append(' ');
				}

				continue;
			}

			if (commentDepth > 0)
			{
				if (character == '{')
					commentDepth++;
				else if (character == '}')
					commentDepth--;

				continue;
			}

			if (variationDepth > 0)
			{
				if (character == '(')
					variationDepth++;
				else if (character == ')')
					variationDepth--;

				continue;
			}

			switch (character)
			{
				case '{':
					AppendWhitespaceIfNeeded(builder);
					commentDepth++;
					break;
				case '(':
					AppendWhitespaceIfNeeded(builder);
					variationDepth++;
					break;
				case ';':
					AppendWhitespaceIfNeeded(builder);
					inLineComment = true;
					break;
				default:
					builder.Append(character);
					break;
			}
		}

		return builder.ToString();
	}

	private static void AppendWhitespaceIfNeeded(StringBuilder builder)
	{
		if (builder.Length > 0 && !char.IsWhiteSpace(builder[^1]))
			builder.Append(' ');
	}

	private static string SanitizeMoveToken(string token)
	{
		string sanitizedToken = token.Trim();

		while (PrefixedMoveTokenRegex().IsMatch(sanitizedToken))
		{
			sanitizedToken = PrefixedMoveTokenRegex().Replace(sanitizedToken, string.Empty, 1).Trim();
		}

		while (sanitizedToken.Length > 4 && sanitizedToken[^1] is '!' or '?' or '+' or '#')
			sanitizedToken = sanitizedToken[..^1];

		return sanitizedToken.Trim();
	}

	private static bool IsNonMoveToken(string token)
	{
		if (string.IsNullOrWhiteSpace(token))
			return true;

		if (token == UnknownResultToken || token is "1-0" or "0-1" or "1/2-1/2")
			return true;

		if (token.StartsWith('$'))
			return true;

		if (MoveNumberRegex().IsMatch(token))
			return true;

		return false;
	}

	private static IEnumerable<MoveNotationType> GetCandidateMoveNotationTypes(IReadOnlyList<string> moves)
	{
		if (moves.Count == 0)
			return [MoveNotationType.TraditionalChinese];

		MoveNotationSignals signals = AnalyzeMoveNotationSignals(moves);

		if (signals.ContainsChinese)
		{
			return signals.ContainsSimplifiedChinese
				? [MoveNotationType.SimplifiedChinese, MoveNotationType.TraditionalChinese]
				: [MoveNotationType.TraditionalChinese, MoveNotationType.SimplifiedChinese];
		}

		if (signals.ContainsNotationSymbol)
			return [MoveNotationType.English];

		if (signals.ContainsRankTen)
			return [MoveNotationType.UCI, MoveNotationType.UCCI];

		if (signals.ContainsZeroRank || signals.ContainsUppercaseCoordinate)
			return [MoveNotationType.UCCI];

		return [MoveNotationType.UCCI, MoveNotationType.UCI];
	}

	private static MoveNotationSignals AnalyzeMoveNotationSignals(IReadOnlyList<string> moves)
	{
		bool containsChinese = false;
		bool containsSimplifiedChinese = false;
		bool containsNotationSymbol = false;
		bool containsRankTen = false;
		bool containsZeroRank = false;
		bool containsUppercaseCoordinate = false;

		foreach (string move in moves)
		{
			if (!containsChinese && ContainsChineseCharacter(move))
				containsChinese = true;

			if (!containsSimplifiedChinese && ContainsSimplifiedChineseMarker(move))
				containsSimplifiedChinese = true;

			if (!containsNotationSymbol && move.IndexOfAny(['+', '-', '=']) >= 0)
				containsNotationSymbol = true;

			if (!containsRankTen && move.Contains("10", StringComparison.Ordinal))
				containsRankTen = true;

			if (!containsZeroRank && ContainsStandaloneZeroRank(move))
				containsZeroRank = true;

			if (!containsUppercaseCoordinate && move.Any(character => character is >= 'A' and <= 'I'))
				containsUppercaseCoordinate = true;

			if (containsChinese &&
			    containsSimplifiedChinese &&
			    containsNotationSymbol &&
			    containsRankTen &&
			    containsZeroRank &&
			    containsUppercaseCoordinate)
			{
				break;
			}
		}

		return new MoveNotationSignals(
			containsChinese,
			containsSimplifiedChinese,
			containsNotationSymbol,
			containsRankTen,
			containsZeroRank,
			containsUppercaseCoordinate);
	}

	private static bool ContainsStandaloneZeroRank(string move)
	{
		for (int index = 0; index < move.Length; index++)
		{
			if (move[index] != '0')
				continue;

			if (index == 0 || move[index - 1] != '1')
				return true;
		}

		return false;
	}

	private static string? GetNotationTag(IReadOnlyDictionary<string, string> tags)
		=> GetTagValue(tags, "Notation")
			?? GetTagValue(tags, "MoveFormat")
			?? GetTagValue(tags, "Format");

	private static bool TryParseMoveNotationType(string? notationTag, out MoveNotationType moveNotationType)
	{
		moveNotationType = default;

		if (string.IsNullOrWhiteSpace(notationTag))
			return false;

		string normalizedNotation = notationTag.Trim()
			.Replace("-", string.Empty, StringComparison.Ordinal)
			.Replace(" ", string.Empty, StringComparison.Ordinal)
			.Replace("_", string.Empty, StringComparison.Ordinal);

		return normalizedNotation.ToUpperInvariant() switch
		{
			"TRADITIONALCHINESE" => SetMoveNotationType(MoveNotationType.TraditionalChinese, out moveNotationType),
			"SIMPLIFIEDCHINESE" => SetMoveNotationType(MoveNotationType.SimplifiedChinese, out moveNotationType),
			"ENGLISH" => SetMoveNotationType(MoveNotationType.English, out moveNotationType),
			"UCCI" => SetMoveNotationType(MoveNotationType.UCCI, out moveNotationType),
			"UCI" => SetMoveNotationType(MoveNotationType.UCI, out moveNotationType),
			_ => false
		};
	}

	private static bool SetMoveNotationType(MoveNotationType value, out MoveNotationType moveNotationType)
	{
		moveNotationType = value;
		return true;
	}

	private static bool ContainsSimplifiedChineseMarker(string move)
		=> move.IndexOfAny(['车', '马', '将', '帅', '进']) >= 0;

	private static bool ContainsChineseCharacter(string move)
		=> move.Any(character => character >= '\u4e00' && character <= '\u9fff');

	private static GameResult ParseGameResult(string? result)
	{
		if (string.IsNullOrWhiteSpace(result))
			return GameResult.Unknown;

		return result.Trim() switch
		{
			"1-0" or "RedWin" or "红胜" or "紅勝" => GameResult.RedWin,
			"0-1" or "BlackWin" or "黑胜" or "黑勝" => GameResult.BlackWin,
			"1/2-1/2" or "Draw" or "和棋" => GameResult.Draw,
			UnknownResultToken => GameResult.Unknown,
			_ => GameResult.Unknown
		};
	}

	private static DateTime? ParseGameDate(string? dateValue)
	{
		if (string.IsNullOrWhiteSpace(dateValue))
			return null;

		string normalizedDate = dateValue.Trim();

		if (normalizedDate.Contains('?'))
			return null;

		string[] supportedFormats = ["yyyy.MM.dd", "yyyy.M.d", "yyyy-MM-dd", "yyyy/M/d", "yyyy/MM/dd"];

		if (DateTime.TryParseExact(
			    normalizedDate,
			    supportedFormats,
			    CultureInfo.InvariantCulture,
			    DateTimeStyles.None,
			    out DateTime exactParsedDate))
		{
			return exactParsedDate;
		}

		return DateTime.TryParse(normalizedDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate)
			? parsedDate
			: null;
	}

	private static string? GetTagValue(IReadOnlyDictionary<string, string> tags, string key)
		=> tags.TryGetValue(key, out string? value) ? value : null;

	private static string ReadPgnContent(string filePath)
	{
		byte[] bytes = File.ReadAllBytes(filePath);

		if (HasUtf8Bom(bytes))
			return Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);

		if (HasUtf16LittleEndianBom(bytes))
			return Encoding.Unicode.GetString(bytes, 2, bytes.Length - 2);

		if (HasUtf16BigEndianBom(bytes))
			return Encoding.BigEndianUnicode.GetString(bytes, 2, bytes.Length - 2);

		try
		{
			return new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true).GetString(bytes);
		}
		catch (DecoderFallbackException)
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			Encoding chineseEncoding = Encoding.GetEncoding(
				"GB18030",
				EncoderFallback.ExceptionFallback,
				DecoderFallback.ExceptionFallback);

			return chineseEncoding.GetString(bytes);
		}
	}

	private static bool HasUtf8Bom(byte[] bytes)
		=> bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF;

	private static bool HasUtf16LittleEndianBom(byte[] bytes)
		=> bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE;

	private static bool HasUtf16BigEndianBom(byte[] bytes)
		=> bytes.Length >= 2 && bytes[0] == 0xFE && bytes[1] == 0xFF;

	[GeneratedRegex(@"^\[(?<key>[^\s]+)\s+""(?<value>(?:\\.|[^""])*)""\]\s*$", RegexOptions.Multiline)]
	private static partial Regex PgnTagRegex();

	[GeneratedRegex(@"^\d+\.(?:\.\.)?$")]
	private static partial Regex MoveNumberRegex();

	[GeneratedRegex(@"^\d+\.(?:\.\.)?")]
	private static partial Regex PrefixedMoveTokenRegex();

	private sealed record PgnDocument(
		IReadOnlyDictionary<string, string> Tags,
		IReadOnlyList<string> Moves);

	private readonly record struct MoveNotationSignals(
		bool ContainsChinese,
		bool ContainsSimplifiedChinese,
		bool ContainsNotationSymbol,
		bool ContainsRankTen,
		bool ContainsZeroRank,
		bool ContainsUppercaseCoordinate);
}
