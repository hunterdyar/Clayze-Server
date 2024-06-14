using System.Net;
using ClayzeBlazorServer;
using ClayzeBlazorServer.Components;
using ClayzeBlazorServer.Datashare;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();
string[] stores = new[] { "test", "ws1", "props","a","b","c","d" };
var app = builder.Build();

var webSocketOptions = new WebSocketOptions
{
	KeepAliveInterval = TimeSpan.FromSeconds(30)
};

app.UseWebSockets(webSocketOptions);

//Create a datastore and an endpoint for our list.
foreach (string storeID in stores)
{
	DataStoreHub.CreateDataStore(storeID, new ListDataStore<byte[]>());

	app.Map(storeID, async context =>
	{
		if (context.WebSockets.IsWebSocketRequest)
		{
			var webSocket = await context.WebSockets.AcceptWebSocketAsync();
			DataStoreHub.TryGetDataStore(storeID, out ListDataStore<byte[]> store);
			SocketClient controller = new SocketClient(webSocket, store);
			await controller.Handle();

		}
		else
		{
			context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
		}
	});

}


//

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