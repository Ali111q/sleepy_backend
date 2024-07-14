using GaragesStructure.DATA.DTOs.Notification;
using GaragesStructure.Entities;
using GaragesStructure.Repository;
using RestSharp;

namespace GaragesStructure.Services;

public interface INotificationProvider
{
    Task<NotificationResponse> SendNotification(Notifications notification, string to);
    Task<(List<Notifications>?  notifications , int? totalCount , string? error)> GetAll(Guid Id);
}

public class NotificationProvider : INotificationProvider
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IRestClient _restClient;
    private readonly IConfiguration _configuration;
    private readonly IRestRequest _restRequest;

    public NotificationProvider(
        IRepositoryWrapper repositoryWrapper,
        IRestClient restClient,
        IConfiguration configuration,
        IRestRequest restRequest
    )
    {
        _repositoryWrapper = repositoryWrapper;
        _restClient = restClient;
        _configuration = configuration;
        _restRequest = restRequest;
    }

    public async Task<NotificationResponse> SendNotification(Notifications notification, string to)
    {
        try
        {
            var body = new
            {
                app_id = _configuration["onesginel:app_id"],
                headings = new { en = notification.Title, ar = notification.Title },
                contents = new { en = notification.Description, ar = notification.Description },
                included_segments = new[] { to },
                data = notification
            };

            var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Authorization", _configuration["onesginel:Authorization"]!),
                new KeyValuePair<string, string>("Content-Type", "application/json"),
                new KeyValuePair<string, string>("Cookie",
                    "__cfduid=d8a2aa2f8395ad68b8fd27b63127834571600976869")
            };
            
            _restRequest.Method = Method.POST;
            _restRequest.Resource = _configuration["onesginel:Url"];
            _restRequest.AddHeaders(headers);
            _restRequest.AddJsonBody(body);
            await _restClient.ExecuteAsync(_restRequest);

            await _repositoryWrapper.Notification.Add(notification);

            return new NotificationResponse(notification, true);
        }
        catch (Exception ex)
        {
            return new NotificationResponse(null, false);
        }
    }

    public async Task<(List<Notifications>? notifications , int? totalCount, string? error)> GetAll(Guid Id)
    {
        var (notifications , totalCount) = await _repositoryWrapper.Notification.GetAll(x => x.UserId == Id);

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
        }

        await _repositoryWrapper.Notification.UpdateAll(notifications);
        
        return (notifications , totalCount , null);
    }
}