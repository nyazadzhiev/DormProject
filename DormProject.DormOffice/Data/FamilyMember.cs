namespace DormProject.DormOffice.Data
{
    public class FamilyMember
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
    }
}
