using IWPCIH.EventTracking;
using IWPCIH.Storage;
using System.Collections.Generic;
using UnityEngine;

namespace IWPCIH.EditorInterface.Components
{
	public class Interface : MonoBehaviour, ISavable<Vector3[]>
	{
		public Transform componentContainer;
		public InterfaceComponent baseComponent;

		private List<InterfaceComponent> interfaceComponents = new List<InterfaceComponent>();


		public void Initialize(TimelineChapter chapter)
		{
			Vector3[] locations = Load(chapter.Name);
			int i = 0; 
			chapter.Foreach((TimelineEventData data) => 
			{
				Spawn(data);
				InterfaceComponent component = interfaceComponents[i];
				component.GetComponent<RectTransform>().position = locations[i];
				
				i++;
			});
		}

		public void Spawn(TimelineEventData data)
		{
			InterfaceComponent component = Instantiate(baseComponent, componentContainer);
			component.Initialize(data);
			component.transform.position = componentContainer.position;
			component.transform.rotation = Quaternion.identity;
			interfaceComponents.Add(component);
		}


		public void Save(string name)
		{
			string s_components = "";
			foreach(InterfaceComponent c in interfaceComponents)
			{
				s_components += JsonUtility.ToJson(c.GetComponent<RectTransform>().position)+ '\n';
			}
			SaveLoad.Save(s_components, name);
		}

		public Vector3[] Load(string name)
		{
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
