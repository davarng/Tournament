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
    /// <summary>
    /// Gets all games with pagination support.
    /// </summary>
    /// <param name="requestParam">Data needed for pagination.</param>
    /// <returns>200, a list of games from the specified size/page and meta data.</returns>
    /// <response code="200">Returns a list of games.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GameDto>), StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<ActionResult> GetGames([FromQuery] RequestParams requestParam)
    {
        var pagedResult = await serviceManager.GameService.GetAllAsync(requestParam);
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

        return Ok(new
        {
            games = pagedResult.gameDtos,
            metaData = pagedResult.metaData
        });
    }

    /// <summary>
    /// Gets the game with the specified title.
    /// </summary>
    /// <param name="title">Title of game that you want to get.</param>
    /// <returns>200 and the games info.</returns>
    /// <response code ="200">Returns the requested game.</response>
    [HttpGet("{title}")]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<ActionResult<GameDto>> GetGame(string title)
    {
        var game = await serviceManager.GameService.GetByTitleAsync(title);

        if (game == null)
            return NotFound();

        return Ok(game);
    }

    /// <summary>
    /// Updates the game with the specified id using the provided game data.
    /// </summary>
    /// <param name="id">The id of the game you want to update.</param>
    /// <param name="gameDto">Data used to update the game.</param>
    /// <returns>No content if update is successful.</returns>
    /// <response code="204">No content if the update is successful.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Consumes("application/json")]
    public async Task<IActionResult> PutGame(int id, [FromBody] GameUpdateDto gameDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await serviceManager.GameService.UpdateAsync(id, gameDto);

        if (!result)
            return NotFound("Game not found or update failed.");

        return NoContent();
    }

    /// <summary>
    /// Creates a game with the provided details.
    /// </summary>
    /// <param name="gameDto">Game data used to create the game.</param>
    /// <returns>201 with the created game.</returns>
    /// <response code="201">Returns the created game.</response>
    [HttpPost]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<ActionResult<GameDto>> PostGame([FromBody] GameCreateDto gameDto)
    {
        var dto = await serviceManager.GameService.CreateAsync(gameDto);
        return CreatedAtAction(nameof(GetGame), new { id = dto.Id }, dto);
    }

    /// <summary>
    /// Deletes the game with the specified id.
    /// </summary>
    /// <param name="id">Id of the game you want to delete.</param>
    /// <returns>No content if the update is successful</returns>
    /// <response code="204">No content if the delete is successful.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGame(int id)
    {
        var result = await serviceManager.GameService.DeleteAsync(id);

        if (!result)
            return NotFound("Game not found or delete failed.");

        return NoContent();
    }

    /// <summary>
    /// Partial update for the game with the specified id using JSON Patch.
    /// </summary>
    /// <param name="id">Id of game you want to update.</param>
    /// <param name="patchDoc">The patch document that is used to change the values of game.</param>
    /// <returns>No content if the update is successful.</returns>
    /// <response code ="204">No content if the patch is successful.</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Consumes("application/json-patch+json")]
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
