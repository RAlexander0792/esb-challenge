using Mgls.Domain.Services;
using Mgls.UtilitiesCli.PersistentDataGeneration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace Mgls.UtilitiesCli.TUI;

enum DataGenerationMatchMenuOptions
{
    OneMatch,
    Continuous,
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
                    await HandleGenerateOne();
                    break;
                case DataGenerationMatchMenuOptions.Continuous:
                    await HandleGenerateContinuous();
                    break;

                case DataGenerationMatchMenuOptions.Exit:
                    leaving = true;
                    return;
            }
        }
        
    }

    private static async Task<Guid> SelectLeaderboard()
    {
        var leaderboardService = DependencyManager.ServiceProvider.GetRequiredService<ILeaderboardService>();

        var leaderboards = await leaderboardService.GetLeaderboards(CancellationToken.None);

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<Guid>()
               .Title("Select a leaderboard for the matches")
               .AddChoices(leaderboards.Select(x => x.Id)) );

        return choice;
    }

    private static async Task HandleGenerateContinuous()
    {
        MatchGenerator generator = await CreateMatchGeneratorPrompt();
        int delayInMS = SelectDelayInMS();

        bool leaving = false;

        while (!leaving)
        {
            var response = await generator.Generate();
            AnsiConsole.WriteLine($"Resulted in {response}");
            await Task.Delay(delayInMS);
        }
    }

    private static async Task<MatchGenerator> CreateMatchGeneratorPrompt()
    {
        var leaderboardId = await SelectLeaderboard();

        var seed = SelectSeed();
        var howManyPlayers = SelectHowManyPlayers();
        var playersPerMatch = SelectPlayersPerMatch();
        var scoresFrom = SelectScoreFrom();
        var scoresTo = SelectScoreTo();
        var generator = new MatchGenerator();
        await generator.Load(seed, leaderboardId, howManyPlayers, playersPerMatch, scoresFrom, scoresTo);
        return generator;
    }

    private static async Task HandleGenerateOne()
    {
        MatchGenerator generator = await CreateMatchGeneratorPrompt();
        var response = await generator.Generate();
        AnsiConsole.WriteLine($"Resulted in {response}");
    }
    private static int SelectSeed()
    {
        var choice = AnsiConsole.Prompt(
            new TextPrompt<int>("Random seed").DefaultValue(1234));

        return choice;
    }

    private static int SelectHowManyPlayers()
    {
        var choice = AnsiConsole.Prompt(
            new TextPrompt<int>("Player pool size").DefaultValue(100));

        return choice;
    }

    private static int SelectPlayersPerMatch()
    {
        var choice = AnsiConsole.Prompt(
            new TextPrompt<int>("Players per match").DefaultValue(2));

        return choice;
    }

    private static int SelectScoreFrom()
    {
        var choice = AnsiConsole.Prompt(
            new TextPrompt<int>("Score from").DefaultValue(600));

        return choice;
    }

    private static int SelectScoreTo()
    {
        var choice = AnsiConsole.Prompt(
            new TextPrompt<int>("Score to").DefaultValue(1800));

        return choice;
    }

    private static int SelectDelayInMS()
    {
        var choice = AnsiConsole.Prompt(
            new TextPrompt<int>("Delay in MS").DefaultValue(25));

        return choice;
    }
}
