using OpenUpData.Dtos;
using OpenUpData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenUpData.Services;

public interface IFriendsService
{
    Task SendRequestAsync(int senderId, int receiverId);
    Task<FriendRequest> UpdateRequestAsync(int requestId, string status);
    Task RemoveFriendAsync(int frienshipId);
    Task<List<UserWithFriendsCountDto>> GetSuggestedFriendsAsync(int userId);
    Task<List<FriendRequest>> GetSentFriendRequestAsync(int userId);
}
