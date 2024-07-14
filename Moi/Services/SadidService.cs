using System.Net.Http.Headers;
using System.Text;
using GaragesStructure.DATA.DTOs.Sadid;
using GaragesStructure.Repository;

namespace GaragesStructure.Services;

public interface ISadidService
{
    
}

public class SadidService : ISadidService
{
    private readonly IRepositoryWrapper _wraperRepository;
    public SadidService(IRepositoryWrapper wraperRepository)
    {
        _wraperRepository = wraperRepository;
    }
    
  
    
    public static async Task<(SadidDto? sadid, string? error)> CreateBill(SadidBody requestBody , string SadidSecretKey)
    {
        try
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("SecretKey", SadidSecretKey);

                string jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("https://api.sadid.app/api/bills/create-bill", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<SadidDto>(responseContent);
                    return (result, null);
                }
                else
                {
                    return (null, "Some error occured while creating bill");
                }
                
            }
        }
        catch (Exception ex)
        {
            return (null, $"Exception: {ex.Message}");
        }
    }
    
    // delete bulk
    public static async Task<(bool? success, string? error)> DeleteBill(List<Guid> billIds, string SadidSecretKey)
    {
        try
        {
            
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("SecretKey", SadidSecretKey);

                string jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(billIds);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("https://api.sadid.app/api/bills/delete-bill", content);
                
                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    return (false, "Some error occured while deleting bill");
                }
                
            }
        }
        catch (Exception ex)
        {
            return (false, $"Exception: {ex.Message}");
        }
    }

}