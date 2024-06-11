using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;

namespace ClayzeBlazorServer;

public class WebSocketController
{
	protected WebSocket _webSocket;
	protected string ClientID;
	protected ArraySegment<byte> Buffer;
	public WebSocketController(WebSocket webSocket)
	{
		_webSocket = webSocket;
		ClientID = Guid.NewGuid().ToString();//ehhh
		//the buffer needs to be large enough to hold the largest SDFs we can use.
		Buffer = new ArraySegment<byte>(new byte[64*64*64*  64*4]);
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
				await OnReceive(data);
			}

			if (receiveResult.MessageType == WebSocketMessageType.Close)
			{
				await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,"client requested",CancellationToken.None);
			}
		}
	}

	protected virtual async Task OnReceive(byte[] data)
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