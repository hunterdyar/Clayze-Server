using System.Text;
using ClayzeBlazorServer.Models;
using Microsoft.Extensions.Primitives;

namespace ClayzeBlazorServer.Models;

public static class MessageExtensions
{
	public static MessageType GetMessageType(this byte[] message)
	{
		return (MessageType)message[0];
	}
	
	public static string PrettyPrint(this byte[] message)
	{
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
				var s = ar;
				PrettyPrintOperation(sb, message);
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

	private static void PrettyPrintOperation(StringBuilder sb, byte[] data)
	{
		int offset = 0;
		OperationName op = (OperationName)data[offset];
		switch (op)
		{
			case OperationName.Sphere:
				sb.Append("Sphere");
				break;
			case OperationName.SDF:
				sb.Append("SDF");
				break;
			case OperationName.Pass:
				sb.Append("Pass");
				break;
			case OperationName.Clear:
				sb.Append("Clear");
				break;
		}
		offset++;
		var opType = (OperationType)data[offset];
		switch (opType)
		{
			case OperationType.Add:
				sb.Append(" (add) ");
				break;
			case OperationType.Pass:
				sb.Append(" (pass) ");
				break;
			case OperationType.Remove:
				sb.Append(" (remove) ");
				break;
		}
		offset++;
		switch (op)
		{
			case OperationName.Pass:
				break;
			case OperationName.Sphere:
				//[...][CenterX:4][CenterY:4][CenterZ:4][Radius:4]
				sb.Append("center:(");
				var posX = BitConverter.ToSingle(new ArraySegment<byte>(data, offset, 4));
				sb.Append(posX.ToString("N2"));
				sb.Append(", ");
				offset += 4;
				var posY = BitConverter.ToSingle(new ArraySegment<byte>(data, offset, 4));
				sb.Append(posY.ToString("N2"));
				sb.Append(", ");
				offset += 4;
				var posZ = BitConverter.ToSingle(new ArraySegment<byte>(data, offset, 4));
				sb.Append(posZ.ToString("N2"));
				sb.Append(") radius: ");
				offset += 4;
				var floatA = BitConverter.ToSingle(new ArraySegment<byte>(data, offset, 4));
				sb.Append(floatA.ToString("N2"));
				offset += 4;
				break;
		}
	}
}