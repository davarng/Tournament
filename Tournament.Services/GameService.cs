using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Http;
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
using Tournament.Core.Exceptions;
using Tournament.Core.Requests;

namespace Tournament.Services;

public class GameService(IMapper mapper, IUnitOfWork unitOfWork) : IGameService
{
    public async Task<GameDto> CreateAsync(GameCreateDto dto)
    {
        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);
        if (!isValid)
        {
            var errorMessage = $"Validation failed for the following reasons:{Environment.NewLine}" +
                $"{string.Join(Environment.NewLine, validationResults.Select(vr => $"- {vr.ErrorMessage}"))}";
            throw new ApiException(StatusCodes.Status400BadRequest, "Invalid data", errorMessage);
        }

        var lessThanTenGames = await unitOfWork.TournamentRepository.NumberOfGamesAsync(dto.TournamentId);

        if (!lessThanTenGames)
            throw new ApiException(StatusCodes.Status400BadRequest, "Invalid operation", "Cannot add more than 10 games to a tournament.");

        var game = mapper.Map<Game>(dto);
        unitOfWork.GameRepository.Create(game);

        await unitOfWork.CompleteAsync();
        return mapper.Map<GameDto>(game);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var game = await unitOfWork.GameRepository.GetAsync(id);
        if (game == null)
            return false;

        unitOfWork.GameRepository.Delete(game);

        await unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var result = await unitOfWork.GameRepository.AnyAsync(id);
        return result;
    }

    public async Task<(IEnumerable<GameDto> gameDtos, MetaData metaData)> GetAllAsync(RequestParams requestParams, bool trackChanges = false)
    {
        var pagedList = await unitOfWork.GameRepository.GetAllAsync(requestParams, trackChanges);
        var gameDtos = mapper.Map<IEnumerable<GameDto>>(pagedList.Items);

        return (gameDtos, pagedList.MetaData);
    }

    public async Task<GameDto?> GetByIdAsync(int id)
    {
        var game = await unitOfWork.GameRepository.GetAsync(id);
        return game is null ? null : mapper.Map<GameDto>(game);
    }

    public async Task<GameDto?> GetByTitleAsync(string title)
    {
        var game = await unitOfWork.GameRepository.GetTitleAsync(title);
        return game is null ? null : mapper.Map<GameDto>(game);
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await unitOfWork.GameRepository.GetTotalCountAsync();
    }

    public async Task<bool> PatchAsync(int id, JsonPatchDocument<GamePatchDto> patchDoc)
    {
        var game = await unitOfWork.GameRepository.GetAsync(id);
        if (game == null)
            return false;

        var gameToPatch = mapper.Map<GamePatchDto>(game);
        patchDoc.ApplyTo(gameToPatch);

        var validationContext = new ValidationContext(gameToPatch);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(gameToPatch, validationContext, validationResults, true);
        if (!isValid)
            return false;

        var updatedGame = mapper.Map<Game>(gameToPatch);
        unitOfWork.GameRepository.Update(updatedGame);

        await unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(int id, GameUpdateDto dto)
    {
        var game = await unitOfWork.GameRepository.GetAsync(id);
        if (game == null)
            return false;

        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);
        if (!isValid)
            return false;

        mapper.Map(dto, game);
        unitOfWork.GameRepository.Update(game);

        await unitOfWork.CompleteAsync();
        return true;
    }


}
