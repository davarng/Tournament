using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Contracts;
using Tournament.Core.Entities;
using Tournament.Core.Requests;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories;

public class GameRepository : RepositoryBase<Game>, IGameRepository
{
    public GameRepository(TournamentContext context) : base(context)
    {

    }

    public async Task<Game?> GetTitleAsync(string title)
    {
        return await Context.Games
            .FirstOrDefaultAsync(g => g.Title == title);
    }
    public void Add(Game game)
    {
        Context.Games.Add(game);
    }

    public async Task<bool> AnyAsync(int id)
    {
        return await Context.Games.AnyAsync(g => g.Id == id);
    }

    public async Task<PagedList<Game>> GetAllAsync(RequestParams requestParams, bool trackChanges = false)
    {
        var games = FindAll(trackChanges);
        return await PagedList<Game>.CreateAsync(games, requestParams.PageNumber, requestParams.PageSize);
    }

    public async Task<Game?> GetAsync(int id)
    {
        return await Context.Games.FindAsync(id);
    }

    public void RemoveGame(Game game)
    {
        Context.Games.Remove(game);
    }

    public void UpdateGame(Game game)
    {
        Context.Entry(game).State = EntityState.Modified;
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await Context.Games.CountAsync();
    }
}
