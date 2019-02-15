using Framework.Utils;
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
		private static MultilanguageSupport Instance;

		private const string DIRECTORY = "Languages/";
		private const string KEYPREFIX = "key_";

		private const string KEYWORDS = "Keywords";
		private const string KEYWORD = "Keyword";


		public string SelectedLanguage = "ENG";


		private Dictionary<string, string> keywords = new Dictionary<string, string>();


		private void Awake()
		{
			if (Instance != null)
			{
				Debug.LogWarning("Can't have two MultiLanguageSupport instances running at the same time!");
			}

			Instance = this;

			UpdateText();
		}

		private void UpdateText()
		{
			// XML loading.
			TextAsset textAsset = Resources.Load<TextAsset>(Path.Combine(DIRECTORY, SelectedLanguage));
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
			transform.Foreach((Transform child) =>
			{
				Text t = child.GetComponent<Text>();
				if (t == null)
					return;

				if (t.text.StartsWith(KEYPREFIX))
				{
					string key = t.text.Substring(KEYPREFIX.Length);
					t.text = GetKeyword(key);
				}
			});
		}

		public string GetKeyword(string key)
		{
			key = key.ToLower();

			if (keywords.ContainsKey(key))
			{
				return keywords[key];
			}
			else
			{
				Debug.LogWarningFormat("Missing language Key: (lang: {0}) - (key: {1})", SelectedLanguage, key);
				return key;
			}
		}

		
		public static string GetKeyWord(string key)
		{
			if (Instance != null)
			{
				return Instance.GetKeyword(key);
			}
			else
			{
				Debug.LogWarning("No MultiLanguageSupport Instance active!");
				return key;
			}
		}
	}
}
