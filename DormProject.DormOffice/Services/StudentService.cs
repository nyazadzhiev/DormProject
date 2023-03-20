using DormProject.DormOffice.Data;
using DormProject.DormOffice.Data.Models;
using DormProject.DormOffice.Data.Repositories;
using DormProject.Messages;
using MassTransit;

namespace DormProject.DormOffice.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<bool> CreateStudentAsync(StudentRequestDTO studentRequest)
        {
            Student student = new Student()
            {
                FirstName = studentRequest.FirstName,
                LastName = studentRequest.LastName,
                Grade = studentRequest.Grade,
            };

            await _studentRepository.AddStudentAsync(student);
            await _studentRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            Student student = _studentRepository.FindStudent(studentId);

            _studentRepository.RemoveStudentAsync(student);
            await _studentRepository.SaveChangesAsync();

            return true;
        }
    }
}
