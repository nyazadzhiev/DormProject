﻿namespace DormProject.DormOffice.Data.Repositories
{
    public interface IStudentRepository
    {
        Task AddStudentAsync(Student student);
        void RemoveStudentAsync(Student student);
        Student FindStudent(int id);
        Task SaveChangesAsync();
    }
}
