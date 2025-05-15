namespace Codebreaker.GameAPIs.Models;

public class Move(Guid id, int moveNumber)
{
    public Guid Id { get; set; }

    public int MoveNumber { get; set; } = moveNumber;

    public required string[] GuessPegs { get; init; }

    public required string[] KeyPegs { get; init; }

    public override string  ToString() => $"{MoveNumber}. " +
        $"{string.Join('#',GuessPegs)} : " +
        $"{string.Join('#', KeyPegs)}";
    
}