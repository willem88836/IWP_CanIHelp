using IWPCIH.Storage;
using IWPCIH.EventTracking;
using IWPCIH.TimelineEvents;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

namespace IWPCIH
{
	public sealed class TimelineExecuter : TimelineController
	{
		private string LoadPath { get { return Path.Combine(Application.dataPath, ProjectName.Value); } }

		public TimelineSaveLoadWrapper timelineSaveLoad;
		public VideoPlayer VideoPlayer;

		private BaseEvent currentEvent;

		private List<float> eventInvokationTimes = new List<float>();
		private int currentEventIndex;


		protected override void Awake()
		{
			base.Awake();

			timelineSaveLoad.HardLoad();
			StartNewChapter(CurrentTimeline.GetFirst());


			VideoPlayer.Play();
		}


		public void StartNewChapter(TimelineChapter newChapter)
		{
			CurrentChapter = CurrentTimeline.GetFirst();
			VideoPlayer.url = CurrentChapter.VideoName;

			currentEventIndex = 0;

			eventInvokationTimes.Clear();
			CurrentChapter.Foreach((TimelineEventData data) =>
			{
				eventInvokationTimes.Add(data.InvokeTime);
			});
			eventInvokationTimes.Sort(); // Check if this actually sorts the stuff from small to big.

			Dictionary<float, int> keyValuePairs = new Dictionary<float, int>();

			// TODO: CONtinue here. figure a way out to efficiently load events .
			//keyValuePairs.Where((KeyValuePair<float, int> a) => a.Key ==  == )
		}


		private IEnumerator WaitForEvent(BaseEvent timelineEvent)
		{
			foreach(float invokeTime in eventInvokationTimes)
			{
				while (VideoPlayer.time < invokeTime)
					yield return new WaitForEndOfFrame();

				CurrentChapter.
			}



			timelineEvent.Invoke();
		}
	}
}
