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

namespace Tournament.Services;

public class TournamentService(IMapper mapper, IUnitOfWork unitOfWork) : ITournamentService
{
    public async Task<TournamentDto> CreateAsync(TournamentCreateDto dto)
    {
        var tournament = mapper.Map<TournamentDetails>(dto);
        unitOfWork.TournamentRepository.Add(tournament);

        await unitOfWork.CompleteAsync();
        return mapper.Map<TournamentDto>(tournament);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var tournament = await unitOfWork.TournamentRepository.GetAsync(id);
        if (tournament == null)
            return false;

        unitOfWork.TournamentRepository.Remove(tournament);

        await unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var result = await unitOfWork.TournamentRepository.AnyAsync(id);
        return result;
    }

    public async Task<IEnumerable<TournamentDto>> GetAllAsync()
    {
        var tournament = await unitOfWork.TournamentRepository.GetAllAsync();
        return mapper.Map<IEnumerable<TournamentDto>>(tournament);
    }

    public async Task<TournamentDto?> GetByIdAsync(int id, bool includeGames = false)
    {
        var tournament = await unitOfWork.TournamentRepository.GetAsync(id, includeGames);
        return tournament is null ? null : mapper.Map<TournamentDto>(tournament);
    }

    public async Task<bool> PatchAsync(int id, JsonPatchDocument<TournamentPatchDto> patchDoc)
    {
        var tournament = await unitOfWork.TournamentRepository.GetAsync(id);
        if (tournament == null)
            return false;

        var tournamentToPatch = mapper.Map<TournamentPatchDto>(tournament);
        patchDoc.ApplyTo(tournamentToPatch);

        var updatedTournament = mapper.Map<TournamentDetails>(tournamentToPatch);
        unitOfWork.TournamentRepository.Update(updatedTournament);

        await unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(int id, TournamentUpdateDto dto)
    {
        var tournament = await unitOfWork.TournamentRepository.GetAsync(id);
        if (tournament == null)
            return false;

        mapper.Map(dto, tournament);
        unitOfWork.TournamentRepository.Update(tournament);

        await unitOfWork.CompleteAsync();
        return true;
    }
}
