using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Repositories;

namespace Tournament.Services
{
    public class TournamentService(IMapper mapper, IUnitOfWork unitOfWork) : ITournamentService
    {
        public Task<TournamentDto> CreateAsync(TournamentCreateDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TournamentDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TournamentDto?> GetByIdAsync(int id, bool includeGames = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PatchAsync(int id, JsonPatchDocument<TournamentPatchDto> patchDoc)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(int id, TournamentUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
