using Asp.Versioning;
using Mgls.Application;
using Mgls.Data;
using Mgls.Shared;
using Microsoft.AspNetCore.HttpLogging;

namespace Mgls.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOptions<OptionsContext>()
        .BindConfiguration("Options")
        .Validate(o => !string.IsNullOrWhiteSpace(o.DbConnectionString), "ConnectionString is required")
        .Validate(o => !string.IsNullOrWhiteSpace(o.DatabaseName), "DatabaseName is required")
        .ValidateOnStart();

        builder.Services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        });

        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });

        builder.Services.AddHttpLogging(o =>
        {
            o.CombineLogs = true;
            o.LoggingFields = HttpLoggingFields.RequestQuery | HttpLoggingFields.RequestMethod |
                              HttpLoggingFields.RequestPath | HttpLoggingFields.RequestBody |
                              HttpLoggingFields.ResponseStatusCode | HttpLoggingFields.ResponseBody |
                              HttpLoggingFields.Duration;
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDatabase();
        builder.Services.AddApplicationServices();

        builder.Services.AddBackgroundServices();


        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            await DBIndexRegister.EnsureIX(app.Services);

            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpLogging();
        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
