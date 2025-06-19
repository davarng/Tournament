using Bogus;
using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;
using Tournament.Data;
using Tournament.Data.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Tournament.Api.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static async Task SeedDataAsync(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var db = serviceProvider.GetRequiredService<TournamentContext>();

                await db.Database.MigrateAsync();
                if (await db.TournamentDetails.AnyAsync())
                {
                    return;
                }

                try
                {
                    var tournaments = SeedData.GenerateTournamentDetails(5);
                    db.TournamentDetails.AddRange(tournaments);
                    await db.SaveChangesAsync();

                    foreach (var tournament in tournaments)
                    {
                        var games = SeedData.GenerateGames(3, tournament.Id);
                        tournament.Games = games;
                    }

                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
