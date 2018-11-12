using System.Collections.Generic;

namespace IWPCIH.EventTracking
{
	[System.Serializable]
	public class TimelineChapter
	{
		public string VideoName;
		private Dictionary<int, TimelineEventData> events;

		public int EventCount { get { return events == null ? 0 : events.Count; } }



		public TimelineChapter(string videoName)
		{
			this.VideoName = videoName;
			this.events = new Dictionary<int, TimelineEventData>();
		}


		public TimelineEventData EventAt(int i)
		{
			return events[i];
		}

		public void AddEvent(TimelineEventData newData)
		{
			events.Add(newData.Id, newData);
		}

		public void UpdateEvent(TimelineEventData updatedData)
		{
			events[updatedData.Id] = updatedData;
		}

		public void RemoveEvent(TimelineEventData data)
		{
			events.Remove(data.Id);
		}
	}
}
