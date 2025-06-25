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
    public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentDetails()
    {
        var tournaments = await unitOfWork.TournamentRepository.GetAllAsync();
        return Ok(tournaments.Select(t => mapper.Map<TournamentDto>(t)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TournamentDto>> GetTournamentDetails(int id)
    {
        var tournament = await unitOfWork.TournamentRepository.GetAsync(id);
        if (tournament == null)
        {
            return NotFound();
        }
        return mapper.Map<TournamentDto>(tournament);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTournamentDetails(int id, TournamentDto tournamentDto)
    {
        var tournament = mapper.Map<TournamentDetails>(tournamentDto);
        unitOfWork.TournamentRepository.Update(tournament);

        try
        {
            await unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await TournamentDetailsExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<TournamentDto>> PostTournamentDetails(TournamentDto tournamentDto)
    {
        var tournament = mapper.Map<TournamentDetails>(tournamentDto);
        unitOfWork.TournamentRepository.Add(tournament);
        await unitOfWork.CompleteAsync();

        var resultDto = mapper.Map<TournamentDto>(tournament);
        return CreatedAtAction("GetTournamentDetails", new { id = tournament.Id }, resultDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTournamentDetails(int id)
    {
        var tournament = await unitOfWork.TournamentRepository.GetAsync(id);
        if (tournament == null)
        {
            return NotFound();
        }

        unitOfWork.TournamentRepository.Remove(tournament);
        await unitOfWork.CompleteAsync();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchTournamentDetails(int id, JsonPatchDocument<TournamentDto> patchDoc)
    {
        var tournament = await unitOfWork.TournamentRepository.GetAsync(id);
        if (tournament == null)
            return NotFound("Tournament not found");

        var dto = mapper.Map<TournamentDto>(tournament);
        patchDoc.ApplyTo(dto, ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!TryValidateModel(dto))
            return BadRequest("Invalid tournament patch data");

        mapper.Map(dto, tournament);
        unitOfWork.TournamentRepository.Update(tournament);
        await unitOfWork.CompleteAsync();

        return NoContent();
    }

    private async Task<bool> TournamentDetailsExists(int id)
    {
        var tournament = await unitOfWork.TournamentRepository.GetAsync(id);
        return tournament != null;
    }
}
