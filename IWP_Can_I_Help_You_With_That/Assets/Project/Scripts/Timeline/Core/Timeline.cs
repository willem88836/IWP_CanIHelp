using IWPCIH.Storage;
using System;
using System.Collections.Generic;

namespace IWPCIH.EventTracking
{
	[Serializable]
	public class Timeline
	{
		public int ChapterCount { get { return Chapters == null ? 0 : Chapters.Count; } }
		public string Name { get; private set; }

		public List<TimelineChapter> Chapters;

		public Timeline(string name)
		{
			Name = name;
			Chapters = new List<TimelineChapter>();
		}


		/// <summary>
		///		Returns chapter at the provided Index.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public TimelineChapter GetChapter(int id)
		{
			return Chapters.Find(c => c.Id == id);
		}

		public TimelineChapter GetFirst()
		{
			return Chapters[0];
		}
		public TimelineChapter GetLast()
		{
			return Chapters[Chapters.Count - 1];
		}

		public void ForEach(Action<TimelineChapter> action)
		{
			foreach(TimelineChapter c in Chapters)
			{
				action.Invoke(c);
			}
		}


		/// <summary>
		///		Adds a new Chapter to the list.
		/// </summary>
		public void AddChapter(TimelineChapter chapter)
		{
			Chapters.Add(chapter);
		}

		/// <summary>
		///		Removes the chapter from the list.
		/// </summary>
		public void RemoveChapter(TimelineChapter chapter)
		{
			Chapters.Remove(chapter);
		}

		/// <summary>
		///		Updates chapter at the provided index;
		/// </summary>
		public void UpdateChapter(int i, TimelineChapter updatedChapter)
		{
			Chapters[i] = updatedChapter;
		}
	}
}
