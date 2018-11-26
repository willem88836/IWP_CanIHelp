using System.Collections.Generic;
using UnityEngine;

namespace Framework.ScriptableObjects.Events
{
	public class GameEvent : ScriptableObject
	{
		private List<GameEventListener> _listeners;

		public void AddGameEventListener(GameEventListener listener)
		{
			_listeners.Add(listener);
		}
		public void RemoveGameEventListener(GameEventListener listener)
		{
			_listeners.Remove(listener);
		}

		public void Raise()
		{
			for (int i = 0; i < _listeners.Count; i++)
			{
				_listeners[i].RaiseEventListener();
			}
		}
	}
}