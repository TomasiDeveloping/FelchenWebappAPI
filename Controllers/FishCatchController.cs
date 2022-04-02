using Api.Dtos;
using Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class FishCatchController : CustomBaseController
{
    private readonly IFishCatchRepository _fishCatchRepository;

    public FishCatchController(IFishCatchRepository fishCatchRepository)
    {
        _fishCatchRepository = fishCatchRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<FishCatchDto>>> GetFishCatches()
    {
        var fishCatches = await _fishCatchRepository.GetFishCatchesAsync();
        if (fishCatches.Count <= 0) return NoContent();
        return Ok(fishCatches);
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<List<FishCatchDto>>> GetPublicFishCatches([FromQuery] string pageSize,
        string pageIndex)
    {
        var pageSizeNumber = int.Parse(pageSize);
        var pageIndexNumber = int.Parse(pageIndex);
        var totalPublicCatches = await _fishCatchRepository.CountPublicCatchesAsync();
        var fishCatches = await _fishCatchRepository.GetPublicFishCatchesAsync(pageSizeNumber, pageIndexNumber);
        if (fishCatches.Count <= 0) return NoContent();
        return Ok(new
        {
            fishCatches,
            totalPublicCatches
        });
    }

    [HttpGet("{fishCatchId:int}")]
    public async Task<ActionResult<FishCatchDto>> GetFishCatchById(int fishCatchId)
    {
        if (fishCatchId <= 0) return BadRequest("Id Fehler");
        var fishCatch = await _fishCatchRepository.GetFishCatchByIdAsync(fishCatchId);
        if (fishCatch == null) return NotFound("Kein Fang mit dieser Id gefunden");
        return Ok(fishCatch);
    }

    [HttpGet("Users/{userId:int}")]
    public async Task<ActionResult<List<FishCatchDto>>> GetFishCatchesByUserId(int userId, [FromQuery] string pageSize,
        [FromQuery] string pageIndex, [FromQuery] string sortBy)
    {
        var pageSizeNumber = int.Parse(pageSize);
        var pageIndexNumber = int.Parse(pageIndex);
        var totalFishCatches = await _fishCatchRepository.CountUserCatchesAsync(userId);
        var fishCatches =
            await _fishCatchRepository.GetFishCatchesByUserIdAsync(userId, pageSizeNumber, pageIndexNumber, sortBy);
        if (fishCatches.Count <= 0) return NoContent();
        return Ok(new
        {
            fishCatches,
            totalFishCatches
        });
    }

    [HttpPost]
    public async Task<ActionResult<FishCatchDto>> InsertFishCatch(FishCatchDto fishCatchDto)
    {
        try
        {
            if (fishCatchDto == null) return BadRequest("Keine Daten übermittelt");
            var newFishCatch = await _fishCatchRepository.InsertFishCatchAsync(fishCatchDto);
            if (newFishCatch == null) return BadRequest("Fang konnte nicht hinzugefügt werden");
            return Ok(newFishCatch);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{fishCatchId:int}")]
    public async Task<ActionResult<FishCatchDto>> UpdateFishCatch(int fishCatchId, FishCatchDto fishCatchDto)
    {
        try
        {
            if (fishCatchId <= 0) return BadRequest("Id Fehler");
            var updatedFishCatch = await _fishCatchRepository.UpdateFishCatchAsync(fishCatchId, fishCatchDto);
            if (updatedFishCatch == null) return BadRequest("Fang konnte nicht geupdatet werden");
            return Ok(updatedFishCatch);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{fishCatchId:int}")]
    public async Task<ActionResult<bool>> DeleteFishCatch(int fishCatchId)
    {
        try
        {
            if (fishCatchId <= 0) return BadRequest("Fehler mit Id");
            var checkDelete = await _fishCatchRepository.DeleteFishCatchByIdAsync(fishCatchId);
            if (!checkDelete) return BadRequest("Fang konnte nicht gelöscht werden");
            return Ok(true);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("Users/{userId:int}")]
    public async Task<ActionResult<bool>> DeleteFishCatchesByUserId(int userId)
    {
        try
        {
            if (userId <= 0) return BadRequest("Fehler mit UserId");
            var checkDelete = await _fishCatchRepository.DeleteFishCatchesByUserIdAsync(userId);
            if (!checkDelete) return BadRequest("Fänge konnten nicht gelöscht werden");
            return Ok(true);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}