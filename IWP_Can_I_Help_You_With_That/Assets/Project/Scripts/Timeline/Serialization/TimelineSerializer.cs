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

			for (int i = 0; i < timeline.ChapterCount; i++)
			{
				TimelineChapter chapter = timeline.GetChapter(i);

				data += chapter.VideoName;
				data += EVENTSPACER;

				for (int j = 0; j < chapter.EventCount; j++)
				{
					TimelineEventData timelineEvent = chapter.EventAt(j);
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
				TimelineChapter chapter = new TimelineChapter(int.Parse(vars[0]), vars[1], vars[2]);

				// Starts at 1 to skip VideoName.
				for (int i = 3; i < vars.Length; i++)
				{
					// converts the var into a TimelineEvent
					string s_event = vars[i];

					if (s_event == "")
						continue;

					TimelineEventData timelineEvent = JsonUtility.FromJson<TimelineEventData>(s_event);

					chapter.AddEvent(timelineEvent);
				}

				timeline.AddChapter(chapter);
			}

			return timeline;
		}
	}
}
