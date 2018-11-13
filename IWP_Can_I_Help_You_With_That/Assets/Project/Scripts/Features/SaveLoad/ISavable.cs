namespace IWPCIH.Storage
{
	interface ISavable<T>
	{
		void Save(string name);
		T Load(string name);
	}
}
