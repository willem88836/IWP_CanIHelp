using Framework.Core;
using IWPCIH.EventTracking;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IWPCIH.EditorInterfaceObjects.Components
{
	public class EditorInterface : MonoBehaviour
	{
		public InterfaceComponent PreviewComponent;
		[Space]
		public Transform TimelineIntervalObjectContainer;
		public TimelineIntervalObject BaseIntervalObject;
		public int IntervalObjectSpawnInterval;
		public float IntervalObjectWidth = 100;
		[Space]
		public Transform TimelineEventButtonContainer;
		public TimelineEventButton baseEventButton;

		private List<TimelineEventButton> eventButtons = new List<TimelineEventButton>();

		public void Initialize(TimelineChapter chapter)
		{
			Clear();

			SpawnIntervalObjects(chapter.VideoLength);
			SpawnEventObjects(chapter);
		}

		private void Clear()
		{
			PreviewComponent.Clear();

			TimelineIntervalObjectContainer.ReversedForeach((Transform child) => Destroy(child.gameObject));
			TimelineEventButtonContainer.ReversedForeach((Transform child) => Destroy(child.gameObject));

			eventButtons.Clear();
		}


		private void SpawnIntervalObjects(int videoLength)
		{
			for (int time = 0; time < videoLength; time+= IntervalObjectSpawnInterval)
			{
				Instantiate(BaseIntervalObject, TimelineIntervalObjectContainer).SetTime(time);
			}
		}

		private void SpawnEventObjects(TimelineChapter chapter)
		{
			chapter.Foreach((TimelineEventData data) => Spawn(data));
		}

		public void Spawn(TimelineEventData data)
		{
			TimelineEventButton newButton = Instantiate(baseEventButton, TimelineEventButtonContainer);
			newButton.SetTime(this, data, IntervalObjectSpawnInterval, IntervalObjectWidth);
			eventButtons.Add(newButton);
		}


		public void OnEventSelected(TimelineEventData selected)
		{
			PreviewComponent.Initialize(this, selected);
		}

		public void OnTimeChanged(TimelineEventData timelineEventData)
		{
			TimelineEventButton button = eventButtons.Find((TimelineEventButton b) => b.TimelineEventData == timelineEventData);
			if (button != null)
			{
				button.SetTime(this, timelineEventData, IntervalObjectSpawnInterval, IntervalObjectWidth);
			}
		}
	}
}
