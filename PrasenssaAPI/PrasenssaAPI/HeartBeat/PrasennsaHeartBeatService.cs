using Microsoft.AspNetCore.SignalR;
using PrasenssaAPI.Hub;
using PrasenssaAPI.PrasenssaConnection;

namespace PrasenssaAPI.HeartBeat;

public class PrasennsaHeartBeatService : BackgroundService
{
    private static readonly TimeSpan TimeSpan = TimeSpan.FromSeconds(5);

    private static ILogger<PrasennsaHeartBeatService> _logger;

    private readonly IHubContext<NotificationsHub, INotificationClient> _hubContext;

    private readonly IPraseansaClient _praseansaClient;

    public PrasennsaHeartBeatService(ILogger<PrasennsaHeartBeatService> logger,
        IHubContext<NotificationsHub, INotificationClient> hubContext, IPraseansaClient praseansaClient)
    {
        _logger = logger;
        _hubContext = hubContext;
        _praseansaClient = praseansaClient;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //run a timer for every  5 seconds
        using var timer = new PeriodicTimer(TimeSpan);

        while (!stoppingToken.IsCancellationRequested &&
               await timer.WaitForNextTickAsync(stoppingToken))
        {
            _praseansaClient.SendHeartBeat();
        }
    }
}