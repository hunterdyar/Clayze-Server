using System.Net.WebSockets;
using ClayzeBlazorServer.Datashare;
using ClayzeBlazorServer.Models;

namespace ClayzeBlazorServer;

public class SocketClient : WebSocketController
{
	//this datastore needs to get initialized in program and a reference injected to here.
	private ListDataStore<byte[]> _testDataStore;

	public SocketClient(WebSocket socket) : base(socket)
	{
		DataStoreHub.TryGetDataStore("test", out _testDataStore);
		_testDataStore.OnItemAdded += OnItemAddedFromOtherClient;
	}

	private async void OnItemAddedFromOtherClient(int itemID, byte[] data, string client)
	{
		//we added this! ignore!
		if (client == ID)
		{
			return;
		}

		//todo: the first 4 bytes should be the id, and we should save it.
		var packet = new byte[data.Length + 5];
		packet[0] = (byte)MessageType.Add; //set message
		BitConverter.GetBytes(itemID).CopyTo(packet, 1); //set id.
		data.CopyTo(packet, 5); //copy the rest.
		await Send(packet);
	}

	protected override async Task OnReceieve(byte[] data)
	{
		if (data.Length == 0)
		{
			return;
		}
		var messageType = (MessageType)data[0];

		switch (messageType)
		{
			case MessageType.Echo:
				await Send(data);
				break;
			case MessageType.Add:
				//remove instruction byte and add.
				var message = new byte[data.Length - 1];
				Array.ConstrainedCopy(data, 1, message, 0, data.Length - 1);
				var id = _testDataStore.AddItem(message, ID);
				var packet = new byte[5];
				BitConverter.GetBytes(id).CopyTo(packet, 1);
				packet[0] = (byte)MessageType.IDReply;
				await Send(packet);
				break;
			case MessageType.Remove:
				// var id = BitConverter.ToInt32([[]])
				break;
			case MessageType.GetAll:
				await SendAllData();
				break;
		}
	}


	private async Task SendAllData()
	{
		// var allDataPacket = DataStoreHub.Data;
		// Send(allDataPacket);
	}
}