using Framework.ScriptableObjects.Variables;
using Framework.Core;
using IWPCIH.EventTracking;
using System.IO;
using UnityEngine;

namespace IWPCIH.Storage
{
	public class TimelineSaveLoadWrapper : MonoBehaviour
	{
		public static TimelineSaveLoadWrapper Instance;

		public StringReference SavePath;
		public StringReference LoadPath;

		private TimelineSaveLoad timelineSaveLoad = new TimelineSaveLoad();

		private void Awake()
		{
			if (Instance != null)
				Destroy(Instance.gameObject);

			Instance = this;
		}


		public void SoftSave()
		{
			TimelineController.Instance.OnSave();
			Timeline timeline = TimelineController.Instance.CurrentTimeline;
			timelineSaveLoad.SoftSave(timeline);
		}

		// TODO: When saving, make sure the project has a proper name. 
		// add an action or warning or something .
		public void HardSave()
		{
			TimelineController.Instance.OnSave();
			Timeline timeline = TimelineController.Instance.CurrentTimeline;
			timelineSaveLoad.HardSave(timeline, SavePath.Value);
		}



		public void SoftLoad()
		{
			Timeline timeline = TimelineController.Instance.CurrentTimeline;
			string fileName = Path.GetFileNameWithoutExtension(SavePath.Value);
			timelineSaveLoad.SoftLoad(ref timeline, fileName);
			TimelineController.Instance.OnLoad();
		}

		public void HardLoad()
		{
			Timeline timeline = TimelineController.Instance.CurrentTimeline;
			timelineSaveLoad.HardLoad(ref timeline, LoadPath.Value);
			TimelineController.Instance.OnLoad();
		}
	}
}
