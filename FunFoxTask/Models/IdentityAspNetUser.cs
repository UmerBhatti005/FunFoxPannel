using Microsoft.AspNetCore.Identity;

namespace FunFoxTask.Models
{
    public class IdentityAspNetUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? EnrolledCourse { get; set; }
    }
}
