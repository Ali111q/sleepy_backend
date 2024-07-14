using GaragesStructure.Entities;

namespace GaragesStructure.DATA.DTOs.Notification;

public record NotificationResponse(
    Notifications? Notification , 
    bool State
    )
{

}