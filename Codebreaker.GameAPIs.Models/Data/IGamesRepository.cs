namespace Codebreaker.GameAPIs.Models.Data;

public interface IGamesRepository
{
    Task AddGameAsync(Game game, CancellationToken cancellationToken = default);
    Task AddMoveAsync(Game game, Move move, CancellationToken cancellation = default);
    Task<bool> DeleteGameAsync(Guid gameId, CancellationToken cancellationToken = default);
    Task<Game?> GetGameAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Game>> GetGamesAsync(GamesQuery gamesQuery, CancellationToken cancellationToken = default);
    Task<Game> UpdateGameAsync(Game game, CancellationToken cancellationToken = default);
}
