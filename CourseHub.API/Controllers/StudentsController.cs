using CourseHub.Application.DTOs.Students;
using CourseHub.Application.Interfaces;
using CourseHub.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CourseHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {

        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterStudent( [FromBody] RegisterStudentDto dto)
        {
            try
            {
                Guid studentId = await _studentService.RegisterStudentAsync(dto);

                return Created($"/api/students/{studentId}", new { Id = studentId });
            }
            catch (RegistrationValidationException ex)
            {
                return BadRequest(new
                {
                    Errors = ex.Errors
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
        }
    }
}
