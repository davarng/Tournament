using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Reflection.Metadata;
using Tournament.Api.Extensions;
using Tournament.Core.Contracts;
using Tournament.Data.Data;
using Tournament.Data.Repositories;

namespace Tournament.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<TournamentContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("TournamentApiContext") ?? throw new InvalidOperationException("Connection string 'TournamentApiContext' not found.")));

        builder.Services.AddControllers(opt => opt.ReturnHttpNotAcceptable = true)
            .AddNewtonsoftJson()
            .AddXmlDataContractSerializerFormatters()
            .AddApplicationPart(typeof(AssemblyReference).Assembly);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(setupAction =>
        {
            var xmlCommentsFile = $"{typeof(Tournament.Presentation.AssemblyReference).Assembly.GetName().Name}.xml";
            var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
            setupAction.IncludeXmlComments(xmlCommentsFullPath);

            var xmlCommentsFileCore = $"{typeof(Tournament.Core.AssemblyReference).Assembly.GetName().Name}.xml";
            var xmlCommentsFullPathCore = Path.Combine(AppContext.BaseDirectory, xmlCommentsFileCore);
            setupAction.IncludeXmlComments(xmlCommentsFullPathCore);
        });
        builder.Services.AddAutoMapper(typeof(TournamentMappings));

        builder.Services.ConfigureServiceLayerServices();
        builder.Services.ConfigureRepositories();

        builder.Services.AddApiVersioning(setupAction =>
        {
            setupAction.ReportApiVersions = true;
            setupAction.AssumeDefaultVersionWhenUnspecified = true;
            setupAction.DefaultApiVersion = new ApiVersion(1, 0);
        }).AddMvc();


        var app = builder.Build();

        app.ConfigureExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            await app.SeedDataAsync();
        }

        app.UseHttpsRedirection();

        app.UseCors("AllowAll");

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
