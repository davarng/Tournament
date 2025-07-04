using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
public class GamesController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameDto>>> GetGames([FromQuery] int page, [FromQuery] int pageSize)
    {
        pageSize = Math.Min(pageSize, 100);

        var games = await serviceManager.GameService.GetAllAsync(page, pageSize);

        var totalCount = await serviceManager.GameService.GetTotalCountAsync();

        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        return Ok(new
        {
            data = games,
            metaData = new
            {
                totalPages,
                pageSize,
                currentPage = page,
                totalItems = totalCount
            }
        });
    }

    [HttpGet("{title}")]
    public async Task<ActionResult<GameDto>> GetGame(string title)
    {
        var game = await serviceManager.GameService.GetByTitleAsync(title);

        if (game == null)
            return NotFound();

        return Ok(game);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutGame(int id, [FromBody] GameUpdateDto gameDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await serviceManager.GameService.UpdateAsync(id, gameDto);

        if (!result)
            return NotFound("Game not found or update failed.");

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<GameDto>> PostGame([FromBody] GameCreateDto gameDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var dto = await serviceManager.GameService.CreateAsync(gameDto);

        if (dto == null)
            return BadRequest("Failed to create game.");

        return CreatedAtAction(nameof(GetGame), new { title = dto.Title }, dto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGame(int id)
    {
        var result = await serviceManager.GameService.DeleteAsync(id);

        if (!result)
            return NotFound("Game not found or delete failed.");

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchGame(int id, [FromBody] JsonPatchDocument<GamePatchDto> patchDoc)
    {
        if (patchDoc == null)
            return BadRequest();

        var result = await serviceManager.GameService.PatchAsync(id, patchDoc);

        if (!result)
            return NotFound("Game not found or patch failed.");

        return NoContent();
    }

    private async Task<bool> GameExists(int id)
    {
        var game = await serviceManager.GameService.GetByIdAsync(id);
        return game != null;
    }
}
