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
        public static async Task Seed(OpenUpContext context)
        {
            // Check if the database is already seeded
            if (context.Users.Any() || context.Posts.Any())
            {
                return; // Database has been seeded
            }
            // Create sample users
            var user1 = new User { FullName = "Alice", ProfilePictureUrl = "https://drive.google.com/file/d/1s4QhuSEwlBT-MeUVZibLwEAC2eM1Bev5/view?usp=drive_link" };
            var user2 = new User { FullName = "Bob", ProfilePictureUrl = "https://www.pexels.com/photo/elegant-portrait-of-a-woman-in-cozy-attire-31036772/" };
            await context.Users.AddRangeAsync(user1, user2);
            await context.SaveChangesAsync();
            // Create sample posts
            var post1 = new Post { Content = "Hello World!", UserId = user1.Id, DateCreated = DateTime.Now, DateUpdated = DateTime.Now };
            var post2 = new Post { Content = "Learning Entity Framework Core!", UserId = user2.Id, DateCreated = DateTime.Now, DateUpdated = DateTime.Now };
            var post3 = new Post()
            {
                Content = "Learning MVC!",
                ImageUrl = "https://www.pexels.com/photo/cozy-outdoor-campfire-tea-and-coffee-mugs-32973553/",
                NrOfReports = 0,
                UserId = user1.Id,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };
            await context.Posts.AddRangeAsync(post1, post2, post3);
            await context.SaveChangesAsync();
        }
    }
}
