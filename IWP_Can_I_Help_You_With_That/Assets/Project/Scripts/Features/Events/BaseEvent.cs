using UnityEngine;
using IWPCIH.EventTracking;

namespace IWPCIH.TimelineEvents
{
	/// <summary>
	///		Provides the base class for TimelineEvent implementations.
	/// </summary>
	public abstract class BaseEvent : MonoBehaviour
	{
		/// <summary>
		///		The event's data object.
		/// </summary>
		public TimelineEvent Event;

		/// <summary>
		///		Is called once the this event's time is reached. 
		/// </summary>
		public abstract void Invoke();
	}
}
