using UnityEngine;

namespace Framework.ScriptableObjects.Variables
{
	public class ValueReference<T> : ScriptableObject
	{
		[SerializeField] private T _value;

		public T Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}
	}
}