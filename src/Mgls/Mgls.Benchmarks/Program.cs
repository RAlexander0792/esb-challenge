using Mgls.Application;
using Mgls.Data;
using Mgls.UtilitiesCli;
using Mgls.UtilitiesCli.ApiProviders;
using Mgls.UtilitiesCli.TUI;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Mgls.Benchmarks;

public class Program
{
    static async Task Main(string[] args)
    {

        DependencyManager.Initiate(); 

        await MainMenu.Load();
    }
}

