using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Contracts;
using Tournament.Data.Data;
using Tournament.Data.Repositories;
using Tournament.Core.Requests;
using System.Text.Json;

namespace Tournament.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TournamentDetailsController(IServiceManager serviceManager) : ControllerBase
{
    /// <summary>
    /// Gets all TournamentDetails with pagination support.
    /// </summary>
    /// <param name="requestParams">Pagination data.</param>
    /// <returns>200, a list of TournamentDetails from the specified size/page and meta data.</returns>
    /// <response code ="200">Returns a list of TournamentDetails.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TournamentDto>), StatusCodes.Status200OK)]
    [Produces("application/json")]
    public async Task<ActionResult<IEnumerable<TournamentDto>>> GetAllTournamentDetails([FromQuery] TournamentRequestParams requestParams)
    {
        var pagedResult = await serviceManager.TournamentService.GetAllAsync(requestParams, false);
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

        return Ok(pagedResult.tournamentDtos);
    }

    /// <summary>
    /// Gets the TournamentDetails with the specified id and optionally includes games.
    /// </summary>
    /// <param name="id">Id of the TournamentDetails you want to get.</param>
    /// <param name="includeGames">Bool that decides whether the games belonging to TournamentDetails should be included.</param>
    /// <returns>200 and the TournamentDetails. Optionally the games belonging to TournamentDetails.</returns>
    /// <response code="200">Returns the requested TournamentDetails.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TournamentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public async Task<ActionResult<TournamentDto>> GetTournamentDetails(int id, bool includeGames = false)
    {
        var tournament = await serviceManager.TournamentService.GetByIdAsync(id, includeGames);

        if (tournament == null)
            return NotFound();

        return Ok(tournament);
    }

    /// <summary>
    /// Updates the TournamentDetails with the specified id using the provided TournamentDetails data.
    /// </summary>
    /// <param name="id">The id of the TournamentDetails you want to update.</param>
    /// <param name="tournamentUpdateDto">Data used to update the TournamentDetails.</param>
    /// <returns>No content if update is successful.</returns>
    /// <response code ="204">No content if the update is successful.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Consumes("application/json")]
    public async Task<IActionResult> PutTournamentDetails(int id, [FromBody] TournamentUpdateDto tournamentUpdateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        var success = await serviceManager.TournamentService.UpdateAsync(id, tournamentUpdateDto);

        if (!success)
            return NotFound("Tournament not found or update failed.");

        return NoContent();
    }

    /// <summary>
    /// Creates a TournamentDetails with the provided details.
    /// </summary>
    /// <param name="tournamentCreateDto">TournamentDetails data used to create the game.</param>
    /// <returns>201 with the created TournamentDetails.</returns>
    /// <response code ="201">Returns the created TournamentDetails.</response>
    [HttpPost]
    [ProducesResponseType(typeof(TournamentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<TournamentDto>> PostTournamentDetails([FromBody] TournamentCreateDto tournamentCreateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await serviceManager.TournamentService.CreateAsync(tournamentCreateDto);

        if (result == null)
            return StatusCode(500, "Failed to create tournament.");

        return CreatedAtAction(nameof(GetTournamentDetails), new { id = result.Id }, result);
    }

    /// <summary>
    /// Deletes the TournamentDetails with the specified id.
    /// </summary>
    /// <param name="id">Id of the TournamentDetails you want to delete.</param>
    /// <returns>No content if the update is successful</returns>
    /// <response code ="204">No content if the deletion is successful.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTournamentDetails(int id)
    {
        var success = await serviceManager.TournamentService.DeleteAsync(id);

        if (!success)
            return StatusCode(500, "Failed to delete tournament.");

        return NoContent();
    }

    /// <summary>
    /// Partial update for the TournamentDetails with the specified id using JSON Patch.
    /// </summary>
    /// <param name="id">Id of TournamentDetails you want to update.</param>
    /// <param name="patchDoc">The patch document that is used to change the values of TournamentDetails.</param>
    /// <returns>No content if the update is successful.</returns>
    /// <response code ="204">No content if the patch is successful.</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Consumes("application/json-patch+json")]
    public async Task<IActionResult> PatchTournamentDetails(int id, [FromBody] JsonPatchDocument<TournamentPatchDto> patchDoc)
    {
        if (patchDoc == null)
            return BadRequest();

        var result = await serviceManager.TournamentService.PatchAsync(id, patchDoc);

        if (!result)
            return NotFound("Tournament not found or failed to patch");

        return NoContent();
    }

    private async Task<bool> TournamentDetailsExists(int id)
    {
        var tournament = await serviceManager.TournamentService.GetByIdAsync(id);

        return tournament != null;
    }
}
