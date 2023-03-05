using DormProject.DormOffice.Data.Models;

namespace DormProject.DormOffice.Services
{
    public interface IStudentService
    {
        Task<bool> CreateStudentAsync(StudentRequestDTO studentRequest);
        Task<bool> DeleteStudentAsync(int studentId);
    }
}
