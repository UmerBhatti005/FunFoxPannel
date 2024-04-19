using System.ComponentModel.DataAnnotations;

namespace FunFoxTask.Models
{
    public class SignUpViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public int EnrolledCourse { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
