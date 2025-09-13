using Microsoft.AspNetCore.Identity;
using OpenUpData.Helpers.Constants;
using OpenUpData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenUpData.Helpers;

// If you want to make a call to any method in this class by using class name, you can make it static. Otherwise, you can instantiate it.
public static class DbInitializer
{
    public static async Task SeedUsersAndRolesAsync(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
    {
        //Roles
        if (!roleManager.Roles.Any())
        {
            foreach (var roleName in AppRoles.All)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(roleName));
                }
            }
        }

        //Users with Roles
        if (!userManager.Users.Any(n => !string.IsNullOrEmpty(n.Email)))
        {
            var userPassword = "Coding@1234?";
            //var newUser = new User()
            //{
            //    UserName = "Anil_Nishad",
            //    Email = "anil.nishad@gmail.com",
            //    FullName = "Anil Nishad",
            //    ProfilePictureUrl = "https://img-b.udemycdn.com/user/200_H/16004620_10db_5.jpg",
            //    EmailConfirmed = true
            //};

            //var result = await userManager.CreateAsync(newUser, userPassword);
            //if (result.Succeeded)
            //    await userManager.AddToRoleAsync(newUser, AppRoles.User);


            var newAdmin = new User()
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                FullName = "Admin",
                ProfilePictureUrl = "https://images.unsplash.com/photo-1527430253228-e93688616381?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8Mnx8Um9ib3R8ZW58MHx8MHx8fDA%3D",
                EmailConfirmed = true
            };

            var resultNewAdmin = await userManager.CreateAsync(newAdmin, userPassword);
            if (resultNewAdmin.Succeeded)
                await userManager.AddToRoleAsync(newAdmin, AppRoles.Admin);
        }
    }
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
