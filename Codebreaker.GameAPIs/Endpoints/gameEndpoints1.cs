using Microsoft.AspNetCore.Http.Extensions;

namespace Codebreaker.GameAPIs.Endpoints;

public static class GameEndpoints
{
    public static void MapGameEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/games");

        group.MapPost("/", async (CreateGameRequest request, IGamesService gameService, HttpContext context, CancellationToken cancellationToken) =>
        {
            Game game;
            try
            {
                game = await gameService.StartGameAsync(request.GameType.ToString(), request.PlayerName, cancellationToken);
            }
            catch (CodebreakerException) when (ex.Code == CodebreakerExceptionCodes.InvalidGameType)
            {
                GameError error = new(ErrorCodes.InvalidGameType,
                    $"Game type {request.GameType} does not exist",
                    context.Request.GetDisplayUrl(),
                    Enum.GetNames<GameType>());
                return Results.BadRequest(error);
        });
    }
}
