using Framework.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace IWPCIH.EditorInterface.Components
{
	public class VisualField : MonoBehaviour
	{
		private const string STRINGFORMAT = "VisualField_{0}";

		public Text Label;
		public InputField Input;
		public Action<string> OnValueChanged;


		private void Awake()
		{
			Input.onEndEdit.AddListener((string s) => { OnValueChanged.SafeInvoke(s); });
		}

		public void Set(string text, Type type, object value, Action<string> onChange)
		{
			// TODO: filter input type based on type. 
			name = string.Format(STRINGFORMAT, text);
			Label.text = text;
			Input.text = value.ToString();
			OnValueChanged = onChange;	
		}
	}
}
