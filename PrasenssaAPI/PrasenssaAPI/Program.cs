using Bosch.PRAESENSA.OpenInterface;
using PrasenssaAPI.HeartBeat;
using PrasenssaAPI.Hub;
using PrasenssaAPI.PrasenssaConnection;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:5046");

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddSignalR(options => { options.DisableImplicitFromServicesParameters = false; });

builder.Services.AddPropertyInjectedServices();

builder.Services.AddCors();

Boolean.TryParse(builder.Configuration.GetSection("MockPraseansaClient").Value,out Boolean mockEnabled);

if (mockEnabled)
{
    Console.WriteLine("Mock Praseansa Client is enabled.");
    builder.Services.AddScoped<IPraseansaClient,MockPraseansaClient>();   
}
else
{
    Console.WriteLine("Mock Praseansa Client is disabled.");
    builder.Services.AddScoped<OpenInterfaceNetClient>();
    builder.Services.AddScoped<IPraseansaClient,PraseansaClient>();
}

var app = builder.Build();

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.MapSwagger();

app.MapHub<NotificationsHub>("notifications");

app.Run();
