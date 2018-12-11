using UnityEngine;
using UnityEngine.UI;
using IWPCIH.EditorInterface.Features;
using IWPCIH.EventTracking;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace IWPCIH.EditorInterface.Components
{
	// TODO: make this super abstract, and let timelinecomponent and this derive from it? 
	[RequireComponent(typeof(RectTransform)), 
	 RequireComponent(typeof(FollowMouse)),
	 RequireComponent(typeof(HorizontalOrVerticalLayoutGroup)),
	 RequireComponent(typeof(Button))]
	public class InterfaceComponent : MonoBehaviour
	{
		private const string NAMEFORMAT = "({0}) - {1}";

		public InterfaceDataField BaseDataField;
		public InterfaceDataField BaseArrayField;
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

			Type t = typeof(TimelineEventData);

			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo info = fields[i];

				if (t.GetField(info.Name) != null)
					continue;

				if (info.FieldType.IsArray)
					SpawnArrayField(data, info);
				else
					SpawnField(data, info);
			}
		}

		private InterfaceDataField SpawnField(TimelineEventData data, FieldInfo dataField)
		{
			InterfaceDataField field = Instantiate(BaseDataField, transform);
			field.Apply(data, dataField);
			dataFields.Add(field);
			return field;
		}

		private InterfaceDataField SpawnArrayField(TimelineEventData data, FieldInfo dataField)
		{
			InterfaceDataField field = Instantiate(BaseArrayField, transform);
			field.Apply(data, dataField);
			dataFields.Add(field);
			return field;
		}

		public void Destroy()
		{
			parent.Destroy(this);
		}
	}
}
