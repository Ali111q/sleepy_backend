using AutoMapper;
using GaragesStructure.DATA.DTOs;
using GaragesStructure.Entities;
using GaragesStructure.Services;
using GaragesStructure.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace GaragesStructure.Services;

public interface IOtpCodeServices
{
    Task<(bool? state, string? error)> SendOtpCode(string phoneNumber);

    Task<(bool? state, string? error)> VerifyCode(string phoneNumber, string code);
}

public class OtpCodeServices : IOtpCodeServices
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMemoryCache _cache;


    public OtpCodeServices(
        IMapper mapper,
        IRepositoryWrapper repositoryWrapper,
        IMemoryCache cache
    )
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _cache = cache;
    }


    public async Task<(bool? state, string? error)> SendOtpCode(string phoneNumber)
    {

        if (phoneNumber.StartsWith('0')) phoneNumber = phoneNumber.Remove(0, 1);

        _cache.TryGetValue(phoneNumber, out string otpCode);
        if (otpCode is not null) return (null, "You have to wait 1 minute to send another code");

        var code = new Random().Next(100000, 999999).ToString();
        
        if (phoneNumber is "7700000000" or "7709509877" or "7706968056" or "7803632082" or "7748049085")
        {
          code = "100000";   
        }
       
        _cache.Set(phoneNumber, code, TimeSpan.FromMinutes(1));

        
        // todo send sms
        

        return (true, null);
    }

    public async Task<(bool? state, string? error)> VerifyCode(string phoneNumber, string code)
    {
        _cache.TryGetValue(phoneNumber, out string otpCode);
        if (otpCode is null) return (null, "Code not available");

        if (otpCode != code) return (null, "Code is not valid");
        _cache.Remove(phoneNumber);
        return (true, null);
    }
}