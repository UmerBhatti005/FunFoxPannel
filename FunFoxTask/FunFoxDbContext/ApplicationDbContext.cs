using FunFoxTask.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace FunFoxTask.FunFoxDbContext
{
    public class ApplicationDbContext : IdentityDbContext<IdentityAspNetUser>
    {
        public DbSet<Course> courses { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {

        }
    }
}
