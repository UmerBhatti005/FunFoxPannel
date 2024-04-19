namespace FunFoxTask.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string Grade { get; set; }
        public string Timing { get; set; }
        public long MaxSize { get; set; }
        public long StudentsEnrolled { get; set; }
         
    }
}
