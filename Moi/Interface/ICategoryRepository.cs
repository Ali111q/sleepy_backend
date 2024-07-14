using BackEndStructuer.Entities;
using GaragesStructure.Interface;

namespace BackEndStructuer.Interface
{
    public interface ICategoryRepository : IGenericRepository<Category , Guid>
    {
         
    }
}
