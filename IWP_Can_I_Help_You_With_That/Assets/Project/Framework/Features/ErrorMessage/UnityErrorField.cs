using System;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Features
{
	/// <summary>
	///		Responsible for visually displaying error messages in build. 
	/// </summary>
	public class UnityErrorField : MonoBehaviour
    {
		public static UnityErrorField Instance;
		private static bool IgnoreInEditor;

		[SerializeField] private bool ignoreInEditor;
		public Text TextField;
		public Button CloseButton;

		private void Awake()
		{
			CloseButton.onClick.AddListener(delegate { gameObject.SetActive(false); });


			if (Instance != null)
			{
				Destroy(Instance.gameObject);
			}

			Instance = this;
			IgnoreInEditor = ignoreInEditor;

			gameObject.SetActive(false);
		}

		private static string GetMessage(Exception ex)
		{
			return string.Format("Error ({0}) at {1}", ex.Message, ex.Source); 
		}

		public void Invoke(Exception ex)
		{
			gameObject.SetActive(true);
			TextField.text = GetMessage(ex);
		}

		public void Invoke(string message)
		{
			gameObject.SetActive(true);
			TextField.text = message;
		}

		public static void SafeInvoke(Exception ex)
		{
			Debug.LogError(GetMessage(ex));

			if (Instance == null || (Application.isEditor && IgnoreInEditor))
				return;

			Instance.Invoke(ex);
		}

		public static void SafeInvoke(string message)
		{
			Debug.LogError(message);

			if (Instance == null || (Application.isEditor && IgnoreInEditor))
				return;

			Instance.Invoke(message);
		}
	}
}
