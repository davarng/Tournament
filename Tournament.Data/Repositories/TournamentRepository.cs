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

public class TournamentRepository : RepositoryBase<TournamentDetails>, ITournamentRepository
{
    public TournamentRepository(TournamentContext context) : base(context)
    {

    }

    public async Task<bool> AnyAsync(int id)
    {
        return await Context.TournamentDetails.AnyAsync(t => t.Id == id);
    }

    public async Task<PagedList<TournamentDetails>> GetAllAsync(TournamentRequestParams requestParams, bool trackChanges = false)
    {
        var tournaments = requestParams.IncludeGames ? FindAll(trackChanges).Include(t => t.Games)
                                                     : FindAll(trackChanges);

        return await PagedList<TournamentDetails>
            .CreateAsync(tournaments, requestParams.PageNumber, requestParams.PageSize);
    }

    public async Task<TournamentDetails?> GetAsync(int id, bool includeGames = false)
    {
        IQueryable<TournamentDetails> query = Context.TournamentDetails;

        if (includeGames)
        {
            query = query.Include(t => t.Games);
        }

        return await query.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await Context.TournamentDetails.CountAsync();
    }

    public async Task<bool> NumberOfGamesAsync(int id)
    {
        var count = await Context.Games
        .Where(g => g.TournamentId == id)
        .CountAsync();

        return count < 10;
    }
}
