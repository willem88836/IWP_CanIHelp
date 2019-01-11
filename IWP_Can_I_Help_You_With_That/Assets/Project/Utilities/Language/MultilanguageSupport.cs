using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Language
{
	/// <summary>
	///		Updates all its childs' text objects that  
	///		refer to a key within the language file.
	/// </summary>
	[RequireComponent(typeof(RectTransform))]
	public class MultilanguageSupport : MonoBehaviour
	{
		private const string DIRECTORY = "Languages/";
		private const string KEYPREFIX = "key_";

		private const string KEYWORDS = "KeyWords";
		private const string KEYWORD = "KeyWord";


		public string SelectedLanguage = "ENG";


		private Dictionary<string, string> keywords = new Dictionary<string, string>();


		private void Awake()
		{
			UpdateText(SelectedLanguage);
		}

		private void UpdateText(string language)
		{
			// XML loading.
			TextAsset textAsset = Resources.Load<TextAsset>(Path.Combine(DIRECTORY, language));
			XmlDocument xml = new XmlDocument();
			xml.LoadXml(textAsset.text);

			// Acquires all keys and values.
			foreach (XmlElement keywordList in xml.SelectNodes(KEYWORDS))
			{
				foreach (XmlElement words in keywordList.SelectNodes(KEYWORD))
				{
					string k = words.ChildNodes.Item(0).InnerText;
					string v = words.ChildNodes.Item(1).InnerText;
					keywords.Add(k, v);
				}
			}

			// Sets values when referenced to in the Text element.
			foreach (Text t in transform.GetComponentsInChildren<Text>())
			{
				if (t.text.StartsWith(KEYPREFIX))
				{
					string key = t.text.Substring(KEYPREFIX.Length);
					t.text = GetKeyword(key);
				}
			}
		}

		public string GetKeyword(string key)
		{
			return keywords.ContainsKey(key) ? keywords[key] : key;
		}
	}
}
