namespace DormProject.UniversityOffice.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentDbContext _context;

        public StudentRepository(StudentDbContext context)
        {
            _context = context;
        }
        public async Task AddStudentAsync(Student student)
        {
            await _context.Students.AddAsync(student);  
        }

        public Student FindStudent(int id)
        {
            return _context.Students.FirstOrDefault(s => s.Id == id);
        }

        public void RemoveStudentAsync(Student student)
        {
            _context.Students.Remove(student);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
