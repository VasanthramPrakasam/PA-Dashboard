namespace PrasenssaAPI.Models;

public class LoginForm
{
    public string User { get; set; }
    public string Pwd { get; set; }
    public string Ip { get; set; }

    public LoginForm(string user, string pwd, string ip)
    {
        User = user;
        Pwd = pwd;
        Ip = ip;
    }
    
}