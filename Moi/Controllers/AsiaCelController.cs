

using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GaragesStructure.Controllers
{
    [ApiController]
    public class AsiaController : ControllerBase
    {
        private readonly HttpClient client;

        public AsiaController()
        {
            client = new HttpClient();
        }

        [HttpPost("/api/asia")]
        public async Task<dynamic> LoginAsia([FromBody] string phone)
        {
            string url = "https://odpapp.asiacell.com/api/v1/smsvalidation?lang=ar";

            // Prepare headers
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("X-ODP-API-KEY", "1ccbc4c913bc4ce785a0a2de444aa0d6");
            client.DefaultRequestHeaders.Add("DeviceID", "334423244");
            client.DefaultRequestHeaders.Add("X-OS-Version", "11");
            client.DefaultRequestHeaders.Add("X-Device-Type", "[Android][realme][RMX2001 11] [Q]");
            client.DefaultRequestHeaders.Add("X-ODP-APP-VERSION", "3.4.1");
            client.DefaultRequestHeaders.Add("X-FROM-APP", "odp");
            client.DefaultRequestHeaders.Add("X-ODP-CHANNEL", "mobile");
            client.DefaultRequestHeaders.Add("X-SCREEN-TYPE", "MOBILE");
            client.DefaultRequestHeaders.Add("Cache-Control", "private, max-age=240");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
            client.DefaultRequestHeaders.Add("User-Agent", "okhttp/5.0.0-alpha.2");

            // Prepare request body
            var data = new
            {
                PID = "your_pid_value_here",
                passcode = "your_code_value_here",
                token = "e1OrgWG9T4mzVKZS4N9EqT:APA91bFxGBHePpzolWWPtl4ICO6UV5y5W7HrPa-kKNz2mEBCuD-a3en50n-EE4dpMwEEfxUt4Lr-ai_hAatoGDDcwNbBKaQ-3Mn3CkMmO1MlXjKZoQuR06NlvdqYJ53uUC2SODMKpznD"
            };
            var jsonContent = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Send POST request
            HttpResponseMessage response = await client.PostAsync(url, content);

            // Handle response
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic responseObject = JsonConvert.DeserializeObject(responseBody);
                return responseObject;
            }
            else
            {
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
