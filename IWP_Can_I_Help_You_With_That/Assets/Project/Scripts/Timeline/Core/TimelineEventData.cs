using IWPCIH.TimelineEvents;
using System;

namespace IWPCIH.EventTracking
{
	[Serializable]
	public class TimelineEventData
	{
		[CoreData] public float InvokeTime;
		[CoreData, NotEditable] public TimelineEventContainer.EventType Type;
		[CoreData, NotEditable] public int Id;
	}
}
