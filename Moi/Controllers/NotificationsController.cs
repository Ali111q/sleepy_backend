using GaragesStructure.DATA.DTOs.Notification;
using GaragesStructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace GaragesStructure.Controllers;
[ServiceFilter(typeof(AuthorizeFilter))]
public class NotificationsController: BaseController
{
    [HttpPost]
    public async Task<ActionResult<NotificationResponse>> SendNotification([FromBody] NotificationForm form) =>
        Ok(new NotificationResponse(State: true, Notification: new Notifications()));
}