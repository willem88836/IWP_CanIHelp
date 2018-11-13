using System;
using UnityEngine;
using IWPCIH.TimelineEvents;

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

			timeline.Foreach((TimelineChapter chapter) =>
			{
				//TODO: make this json as well. 
				data += chapter.Id.ToString() + EVENTSPACER;
				data += chapter.Name + EVENTSPACER;
				data += chapter.VideoName + EVENTSPACER;

				for (int j = 0; j < chapter.EventCount; j++)
				{
					TimelineEventData timelineEvent = chapter.EventAt(j);
					data += JsonUtility.ToJson(timelineEvent);
					data += EVENTSPACER;
				}

				data += CHAPTERSPACER;
			});

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
				if (s_Chapter == "")
					continue;

				// Splits the chapter into data pieces.
				string[] vars = s_Chapter.Split(EVENTSPACER);

				if (vars.Length == 0)
					continue;

				// A new chapter is created.
				TimelineChapter chapter = new TimelineChapter(int.Parse(vars[0]), vars[1], vars[2]);

				// Starts later to skip chapter fields.
				for (int i = 3; i < vars.Length; i++)
				{
					// converts the var into a TimelineEvent
					string s_event = vars[i];

					if (s_event == "")
						continue;
					
					TimelineEventData timelineData = JsonUtility.FromJson<TimelineEventData>(s_event);
					Type t = TimelineEventContainer.TypeOf(timelineData.Type);
					timelineData = (TimelineEventData)JsonUtility.FromJson(s_event, t);	

					chapter.AddEvent(timelineData);
				}

				timeline.AddChapter(chapter);
			}

			return timeline;
		}
	}
}
