using CourseHub.Application.DTOs.Courses;
using CourseHub.Application.Exceptions;
using CourseHub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CourseHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {

        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(
            [FromBody] CreateCourseDto dto)
        {
            try
            {
                var courseId = await _courseService.CreateCourseAsync(dto);

                return CreatedAtAction( nameof(GetCourseById),new { id = courseId },new { Id = courseId });
            }
            catch (ApplicationValidationException ex)
            {
                return BadRequest(new { Errors = ex.Errors });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();

            return Ok(courses);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCourseById(Guid id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);

            if (course == null)
                return NotFound(new { Message = "Course not found." });

            return Ok(course);
        }

        
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCourse(
            Guid id,
            [FromBody] UpdateCourseDto dto)
        {
            try
            {
                var updated = await _courseService.UpdateCourseAsync(id, dto);

                if (!updated)
                    return NotFound(new { Message = "Course not found." });

                return NoContent();
            }
            catch (ApplicationValidationException ex)
            {
                return BadRequest(new { Errors = ex.Errors });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            try
            {
                bool deleted = await _courseService.DeleteCourseAsync(id);

                if (!deleted)
                    return NotFound(new { Message = "Course not found." });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
        }
    }
}
