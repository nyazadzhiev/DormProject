namespace DormProject.DormOffice.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DormDbContext _context;

        public StudentRepository(DormDbContext context)
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
