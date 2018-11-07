using IWPCIH.EventTracking;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceDataField : MonoBehaviour
{
	private const string NAMEFORMAT = "Datafield_{0}_{1}";

	public Text Label;
	public Text InputText;
	public InputField InputPanel;

	private Type fieldType;
	private FieldInfo info;
	private TimelineEventData eventData;

	private object lastInput;


	/// <summary>
	///		Sets values corresponding to the required values. 
	///		Updates the data when values are changed. 
	/// </summary>
	/// <param name="info"></param>
	/// <param name="eventData"></param>
	public void Apply(FieldInfo info, TimelineEventData eventData)
	{
		this.info = info;
		this.eventData = eventData;

		gameObject.name = string.Format(NAMEFORMAT, info.GetType().ToString(), info.Name);
		Label.text = info.Name;
		InputText.text = info.GetValue(eventData).ToString();
		fieldType = info.GetType();

		InputPanel.onValueChanged.RemoveAllListeners();
		InputPanel.onValueChanged.AddListener((string s) => { OnValueChanged(s); });
	}

	private void OnValueChanged(string s)
	{
		object o = ParseString(s, fieldType);

		if (o == null)
		{
			info.SetValue(eventData, o);
			lastInput = o;
		}
		else
		{
			Label.text = lastInput.ToString();
			info.SetValue(eventData, lastInput);
		}
	}

	/// <summary>
	///		Parses the string to the corresponding type. 
	/// </summary>
	private static object ParseString(string s, Type fieldType)
	{
		try
		{
			if (fieldType == typeof(int))
			{
				return int.Parse(s);
			}
			else if (fieldType == typeof(float))
			{
				return float.Parse(s);
			}
			else if (fieldType == typeof(double))
			{
				return double.Parse(s);
			}

			return s;
		}
		catch (Exception ex)
		{
			Debug.LogWarning(ex.Message);
			return null;
		}
	}
}
