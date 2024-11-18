using Bosch.PRAESENSA.OpenInterface;
using PrasenssaAPI.Models;

namespace PrasenssaAPI.PrasenssaConnection;

 public interface IPraseansaClient
{
    Device[] GetAllDevices(string connectionId, out List<string> zones);

    TOIErrorCode Connect(String username, String password,String ipOrHostname);

    void SendHeartBeat();
    
    void Disconnect();

    void SetConnectionParams(string connectionID, ConnectionParams connectionParams);
    
     event EventHandler<OIResourceStateEventArgs> ResourceStateChanged;

     event EventHandler<OIResourceFaultStateEventArgs> ResourceFaultStateChanged;
    
     event EventHandler<OICallStateChangedEventArgs> CallStateChanged;
    
     event EventHandler<OIDiagEventEventArgs> DiagEventNotification;
}