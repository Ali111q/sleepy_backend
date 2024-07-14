using AutoMapper;
using BackEndStructuer.DATA;
using BackEndStructuer.Entities;
using BackEndStructuer.Interface;
using GaragesStructure.DATA;
using GaragesStructure.Repository;

namespace BackEndStructuer.Repository
{

    public class MusicRepository : GenericRepository<Music , Guid> , IMusicRepository
    {
        public MusicRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
