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
using Tournament.Core.Contracts;
using Tournament.Data.Data;
using Tournament.Core.Requests;
using System.Text.Json;

namespace Tournament.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameDto>>> GetGames([FromQuery] RequestParams requestParam)
    {
        var pagedResult = await serviceManager.GameService.GetAllAsync(requestParam);
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

        return Ok(pagedResult.gameDtos);
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

        try
        {
            var dto = await serviceManager.GameService.CreateAsync(gameDto);
            return CreatedAtAction(nameof(GetGame), new { id = dto.Id }, dto);
        }
        catch (InvalidOperationException ex)
        {
            Response.ContentType = "application/json";
            return Problem(
                title: "Invalid Operation",
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest
                );
        }
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
