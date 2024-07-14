using AutoMapper;
using GaragesStructure.DATA;
using GaragesStructure.Entities;
using GaragesStructure.Interface;

namespace GaragesStructure.Repository;

public class NotificationRepository : GenericRepository<Notifications, Guid>, INotificationRepository
{
    public NotificationRepository(DataContext context, IMapper mapper) : base(context, mapper)
    {
    }
}
