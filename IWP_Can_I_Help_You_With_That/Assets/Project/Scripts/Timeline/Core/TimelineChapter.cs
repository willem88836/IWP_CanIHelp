using System.Collections.Generic;

namespace IWPCIH.EventTracking
{
	[System.Serializable]
	public class TimelineChapter
	{
		public string VideoName;
		private List<TimelineEvent> events;

		public int Count { get { return events == null ? 0 : events.Count; } }



		public TimelineChapter(string videoName)
		{
			this.VideoName = videoName;
			this.events = new List<TimelineEvent>();
		}

		public TimelineChapter(string videoName, params TimelineEvent[] events)
		{
			this.VideoName = videoName;
			this.events = new List<TimelineEvent>(events);
		}

		public TimelineChapter(string videoName, List<TimelineEvent> events)
		{
			this.VideoName = videoName;
			this.events = events;
		}




		public TimelineEvent EventAt(int i)
		{
			return events[i];
		}

		public void AddEvent(TimelineEvent newEvent)
		{
			events.Add(newEvent);
		}
	}
}
