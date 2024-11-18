namespace PrasenssaAPI.Models;

public class Device
{
    public Device(string name, List<string> fault, DeviceState isConnected)
    {
        Name = name;
        Fault = fault;
        IsConnected = isConnected;
    }

    public string Name { get; set; }
    public List<string> Fault { get; set; }
    public DeviceState IsConnected { get; set; }

    public string DisplayName
    {
        get
        {
            var index = Name.IndexOf('_');
            return index != -1 ? Name.Substring(index+1) : Name;   
        }
    }

    public string DisplayFault
    {
        get
        {
            var index = Name.IndexOf('_');
            return (index != -1 && IsConnected == DeviceState.Faulted) ? Name.Substring(index + 1) : string.Empty;       
        }
    }

    public override string ToString()
    {
        return $"Device Name: {DisplayName}, Fault: {Fault}, IsConnected: {IsConnected}";
    }
}