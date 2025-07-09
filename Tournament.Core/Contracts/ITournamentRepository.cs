using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Core.Requests;

namespace Tournament.Core.Contracts;

public interface ITournamentRepository
{
    Task<PagedList<TournamentDetails>> GetAllAsync(TournamentRequestParams requestParams, bool trackChanges = false);
    Task<TournamentDetails?> GetAsync(int id, bool includeGames = false);
    Task<bool> AnyAsync(int id);
    Task<bool> NumberOfGamesAsync(int parentId);
    void Add(TournamentDetails tournament);
    void Update(TournamentDetails tournament);
    void Remove(TournamentDetails tournament);
    Task<int> GetTotalCountAsync();
}
