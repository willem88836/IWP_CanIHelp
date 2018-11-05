using UnityEngine;

namespace IWPCIH.EventTracking
{
	/// <summary>
	///		Serializes and deserializes Timeline
	/// </summary>
	public static class TimelineSerializer
	{
		private const char CHAPTERSPACER = '\n';
		private const char EVENTSPACER = 'Ϩ';


		/// <summary>
		///		Serializes the provided Timeline.
		/// </summary>
		public static string Serialize(Timeline timeline)
		{
			string data = "";

			for (int i = 0; i < timeline.Count; i++)
			{
				TimelineChapter chapter = timeline.ChapterAt(i);

				data += chapter.VideoName;
				data += EVENTSPACER;

				for (int j = 0; j < chapter.Count; j++)
				{
					TimelineEvent timelineEvent = chapter.EventAt(j);
					data += JsonUtility.ToJson(timelineEvent);
					data += EVENTSPACER;
				}

				data += CHAPTERSPACER;
			}

			return data;
		}

		/// <summary>
		///		Deseralizes the provided serialized timeline.
		/// </summary>
		public static Timeline Deserialize(string s_Timeline)
		{
			Timeline timeline = new Timeline();

			// Splits the data file into chapters. 
			string[] s_Chapters = s_Timeline.Split(CHAPTERSPACER);

			foreach (string s_Chapter in s_Chapters)
			{
				// Splits the chapter into data pieces.
				string[] vars = s_Chapter.Split(EVENTSPACER);

				if (vars.Length == 0)
					continue;

				// A new chapter is created.
				string videoName = vars[0];
				TimelineChapter chapter = new TimelineChapter(videoName);

				// Starts at 1 to skip VideoName.
				for (int i = 1; i < vars.Length; i++)
				{
					// converts the var into a TimelineEvent
					string s_event = vars[i];
					TimelineEvent timelineEvent = JsonUtility.FromJson<TimelineEvent>(s_event);

					if (timelineEvent == null)
						continue;

					chapter.AddEvent(timelineEvent);
				}

				timeline.AddChapter(chapter);
			}

			return timeline;
		}
	}
}
