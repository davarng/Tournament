using Service.Contracts;
using Tournament.Core.Contracts;
using Tournament.Data.Repositories;
using Tournament.Services;

namespace Tournament.Api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(builder =>
        {
            builder.AddPolicy("AllowAll", p =>
            p.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
        });
    }

    public static void ConfigureServiceLayerServices(this IServiceCollection services)
    {
        services.AddScoped<ITournamentService, TournamentService>();
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IServiceManager, ServiceManager>();
        services.AddLazy<ITournamentService>();
        services.AddLazy<IGameService>();
    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<ITournamentRepository, TournamentRepository>();
        services.AddLazy<IGameRepository>();
        services.AddLazy<ITournamentRepository>();
    }
}
public static class ServiceManagerExtensions
{
    public static IServiceCollection AddLazy<TService>(this IServiceCollection services) where TService : class
    {
        return services.AddScoped(provide => new Lazy<TService>(() => provide.GetRequiredService<TService>()));
    }
}
