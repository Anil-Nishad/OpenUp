using System.ComponentModel.DataAnnotations;

namespace OpenUpData.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public int NrOfReports { get; set; }
        public DateTime Datecreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
