using Microsoft.AspNetCore.Mvc;
using Sytycc_Service.Domain;



namespace Sytycc_Service.Api;

[ApiController]
[Route("api/[controller]")]
public class FacilitatorController : ControllerBase
{
    private readonly IFacilitatorService _facilitatorService;

    public FacilitatorController(IFacilitatorService facilitatorService)
    {
        _facilitatorService = facilitatorService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateFacilitator([FromBody] CreateFacilitatorDto createFacilitatorDto)
    {
        try
        {
            var result = await _facilitatorService.CreateFacilitator(createFacilitatorDto);
            return Ok(new { reference = result });
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpPut("{reference}")]
    public async Task<IActionResult> UpdateFacilitator(string reference, [FromBody] UpdateFacilitatorDto updateFacilitatorDto)
    {
        try
        {
            var result = await _facilitatorService.UpdateFacilitator(reference, updateFacilitatorDto);
            return Ok(new { reference = result });
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpDelete("{reference}")]
    public async Task<IActionResult> DeleteFacilitator(string reference)
    {
        try
        {
            await GetFacilitatorByReference(reference);
            var result = await _facilitatorService.DeleteFacilitator(reference);
            return Ok(new { reference = result });
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpGet("{reference}")]
    public async Task<IActionResult> GetFacilitatorByReference(string reference)
    {
        try
        {
            var result = await _facilitatorService.GetFacilitatorByReference(reference) ?? throw new NotFoundException($"No facilitator found with reference: {reference}");
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }
    
    [HttpGet("fullname/{fullname}")]
    public async Task<IActionResult> GetFacilitatorByFullName(string fullname)
    {
        try
        {
            var result = await _facilitatorService.GetFacilitatorByFullName(fullname) ?? throw new NotFoundException($"No facilitator found with fullname: {fullname}");
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpGet("list/{page:int:min(1)}")]
    public async Task<IActionResult> GetFacilitatorList(int page)
    {
        try
        {
            var result = await _facilitatorService.GetFacilitatorList(page);
            if (result == null || result.Count == 0)
                throw new NotFoundException($"No facilitators found for page: {page}");
            
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpPost("search/{page:int:min(1)}")]
    public async Task<IActionResult> SearchFacilitatorList(int page,[FromQuery] string fullname)
    {
        try
        {
            var result = await _facilitatorService.SearchFacilitatorList(page, fullname);
            if (result == null || result.Count == 0)
                throw new NotFoundException($"No facilitators found with name: {fullname} on page: {page}");
            
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }
}

