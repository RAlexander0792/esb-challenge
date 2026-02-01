using ESBC.API.Controllers.Dtos;
using ESBC.Benchmarks.EntityBuilders;
using ESBC.Domain.Entities;
using ESBC.Domain.Services;
using ESBC.UtilitiesCli.ApiProviders;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace ESBC.UtilitiesCli.TUI;

enum DataGenerationMatchMenuOptions
{
    OneMatch,
    Continous,
    Exit
}

public class DataGenerationMatchMenu
{
    public static async Task Load()
    {
        bool leaving = false;
        while (!leaving)
        {
            AnsiConsole.Clear();
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<DataGenerationMatchMenuOptions>()
               .Title("Select a generator")
               .AddChoices(Enum.GetValues<DataGenerationMatchMenuOptions>()));

            AnsiConsole.MarkupLine($"[green]You selected:[/] {choice}");
            switch (choice)
            {
                case DataGenerationMatchMenuOptions.OneMatch:
                    await HandleGenerateOneMatch();
                    break;
                case DataGenerationMatchMenuOptions.Continous:
                    await HandleGenerateContinous();
                    break;

                case DataGenerationMatchMenuOptions.Exit:
                    leaving = true;
                    return;
            }
        }
        
    }

    private static async Task<Guid> SelectRankedBoard()
    {
        var rankedBoardService = DependencyManager.ServiceProvider.GetRequiredService<IRankedBoardService>();

        var rankedBoards = await rankedBoardService.GetRankedBoards();

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<Guid>()
               .Title("Select a ranked board for the matches")
               .AddChoices(rankedBoards.Select(x => x.Id)) );

        return choice;
    }

    private static async Task HandleGenerateOneMatch()
    {
        var rankBoard = await SelectRankedBoard();
        
        var playerService = DependencyManager.ServiceProvider.GetRequiredService<IPlayerService>();

        var players = await playerService.GetPlayers(100);
        var rankedPlayers = players
            .Select(x => new RankedBoardPlayer() { Id = new RankedBoardPlayerId(rankBoard, x.Id) });


        var matchBuilder = new MatchBuilder(1234);

        matchBuilder.WithPlayersPerMatch(2);
        matchBuilder.WithPlayers(rankedPlayers.ToList());
        matchBuilder.WithScoreRange(600, 1800);


    }
    private static async Task HandleGenerateContinous()
    {
        var rankBoard = await SelectRankedBoard();

        var playerService = DependencyManager.ServiceProvider.GetRequiredService<IPlayerService>();
        var matchProvider = DependencyManager.ServiceProvider.GetRequiredService<IMatchProvider>();

        var players = await playerService.GetPlayers(100);
        var rankedPlayers = players
            .Select(x => new RankedBoardPlayer() { Id = new RankedBoardPlayerId(rankBoard, x.Id) });


        var matchBuilder = new MatchBuilder(1234);

        matchBuilder.WithPlayersPerMatch(2);
        matchBuilder.WithPlayers(rankedPlayers.ToList());
        matchBuilder.WithScoreRange(600, 1800);
        bool leaving = false;

        while (!leaving)
        {
            var match = matchBuilder.BuildOne();
            await matchProvider.PostMatch(new CreateMatchRequest() { RankedBoardId = rankBoard, PlayerScores = match.PlayerScores   });
            await Task.Delay(250);
        }

    }
}
