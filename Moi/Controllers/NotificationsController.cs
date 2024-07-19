using GaragesStructure.DATA.DTOs.Notification;
using GaragesStructure.Entities;
using GaragesStructure.Helpers;
using GaragesStructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using OneSignalApi.Model;

namespace GaragesStructure.Controllers;
[ServiceFilter(typeof(AuthorizeActionFilter))]

public class NotificationsController: BaseController
{
    public INotificationProvider _NotificationProvider;

    public NotificationsController(INotificationProvider notificationProvider)
    {
        _NotificationProvider = notificationProvider;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<NotificationResponse>> SendNotification([FromBody] Notifications form) =>
        Ok(_NotificationProvider.SendNotification(form, "ecb79277-ca29-48af-8969-341aac346c1d"));
}