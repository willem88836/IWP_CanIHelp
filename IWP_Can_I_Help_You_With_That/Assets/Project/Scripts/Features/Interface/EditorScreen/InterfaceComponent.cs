using UnityEngine;
using UnityEngine.UI;
using IWPCIH.EventTracking;
using IWPCIH.EditorInterface.Features;
using System.Reflection;

namespace IWPCIH.EditorInterface.Components
{
	[RequireComponent(typeof(RectTransform)), 
	 RequireComponent(typeof(FollowMouse)),
	 RequireComponent(typeof(HorizontalOrVerticalLayoutGroup)),
	 RequireComponent(typeof(Button))]
	public class InterfaceComponent : MonoBehaviour
	{
		private const string NAMEFORMAT = "{0}_InterfaceComponent_{1}";

		public InterfaceDataField BaseDataField;

		private TimelineEventData eventData;

		private RectTransform rect;
		private FollowMouse followMouse;
		private InterfaceDataField[] dataFields;


		private void Awake()
		{
			rect = GetComponent<RectTransform>();
			followMouse = GetComponent<FollowMouse>();

			GetComponent<Button>().onClick.AddListener(delegate 
			{
				followMouse.OffSet = rect.position - Input.mousePosition;
				followMouse.Following = !followMouse.Following;
			});
		}


		public void Initialize(TimelineEventData eventData)
		{
			this.eventData = eventData;

			gameObject.name = string.Format(NAMEFORMAT, eventData.Id, eventData.Type.ToString());

			ApplyFields(eventData);
		}

		private void ApplyFields(TimelineEventData data)
		{
			FieldInfo[] fields = data.GetType().GetFields();
			dataFields = new InterfaceDataField[fields.Length];

			System.Type t = typeof(TimelineEventData);

			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo info = fields[i];

				if (t.GetField(info.Name) != null)
					continue;

				InterfaceDataField field = Instantiate(BaseDataField, transform);
				field.Apply(info, data);

				dataFields[i] = field;
			}
		}

		public void Destroy()
		{
			TimelineController.instance.RemoveEvent(eventData);
			Destroy(gameObject);
		}
	}
}
