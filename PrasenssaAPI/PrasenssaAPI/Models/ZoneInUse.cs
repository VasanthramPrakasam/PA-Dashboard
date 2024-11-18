namespace PrasenssaAPI.Models;

public class ZoneInUse
{
    public ZoneInUse(string name, bool inUse)
    {
        Name = name;
        InUse = inUse;
    }

    public string Name { get; set; }
    public bool InUse { get; set; }
    
    public override string ToString()
    {
        return $"Zone Use: {Name}, InUse: {InUse}";
    }
    
}