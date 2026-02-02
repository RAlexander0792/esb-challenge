using BenchmarkDotNet.Running;
using Mgls.Benchmarks.Resolvers;
using Mgls.UtilitiesCli.PersistentDataGeneration;
using Spectre.Console;

namespace Mgls.UtilitiesCli.TUI;

enum DataGenerationMenuOptions
{
    Player,
    Match,
    Exit
}

public class DataGenerationMenu
{
    public static async Task Load()
    {
        bool leaving = false;
        while (!leaving)
        {
            AnsiConsole.Clear();
            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<DataGenerationMenuOptions>()
               .Title("Select a generator")
               .AddChoices(Enum.GetValues<DataGenerationMenuOptions>()));

            AnsiConsole.MarkupLine($"[green]You selected:[/] {choice}");
            switch (choice)
            {
                case DataGenerationMenuOptions.Player:
                    await HandlePlayerDataGeneration();
                    break;
                case DataGenerationMenuOptions.Match:
                    await DataGenerationMatchMenu.Load();
                    break;

                case DataGenerationMenuOptions.Exit:
                    leaving = true;
                    return;
            }
        }
        
    }

    private static async Task HandlePlayerDataGeneration()
    {
        var playerGenerator = new PlayerGenerator();

        var howMany = AnsiConsole.Prompt(new TextPrompt<int>("How many?"));

        AnsiConsole.Prompt(new ConfirmationPrompt($"About to insert {howMany} new players"));
        await playerGenerator.CreateManyTestPlayer(howMany);
    }
}
