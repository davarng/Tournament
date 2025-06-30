using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories;

public class GameRepository(TournamentContext context) : IGameRepository
{
    public async Task<Game?> GetTitleAsync(string title)
    {
        return await context.Games
            .FirstOrDefaultAsync(g => g.Title == title);
    }
    public void Add(Game game)
    {
        context.Games.Add(game);
    }

    public async Task<bool> AnyAsync(int id)
    {
        return await context.Games.AnyAsync(g => g.Id == id);
    }

    public async Task<IEnumerable<Game>> GetAllAsync()
    {
        return await context.Games.ToListAsync();
    }

    public async Task<Game?> GetAsync(int id)
    {
        return await context.Games.FindAsync(id);
    }

    public void Remove(Game game)
    {
        context.Games.Remove(game);
    }

    public void Update(Game game)
    {
        context.Entry(game).State = EntityState.Modified;
    }
}
