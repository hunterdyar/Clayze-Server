// using ClayzeBlazorServer.Datashare;
// using ClayzeBlazorServer.Models;
//
// namespace ClayzeBlazorServer;
//
// public class SocketClient
// {
// 	
// 	//this datastore needs to get initialized in program and a reference injected to here.
// 	private ListDataStore<byte[]> _testDataStore;
//
// 	public SocketClient()
// 	{
// 		DataStoreHub.TryGetDataStore("test", out _testDataStore);
// 		_testDataStore.OnItemAdded += OnItemAddedFromOtherClient;
// 	}
//
// 	private void OnItemAddedFromOtherClient(int itemID, byte[] data, string client)
// 	{
// 		//we added this! ignore!
// 		if (client == ID)
// 		{
// 			return;
// 		}
//
// 		//todo: the first 4 bytes should be the id, and we should save it.
// 		var packet = new byte[data.Length + 5];
// 		packet[0] = (byte)MessageType.Add; //set message
// 		BitConverter.GetBytes(itemID).CopyTo(packet, 1); //set id.
// 		data.CopyTo(packet, 5); //copy the rest.
// 		Send(packet);
// 	}
//
// 	protected override void OnMessage(MessageEventArgs e)
// 	{
// 		Console.WriteLine("Message Got");
// 		var messageType = (MessageType)e.RawData[0];
//
// 		switch (messageType)
// 		{
// 			case MessageType.Echo:
// 				Send(e.RawData);
// 				break;
// 			case MessageType.Add:
// 				//remove instruction byte and add.
// 				var message = new byte[e.RawData.Length - 1];
// 				Array.ConstrainedCopy(e.RawData, 1, message, 0, e.RawData.Length - 1);
// 				var id = _testDataStore.AddItem(message, ID);
// 				var packet = new byte[5];
// 				BitConverter.GetBytes(id).CopyTo(packet, 1);
// 				packet[0] = (byte)MessageType.IDReply;
// 				Send(packet);
// 				break;
// 			case MessageType.Remove:
// 				// var id = BitConverter.ToInt32([[]])
// 				break;
// 			case MessageType.GetAll:
// 				SendAllData();
// 				break;
// 		}
// 	}
//
//
// 	private void SendAllData()
// 	{
// 		// var allDataPacket = DataStoreHub.Data;
// 		// Send(allDataPacket);
// 	}
//
// 	protected override void OnOpen()
// 	{
// 		Console.WriteLine($"Client Connected: {ID}");
// 		base.OnOpen();
// 	}
//
// 	protected override void OnClose(CloseEventArgs e)
// 	{
// 		base.OnClose(e);
// 	}
// }