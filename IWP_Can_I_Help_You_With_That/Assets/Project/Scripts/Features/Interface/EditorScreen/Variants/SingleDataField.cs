using IWPCIH.EventTracking;
using System;
using System.Reflection;

namespace IWPCIH.EditorInterfaceObjects.Components
{
	public sealed class SingleDataField : InterfaceDataField
	{
		/// <inheritdoc />
		public override void Apply(TimelineEventData eventData, FieldInfo fieldInfo, Action<TimelineEventData> onCoreValueChanged)
		{
			base.Apply(eventData, fieldInfo, onCoreValueChanged);

			Spawn(
				fieldInfo.Name,
				fieldInfo.FieldType,
				fieldInfo.GetValue(eventData),
				(string s) =>
				{ 
					fieldInfo.SetValue(eventData, Convert.ChangeType(s, fieldInfo.FieldType));
					onCoreValueChanged.Invoke(eventData);
				});
		}
	}
}
