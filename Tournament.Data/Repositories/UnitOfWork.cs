using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Repositories;

namespace Tournament.Data.Repositories;

public class UnitOfWork(
    DbContext context,
    ITournamentRepository tournamentRepository,
    IGameRepository gameRepository) : IUnitOfWork
{
    public ITournamentRepository TournamentRepository => tournamentRepository;

    public IGameRepository GameRepository => gameRepository;

    public async Task CompleteAsync() => await context.SaveChangesAsync();
}
