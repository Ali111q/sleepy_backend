using System.Text;
using GaragesStructure.DATA.DTOs.roles;
using GaragesStructure.Interface;
using GaragesStructure.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OneSignalApi.Model;

namespace GaragesStructure.Controllers;

public class RoleController: BaseController
{
    private static readonly HttpClient client = new HttpClient();
    private readonly IRoleService _roleService;


    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;

    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] RoleForm form) => Ok(await _roleService.Add(form));

    [HttpPost("/api/add-permission")]
    public async Task<ActionResult> AddPermissionToRole([FromBody] AssignPermissionsForm form) =>
        Ok(await _roleService.AddPermissionToRole(form.RoleId, form.Permissions));
    
    [HttpGet("/api/get-permissions")]
    public async Task<ActionResult> GetPermissions([FromQuery] PermissionsFilter filter) => Ok(await _roleService.GetAllPermissions(filter));
    
    [HttpGet]
    public async Task<ActionResult<List<RoleDto>>> GetAll([FromQuery] Rolefilter rolefilter) => Ok(await _roleService.GetAll(rolefilter), rolefilter.PageNumber);
    
    [HttpPost("/api/asiaa")]
    public async Task<dynamic> LoginAsia([FromBody] string phone)
    {
        string url = "https://odpapp.asiacell.com/api/v1/login?lang=ar";

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
        var data = new {
            captchaCode = "",
            username = phone
        };
        var jsonContent = JsonConvert.SerializeObject(data);
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

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
            return response.Content;
        }
    }
    [HttpPost("/api/asia/verify")]
 public async Task<dynamic> LoginAsia([FromBody] LoginRequestModel model)
        {
            string url = "https://odpapp.asiacell.com/api/v1/smsvalidation?lang=ar";

            // Prepare headers
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("X-ODP-API-KEY", "1ccbc4c913bc4ce785a0a2de444aa0d6");
            client.DefaultRequestHeaders.Add("DeviceID", model.DeviceID);
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
                PID = model.PID,
                passcode = model.Code,
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

    [HttpPost("/api/asia/transfer")]
    public async Task<IActionResult> StartTransfer([FromBody] TransferRequestModel model)
        {
            string url = "https://odpapp.asiacell.com/api/v1/credit-transfer/start?lang=ar";

            // Prepare headers
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("X-ODP-API-KEY", "1ccbc4c913bc4ce785a0a2de444aa0d6");
            client.DefaultRequestHeaders.Add("DeviceID", model.DeviceID);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {model.AccessToken}");
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
                amount = $"{model.Amount}.0",
                receiverMsisdn = model.Target
            };
            var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            // Send POST request
            HttpResponseMessage response = await client.PostAsync(url, jsonContent);

            // Handle response
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic responseObject = System.Text.Json.JsonSerializer.Deserialize<dynamic>(responseBody);
                return Ok(responseObject);
            }
            else
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                return BadRequest(errorResponse);
            }
        }

        public class TransferRequestModel
        {
            public string Target { get; set; }
            public decimal Amount { get; set; }
            public string DeviceID { get; set; }
            public string AccessToken { get; set; }
        }
    }

        public class LoginRequestModel
        {
            public string Code { get; set; }
            public string DeviceID { get; set; }
            public string PID { get; set; }
        }
