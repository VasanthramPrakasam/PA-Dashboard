namespace PrasenssaAPI.Models;

public class DeviceFault
{
    public string Name { get; set; }
    public string Fault { get; set; }

    public DeviceFault(string name, string fault)
    {
        Name = name;
        Fault = fault;
    }
}