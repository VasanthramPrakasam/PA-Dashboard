namespace PrasenssaAPI.Models;

public class ConnectionParams
{
    public ConnectionParams(string userId, string pwd, string host)
    {
        UserId = userId;
        Pwd = pwd;
        Host = host;
    }

    public string UserId { get; set; }
    public string Pwd { get; set; }
    public string Host { get; set; }

    public override string ToString()
    {
        return $"user Name: {UserId}, Fault: {Host}, pwd: {Pwd}";
    }
}