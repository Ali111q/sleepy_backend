using AutoMapper;
using GaragesStructure.DATA;
using GaragesStructure.Interface;
using Role = GaragesStructure.Entities.Role;

namespace GaragesStructure.Repository{
    public class RoleRepository : GenericRepository<Entities.Role, Guid>, IRoleRepository{
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RoleRepository(DataContext context, IMapper mapper) : base(context, mapper) {
            _context = context;
            _mapper = mapper;
        }
    }
}