using System;

namespace IWPCIH.EventTracking
{
	[AttributeUsage(AttributeTargets.Field)]
	public class NotEditable : Attribute { }
}
