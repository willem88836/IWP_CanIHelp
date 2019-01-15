using System;
using UnityEngine;

namespace Framework.Core
{
#if UNITY_EDITOR
	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class Int2RangeAttribute : PropertyAttribute
	{
		public readonly int Min;
		public readonly int Max;

		public Int2RangeAttribute(int min, int max)
		{
			this.Min = min;
			this.Max = max;
		}

#else
	public sealed class Int2RangeAttribute
	{
#endif
	}
}
