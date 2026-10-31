using CourseHub.Application.DTOs.Enrollments;
using CourseHub.Application.Exceptions;
using CourseHub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace CourseHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpPost]
        public async Task<IActionResult> EnrollStudent([FromBody] EnrollStudentDto dto)
        {
            try
            {
                var enrollmentId = await _enrollmentService.EnrollStudentAsync(dto);

                return StatusCode(201, new { Id = enrollmentId });
            }
            catch (ApplicationValidationException ex)
            {
                return BadRequest(new { Errors = ex.Errors });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEnrollments()
        {
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();

            return Ok(enrollments);
        }
    }
}
