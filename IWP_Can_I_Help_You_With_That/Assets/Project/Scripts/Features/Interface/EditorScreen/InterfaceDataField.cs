using IWPCIH.EventTracking;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace IWPCIH.EditorInterface.Components
{
	[RequireComponent(typeof(ContentSizeFitter))]
	public abstract class InterfaceDataField : MonoBehaviour
	{
		private const string NAMEFORMAT = "Datafield_{0}_{1}";

		public VisualField visualFieldPrefab;

		protected TimelineEventData eventData;
		protected FieldInfo fieldInfo;


		public virtual void Apply(TimelineEventData eventData, FieldInfo fieldInfo)
		{
			this.eventData = eventData;
			this.fieldInfo = fieldInfo;
			gameObject.name = string.Format(NAMEFORMAT, fieldInfo.FieldType.ToString(), fieldInfo.Name);
		}

		/// <summary>
		///		Spawns a  VisualField based on the provided info.
		/// </summary>
		protected VisualField Spawn(string name, Type type, object value, Action<string> onChange)
		{
			VisualField newField = Instantiate(visualFieldPrefab, transform);
			newField.Set(name, type, value, onChange);
			return newField;
		}

		protected void Clear()
		{
			for(int i = transform.childCount - 1; i >= 0; i--)
			{
				Destroy(transform.GetChild(i).gameObject);
			}
		}

		/// <summary>
		///		Parses the string to the corresponding type. 
		/// </summary>
		protected object ParseString(string s, Type t)
		{
			try
			{
				if (t == typeof(int))
				{
					return int.Parse(s);
				}
				else if (t == typeof(float))
				{
					return float.Parse(s);
				}
				else if (t == typeof(double))
				{
					return double.Parse(s);
				}
				else if (t == typeof(char))
				{
					return char.Parse(s);
				}

				return s;
			}
			catch (Exception ex)
			{
				Debug.LogWarningFormat("Parse to {0} Cancelled with: {1}", t.ToString(), ex.Message);
				return null;
			}
		}
	}
}
