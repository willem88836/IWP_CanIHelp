using IWPCIH.EventTracking;
using IWPCIH.Storage;
using System.Collections.Generic;
using UnityEngine;

namespace IWPCIH.EditorInterface.Components
{
	public class Interface : MonoBehaviour, ISavable<Vector3[]>
	{
		public Transform InterfaceComponentContainer;
		public InterfaceComponent baseInterfaceComponent;

		[Space]
		public Transform TimelineComponentContainer;
		public TimelineComponent BaseTimelineComponent;


		private List<InterfaceComponent> interfaceComponents = new List<InterfaceComponent>();
		private List<TimelineComponent> timelineComponents = new List<TimelineComponent>();


		public void Initialize(TimelineChapter chapter)
		{
			Clear();
			Vector3[] locations = Load(chapter.Name);

			int i = 0; 
			chapter.Foreach((TimelineEventData data) => 
			{
				Spawn(data);
				InterfaceComponent component = interfaceComponents[i];
				component.gameObject.GetComponent<RectTransform>().position = locations[i];
				i++;
			});
		}

		public void Spawn(TimelineEventData data)
		{
			// Creates a new interface component.
			InterfaceComponent interfaceComponent = Instantiate(
				baseInterfaceComponent, 
				InterfaceComponentContainer.position, 
				Quaternion.identity, 
				InterfaceComponentContainer);
			interfaceComponent.Initialize(this, data);
			interfaceComponents.Add(interfaceComponent);

			// Creates a new timeline Component.
			TimelineComponent timelineComponent = Instantiate(
				BaseTimelineComponent, 
				TimelineComponentContainer.position, 
				Quaternion.identity, 
				TimelineComponentContainer);
			timelineComponent.Initialize(this, data);
			timelineComponents.Add(timelineComponent);
		}
		public void Destroy(InterfaceComponent component)
		{
			(TimelineController.Instance as TimelineEditor).RemoveEvent(component.EventData);
			interfaceComponents.Remove(component);
			Destroy(component.gameObject);
		}

		public void Clear()
		{
			foreach (InterfaceComponent c in interfaceComponents)
			{
				Destroy(c.gameObject);
			}
			interfaceComponents.Clear();
		}


		public void Save(string name)
		{
			SaveLoad.Extention = "";
			string s_components = "";
			foreach(InterfaceComponent c in interfaceComponents)
			{
				s_components += JsonUtility.ToJson(c.GetComponent<RectTransform>().position)+ '\n';
			}
			SaveLoad.Save(s_components, name);
		}

		public Vector3[] Load(string name)
		{
			SaveLoad.Extention = "";
			SaveLoad.SavePath = TimelineSaveLoad.SoftSavePath;
			string s_components = SaveLoad.Load(name);
			string[] split = s_components.Split('\n');
			Vector3[] locations = new Vector3[Mathf.Clamp(split.Length - 1, 0, int.MaxValue)];
			for(int i = 0; i < split.Length - 1; i ++)
			{
				string c = split[i];

				if (c == "")
					continue;

				locations[i] = JsonUtility.FromJson<Vector3>(c);
			}

			return locations;
		}
	}
}
