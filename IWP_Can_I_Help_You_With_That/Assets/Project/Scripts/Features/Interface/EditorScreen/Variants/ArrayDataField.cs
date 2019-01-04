using IWPCIH.EventTracking;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace IWPCIH.EditorInterfaceObjects.Components
{
	[RequireComponent(typeof(VerticalLayoutGroup))]
	public sealed class ArrayDataField : InterfaceDataField
	{
		private const string COUNTNAME = "Count";
		private const string NODATA = "";

		private object[] content;


		/// <inheritdoc />
		public override void Apply(TimelineEventData eventData, FieldInfo fieldInfo, Action<TimelineEventData> onCoreValueChanged)
		{
			base.Apply(eventData, fieldInfo, onCoreValueChanged);

			content = (object[])fieldInfo.GetValue(eventData);
			Create(onCoreValueChanged);
		}

		private void Create(Action<TimelineEventData> OnCoreValueChanged)
		{
			Clear();
			Spawn(COUNTNAME, typeof(int), content.Length, (string s) =>
			{
				int count = int.Parse(s);
				object[] copy = content;
				content = new object[count];
				for (int i = 0; i < content.Length; i++)
				{
					content[i] = copy.Length <= i ? NODATA : copy[i];
				}
				Create(OnCoreValueChanged);
			});
			UpdateField();
			SpawnContent(OnCoreValueChanged);
			// Is called to make sure the UI is updated.
			LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
		}

		/// <summary>
		///		Spawns all individual data fields. 
		/// </summary>
		private void SpawnContent(Action<TimelineEventData> onChange)
		{
			Type t = content.GetType().GetElementType();

			for (int i = 0; i < content.Length; i++)
			{
				int ix = i;
				Spawn(
					fieldInfo.Name + i,
					fieldInfo.FieldType,
					content[i],
					(string s) =>
					{
						content[ix] = Convert.ChangeType(s, t);
						UpdateField();
						onChange.Invoke(eventData);
					});
			}
		}

		/// <summary>
		///		Updates the stored array variable. 
		/// </summary>
		private void UpdateField()
		{
			Type t = fieldInfo.FieldType.GetElementType();
			Array parse = Array.CreateInstance(t, content.Length);
			Array.Copy(content, parse, content.Length);
			fieldInfo.SetValue(eventData, parse);
		}
	}
}
