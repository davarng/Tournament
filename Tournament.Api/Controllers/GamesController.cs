using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameDto>>> GetGame()
    {
        var games = await unitOfWork.GameRepository.GetAllAsync();
        return Ok(games.Select(g => mapper.Map<GameDto>(g)));
    }

    [HttpGet("{title}")]
    public async Task<ActionResult<GameDto>> GetGame(string title)
    {
        var game = await unitOfWork.GameRepository.GetTitleAsync(title);

        if (game == null)
        {
            return NotFound();
        }

        return mapper.Map<GameDto>(game);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutGame(int id, GameUpdateDto gameDto)
    {
        var existingGame = await unitOfWork.GameRepository.GetAsync(id);
        if (existingGame == null)
            return NotFound();

        mapper.Map(gameDto, existingGame);
        unitOfWork.GameRepository.Update(existingGame);

        try
        {
            await unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await GameExists(id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<GameDto>> PostGame(GameCreateDto gameDto)
    {
        var game = mapper.Map<Game>(gameDto);
        unitOfWork.GameRepository.Add(game);
        await unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(GetGame), new { title = game.Title }, mapper.Map<GameDto>(game));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGame(int id)
    {
        var game = await unitOfWork.GameRepository.GetAsync(id);
        if (game == null)
        {
            return NotFound();
        }
        unitOfWork.GameRepository.Remove(game);
        await unitOfWork.CompleteAsync();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchGame(int id, JsonPatchDocument<GamePatchDto> patchDoc)
    {
        var game = await unitOfWork.GameRepository.GetAsync(id);
        if (game == null)
            return NotFound("Game not found");

        var dto = mapper.Map<GamePatchDto>(game);
        patchDoc.ApplyTo(dto, ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!TryValidateModel(dto))
            return BadRequest("Invalid game patch data");

        mapper.Map(dto, game);
        unitOfWork.GameRepository.Update(game);
        await unitOfWork.CompleteAsync();

        return NoContent();
    }

    private async Task<bool> GameExists(int id)
    {
        var game = await unitOfWork.GameRepository.GetAsync(id);
        return game != null;
    }
}
