using UnityEngine;
using IWPCIH.EventTracking;
using System.Reflection;

[RequireComponent(typeof(RectTransform))]
public class InterfaceComponent : MonoBehaviour
{
	private const string NAMEFORMAT = "InterfaceComponent_{0}";

	public InterfaceDataField BaseDataField;

	private InterfaceDataField[] dataFields;


	public void Initialize(TimelineEventData eventData)
	{
		FieldInfo[] fields = eventData.GetType().GetFields();
		dataFields = new InterfaceDataField[fields.Length];

		for (int i = 0; i < fields.Length; i++)
		{
			FieldInfo info = fields[i];
			InterfaceDataField field = Instantiate(BaseDataField, transform);
			field.Apply(info, eventData);

			dataFields[i] = field;
		}

		gameObject.name = string.Format(NAMEFORMAT, eventData.Type.ToString());
		RectTransform rect = GetComponent<RectTransform>();
		rect.sizeDelta = new Vector2(
			rect.sizeDelta.x, 
			fields.Length * BaseDataField.GetComponent<RectTransform>().sizeDelta.y);
	}
}
