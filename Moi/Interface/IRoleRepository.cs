using Role = GaragesStructure.Entities.Role;

namespace GaragesStructure.Interface
{
    public interface IRoleRepository : IGenericRepository<Entities.Role,Guid>
    {
        
    }
}