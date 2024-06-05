namespace ClayzeBlazorServer.Models;

public static class MessageUtilityExtensions
{
	static MessageType GetMessageType(this byte[] )
	static string PrettyPrint(byte[] message)
	{
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
	
}