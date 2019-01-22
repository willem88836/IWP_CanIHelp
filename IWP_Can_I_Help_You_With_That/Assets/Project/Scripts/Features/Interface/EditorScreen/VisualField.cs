using Framework.Core;
using Framework.Language;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace IWPCIH.EditorInterfaceObjects.Components
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
			// TODO: filter input type settings (numerical, alphanumerical, etc.) based on type. 
			name = string.Format(STRINGFORMAT, text);
			Label.text = MultilanguageSupport.GetKeyWord(text);
			Input.text = value == null ? "" : value.ToString();
			OnValueChanged = onChange;	
		}
	}
}
