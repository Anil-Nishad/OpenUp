using OpenUpData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenUpData.Helpers
{
    // If you want to make a call to any method in this class by using class name, you can make it static. Otherwise, you can instantiate it.
    public static class DbInitializer
    {
        public static void Seed(OpenUpContext context)
        {
            // Check if the database is already seeded
            if (context.Users.Any() || context.Posts.Any())
            {
                return; // Database has been seeded
            }
            // Create sample users
            var user1 = new User { Name = "Alice", ProfilePictureUrl = "https://drive.google.com/file/d/1s4QhuSEwlBT-MeUVZibLwEAC2eM1Bev5/view?usp=drive_link" };
            var user2 = new User { Name = "Bob", ProfilePictureUrl = "https://www.pexels.com/photo/elegant-portrait-of-a-woman-in-cozy-attire-31036772/" };
            context.Users.AddRange(user1, user2);
            context.SaveChanges();
            // Create sample posts
            var post1 = new Post { Content = "Hello World!", UserId = user1.Id, Datecreated = DateTime.Now, DateUpdated = DateTime.Now };
            var post2 = new Post { Content = "Learning Entity Framework Core!", UserId = user2.Id, Datecreated = DateTime.Now, DateUpdated = DateTime.Now };
            var post3 = new Post()
            {
                Content = "Learning MVC!",
                ImageUrl = "https://www.pexels.com/photo/cozy-outdoor-campfire-tea-and-coffee-mugs-32973553/",
                NrOfReports = 0,
                UserId = user1.Id,
                Datecreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };
            context.Posts.AddRange(post1, post2, post3);
            context.SaveChanges();
        }
    }
}
