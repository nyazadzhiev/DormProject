namespace DormProject.DormOffice.Data
{
    public class Speciality
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }
        public virtual List<Student> Students { get; set; }
    }
}
