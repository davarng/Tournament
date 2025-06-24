using AutoMapper;
using Microsoft.AspNetCore.Http;
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

    [HttpGet("{id}")]
    public async Task<ActionResult<GameDto>> GetGame(int id)
    {
        var game = await unitOfWork.GameRepository.GetAsync(id);

        if (game == null)
        {
            return NotFound();
        }

        return mapper.Map<GameDto>(game);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutGame(int id, GameDto gameDto)
    {
        var game = mapper.Map<Game>(gameDto);
        unitOfWork.GameRepository.Update(game);

        try
        {
            await unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await GameExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<GameDto>> PostGame(GameDto gameDto)
    {
        var game = mapper.Map<Game>(gameDto);
        unitOfWork.GameRepository.Add(game);
        await unitOfWork.CompleteAsync();

        return CreatedAtAction("GetGame", new { id = game.Id }, mapper.Map<GameDto>(game));
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

    private async Task<bool> GameExists(int id)
    {
        var game = await unitOfWork.GameRepository.GetAsync(id);
        return game != null;
    }
}
