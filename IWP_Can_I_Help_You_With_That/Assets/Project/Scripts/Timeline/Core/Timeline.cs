using System.Collections.Generic;

namespace IWPCIH.EventTracking
{
	[System.Serializable]
	public class Timeline
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
		/// <param name="i"></param>
		/// <returns></returns>
		public TimelineChapter ChapterAt(int i )
		{
			return chapters[i];
		}

		/// <summary>
		///		Adds a new Chapter to the list.
		/// </summary>
		public void AddChapter(TimelineChapter chapter)
		{
			chapters.Add(chapter);
		}

		/// <summary>
		///		Updates chapter at the provided index;
		/// </summary>
		public void UpdateChapter(int i, TimelineChapter updatedChapter)
		{
			chapters[i] = updatedChapter;
		}
	}
}
