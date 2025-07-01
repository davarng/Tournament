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
using Tournament.Data.Repositories;

namespace Tournament.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TournamentDetailsController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TournamentDto>>> GetAllTournamentDetails()
    {
        var tournaments = await unitOfWork.TournamentRepository.GetAllAsync();
        return Ok(tournaments.Select(t => mapper.Map<TournamentDto>(t)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TournamentDto>> GetTournamentDetails(int id, bool includeGames = false)
    {
        var tournament = await unitOfWork.TournamentRepository.GetAsync(id, includeGames);
        if (tournament == null)
            return NotFound();

        return Ok(mapper.Map<TournamentDto>(tournament));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTournamentDetails(int id, [FromBody] TournamentUpdateDto tournamentUpdateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var tournament = await unitOfWork.TournamentRepository.GetAsync(id);
        if (tournament == null)
            return NotFound();

        mapper.Map(tournamentUpdateDto, tournament);
        unitOfWork.TournamentRepository.Update(tournament);

        try
        {
            await unitOfWork.CompleteAsync();
        }
        catch
        {
            return StatusCode(500, "Failed to update tournament.");
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<TournamentDto>> PostTournamentDetails([FromBody] TournamentCreateDto tournamentCreateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var tournament = mapper.Map<TournamentDetails>(tournamentCreateDto);
        unitOfWork.TournamentRepository.Add(tournament);

        try
        {
            await unitOfWork.CompleteAsync();
        }
        catch
        {
            return StatusCode(500, "Failed to create tournament.");
        }

        var resultDto = mapper.Map<TournamentDto>(tournament);
        return CreatedAtAction(nameof(GetTournamentDetails), new { id = tournament.Id }, resultDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTournamentDetails(int id)
    {
        var tournament = await unitOfWork.TournamentRepository.GetAsync(id);
        if (tournament == null)
            return NotFound();

        unitOfWork.TournamentRepository.Remove(tournament);

        try
        {
            await unitOfWork.CompleteAsync();
        }
        catch
        {
            return StatusCode(500, "Failed to delete tournament.");
        }

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchTournamentDetails(int id, [FromBody] JsonPatchDocument<TournamentPatchDto> patchDoc)
    {
        var tournament = await unitOfWork.TournamentRepository.GetAsync(id);
        if (tournament == null)
            return NotFound("Tournament not found");

        var dto = mapper.Map<TournamentPatchDto>(tournament);
        patchDoc.ApplyTo(dto, ModelState);

        if (!ModelState.IsValid || !TryValidateModel(dto))
            return BadRequest(ModelState);

        mapper.Map(dto, tournament);
        unitOfWork.TournamentRepository.Update(tournament);

        try
        {
            await unitOfWork.CompleteAsync();
        }
        catch
        {
            return StatusCode(500, "Failed to patch tournament.");
        }

        return NoContent();
    }

    private async Task<bool> TournamentDetailsExists(int id)
    {
        var tournament = await unitOfWork.TournamentRepository.GetAsync(id);
        return tournament != null;
    }
}
