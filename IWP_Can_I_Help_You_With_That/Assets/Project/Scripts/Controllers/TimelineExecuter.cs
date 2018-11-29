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
		public Transform Container;

		[Space]
		public List<BaseEvent> BaseEvents;


		private BaseEvent currentEvent;

		private List<TimelineEventData> eventData = new List<TimelineEventData>();
		private int currentEventIndex;


		protected override void Awake()
		{
			base.Awake();

			timelineSaveLoad.HardLoad();
			StartNewChapter(CurrentTimeline.GetFirst());
		}


		public void StartNewChapter(TimelineChapter newChapter)
		{
			CurrentChapter = CurrentTimeline.GetFirst();
			if (File.Exists(CurrentChapter.VideoName))
				VideoPlayer.url = CurrentChapter.VideoName;

			currentEventIndex = 0;
			eventData = CurrentChapter.GetChronolocalList();

			StopCoroutine(WaitForEvent());
			StartCoroutine(WaitForEvent());
			VideoPlayer.Play();
		}


		private IEnumerator WaitForEvent()
		{
			foreach (TimelineEventData data in eventData)
			{
				while (VideoPlayer.time < data.InvokeTime)
					yield return null;

				BaseEvent newEvent = BaseEvents.Find((BaseEvent e) => e.EventType == data.GetType());
				Instantiate(newEvent, Container);

				Debug.LogFormat("invoke datafield: (id: {0})!", data.Id);
			}
		}
	}
}
