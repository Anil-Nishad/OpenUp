using Microsoft.EntityFrameworkCore;
using OpenUpData.Helpers;
using OpenUpData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenUpData.Services;


public class HashtagsService : IHashtagsService
{
    private readonly OpenUpContext _context;
    public HashtagsService(OpenUpContext context)
    {
        _context = context;
    }

    public async Task ProcessHashtagsForNewPostAsync(string content)
    {
        //Find and store hashtags
        var postHashtags = HashtagHelper.GetHashtags(content);
        foreach (var hashTag in postHashtags)
        {
            var hashtagDb = await _context.Hashtags.FirstOrDefaultAsync(n => n.Name == hashTag);
            if (hashtagDb != null)
            {
                hashtagDb.Count += 1;
                hashtagDb.DateUpdated = DateTime.UtcNow;

                _context.Hashtags.Update(hashtagDb);
                await _context.SaveChangesAsync();
            }
            else
            {
                var newHashtag = new Hashtag()
                {
                    Name = hashTag,
                    Count = 1,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow
                };
                await _context.Hashtags.AddAsync(newHashtag);
                await _context.SaveChangesAsync();
            }
        }
    }

    public async Task ProcessHashtagsForRemovedPostAsync(string content)
    {
        var postHashtags = HashtagHelper.GetHashtags(content);
        foreach (var hashtag in postHashtags)
        {
            var hashtagDb = await _context.Hashtags.FirstOrDefaultAsync(n => n.Name == hashtag);
            if (hashtagDb != null)
            {
                hashtagDb.Count -= 1;
                hashtagDb.DateUpdated = DateTime.UtcNow;

                _context.Hashtags.Update(hashtagDb);
                await _context.SaveChangesAsync();
            }
        }
    }
}
