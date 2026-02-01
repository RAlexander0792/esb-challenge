using BenchmarkDotNet.Running;
using ESBC.Benchmarks.Resolvers;
using Spectre.Console;

namespace ESBC.UtilitiesCli.TUI;

enum MainMenuOptions
{
    RunBenchmark,
    PlotCharts,
    DataGeneration,
    Exit
}
public class MainMenu
{
    public static async Task Load()
    {
        bool leaving = false;
        while (!leaving)
        {
            AnsiConsole.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuOptions>()
                   .Title("Select an option")
                   .AddChoices(Enum.GetValues<MainMenuOptions>()));

            AnsiConsole.MarkupLine($"[green]You selected:[/] {choice}");
            switch (choice)
            {
                case MainMenuOptions.RunBenchmark:
                    BenchmarkRunner.Run<ClassicEloResolverBench>();
                    break;

                case MainMenuOptions.PlotCharts:
                    var bench = new ClassicEloResolverBench();
                    bench.PlotRating_2Players_100Matches_25PlayerPool();
                    break;

                case MainMenuOptions.DataGeneration:
                    await DataGenerationMenu.Load();
                    break;

                case MainMenuOptions.Exit:
                    leaving = true;
                    return;
            }
        }
    }
}
