using IWPCIH.EventTracking;

namespace IWPCIH.TimelineEvents
{
	public static class EventContainer
	{
		public enum EventType { CropStart };


		public static TimelineEvent CreateEventOfType(EventType type)
		{
			switch (type)
			{
				case EventType.CropStart:
					return new CropEvent.CropEventData();
				default:
					return null;
			}
		}
	}
}
