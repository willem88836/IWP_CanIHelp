using IWPCIH.EventTracking;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceDataField : MonoBehaviour
{
	private const string NAMEFORMAT = "Datafield_{0}_{1}";

	public Text Label;
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
		lastInput = info.GetValue(eventData);
		InputPanel.text = lastInput.ToString();
		fieldType = info.GetValue(eventData).GetType();
		
		InputPanel.onValueChanged.RemoveAllListeners();
		InputPanel.onEndEdit.AddListener((string s) => { OnValueChanged(s); });
	}

	private void OnValueChanged(string s)
	{
		object o = ParseString(s, fieldType);

		Debug.Log(o.GetType());

		if (o == null || o.GetType() != fieldType)
		{
			Debug.Log("Field input type not recognized, field reset");
			Label.text = lastInput.ToString();
			info.SetValue(eventData, lastInput);
		}
		else
		{
			info.SetValue(eventData, o);
			lastInput = o;
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
