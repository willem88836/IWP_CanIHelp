using IWPCIH.Storage;
using System;
using System.Collections.Generic;

namespace IWPCIH.EventTracking
{
	[Serializable]
	public class Timeline : ISavable<Timeline>
	{
		private List<TimelineChapter> chapters;
		public int ChapterCount { get { return chapters == null ? 0 : chapters.Count; } }


		public Timeline()
		{
			chapters = new List<TimelineChapter>();
		}


		/// <summary>
		///		Returns chapter at the provided Index.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public TimelineChapter GetChapter(int id)
		{
			return chapters.Find(c => c.Id == id);
		}

		public TimelineChapter GetFirst()
		{
			return chapters[0];
		}


		public void Foreach(Action<TimelineChapter> action)
		{
			foreach(TimelineChapter c in chapters)
			{
				action.Invoke(c);
			}
		}


		/// <summary>
		///		Adds a new Chapter to the list.
		/// </summary>
		public void AddChapter(TimelineChapter chapter)
		{
			chapters.Add(chapter);
		}

		/// <summary>
		///		Removes the chapter from the list.
		/// </summary>
		public void RemoveChapter(TimelineChapter chapter)
		{
			chapters.Remove(chapter);
		}

		/// <summary>
		///		Updates chapter at the provided index;
		/// </summary>
		public void UpdateChapter(int i, TimelineChapter updatedChapter)
		{
			chapters[i] = updatedChapter;
		}


		public void Save(string name)
		{
			string s_timeline = TimelineSerializer.Serialize(this);
			SaveLoad.Save(s_timeline, name);
		}

		public Timeline Load(string name)
		{
			string s_timeline = SaveLoad.Load(name);
			Timeline timeline = TimelineSerializer.Deserialize(s_timeline);
			return timeline;
			// TODO: somehow translate the deserialized timeline to the current instance.
		}
	}
}
