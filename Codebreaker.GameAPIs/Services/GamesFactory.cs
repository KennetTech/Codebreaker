using Microsoft.AspNetCore.Http.HttpResults;
using System.Drawing;

namespace Codebreaker.GameAPIs.Services;

public static class GamesFactory
{
    private static readonly string[] s_colors6 = [Colors.Red, Colors.Green, Colors.Blue, Colors.Yellow, Colors.Purple, Colors.Orange];
    private static readonly string[] s_colors8 = [Colors.Red, Colors.Green, Colors.Blue, Colors.Yellow, Colors.Purple, Colors.Orange, Colors.Pink, Colors.Brown];
    private static readonly string[] s_colors5 = [Colors.Red, Colors.Green, Colors.Blue, Colors.Yellow, Colors.Purple];
    private static readonly string[] s_shapes5 = [Sharpes.Circle, Sharpes.Square, Sharpes.Triangle, Sharpes.Star, Sharpes.Rectangle];

    public static Game CreateGame(string gameType, string playerName)
    {
        Game Create6x4SimpleGame() =>
            new(Guid.NewGuid(), gameType, playerName, DateTime.UtcNow, 4, 12)
            {
                FieldValues = new Dictionary<string, IEnumerable<string>>()
                {
                    { FieldCategories.Colors, s_colors6 }
                },
                Codes = Random.Shared.GetItems(s_colors6, 4)
            };

        Game Create6x4Game() =>
            new(Guid.NewGuid(), gameType, playerName, DateTime.UtcNow, 4, 12)
            {
                FieldValues = new Dictionary<string, IEnumerable<string>>()
                {
                    { FieldCategories.Colors, s_colors6 }
                },
                Codes = Random.Shared.GetItems(s_colors6, 4)
            };

        Game Create8x5Game() =>
            new(Guid.NewGuid(), gameType, playerName, DateTime.UtcNow, 5, 12)
            {
                FieldValues = new Dictionary<string, IEnumerable<>>()
                {
                    { FieldCategories.Colors, s_colors8 }
                },
                StatusCodes = Random.Shared.GetItems(s_colors8, 5)
            };

        Game Create5x5x4Game() =>
            new(Guid.NewGuid(), gameType, playerName, DateTime.UtcNow, 4, 14)
            {
                FieldValues = new Dictionary<string, IEnumerable<string>>()
                {
                    { FieldCategories.Colors, s_colors5 }
                    { Fieldcategories.Shapes, s_shapes5 }
                },
                Codes = Random.Shared.GetItems(s_shapes5, 4)
                    .Zip(Random.Shared.GetItems(s_colors5, 4), (shape, color) => (Shape: shape, Color: color))
                    .Select(item => string.Join(';', item.Shape, item,Color))
                    .ToArray()

            };

        return gameType switch
        {
            GameTypes.Game6x4Mini => Create6x4SimpleGame(),
            GameTypes.Game6x4 => Create6x4Game(),
            GameTypes.Game8x5 => Create8x5Game(),
            GameTypes.Game5x5x4 => Create5x5x4Game()
            _ => throw new CodebreakerExeption("Invalid game type") { Code = CodebreakerExceptionCodes.InvalidGameTypes }
        };
    }

    public static Move ApplyMove(this Game game, string[] guesses, int moveNumber)
    {
        static TField[] GetGuesses<TField>(IEnumerable<string> guesses)
            where TField : IParsable<TField> => guesses
            .Select(g => TField.Parse(g, default))
            .ToArray();

        string[] GetColorGameGuessAnalyzerResult()
        {
            GetColorGameGuessAnalyzer analyzer = new(game, GetGuesses<ColorField>(guesses), moveNumber);
            return analyzer.GetResult().ToStringResults();
        }

        string[] GetSimpleGameGuessAnalyzerResult()
        {
            SimpleGameGuessAnalyzer analyser = new(game, GetGuesses<ColorField>(guesses), moveNumber);
            return analyser.GetResult().ToStringResults();
        }

        string[] GetShapeGameGuessAnalyzerResult()
        {
            ShapeGameGuessAnalyzer analyzer = new(game, GetGuesses<ShapeAndColorField>(guesses), moveNumber);
            return analyzer.GetResult().ToStringResults();
        }

        string[] results = game.GameType switch
        {
            GameTypes.Game6x4 => GetColorGameGuessAnalyzerResult(),
            GameTypes.Game8x4 => GetColorGameGuessAnalyzerResult(),
            GameTypes.Game6x4Mini => GetSimpleGameGuessAnalyzerResult(),
            GameTypes.Game5x5x4 => GetShapeGameGuessAnalyzerResult(),
            _ => throw new CodebreakerException("Invalid game type") { Code = CodebreakerExceptionCodes.InvalidGameType }
        };

        Move move = new(Guid.NewGuid(), moveNumber)
        {
            GuessPegs = guesses,
            KeyPegs = results
        };

        game.Moves.Add(move);
        return move;
    }
}
