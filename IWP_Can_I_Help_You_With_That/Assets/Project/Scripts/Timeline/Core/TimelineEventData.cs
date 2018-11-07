using IWPCIH.TimelineEvents;
using System;

namespace IWPCIH.EventTracking
{
	[Serializable]
	public class TimelineEventData
	{
		public float InvokeTime;
		public EventContainer.EventType Type;
		public int Id;
	}
}
