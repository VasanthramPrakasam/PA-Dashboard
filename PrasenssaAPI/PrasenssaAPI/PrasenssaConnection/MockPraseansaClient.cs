using Bosch.PRAESENSA.OpenInterface;
using PrasenssaAPI.Models;

namespace PrasenssaAPI.PrasenssaConnection;

public class MockPraseansaClient : IPraseansaClient
{
    private string[] zones = ["a", "b", "c"];
    
    public event EventHandler<OIResourceStateEventArgs> ResourceStateChanged;

    public event EventHandler<OIResourceFaultStateEventArgs> ResourceFaultStateChanged;
    
    public event EventHandler<OICallStateChangedEventArgs> CallStateChanged;
    
    public event EventHandler<OIDiagEventEventArgs> DiagEventNotification;
    public Device[] GetAllDevices(string connectionId, out List<string> zones)
    {
        Console.WriteLine("MockPraseansaClient::GetAllDevices()");
        
        var msp = new Device("mfs-msp", [], DeviceState.Connected);

        var audio1 = new Device("mfs-audio1", [], DeviceState.Connected);

        var audio2 = new Device("f12-audio2", [], DeviceState.Connected);

        var audio3 = new Device("f12-audio3", [], DeviceState.Connected);

        //Device[] devices = [msp, audio1, audio2, audio3];

        List<string> configuredUnits = ["mfs-msp", "mfs-audio1", "f12-audio2", "f12-audio3"];

        List<string> connectedUnits = [];
        
        var devices = configuredUnits.Select(conn => new Device(conn, [], DeviceState.Connected)).ToList();
            
        devices.Where(dev => !connectedUnits.Contains(dev.Name)).ToList().ForEach(dev => dev.IsConnected = DeviceState.Disconnected);
        
        zones = ["a", "b", "c","d","e","f","g"];

        OnResourceStateChanged(1);

        OnResourceFaultStateChanged("f12-audio2");
        
        OnDiagEvent("f12_audio3");

        return devices.ToArray();    
        
    }

    public TOIErrorCode Connect(string username, string password, string ipOrHostname)
    {
        if (username == "error")
        {
            return TOIErrorCode.OIERROR_ALREADY_LOGGED_IN;
        }

        return TOIErrorCode.OIERROR_OK;
    }

    public void Disconnect()
    {
        //nothing
    }

    private async Task OnResourceStateChanged(int count)
    {
        Console.WriteLine("MockPraseansaClient::OnResourceStateChanged() started");
        var zonesInUse = new List<string>();
        zonesInUse = zonesInUse.Append(zones[count]).ToList();
        await Task.Delay(2000).ContinueWith(_ =>
        {
             ResourceStateChanged?.Invoke(this, new OIResourceStateEventArgs(zonesInUse.ToList(), Int32.MaxValue,
                Int32.MaxValue, TOIResourceState.OIRS_INUSE));
        });
        Console.WriteLine("MockPraseansaClient::OnResourceStateChanged() completed");
    }

    private async Task OnResourceFaultStateChanged(string device)
    {
        Console.WriteLine("MockPraseansaClient::OnResourceFaultStateChanged() started");
        var zonesInUse = new List<string>();
        zonesInUse = zonesInUse.Append(device).ToList();
        await Task.Delay(2000).ContinueWith(_ =>
        {
            ResourceFaultStateChanged?.Invoke(this, new OIResourceFaultStateEventArgs(zonesInUse.ToList(), TOIResourceFaultState.OIRS_FAULT));
        });
        Console.WriteLine("MockPraseansaClient::OnResourceFaultStateChanged() completed");
    }
    
    private async Task OnDiagEvent(string device)
    {
        Console.WriteLine("MockPraseansaClient::OnDiagEvent() started");
        await Task.Delay(2000).ContinueWith(_ =>
        {
            var callComplete = new CallComplete();
            //callComplete.AddEventOriginator += new UnitEventOriginator();
            DiagEventNotification?.Invoke(this,
                new OIDiagEventEventArgs(TOIActionType.OIACT_RESET, new CallComplete()));
        });
        Console.WriteLine("MockPraseansaClient::OnResourceStateChanged() completed");
    }

    
    public void SendHeartBeat()
    {
        //nothing
    }

    public void SetConnectionParams(string connectionID, ConnectionParams connectionParams)
    {
        //nothing
    }
}