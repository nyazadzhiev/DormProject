using DormProject.Messages;
using DormProject.UniversityOffice.Data;
using DormProject.UniversityOffice.Data.Models;
using DormProject.UniversityOffice.Data.Repositories;
using MassTransit;

namespace DormProject.UniversityOffice.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IBus _publisher;

        public StudentService(IStudentRepository studentRepository, IBus publisher)
        {
            _studentRepository = studentRepository;
            _publisher = publisher;
        }

        public async Task<bool> CreateStudentAsync(StudentRequestDTO studentRequest)
        {
            Student student = new Student()
            {
                FirstName = studentRequest.FirstName,
                LastName = studentRequest.LastName,
                Grade = studentRequest.Grade
            };

            await _studentRepository.AddStudentAsync(student);
            await _studentRepository.SaveChangesAsync();

            await _publisher.Publish(new StudentAddedMessage()
            {
                StudentId = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Grade = student.Grade
            });

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
