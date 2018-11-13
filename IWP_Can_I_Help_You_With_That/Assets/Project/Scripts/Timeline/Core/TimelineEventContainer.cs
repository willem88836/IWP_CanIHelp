using IWPCIH.EventTracking;
using System;

namespace IWPCIH.TimelineEvents
{
	public static class TimelineEventContainer
	{
		public enum EventType { CropStart, MultipleChoiceMenu };


		public static TimelineEventData CreateEventDataOfType(EventType type)
		{
			switch (type)
			{
				case EventType.CropStart:
					return new CropEvent.CropEventData();
				case EventType.MultipleChoiceMenu:
					return new CropEvent.CropEventData();
				default:
					return null;
			}
		}

		public static Type TypeOf(EventType type)
		{
			switch (type)
			{
				case EventType.CropStart:
					return typeof(CropEvent.CropEventData);
				case EventType.MultipleChoiceMenu:
					return typeof(CropEvent.CropEventData);
				default:
					return null;
			}
		}
	}
}
