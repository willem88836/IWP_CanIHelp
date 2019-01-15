using UnityEngine;

namespace Framework.Core
{
#if UNITY_EDITOR
	using UnityEditor;

	[CustomPropertyDrawer(typeof(Int2RangeAttribute))]
	public sealed class Int2Drawer : PropertyDrawer
	{
		const int boxHeight = 16;
		const int boxInterval = 18;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight(property, label) * 2 + 20;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Int2RangeAttribute rangeAttribute = (Int2RangeAttribute)attribute;

			SerializedProperty x = property.FindPropertyRelative("X");
			SerializedProperty y = property.FindPropertyRelative("Y");

			Rect xPosition = new Rect(position.x, position.y + (boxInterval * 1), position.width, boxHeight);
			Rect yPosition = new Rect(position.x, position.y + (boxInterval * 2), position.width, boxHeight);

			EditorGUI.LabelField(position, label.text);
			EditorGUI.IntSlider(xPosition, x, rangeAttribute.Min, rangeAttribute.Max, "       X");
			EditorGUI.IntSlider(yPosition, y, rangeAttribute.Min, rangeAttribute.Max, "       Y");
		}
#else
	public sealed class Int2Drawer
	{ 
#endif
	}
}
