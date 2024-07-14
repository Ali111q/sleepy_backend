using AutoMapper;
using BackEndStructuer.DATA;
using BackEndStructuer.Entities;
using BackEndStructuer.Interface;
using GaragesStructure.DATA;
using GaragesStructure.Repository;

namespace BackEndStructuer.Repository
{

    public class CategoryRepository : GenericRepository<Category , Guid> , ICategoryRepository
    {
        public CategoryRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
