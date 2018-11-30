using IWPCIH.EventTracking;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace IWPCIH.EditorInterface.Components
{
	[RequireComponent(typeof(VerticalLayoutGroup))]
	public sealed class ArrayDataField : InterfaceDataField
	{
		private object[] content;


		public override void Apply(TimelineEventData eventData, FieldInfo fieldInfo)
		{
			base.Apply(eventData, fieldInfo);

			content = (object[])fieldInfo.GetValue(eventData);
			Create();
		}

		private void Create()
		{
			Clear();
			Spawn("Count", typeof(int), content.Length, (string s) =>
			{
				int count = int.Parse(s);
				object[] copy = content;
				content = new object[count];
				for (int i = 0; i < content.Length; i++)
				{
					content[i] = copy.Length <= i ? "" : copy[i];
				}
				Create();
			});

			SpawnContent();
			LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
		}

		private void SpawnContent()
		{
			Type t = content.GetType().GetElementType();

			for (int i = 0; i < content.Length; i++)
			{
				Spawn(
					fieldInfo.Name,
					fieldInfo.FieldType,
					content[i],
					(string s) =>
					{
						content[i] = ParseString(s, t);
						fieldInfo.SetValue(eventData, content);
					});
			}
		}
	}
}
