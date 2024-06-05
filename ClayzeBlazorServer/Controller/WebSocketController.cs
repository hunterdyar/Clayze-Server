using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;

namespace ClayzeBlazorServer;

public class WebSocketController
{
	protected WebSocket _webSocket;
	protected string ID;
	protected ArraySegment<byte> Buffer;
	public WebSocketController(WebSocket webSocket)
	{
		_webSocket = webSocket;
		ID = Guid.NewGuid().ToString();//ehhh
		Buffer = new ArraySegment<byte>(new byte[1024 * 4]);
	}
	public async Task Handle()
	{
		//loop that keeps the conenction open
		while (_webSocket.State == WebSocketState.Open)
		{
			var receiveResult = await _webSocket.ReceiveAsync(Buffer, CancellationToken.None);
			if (receiveResult.EndOfMessage)
			{
				var data = Buffer.Slice(0, receiveResult.Count).ToArray();
				await OnReceieve(data);
			}

			if (receiveResult.MessageType == WebSocketMessageType.Close)
			{
				await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,"client requested",CancellationToken.None);
			}
		}
	}

	protected virtual async Task OnReceieve(byte[] data)
	{
		Console.WriteLine("Received." + data.Length);
	}

	public async Task Send(byte[] packet)
	{
		var data = new ArraySegment<byte>(packet);
		if (_webSocket.State != WebSocketState.Open)
		{
			return;
		}
		await _webSocket.SendAsync(data, WebSocketMessageType.Binary, true,CancellationToken.None);
	}
}