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
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();

            return Ok(students);
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetStudentById(Guid id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);

            if (student == null)
            {
                return NotFound(new { Message = "Student not found."});
            }

            return Ok(student);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateStudent( Guid id, [FromBody] UpdateStudentDto dto)
        {
            try
            {
                bool updated = await _studentService.UpdateStudentAsync(id, dto);

                if (!updated)
                    return NotFound(new { Message = "Student not found." });

                return NoContent();
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

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            bool deleted = await _studentService.DeleteStudentAsync(id);

            if (!deleted)
                return NotFound(new { Message = "Student not found." });

            return NoContent();
        }
    }
}
