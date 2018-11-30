using UnityEngine;
using UnityEngine.UI;
using IWPCIH.EventTracking;
using IWPCIH.EditorInterface.Features;
using System.Reflection;
using System.Collections.Generic;

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
		public Text Header;

		public TimelineEventData EventData { get; private set; }
		private Interface parent;

		private RectTransform rect;
		private FollowMouse followMouse;
		private List<InterfaceDataField> dataFields = new List<InterfaceDataField>();


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


		public void Initialize(Interface parent, TimelineEventData eventData)
		{
			this.parent = parent;
			this.EventData = eventData;

			string name = string.Format(NAMEFORMAT, eventData.Id, eventData.Type.ToString());
			Header.text = gameObject.name = name;

			ApplyFields(eventData);
		}

		private void ApplyFields(TimelineEventData data)
		{
			FieldInfo[] fields = data.GetType().GetFields();
			dataFields.Clear();

			System.Type t = typeof(TimelineEventData);

			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo info = fields[i];

				if (t.GetField(info.Name) != null)
					continue;

				SpawnField(info, data);
			}
		}

		public InterfaceDataField SpawnField(FieldInfo info, TimelineEventData data)
		{
			InterfaceDataField field = Instantiate(BaseDataField, transform);
			field.Apply(this, info, data);
			dataFields.Add(field);
			return field;
		}

		public int IndexOf(InterfaceDataField field)
		{
			return dataFields.IndexOf(field); 
		}


		public void Destroy()
		{
			parent.Destroy(this);
		}
	}
}
