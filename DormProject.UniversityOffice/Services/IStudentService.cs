using DormProject.UniversityOffice.Data.Models;

namespace DormProject.UniversityOffice.Services
{
    public interface IStudentService
    {
        Task<bool> CreateStudentAsync(StudentRequestDTO studentRequest);
        Task<bool> DeleteStudentAsync(int studentId);
    }
}
