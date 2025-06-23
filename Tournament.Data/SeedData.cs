using Bogus;
using Tournament.Core.Entities;

namespace Tournament.Data;

public class SeedData
{
    public static List<TournamentDetails> GenerateTournamentDetails(int nrOfTournamentDetails)
    {
        var faker = new Faker<TournamentDetails>("sv")
            .RuleFor(t => t.Title, f => f.Company.CatchPhrase())
            .RuleFor(t => t.StartDate, f => f.Date.Future(365))
            .RuleFor(t => t.Games, _ => new List<Game>());

        return faker.Generate(nrOfTournamentDetails);
    }

    public static ICollection<Game> GenerateGames(int nrOfGames, int tournamentId)
    {
        var faker = new Faker<Game>("sv")
            .RuleFor(g => g.Title, f => f.Lorem.Sentence(3))
            .RuleFor(g => g.Time, f => f.Date.Future(365))
            .RuleFor(g => g.TournamentId, _ => tournamentId);

        return faker.Generate(nrOfGames);
    }
}
