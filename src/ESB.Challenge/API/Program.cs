using Asp.Versioning;
using ESBC.Application;
using ESBC.Core;
using ESBC.Data;
using Microsoft.AspNetCore.HttpLogging;

namespace ESBC.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOptions<OptionsContext>()
        .BindConfiguration("Options")
        .Validate(o => !string.IsNullOrWhiteSpace(o.DbConnectionString), "ConnectionString is required")
        .Validate(o => !string.IsNullOrWhiteSpace(o.DatabaseName), "DatabaseName is required")
        .ValidateOnStart();
        // Add services to the container.

        builder.Services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        });

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
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
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
