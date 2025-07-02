using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesController(IUnitOfWork unitOfWork, IMapper mapper, IServiceManager service) : ControllerBase
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
            return NotFound();

        return Ok(mapper.Map<GameDto>(game));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutGame(int id, [FromBody] GameUpdateDto gameDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingGame = await unitOfWork.GameRepository.GetAsync(id);
        if (existingGame == null)
            return NotFound();

        mapper.Map(gameDto, existingGame);
        unitOfWork.GameRepository.Update(existingGame);

        try
        {
            await unitOfWork.CompleteAsync();
        }
        catch
        {
            return StatusCode(500, "Failed to update game.");
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<GameDto>> PostGame([FromBody] GameCreateDto gameDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var game = mapper.Map<Game>(gameDto);
        unitOfWork.GameRepository.Add(game);

        try
        {
            await unitOfWork.CompleteAsync();
        }
        catch
        {
            return StatusCode(500, "Failed to create game.");
        }

        return CreatedAtAction(nameof(GetGame), new { title = game.Title }, mapper.Map<GameDto>(game));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGame(int id)
    {
        var game = await unitOfWork.GameRepository.GetAsync(id);
        if (game == null)
            return NotFound();

        unitOfWork.GameRepository.Remove(game);

        try
        {
            await unitOfWork.CompleteAsync();
        }
        catch
        {
            return StatusCode(500, "Failed to delete game.");
        }

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchGame(int id, [FromBody] JsonPatchDocument<GamePatchDto> patchDoc)
    {
        var game = await unitOfWork.GameRepository.GetAsync(id);
        if (game == null)
            return NotFound("Game not found");

        var dto = mapper.Map<GamePatchDto>(game);
        patchDoc.ApplyTo(dto, ModelState);

        if (!ModelState.IsValid || !TryValidateModel(dto))
            return BadRequest(ModelState);

        mapper.Map(dto, game);
        unitOfWork.GameRepository.Update(game);

        try
        {
            await unitOfWork.CompleteAsync();
        }
        catch
        {
            return StatusCode(500, "Failed to patch game.");
        }

        return NoContent();
    }

    private async Task<bool> GameExists(int id)
    {
        var game = await unitOfWork.GameRepository.GetAsync(id);
        return game != null;
    }
}
