using UnityEngine;
using UnityEngine.UI;
using IWPCIH.EventTracking;
using IWPCIH.EditorInterface.Features;
using System.Reflection;

namespace IWPCIH.EditorInterface.Components
{
	[RequireComponent(typeof(RectTransform)), 
	 RequireComponent(typeof(FollowMouse)),
	 RequireComponent(typeof(HorizontalOrVerticalLayoutGroup))]
	public class InterfaceComponent : MonoBehaviour
	{
		private const string NAMEFORMAT = "{0}_InterfaceComponent_{1}";

		public InterfaceDataField BaseDataField;

		private FollowMouse followMouse;
		private InterfaceDataField[] dataFields;


		private void Awake()
		{
			followMouse = GetComponent<FollowMouse>();
		}


		public void Initialize(TimelineEventData eventData)
		{
			gameObject.name = string.Format(NAMEFORMAT, eventData.Id, eventData.Type.ToString());

			int fieldCount;
			ApplyFields(eventData, out fieldCount);
			ApplySize(fieldCount);
		}

		private void ApplyFields(TimelineEventData data, out int fieldCount)
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

			fieldCount = fields.Length - t.GetFields().Length;
		}

		private void ApplySize(int fieldCount)
		{
			HorizontalOrVerticalLayoutGroup vlg = GetComponent<HorizontalOrVerticalLayoutGroup>();
			RectOffset padding = vlg.padding;

			RectTransform rect = GetComponent<RectTransform>();
			rect.sizeDelta = new Vector2(
				rect.sizeDelta.x + padding.right + padding.left,
				fieldCount * BaseDataField.GetComponent<RectTransform>().sizeDelta.y + padding.top + padding.bottom + fieldCount * vlg.spacing);
		}
	}
}
