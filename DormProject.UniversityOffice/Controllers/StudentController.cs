using DormProject.Controllers;
using DormProject.UniversityOffice.Data.Models;
using DormProject.UniversityOffice.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace DormProject.UniversityOffice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ApiController
    {
        private readonly IStudentService studentService;

        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateUserAsync(StudentRequestDTO student)
        {
            bool isCreated = await studentService.CreateStudentAsync(student);
            if (isCreated && ModelState.IsValid)
            {
                return Created(nameof(HttpPostAttribute), "Student was created");
            }

            return BadRequest();
        }

        [HttpDelete("{studentId}")]
        public async Task<IActionResult> DeleteTeamAsync(int studentId)
        {
            bool isDeleted = await studentService.DeleteStudentAsync(studentId);
            if (isDeleted)
            {
                return Ok("Student deleted successfully.");
            }

            return BadRequest();
        }
    }
}
