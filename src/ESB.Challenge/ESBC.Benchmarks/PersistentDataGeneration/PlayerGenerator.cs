using Bogus;
using ESBC.API.Controllers.Dtos;
using ESBC.UtilitiesCli.ApiProviders;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace ESBC.UtilitiesCli.PersistentDataGeneration;

public class PlayerGenerator
{
    private readonly IPlayerProvider _playerProvider;
    private readonly Faker faker = new Faker();
    public PlayerGenerator()
    {
        _playerProvider = DependencyManager.ServiceProvider.GetRequiredService<IPlayerProvider>();
    }

    public async Task CreateTestPlayer()
    {
        string username = faker.Internet.UserName();
        var response = await _playerProvider.PostPlayer(new CreatePlayerRequest() { Name = username });
        AnsiConsole.WriteLine($"Creating user \"{username}\" resulted in status code {response.StatusCode}");
    }

    public async Task CreateManyTestPlayer(int howMany)
    {
        for (int i = 0; i < howMany; i++)
        {
            await CreateTestPlayer();
        }
    }
}
