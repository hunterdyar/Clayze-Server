namespace ClayzeBlazorServer.Models;

public enum OperationName : byte
{
	Pass = 0,
	Sphere = 1,
	SDF = 2,
	Clear = 3
}

public enum OperationType : byte
{
    Add,
    Remove,
    Pass,
}