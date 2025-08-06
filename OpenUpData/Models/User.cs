using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenUpData.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ProfilePictureUrl { get; set; }

        // Navigation properties
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
