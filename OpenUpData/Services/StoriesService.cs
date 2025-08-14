using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OpenUpData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenUpData.Services;

public class StoriesService : IStoriesService
{
    private readonly OpenUpContext _context;
    public StoriesService(OpenUpContext context)
    {
        _context = context;
    }

    public async Task<Story> CreateStoryAsync(Story story, IFormFile image)
    {
        if (image != null && image.Length > 0)
        {
            string rootFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            if (image.ContentType.Contains("image"))
            {
                string rootFolderPathImages = Path.Combine(rootFolderPath, "images/stories");
                Directory.CreateDirectory(rootFolderPathImages);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                string filePath = Path.Combine(rootFolderPathImages, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    await image.CopyToAsync(stream);

                //Set the URL to the newPost object
                story.ImageUrl = "/images/stories/" + fileName;
            }
        }

        await _context.Stories.AddAsync(story);
        await _context.SaveChangesAsync();

        return story;
    }

    public async Task<List<Story>> GetAllStoriesAsync()
    {
        var allStories = await _context.Stories
                .Where(n => n.DateCreated >= DateTime.UtcNow.AddHours(-24))
                .Include(s => s.User)
                .ToListAsync();

        return allStories;
    }
}
