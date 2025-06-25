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
        return await context.Game
            .FirstOrDefaultAsync(g => g.Title == title);
    }
    public void Add(Game game)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AnyAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Game>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Game> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    public void Remove(Game game)
    {
        throw new NotImplementedException();
    }

    public void Update(Game game)
    {
        context.Entry(game).State = EntityState.Modified;
    }
}
