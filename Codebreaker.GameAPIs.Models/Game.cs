namespace Codebreaker.GameAPIs.Models;

public class Game(
    Guid id,
    string gameType,
    string playerName,
    DateTime startTime,
    int numberCodes,
    int maxMoves) : IGame
{
    public Guid Id { get; } = id;
    public string GameType { get; } = gameType;
    public string PlayerName { get; } = playerName;
    public DateTime StartTime { get; } = startTime;
    public DateTime? EndTime { get; set; }
    public TimeSpan? Duration { get; set; }
    public int LastMoveNumber { get; set; } = 0;
    public int NumberCodes { get; private set; } = numberCodes;
    public int MaxMoves { get; private set; } = maxMoves;
    public bool IsVictory { get; set; } = false;

    public required IDictionary<string, IEnumerable<string>> FieldValues { get; set; }

    public required string[] Codes { get; set; }

    public ICollection<Move> Moves { get; set; } = [];

    public override string ToString() => $"{Id}:{GameType} - {StartTime}";
