﻿namespace ClayzeBlazorServer.Models;

public enum MessageType
{
	Echo = 0,
	Add = 1,
	Remove = 2,
	GetAll = 3,
	IDReply = 4,
	Clear = 5,
	Change = 6,
	ChangeConfirm = 7,
	Event = 8,
}