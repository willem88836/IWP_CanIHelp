using System;
using System.Collections.Generic;

namespace IWPCIH.EventTracking
{
	[Serializable]
	public class TimelineChapter
	{
		public int Id;
		public string Name;
		public string VideoName;
		private Dictionary<int, TimelineEventData> events;

		public int EventCount { get { return events == null ? 0 : events.Count; } }



		public TimelineChapter(int id, string name, string videoName)
		{
			this.Id = id;
			this.Name = name;
			this.VideoName = videoName;
			this.events = new Dictionary<int, TimelineEventData>();
		}


		public TimelineEventData EventAt(int i)
		{
			return events[i];
		}

		public void Foreach(Action<TimelineEventData> action)
		{
			foreach(TimelineEventData d in events.Values)
			{
				action.Invoke(d);
			}
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
