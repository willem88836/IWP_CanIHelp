using UnityEngine;
using UnityEngine.UI;

namespace Framework.Features
{
	[RequireComponent(typeof(Text))]
	public class FPSCounter : MonoBehaviour
	{
		public Color low = Color.red;
		public Color high = Color.green;

		[Space]
		[Range(1, 300)] public int TargetFrameRate = 60;


		private Text fpsField;


		// Use this for initialization
		void Awake()
		{
			fpsField = GetComponent<Text>();
		}

		// Update is called once per frame
		void Update()
		{
			int fps = Mathf.RoundToInt(1f / Time.deltaTime);
			fpsField.text = fps.ToString();
			fpsField.color = Color.Lerp(low, high, Mathf.Clamp01(fps / (float)TargetFrameRate));
		}
	}
}
