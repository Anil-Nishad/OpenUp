using Microsoft.EntityFrameworkCore;
using OpenUp.Data.Models;

namespace OpenUp.Data
{
    public class OpenUpContext : DbContext
    {
        public OpenUpContext(DbContextOptions<OpenUpContext> options) : base(options)
        {
        }
        public DbSet<Post> Posts { get; set; }
    }
}
