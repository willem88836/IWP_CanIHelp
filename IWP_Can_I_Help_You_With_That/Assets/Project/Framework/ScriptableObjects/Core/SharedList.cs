using System.Collections.Generic;

namespace Framework.ScriptableObjects.Variables
{
	public abstract class SharedList<T> : ValueReference<List<T>>
	{
		public T this[int i] 
		{
			get { return Value[i]; }
		}
	}
}
