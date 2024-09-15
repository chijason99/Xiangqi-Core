namespace XiangqiCore.Game;

public record Competition(DateTime GameDate, string Round, string Name, string Location);

	public class CompetitionBuilder
{
    private DateTime _gameDate { get; set; } = DateTime.Today;
    private string _round { get; set; } = "Unknown";
    private string _name { get; set; } = "Unknown";
    private string _location { get; set; } = "Unknown";

    public CompetitionBuilder WithGameDate(DateTime gameDate)
    {
        _gameDate = gameDate;
        return this;
    }

    public CompetitionBuilder WithRound(string round)
    {
        _round = round;
        return this;
    }

    public CompetitionBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public CompetitionBuilder WithLocation(string location)
    {
        _location = location;
        return this;
    }

    public Competition Build()
    {
        return new Competition(_gameDate, _round, _name, _location);
    }
}

