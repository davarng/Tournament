using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;

namespace Tournament.Services
{
    public class GameService(IMapper mapper, IUnitOfWork unitOfWork) : IGameService
    {
        public async Task<GameDto> CreateAsync(GameCreateDto dto)
        {
            var game = mapper.Map<Game>(dto);
            unitOfWork.GameRepository.Add(game);

            await unitOfWork.CompleteAsync();
            return mapper.Map<GameDto>(game);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var game = await unitOfWork.GameRepository.GetAsync(id);
            if (game == null)
                return false;

            unitOfWork.GameRepository.Remove(game);

            await unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var result = await unitOfWork.GameRepository.AnyAsync(id);
            return result;
        }

        public Task<IEnumerable<GameDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GameDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<GameDto?> GetByTitleAsync(string title)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PatchAsync(int id, JsonPatchDocument<GamePatchDto> patchDoc)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(int id, GameUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
