using IWPCIH.EventTracking;
using System;
using System.Collections.Generic;

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
					return new MultipleChoiceMenu.MultipleChoiceData();
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
					return typeof(MultipleChoiceMenu.MultipleChoiceData);
				default:
					return null;
			}
		}
	}
}
