using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Contracts;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Requests;

namespace Tournament.Services;

public class TournamentService(IMapper mapper, IUnitOfWork unitOfWork) : ITournamentService
{
    public async Task<TournamentDto> CreateAsync(TournamentCreateDto dto)
    {
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);
        if (!isValid)
            throw new ValidationException("Invalid game data provided.");

        var tournament = mapper.Map<TournamentDetails>(dto);
        unitOfWork.TournamentRepository.Create(tournament);

        await unitOfWork.CompleteAsync();
        return mapper.Map<TournamentDto>(tournament);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var tournament = await unitOfWork.TournamentRepository.GetAsync(id);
        if (tournament == null)
            return false;

        unitOfWork.TournamentRepository.Delete(tournament);

        await unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var result = await unitOfWork.TournamentRepository.AnyAsync(id);
        return result;
    }

    public async Task<(IEnumerable<TournamentDto> tournamentDtos, MetaData metaData)> GetAllAsync(TournamentRequestParams requestParams, bool trackChanges = false)
    {
        var pagedList = await unitOfWork.TournamentRepository.GetAllAsync(requestParams, trackChanges);
        var tournamentDtos = mapper.Map<IEnumerable<TournamentDto>>(pagedList.Items);

        return (tournamentDtos, pagedList.MetaData);
    }

    public async Task<TournamentDto?> GetByIdAsync(int id, bool includeGames = false)
    {
        var tournament = await unitOfWork.TournamentRepository.GetAsync(id, includeGames);
        return tournament is null ? null : mapper.Map<TournamentDto>(tournament);
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await unitOfWork.TournamentRepository.GetTotalCountAsync();
    }

    public async Task<bool> PatchAsync(int id, JsonPatchDocument<TournamentPatchDto> patchDoc)
    {
        var tournament = await unitOfWork.TournamentRepository.GetAsync(id);
        if (tournament == null)
            return false;

        var tournamentToPatch = mapper.Map<TournamentPatchDto>(tournament);
        patchDoc.ApplyTo(tournamentToPatch);

        var validationContext = new ValidationContext(tournamentToPatch);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(tournamentToPatch, validationContext, validationResults, true);
        if (!isValid)
            return false;

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

        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);
        if (!isValid)
            return false;

        mapper.Map(dto, tournament);
        unitOfWork.TournamentRepository.Update(tournament);

        await unitOfWork.CompleteAsync();
        return true;
    }
}
