using OpenUpData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenUpData.Services;

public interface INotificationsService
{
    Task AddNewNotificationAsync(int userId, string message, string notificationType);
    //Task SendNotificationAsync(Notification notification);
    //Task MarkNotificationAsReadAsync(int notificationId);
    //Task<List<Notification>> GetUserNotificationsAsync(int userId);
}
