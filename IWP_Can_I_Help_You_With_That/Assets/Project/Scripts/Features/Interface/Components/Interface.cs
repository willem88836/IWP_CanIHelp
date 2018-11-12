using IWPCIH.EventTracking;
using System.Collections.Generic;
using UnityEngine;

namespace IWPCIH.EditorInterface.Components
{
	public class Interface : MonoBehaviour
	{
		public Transform componentContainer;
		public InterfaceComponent baseComponent;

		private List<InterfaceComponent> interfaceComponents = new List<InterfaceComponent>();


		public void Spawn(TimelineEventData data)
		{
			InterfaceComponent component = Instantiate(baseComponent, componentContainer);
			component.Initialize(data);
			component.transform.position = componentContainer.position;
			component.transform.rotation = Quaternion.identity;
			interfaceComponents.Add(component);
		}
	}
}
