namespace DormProject.DormOffice.Data
{
    public class Room
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public virtual List<Student> Students { get; set; } = new List<Student>();
    }
}
