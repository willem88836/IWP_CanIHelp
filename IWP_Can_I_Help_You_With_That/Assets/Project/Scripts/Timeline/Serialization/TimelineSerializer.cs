using System;
using UnityEngine;

namespace IWPCIH.EventTracking
{
	public static class TimelineSerializer
	{
		const char CHAPTERSPACER = '\n';
		const char EVENTSPACER = (char)1000;


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



		public static Timeline Deserialize(string s_Timeline)
		{
			Timeline timeline = new Timeline();

			string[] s_Chapters = s_Timeline.Split(CHAPTERSPACER);

			foreach (string s_Chapter in s_Chapters)
			{
				string[] vars = s_Chapter.Split(EVENTSPACER);

				if (vars.Length == 0)
					continue;

				TimelineChapter chapter = new TimelineChapter(vars[0]);

				for (int i = 1; i < vars.Length; i++)
				{
					TimelineEvent timelineEvent = JsonUtility.FromJson<TimelineEvent>(vars[i]);

					if (timelineEvent == null)
						continue;

					chapter.AddEvent(timelineEvent);

					Debug.Log(timelineEvent.InvokeTime.ToString());
				}

				timeline.AddChapter(chapter);
			}

			return timeline;
		}
	}
}
