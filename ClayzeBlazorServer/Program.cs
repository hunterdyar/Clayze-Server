using System.Net;
using ClayzeBlazorServer;
using ClayzeBlazorServer.Components;
using ClayzeBlazorServer.Datashare;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

var app = builder.Build();

DataStoreHub.CreateDataStore("test",new ListDataStore<byte[]>());
var webSocketOptions = new WebSocketOptions
{
	KeepAliveInterval = TimeSpan.FromSeconds(30)
};

app.UseWebSockets(webSocketOptions);
app.Map("/ws1", async context =>
{
	if (context.WebSockets.IsWebSocketRequest)
	{
		var webSocket = await context.WebSockets.AcceptWebSocketAsync();
		SocketClient controller = new SocketClient(webSocket);
		await controller.Handle();
		Console.WriteLine($"websocket state: {webSocket.State}");
	}
	else
	{
		context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
	}
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.Run();