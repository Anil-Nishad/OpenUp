using Microsoft.EntityFrameworkCore;
using OpenUpData.Models;

namespace OpenUpData
{
    public class OpenUpContext : DbContext
    {
        public OpenUpContext(DbContextOptions<OpenUpContext> options) : base(options)
        {
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
