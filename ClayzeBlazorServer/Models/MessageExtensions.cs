using System.Text;
using ClayzeBlazorServer.Models;

namespace ClayzeBlazorServer.Models;

public static class MessageExtensions
{
	public static MessageType GetMessageType(this byte[] message)
	{
		return (MessageType)message[0];
	}
	
	public static string PrettyPrint(this byte[] message)
	{
		return System.Text.Encoding.UTF8.GetString(message);
		StringBuilder sb = new StringBuilder();
		var mt = message.GetMessageType();
		switch (mt)
		{
			case MessageType.Echo:
				sb.Append("Echo");
				break;
			case MessageType.Add:
				sb.Append("Add: ");
				var ar = new ArraySegment<byte>(message);
				var s = ar.Slice(5).ToArray();
				sb.Append(System.Text.Encoding.UTF8.GetString(s));
				break;
			case MessageType.Remove:
				sb.Append("Remove");
				break;
			case MessageType.GetAll:
				sb.Append("Get All");
				break;
		}

		return sb.ToString();
	}
	
}