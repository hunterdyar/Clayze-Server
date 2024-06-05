namespace ClayzeBlazorServer.Datashare;


public static class DataStoreHub
{
	public static Dictionary<string, IDataStore> DataStores = new Dictionary<string, IDataStore>();

	public static bool TryGetDataStore<T>(string storeid, out T store)
	{
		if(DataStores.TryGetValue(storeid, out var s))
		{
			if (s is T dstore)
			{
				 store = dstore;
				 return true;
			}
		}

		store = default(T);
		return false;
	}

	public static void CreateDataStore(string id, IDataStore store)
	{
		DataStores.Add(id,store);
	}
}