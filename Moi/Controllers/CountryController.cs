using GaragesStructure.DATA.DTOs.Country;
using GaragesStructure.Entities;
using GaragesStructure.Helpers;
using GaragesStructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GaragesStructure.Controllers;



public class CountryController : BaseController
{
    
    private readonly ICountryService _countryService;

    public CountryController(ICountryService countryService)
    {
        _countryService = countryService;
    }

    
    [HttpGet]
    public async Task<ActionResult<CountryDto>> GetAll([FromQuery] CountryFilter filter) => Ok(await _countryService.GetAll(filter) , filter.PageNumber);
    
    [HttpPost]
    public async Task<ActionResult<CountryDto>> Create([FromBody] CountryForm countryForm) => Ok(await _countryService.Create(countryForm));
    
    [HttpPut("{id}")]
    public async Task<ActionResult<CountryDto>> Update(Guid id, [FromBody] CountryUpdate update) => Ok(await _countryService.Update(id, update));
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<CountryDto>> Delete(Guid id) => Ok(await _countryService.Delete(id));

}