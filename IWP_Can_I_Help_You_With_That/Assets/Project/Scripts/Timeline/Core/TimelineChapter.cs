using System.Collections.Generic;

namespace IWPCIH.EventTracking
{
	[System.Serializable]
	public class TimelineChapter
	{
		public string VideoName;
		private Dictionary<int, TimelineEvent> events;

		public int EventCount { get { return events == null ? 0 : events.Count; } }



		public TimelineChapter(string videoName)
		{
			this.VideoName = videoName;
			this.events = new Dictionary<int, TimelineEvent>();
		}


		public TimelineEvent EventAt(int i)
		{
			return events[i];
		}

		public void AddEvent(TimelineEvent newEvent)
		{
			events.Add(newEvent.Id, newEvent);
		}

		public void UpdateEvent(TimelineEvent updatedEvent)
		{
			events[updatedEvent.Id] = updatedEvent;
		}
	}
}
