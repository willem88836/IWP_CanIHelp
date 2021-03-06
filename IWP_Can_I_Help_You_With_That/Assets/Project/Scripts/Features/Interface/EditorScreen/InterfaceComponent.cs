﻿using UnityEngine;
using UnityEngine.UI;
using IWPCIH.EventTracking;
using System;
using System.Collections.Generic;
using System.Reflection;
using Framework.Language;

namespace IWPCIH.EditorInterfaceObjects.Components
{
	[RequireComponent(typeof(RectTransform)), 
	 RequireComponent(typeof(HorizontalOrVerticalLayoutGroup))]
	public class InterfaceComponent : MonoBehaviour
	{
		private const string NAMEFORMAT = "InterfaceComponent: (id: {0}) - (type: {1})";

		public InterfaceDataField BaseDataField;
		public InterfaceDataField BaseArrayField;
		public Text Header;

		public TimelineEventData EventData { get; private set; }
		private EditorInterface parent;

		private RectTransform rect; // TODO: Should this be removed? 
		private List<InterfaceDataField> dataFields = new List<InterfaceDataField>();
		private string baseHeaderText;

		private void Start()
		{
			baseHeaderText = Header.text;
			Header.text = string.Format(baseHeaderText, ' ');
			rect = GetComponent<RectTransform>();
		}


		public void Initialize(EditorInterface parent, TimelineEventData eventData)
		{
			Clear();

			this.parent = parent;
			this.EventData = eventData;

			string name = string.Format(NAMEFORMAT, eventData.Id, eventData.Type.ToString());
			gameObject.name = name;
			Header.text = string.Format(baseHeaderText, MultilanguageSupport.GetKeyWord(eventData.Type.ToString()));

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

				if (Attribute.IsDefined(info, typeof(NotEditable)))
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
			field.Apply(data, dataField, OnCoreValueChanged);
			dataFields.Add(field);
			return field;
		}

		private InterfaceDataField SpawnArrayField(TimelineEventData data, FieldInfo dataField)
		{
			InterfaceDataField field = Instantiate(BaseArrayField, transform);
			field.Apply(data, dataField, OnCoreValueChanged);
			dataFields.Add(field);
			return field;
		}

		public void Delete()
		{
			if (EventData == null)
				return;

			Clear();
			(TimelineController.Instance as TimelineEditor).RemoveEvent(EventData);
			EventData = null;
		}

		public void Clear()
		{
			Header.text = string.Format(baseHeaderText, "");

			foreach(InterfaceDataField field in dataFields)
			{
				Destroy(field.gameObject);
			}
			dataFields.Clear();
		}

		private void OnCoreValueChanged(TimelineEventData timelineEventData)
		{
			// If at some point you add additional core 
			// variables, you can update them here.
			parent.OnTimeChanged(timelineEventData);
		}
	}
}
