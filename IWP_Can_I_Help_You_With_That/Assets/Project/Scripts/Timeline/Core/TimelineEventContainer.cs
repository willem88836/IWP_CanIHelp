using IWPCIH.EventTracking;
using System;

namespace IWPCIH.TimelineEvents
{
	//TODO: Please remove this script in some way. This is the ugliest thing I've seen in a long time.
	public static class TimelineEventContainer
	{
		public enum EventType { CropStart, MultipleChoiceMenu, SwitchScenes, VideoEnd };


		public static TimelineEventData CreateEventDataOfType(EventType type)
		{
			switch (type)
			{
				case EventType.CropStart:
					return new CropEvent.CropEventData();
				case EventType.MultipleChoiceMenu:
					return new MultipleChoiceMenu.MultipleChoiceData();
				case EventType.SwitchScenes:
					return new SwitchScenes.SwitchScenesData();
				case EventType.VideoEnd:
					return new TimelineEventData();
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
				case EventType.SwitchScenes:
					return typeof(SwitchScenes.SwitchScenesData);
				case EventType.VideoEnd:
					return typeof(TimelineEventData);
				default:
					return null;
			}
		}
	}
}
