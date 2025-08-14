﻿using Microsoft.AspNetCore.Http;
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

    public async Task<Story> CreateStoryAsync(Story story)
    {
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
