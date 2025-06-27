﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories;

public class TournamentRepository(TournamentContext context) : ITournamentRepository
{
    public void Add(TournamentDetails tournament)
    {
        context.TournamentDetails.Add(tournament);
    }

    public async Task<bool> AnyAsync(int id)
    {
        return await context.TournamentDetails.AnyAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<TournamentDetails>> GetAllAsync()
    {
        return await context.TournamentDetails.ToListAsync();
    }

    public async Task<TournamentDetails?> GetAsync(int id, bool includeGames = false)
    {
        IQueryable<TournamentDetails> query = context.TournamentDetails;

        if (includeGames)
        {
            query = query.Include(t => t.Games);
        }

        return await query.FirstOrDefaultAsync(t => t.Id == id);
    }

    public void Remove(TournamentDetails tournament)
    {
        context.TournamentDetails.Remove(tournament);
    }

    public void Update(TournamentDetails tournament)
    {
        context.Entry(tournament).State = EntityState.Modified;
    }
}
