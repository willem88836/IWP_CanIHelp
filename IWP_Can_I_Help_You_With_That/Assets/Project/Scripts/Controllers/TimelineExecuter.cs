using IWPCIH.Storage;
using IWPCIH.EventTracking;
using IWPCIH.TimelineEvents;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

namespace IWPCIH
{
	// TODO: rename this to player? I mean, we're not killing anyone ;P
	/// <summary>
	///		Executes TimelineEvents in chronological order
	///		when the video has reached the event's invokation time.
	/// </summary>
	public sealed class TimelineExecuter : TimelineController
	{
		private string LoadPath { get { return Path.Combine(Application.dataPath, ProjectName.Value); } }

		public TimelineSaveLoadWrapper timelineSaveLoad;
		public VideoPlayer VideoPlayer;
		public Transform Container;

		[Space]
		public List<BaseEvent> BaseEvents;


		private List<TimelineEventData> eventData = new List<TimelineEventData>();


		protected override void Awake()
		{
			base.Awake();

			timelineSaveLoad.HardLoad();
			StartNewChapter(CurrentTimeline.GetFirst());
		}

		/// <summary>
		///		Loads a new chapter and it's events. 
		/// </summary>
		public void StartNewChapter(TimelineChapter newChapter)
		{
			CurrentChapter = CurrentTimeline.GetFirst();
			if (File.Exists(CurrentChapter.VideoName))
				VideoPlayer.url = CurrentChapter.VideoName;
			else
				throw new FileNotFoundException(string.Format("Unable to find the specified file: {0}", CurrentChapter.VideoName));

			eventData = CurrentChapter.GetChronolocalList();

			StopCoroutine(WaitForEvent());
			StartCoroutine(WaitForEvent());
		}

		private IEnumerator WaitForEvent()
		{
			VideoPlayer.Prepare();

			while (!VideoPlayer.isPrepared)
			{
				yield return null;
			}

			Debug.Log("Video player is prepared!");

			VideoPlayer.Play();

			if (!VideoPlayer.isPlaying)
			{ 
				Debug.LogWarning("Started waiting for event while videoplayer is not playing");
			}

			foreach (TimelineEventData data in eventData)
			{
				while (VideoPlayer.time < data.InvokeTime)
				{
					yield return null;
				}

				BaseEvent newEvent = BaseEvents.Find((BaseEvent e) => e.EventType == data.GetType());
				newEvent = Instantiate(newEvent, Container);
				newEvent.Event = data;
				newEvent.Invoke();

				Debug.LogFormat("Invoke timelineEvent: (id: {0}) of (type: {1}) at (time: {2})", data.Id, data.Type, data.InvokeTime);
			}
		}
	}
}
