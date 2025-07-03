using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;

namespace Service.Contracts;

public interface IGameService
{
    Task<IEnumerable<GameDto>> GetAllAsync();
    Task<GameDto?> GetByIdAsync(int id);
    Task<GameDto?> GetByTitleAsync(string title);
    Task<bool> ExistsAsync(int id);
    Task<GameDto> CreateAsync(GameCreateDto dto);
    Task<bool> PatchAsync(int id, JsonPatchDocument<GamePatchDto> patchDoc);
    Task<bool> UpdateAsync(int id, GameUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
