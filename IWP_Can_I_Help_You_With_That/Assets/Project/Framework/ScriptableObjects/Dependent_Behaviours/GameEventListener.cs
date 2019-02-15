using UnityEngine;
using UnityEngine.Events;

namespace Framework.ScriptableObjects.Events
{
	public class GameEventListener : MonoBehaviour
	{
		[SerializeField] GameEvent _gameEvent;
		[SerializeField] UnityEvent _events;


		// Use this for initialization
		void Awake()
		{
			_gameEvent.AddGameEventListener(this);
		}

		public void RaiseEventListener()
		{
			_events.Invoke();
		}
	}
}