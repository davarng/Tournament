using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Core.Requests;

namespace Tournament.Core.Contracts;

public interface IGameRepository
{
    Task<PagedList<Game>> GetAllAsync(RequestParams requestParams, bool trackChanges = false);
    Task<Game?> GetAsync(int id);
    Task<bool> AnyAsync(int id);
    Task<Game?> GetTitleAsync(string title);
    void Create(Game game);
    void Update(Game game);
    void Delete(Game game);
    Task<int> GetTotalCountAsync();
}
