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
	TakeOwnership = 9,
	ReleaseOwnership = 10,
	InkStart = 16,
	InkAdd = 17,
	InkEnd = 18,
	InkNewCanvas = 19,
	InkAddConfirm = 20,

}