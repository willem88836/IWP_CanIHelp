using IWPCIH.EventTracking;
using System;
using System.Reflection;

namespace IWPCIH.EditorInterface.Components
{
	public sealed class SingleDataField : InterfaceDataField
	{
		/// <inheritdoc />
		public override void Apply(TimelineEventData eventData, FieldInfo fieldInfo)
		{
			base.Apply(eventData, fieldInfo);

			Spawn(
				fieldInfo.Name,
				fieldInfo.FieldType,
				fieldInfo.GetValue(eventData),
				(string s) => { fieldInfo.SetValue(eventData, Convert.ChangeType(s, fieldInfo.FieldType)); });
		}
	}
}
