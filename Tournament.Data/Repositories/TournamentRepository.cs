using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;

namespace Tournament.Data.Repositories;

public class TournamentRepository(DbContext context) : ITournamentRepository
{
    public void Add(TournamentDetails tournament)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AnyAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TournamentDetails>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<TournamentDetails> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    public void Remove(TournamentDetails tournament)
    {
        throw new NotImplementedException();
    }

    public void Update(TournamentDetails tournament)
    {
        context.Entry(tournament).State = EntityState.Modified;
    }
}
