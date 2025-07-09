using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Requests;

namespace Service.Contracts;

public interface ITournamentService
{
    Task<(IEnumerable<TournamentDto> tournamentDtos, MetaData metaData)> GetAllAsync(TournamentRequestParams requestParams, bool trackChanges = false);
    Task<TournamentDto?> GetByIdAsync(int id, bool includeGames = false);
    Task<bool> ExistsAsync(int id);
    Task<bool> PatchAsync(int id, JsonPatchDocument<TournamentPatchDto> patchDoc);
    Task<TournamentDto> CreateAsync(TournamentCreateDto dto);
    Task<bool> UpdateAsync(int id, TournamentUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<int> GetTotalCountAsync();
}
