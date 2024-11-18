using System.Text;
using Bosch.PRAESENSA.OpenInterface;
using Microsoft.AspNetCore.SignalR;
using PrasenssaAPI.Models;
using PrasenssaAPI.PrasenssaConnection;

namespace PrasenssaAPI.Hub;

public class NotificationsHub : Hub<INotificationClient>
{
    private Device[] devices;
    
    public NotificationsHub(IPraseansaClient praseansaClient)
    {
        PraseansaClient = praseansaClient;
    }
    [Inject] public IPraseansaClient PraseansaClient { get; set; }

    public override async Task OnConnectedAsync()
    {
        PraseansaClient.Disconnect();
        
        Thread.Sleep(500);
        
        var httpCtx = Context.GetHttpContext();

        var builder = new StringBuilder(Environment.NewLine);
        foreach (var header in httpCtx.Request.Query) builder.AppendLine($"{header.Key}: {header.Value}");

        Console.WriteLine("Headers {0}", builder);

        var sessionId = httpCtx.Request.Query["SessionID"];
        
        var connectionParams = new ConnectionParams(httpCtx.Request.Query["username"],
            httpCtx.Request.Query["password"], httpCtx.Request.Query["ip"]);

        Console.WriteLine("DevicesStub in hub {0}", PraseansaClient);

        PraseansaClient.SetConnectionParams(Context.ConnectionId, connectionParams);

        PraseansaClient.ResourceStateChanged += async (sender, args) =>
        {
            Console.WriteLine("Resource state changed in hub: {0}, {1}", DateTime.Now, args);
            var inUse = args.State == TOIResourceState.OIRS_INUSE;
            Console.WriteLine("Resources {0} : ", args.Resources.ToString());
            Console.WriteLine("Resource state: {0} ",args.State.ToString());
            Console.WriteLine("----------------------------------------------   ");
            var update = args.Resources.Select(name => new ZoneInUse(name, inUse));
            await Clients.All.SendZoneInUse(update.ToList());   
        };

        PraseansaClient.ResourceFaultStateChanged += async (sender, args) =>
        {
            Console.WriteLine("Resource fault state changed in hub: {0}, {1}", DateTime.Now, args);
            Console.WriteLine("Resources {0} : ", args.Resources.ToString());
            Console.WriteLine("Fault state: {0} ",args.FaultState.ToString());
            Console.WriteLine("----------------------------------------------   ");
            var status = args?.FaultState == TOIResourceFaultState.OIRS_FAULT ? DeviceState.Faulted : DeviceState.Connected;
            var devicesInFault = args.Resources.Select(device => new Device(device,["fault1","fault1","fault1","fault1","fault1"],status));
            await Clients.All.SendDevicesFault(devicesInFault.ToArray());
        };

        PraseansaClient.CallStateChanged += async (sender, args) =>
        {
            Console.WriteLine("call state changed in hub: {0}, {1}", DateTime.Now, args);
            Console.WriteLine("call Id:{0}",args.CallId);
            Console.WriteLine("call state: {0}", args.State.ToString());
            Console.WriteLine("---------------------------------------------   ");
            await Clients.All.SendCallState(args.ToString());
        };

        PraseansaClient.DiagEventNotification += async (sender, args) =>
        {
            if (args.Event.AddEventOriginator is UnitEventOriginator)
            {
                Console.WriteLine("diag event : {0}, {1}", DateTime.Now, args.ToString());
                Console.WriteLine("diag event type {0}", args.Event.ToString());
                Console.WriteLine("diag action type: {0}",args.ActionType.ToString());
                Console.WriteLine("diag event id: {0}", args.Event.EventId.ToString());
                Console.WriteLine("diag event group: {0}",args.Event.EventGroup.ToString());  
                Console.WriteLine("diag event state: {0}",args.Event.EventState.ToString());  
                Console.WriteLine("diag event originator: {0}", ((UnitEventOriginator)args.Event.AddEventOriginator)?.UnitName.ToString());
                Console.WriteLine("---------------------------------------------   ");
                await Clients.All.SendDeviceFault(AddOrRemoveFaults(args));
            }
        };

        devices = PraseansaClient.GetAllDevices(Context.ConnectionId, out var zones);
        await Clients.Client(Context.ConnectionId)
            .SendDevices(devices);

        await Clients.Client(Context.ConnectionId).SendZones(zones.Select(name => new ZoneInUse(name,false)).ToList());
        
        await base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        PraseansaClient.Disconnect();
        return base.OnDisconnectedAsync(exception);
    }

    private Device AddOrRemoveFaults(OIDiagEventEventArgs args)
    {
        var deviceName = ((UnitEventOriginator)args.Event.AddEventOriginator)?.UnitName;
        var faultName = args.Event.ToString().Substring(args.Event.ToString().IndexOf("_") + 1);

        var device = devices.First(device => device.Name.Contains(deviceName));
        
        if (args?.Event.EventState == TOIDiagEventState.OIDES_NEW)
        {
            if (device.Fault.Contains(faultName))
            {
                device.Fault.Remove(faultName);
                if (device.Fault.Count == 0)
                {
                    device.IsConnected = device.IsConnected == DeviceState.Faulted ? DeviceState.Connected : DeviceState.Disconnected;   
                }
                else
                {   
                    device.IsConnected = device.IsConnected == DeviceState.Connected ? DeviceState.Faulted : DeviceState.Disconnected;
                }
            }
            else
            {
                device.Fault.Add(faultName);
            }
        }

        return device;
    }
}

public interface INotificationClient
{
    Task SendDevices(Device[] devices);

    Task SendZoneInUse(List<ZoneInUse> updates);

    Task SendFaultState(string message);
    
    Task SendCallState(string message);
    
    Task SendZones(List<ZoneInUse> message);
    
    Task SendDeviceFault(Device fault);
    
    Task SendDevicesFault(Device[] devices);
}


