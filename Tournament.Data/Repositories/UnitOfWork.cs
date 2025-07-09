using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Contracts;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly TournamentContext _context;

    public ITournamentRepository TournamentRepository { get; }
    public IGameRepository GameRepository { get; }

    public UnitOfWork(TournamentContext context)
    {
        _context = context;
        TournamentRepository = new TournamentRepository(context);
        GameRepository = new GameRepository(context);
    }

    public async Task CompleteAsync() => await _context.SaveChangesAsync();
}
