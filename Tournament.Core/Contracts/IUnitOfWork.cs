using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Contracts;

public interface IUnitOfWork
{
    ITournamentRepository TournamentRepository { get; }
    IGameRepository GameRepository { get; }
    Task CompleteAsync();
}
