using Framework.ScriptableObjects.Variables;
using IWPCIH.EventTracking;
using System;
using UnityEngine;

namespace IWPCIH
{
	public abstract class TimelineController : MonoBehaviour
	{
		public static TimelineController Instance;

		public StringReference ProjectName;

		[NonSerialized] public Timeline CurrentTimeline;
		public TimelineChapter CurrentChapter;

		protected virtual void Awake()
		{
			if (Instance != null)
			{
				Destroy(Instance.gameObject);
			}

			Instance = this;

			// TODO: Do something with proper loading instead of instantly creating a new one. 
			CurrentTimeline = new Timeline(ProjectName.Value);
		}

		public virtual void SwitchChapterTo(int id)
		{
			CurrentChapter = CurrentTimeline.GetChapter(id);
			Debug.LogFormat("Switching to Chapter (id: {0}) - {1}", id, CurrentChapter.Name);
		}


		public virtual void OnSave() { }
		public virtual void OnLoad() { }
	}
}
