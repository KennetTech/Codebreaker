namespace Codebreaker.GameAPIs.Services;

public class GamesService(IGamesRepository dataRepository) : IGamesService
{
    public async Task<Game> StartGameAsync(string gameType, string playerName, CancellationToken cancellationToken = default)
    {
        Game game = GamesFactory.CreateGame(gameType, playerName);

        await dataRepository.AddGameAsync(game, cancellationToken);
        return game;
    }

    public async Task<(Game Game, Move Move)> SetMoveAsync(Guid id, string[] guesses, int moveNumber, CancellationToken cancellationToken = default)
    {
        Game? game = await dataRepository.GetGameAsync(id, cancellationToken);
        CodebreakerException.ThrowIfNull(game);
        CodebreakerException.ThrowIfEnded(game);

        Move move = game.ApplyMove(guesses, moveNumber);

        await dataRepository.AddMoveAsync(game, move, cancellationToken);

        return (game, move);
    }

    public async ValueTask<Game?> GetGameAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var game = await dataRepository.GetGameAsync(id, cancellationToken);
        return game;
    }

    public async Task DeleteGameAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await dataRepository.DeleteGameAsync(id, cancellationToken);
    }

    public async Task<Game> EndGameAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Game? game = await dataRepository.GetGameAsync(id, cancellationToken);
        CodebreakerException.ThrowIfNull(game);

        game.Endtime = DateTime.UtcNow;
        game.Duration = game.Endtime - game.StartTime;
        game = await dataRepository.UpdateGameAsync(game, cancellationToken);
        return game;
    }

    public async Task<IEnumerable<Game>> GetGamesAsync(GamesQuery gamesQuery, CancellationToken cancellationToken = default)
    {
        return await dataRepository.GetGamesAsync(gamesQuery, cancellationToken);
    }
}
