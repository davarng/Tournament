using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;

namespace Service.Contracts
{
    public interface ITournamentService
    {
        Task<IEnumerable<TournamentDto>> GetAllAsync();
        Task<TournamentDto?> GetByIdAsync(int id, bool includeGames = false);
        Task<bool> ExistsAsync(int id);

        Task<TournamentDto> CreateAsync(TournamentCreateDto dto);
        Task<bool> UpdateAsync(int id, TournamentUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
