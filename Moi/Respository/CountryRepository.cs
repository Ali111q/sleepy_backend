using AutoMapper;
using GaragesStructure.DATA;
using GaragesStructure.Entities;
using GaragesStructure.Interface;

namespace GaragesStructure.Repository;

public class CountryRepository : GenericRepository<Country , Guid> , ICountryRepository 
{
    public CountryRepository(DataContext context, IMapper mapper) : base(context, mapper)
    {
    }
}