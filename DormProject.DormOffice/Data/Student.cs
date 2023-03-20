namespace DormProject.DormOffice.Data
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Grade { get; set; }
        public string? Adress { get; set; }
        public string? FacultyNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public virtual List<FamilyMember> FamilyMembers { get; set; } = new List<FamilyMember>();
        public int? SpecialityId { get; set; }
        public virtual Speciality? Speciality { get; set; }
        public int? RoomId { get; set; }
        public virtual Room? Room { get; set; }

    }
}
