using Bogus;
using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;
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
                    return; // Database has been seeded
                }

                try
                {
                    var tournamentDetails = GenerateTournamentDetails(5);
                    db.AddRange(tournamentDetails);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }

        private static int tournamentIdCounter = 1;

        private static List<TournamentDetails> GenerateTournamentDetails(int nrOfTournamentDetails)
        {
            var faker = new Faker<TournamentDetails>("sv")
                .RuleFor(t => t.Id, _ => tournamentIdCounter++)
                .RuleFor(t => t.Title, f => f.Company.CatchPhrase())
                .RuleFor(t => t.StartDate, f => f.Date.Future(365))
                .RuleFor(t => t.Games, (f, t) => GenerateGames(f.Random.Int(2, 10), t.Id));

            return faker.Generate(nrOfTournamentDetails);
        }

        private static ICollection<Game> GenerateGames(int nrOfGames, int tournamentId)
        {
            var faker = new Faker<Game>("sv")
                .RuleFor(g => g.Title, f => f.Lorem.Sentence(3))
                .RuleFor(g => g.Time, f => f.Date.Future(365))
                .RuleFor(g => g.TournamentId, _ => tournamentId);

            return faker.Generate(nrOfGames);
        }
    }
}
