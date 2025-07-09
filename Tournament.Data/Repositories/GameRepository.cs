using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Contracts;
using Tournament.Core.Entities;
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

    public async Task<IEnumerable<Game>> GetAllAsync(int page, int pageSize)
    {
        return await context.Games.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
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

    public async Task<int> GetTotalCountAsync()
    {
        return await context.Games.CountAsync();
    }
}
