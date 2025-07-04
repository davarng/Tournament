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
using Tournament.Data.Repositories;

namespace Tournament.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TournamentDetailsController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TournamentDto>>> GetAllTournamentDetails([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        pageSize = Math.Min(pageSize, 100);
        var tournaments = await serviceManager.TournamentService.GetAllAsync(page, pageSize);
        return Ok(tournaments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TournamentDto>> GetTournamentDetails(int id, bool includeGames = false)
    {
        var tournament = await serviceManager.TournamentService.GetByIdAsync(id, includeGames);

        if (tournament == null)
            return NotFound();

        return Ok(tournament);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTournamentDetails(int id, [FromBody] TournamentUpdateDto tournamentUpdateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        var success = await serviceManager.TournamentService.UpdateAsync(id, tournamentUpdateDto);

        if (!success)
            return NotFound("Tournament not found or update failed.");

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<TournamentDto>> PostTournamentDetails([FromBody] TournamentCreateDto tournamentCreateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await serviceManager.TournamentService.CreateAsync(tournamentCreateDto);

        if (result == null)
            return StatusCode(500, "Failed to create tournament.");

        return CreatedAtAction(nameof(GetTournamentDetails), new { id = result.Id }, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTournamentDetails(int id)
    {
        var success = await serviceManager.TournamentService.DeleteAsync(id);

        if (!success)
            return StatusCode(500, "Failed to delete tournament.");

        return NoContent();
    }

    [HttpPatch("{id}")]
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
