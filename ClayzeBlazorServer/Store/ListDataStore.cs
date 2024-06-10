namespace ClayzeBlazorServer.Datashare;

public class ListDataStore<T> : IDataStore
{
	//Every item in the list gets a unique ID. It's not the index, it's independent of removal/operations.
	public Action<uint,T,string> OnItemAdded;
	public Action<uint, string> OnItemRemoved;
	public Action<string> OnClear;
	private readonly List<(uint id, T item)> _data = new List<(uint,T)>();
	private uint _nextID = 1;//We use 0 for "waiting for unique id".
	public uint AddItem(T item, string client)
	{
		var id = _nextID;
		_data.Add((_nextID,item));
		_nextID++;
		OnItemAdded?.Invoke(id,item,client);
		return id;
	}
	
	public void RemoveItem(uint id, string client)
	{
		var item = _data.Find(x => x.Item1 == id);
		_data.Remove(item);
		OnItemRemoved?.Invoke(id,client);
	}

	public void Clear(string client)
	{
		_data.Clear();
		OnClear?.Invoke(client);
	}
	
	public List<(uint,T)> GetAllRawData()
	{
		return _data;
	}
	
	
}