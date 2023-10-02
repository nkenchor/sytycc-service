using Microsoft.AspNetCore.Mvc;
using Sytycc_Service.Domain;

namespace Sytycc_Service.Api;

[Route("api/[controller]")]
[ApiController]
public class CourseController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto courseDto)
    {
        try
        {
            var result = await _courseService.CreateCourse(courseDto);
            return Ok(new { reference = result });
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpPut("{reference}")]
    public async Task<IActionResult> UpdateCourse(string reference, [FromBody] UpdateCourseDto courseDto)
    {
        try
        {
            var result = await _courseService.UpdateCourse(reference, courseDto);
            return Ok(new { reference = result });
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpDelete("{reference}")]
    public async Task<IActionResult> DeleteCourse(string reference)
    {
        try
        {
            var result = await _courseService.DeleteCourse(reference);
            return Ok(new { reference = result });
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpGet("{reference}")]
    public async Task<IActionResult> GetCourseByReference(string reference)
    {
        try
        {
            var result = await _courseService.GetCourseByReference(reference);
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpGet("title/{title}")]
    public async Task<IActionResult> GetCourseByTitle(string title)
    {
        try
        {
            var result = await _courseService.GetCourseByTitle(title);
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }
    [HttpGet("list/{page:int:min(1)}")]
    public async Task<IActionResult> GetCourseList(int page)
    {
        try
        {
            var result = await _courseService.GetCourseList(page);
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }
    [HttpGet("list/upcoming/{page:int:min(1)}")]
    public async Task<IActionResult> GetUpcomingCourses(int page)
    {
        try
        {
            var result = await _courseService.GetUpcomingCourses(page);
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpGet("search/{page:int:min(1)}")]
    public async Task<IActionResult> SearchCourseList(int page, [FromQuery]string title)
    {
        try
        {
            var result = await _courseService.SearchCourseList(page, title);
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }
}

