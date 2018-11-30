using IWPCIH.EventTracking;
using System.Reflection;

namespace IWPCIH.EditorInterface.Components
{
	public sealed class SingleDataField : InterfaceDataField
	{
		public override void Apply(TimelineEventData eventData, FieldInfo fieldInfo)
		{
			base.Apply(eventData, fieldInfo);

			Spawn(
				fieldInfo.Name,
				fieldInfo.FieldType,
				fieldInfo.GetValue(eventData),
				(string s) => { fieldInfo.SetValue(eventData, ParseString(s, fieldInfo.FieldType)); });
		}
	}
}
