using Bosch.PRAESENSA.OpenInterface;
using PrasenssaAPI.Models;

namespace PrasenssaAPI.PrasenssaConnection;


public class PraseansaClient : IPraseansaClient
{
    private static ILogger<PraseansaClient> _logger;

    private readonly OpenInterfaceNetClient oiClient;

    public Dictionary<string, ConnectionParams> idClientMap = new();

    public PraseansaClient(OpenInterfaceNetClient openInterfaceNetClient, ILogger<PraseansaClient> logger)
    {
        oiClient = openInterfaceNetClient;
        _logger = logger;
    }

    public event EventHandler<OIResourceStateEventArgs> ResourceStateChanged;

    public event EventHandler<OIResourceFaultStateEventArgs> ResourceFaultStateChanged;
    
    public event EventHandler<OICallStateChangedEventArgs> CallStateChanged;
    
    public event EventHandler<OIDiagEventEventArgs> DiagEventNotification;

    public Device[] GetAllDevices(string connectionId, out List<string> zones)
    {
        Console.WriteLine("GetAllDevices of real client");
        
        var connectionParams = idClientMap[connectionId];

        if (connectionParams == null)
        {
            zones = [];
            return [];
        }

        Console.WriteLine("Using details to connect userId :{0} , pwd : {1} , ip : {2}", connectionParams.UserId,
            connectionParams.UserId, connectionParams.Pwd);

        _logger.LogInformation("Connection log using: {user}, {password}, {host}", connectionParams.UserId,
            connectionParams.UserId, connectionParams.Pwd);

        var userId = connectionParams.UserId;
        var pwd = connectionParams.Pwd;
        var ip = connectionParams.Host;

        try
        {
            //is not connecting enough??
            oiClient.Disconnect();
            
            oiClient.Connect(ip, userId, pwd);
        
            oiClient.GetConfiguredUnits(out var configuredUnits);
        
            oiClient.GetConnectedUnits(out var connectedUnits);
            
            oiClient.GetZoneNames("",out var zoneNames);
        
            oiClient.ConnectionBroken += OnConnectionBroken;
        
            oiClient.ResourceStateChanged += ResourceStateChanged;
            
            oiClient.ResourceFaultStateChanged += ResourceFaultStateChanged;
            
            oiClient.CallStateChanged += CallStateChanged;

            oiClient.DiagEventNotification += DiagEventNotification;
            
            oiClient.SetSubscriptionEvents(true, TOIDiagEventGroup.OIDEG_FAULTEVENTGROUP);
            
            oiClient.SetSubscriptionEvents(true, TOIDiagEventGroup.OIDEG_UNKNOWNDIAGEVENTGROUP);
            
            oiClient.SetSubscriptionEvents(true, TOIDiagEventGroup.OIDEG_CALLEVENTGROUP);

            oiClient.SetSubscriptionResourcesFaultState(true, configuredUnits);
            
            Console.WriteLine($"Connected Units : ${String.Join(":",connectedUnits)}");
            
            Console.WriteLine($"configuredUnits Units : ${String.Join(":",configuredUnits)}");
        
            Console.WriteLine($"Get all zones : ${String.Join(":",zoneNames)}");

            var devices = configuredUnits.Select(conn => new Device(conn, [], DeviceState.Connected)).ToList();
            
            devices.Where(dev => !connectedUnits.Contains(dev.Name)).ToList().ForEach(dev => dev.IsConnected = DeviceState.Disconnected);
            
            zones = zoneNames;

            //subscribe to the zones
            oiClient.SetSubscriptionResources(true, zones);
            
            return devices.ToArray();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        zones = [];    

        return [];
    }

    public TOIErrorCode Connect(string username, string password, string ipOrHostname)
    {
        return oiClient.Connect(ipOrHostname, username, password);
    }

    public void Disconnect()
    {
        oiClient.Disconnect();
    }

    public void SendHeartBeat()
    {
        //any call to keep the connection alive
        oiClient.GetConfigId(out var id);
    }
    
    public void SetConnectionParams(string connectionID, ConnectionParams connectionParams)
    {
        idClientMap.Add(connectionID, connectionParams);
    }

    private void OnConnectionBroken(object sender, EventArgs args)
    {
        Console.WriteLine("OnConnectionBroken");
    }

    private void OnResourceStateChanged(object sender, OIResourceStateEventArgs args)
    {
        Console.WriteLine("OnResourceStateChanged");
        ResourceStateChanged.Invoke(sender, args);
    }
}