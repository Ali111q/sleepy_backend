using AutoMapper;
using GaragesStructure.DATA;
using GaragesStructure.DATA.DTOs.Country;
using GaragesStructure.Entities;
using GaragesStructure.Repository;

namespace GaragesStructure.Services;

public interface ICountryService
{
    Task<(CountryDto? country, string? error)> Create(CountryForm countryForm);

    Task<(List<CountryDto>? countries, int? totalCount, string? error)> GetAll(CountryFilter filter);
    
    // update 
    Task<(CountryDto? country, string? error)> Update(Guid id, CountryUpdate update);
    
    // delete
    Task<(CountryDto country, string? error)> Delete(Guid id);
}

public class CountryService : ICountryService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;


    public CountryService(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }


    public async Task<(CountryDto? country, string? error)> Create(CountryForm countryForm)
    {
        var country = _mapper.Map<Country>(countryForm);
        country = await _repositoryWrapper.Country.Add(country);
        if (country == null) return (null, "Error creating country");
        var countryDto = _mapper.Map<CountryDto>(country);
        return (countryDto, null);
    }

    public async Task<(List<CountryDto>? countries, int? totalCount, string? error)> GetAll(CountryFilter filter)
    {
        var (countries, totalCount) = await _repositoryWrapper.Country.GetAll<CountryDto>(
            x => x.Name == null || x.Name.Contains(filter.Name),
            filter.PageNumber, filter.PageSize, filter.Deleted
        );
        
        return (countries, totalCount, null);
    }

    public async Task<(CountryDto? country, string? error)> Update(Guid id, CountryUpdate update)
    {
        var country = await _repositoryWrapper.Country.GetById(id);
        if (country == null) return (null, "Country not found");
        _mapper.Map(update, country);
        country = await _repositoryWrapper.Country.Update(country);
        if (country == null) return (null, "Error updating country");
        var countryDto = _mapper.Map<CountryDto>(country);
        return (countryDto, null);
    }

    public async Task<(CountryDto country, string? error)> Delete(Guid id)
    {
        var country = await _repositoryWrapper.Country.GetById(id);
        if (country == null) return (null, "Country not found");
        country = await _repositoryWrapper.Country.SoftDelete(id);
        if (country == null) return (null, "Error deleting country");
        var countryDto = _mapper.Map<CountryDto>(country);
        return (countryDto, null);
    }
}