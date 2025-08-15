using Microsoft.EntityFrameworkCore;
using OpenUpData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenUpData.Services;

public class UsersService : IUsersService
{
    private readonly OpenUpContext _context;
    public UsersService(OpenUpContext context)
    {
        _context = context;
    }

    public async Task<User> GetUser(int loggedInUserId)
    {
        return await _context.Users.FirstOrDefaultAsync(n => n.Id == loggedInUserId) ?? new User();
    }

    public async Task UpdateUserProfilePicture(int loggedInUserId, string profilePictureUrl)
    {
        var userDb = await _context.Users.FirstOrDefaultAsync(n => n.Id == loggedInUserId);

        if (userDb != null)
        {
            userDb.ProfilePictureUrl = profilePictureUrl;
            _context.Users.Update(userDb);
            await _context.SaveChangesAsync();
        }
    }
}
