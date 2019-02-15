using UnityEngine;

namespace Framework.ScriptableObjects.Events
{
	public class SharedValue<T> : GameEvent
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
				Raise();
			}
		}
	}
}