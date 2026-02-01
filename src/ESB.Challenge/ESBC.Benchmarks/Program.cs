using ESBC.Application;
using ESBC.Data;
using ESBC.UtilitiesCli;
using ESBC.UtilitiesCli.ApiProviders;
using ESBC.UtilitiesCli.TUI;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace ESBC.Benchmarks;

public class Program
{
    static async Task Main(string[] args)
    {


        DependencyManager.Initiate(); 

        await MainMenu.Load();
    }
}

