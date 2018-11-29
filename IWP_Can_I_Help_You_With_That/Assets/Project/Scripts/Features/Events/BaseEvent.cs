using UnityEngine;
using IWPCIH.EventTracking;
using System;

namespace IWPCIH.TimelineEvents
{
	/// <summary>
	///		Provides the base class for TimelineEvent implementations.
	/// </summary>
	public abstract class BaseEvent : MonoBehaviour
	{
		public abstract Type EventType { get; }

		/// <summary>
		///		The event's data object.
		/// </summary>
		public TimelineEventData Event;
		
		/// <summary>
		///		Is called once the this event's time is reached. 
		/// </summary>
		public abstract void Invoke();
	}
}
