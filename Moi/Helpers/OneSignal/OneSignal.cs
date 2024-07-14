using GaragesStructure.DATA;
using GaragesStructure.Repository;
using GaragesStructure.Entities;
using RestSharp;

namespace GaragesStructure.Helpers.OneSignal
{
    

    public class OneSignal
    {
        
        public static async Task<bool> SendNoitications(Notifications notification , string to)
        {
            IConfiguration configuration = ConfigurationProvider.Configuration;
            
            var client = new RestClient(configuration["onesginel:Url"]!);
            var request = new RestRequest(configuration["onesginel:Url"], Method.POST);
            request.AddHeader("Authorization", configuration["onesginel:Authorization"]!);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "__cfduid=d8a2aa2f8395ad68b8fd27b63127834571600976869");
            try
            {
                var body = new
                {
                    app_id = configuration["onesginel:app_id"],
                    headings = new { en = notification.Title, ar = notification.Title },
                    contents = new { en = notification.Description, ar = notification.Description },
                    included_segments = new[] { to },
                    data = notification
                };
                request.AddJsonBody(body);
                client.Execute(request);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}