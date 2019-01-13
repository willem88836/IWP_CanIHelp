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
		public int VideoLength;
		private Dictionary<int, TimelineEventData> events;

		public int EventCount { get { return events == null ? 0 : events.Count; } }


		public TimelineChapter(int id, string name, string videoName, int videoLength)
		{
			this.Id = id;
			this.Name = name;
			this.VideoName = videoName;
			this.VideoLength = videoLength;
			this.events = new Dictionary<int, TimelineEventData>();
		}

		public int NextId()
		{
			int lastId = -1;
			Foreach((TimelineEventData data) => { if (data.Id > lastId) lastId = data.Id; });
			return ++lastId;
		}

		public TimelineEventData EventAt(int id)
		{
			UnityEngine.Debug.Log(id + " is Contained = " + events.ContainsKey(id));
			if (events.ContainsKey(id))
				return events[id];
			else
				return null;
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
			UnityEngine.Debug.LogFormat("Added Event (Id: {0}) of type {1}", newData.Id.ToString(), newData.Type.ToString());
		}

		public void UpdateEvent(TimelineEventData updatedData)
		{
			events[updatedData.Id] = updatedData;
			UnityEngine.Debug.LogFormat("Updated Event (Id: {0}) of type {1}", updatedData.Id.ToString(), updatedData.Type.ToString());
		}

		public void RemoveEvent(TimelineEventData data)
		{
			events.Remove(data.Id);
			UnityEngine.Debug.LogFormat("Removed Event (Id: {0}) of type {1}", data.Id.ToString(), data.Type.ToString());
		}

		public List<TimelineEventData> GetChronolocalList()
		{
			List<TimelineEventData> chronologicalEvents = new List<TimelineEventData>();
			foreach (TimelineEventData data in events.Values)
			{
				chronologicalEvents.Add(data);
			}
			chronologicalEvents.Sort(delegate (TimelineEventData a, TimelineEventData b) 
				{ return (a.InvokeTime < b.InvokeTime) ? -1 : 1; });

			return chronologicalEvents;
		}
	}
}
