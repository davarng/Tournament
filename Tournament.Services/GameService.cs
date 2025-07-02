using AutoMapper;
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
    public class GameService(IMapper mapper, IUnitOfWork unitOfWork) : IGameService
    {
        public Task<GameDto> CreateAsync(GameCreateDto dto)
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

        public Task<bool> UpdateAsync(int id, GameUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
