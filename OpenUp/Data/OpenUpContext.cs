using Microsoft.EntityFrameworkCore;

namespace OpenUp.Data
{
    public class OpenUpContext : DbContext
    {
        public OpenUpContext(DbContextOptions<OpenUpContext> options) : base(options)
        {
        }
    }
}
