using GaragesStructure.DATA.DTOs.Notification;
using GaragesStructure.Entities;
using GaragesStructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace GaragesStructure.Controllers;
[ServiceFilter(typeof(AuthorizeActionFilter))]

public class NotificationsController: BaseController
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<NotificationResponse>> SendNotification([FromBody] NotificationForm form) =>
        Ok(new NotificationResponse(State: true, Notification: new Notifications()));
}