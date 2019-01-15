using UnityEngine;

namespace Framework.Features.DebugTools
{
	// ty 8D 
	public class Cheater : MonoBehaviour
	{
		public static Cheater instance;
		public static bool CheatsEnabled { get; private set; }
		public static bool OneKeysEnabled { get; private set; }

		// Cheat variables

		// Cheats
		public Cheat[] Cheats =
		{
			// ONEKEY: Enable Single Key Cheats For Extra Speed
			new Cheat
			{
				Code = new KeyCode[]
				{
					KeyCode.O,
					KeyCode.N,
					KeyCode.E,
					KeyCode.K,
					KeyCode.E,
					KeyCode.Y,
				},
				Effect = () =>
				{
					OneKeysEnabled = !OneKeysEnabled;
					Debug.Log("<color=grey>Single Key Cheats Enabled toggled to: \n" + CheatsEnabled + "</color>");
				},
				Text = "Single Key Cheats Enabled toggled",
			},
		};

		private void Awake()
		{
			DontDestroyOnLoad(gameObject);

#if UNITY_EDITOR
			CheatsEnabled = true;
#else
			CheatsEnabled = false;
#endif
			if (instance == null)
				OneKeysEnabled = false;
			else
				Destroy(instance.gameObject);

			instance = this;
		}

		void Update()
		{
			if (Input.anyKeyDown)
			{
				for (var i = 0; i < Cheats.Length; i++)
					Cheats[i].Update();
			}
		}

		/// <summary>
		/// Cheat
		/// </summary>
		public struct Cheat
		{
			int Index;
			public KeyCode[] Code;
			public KeyCode OneKey;
			public System.Action Effect;
			public string Text;
			public bool IgnoreCheatProtection;

			public void Update()
			{
				var target = Code[Index];
				if (OneKeysEnabled == true && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(OneKey))
				{
					Debug.Log(string.Format("<color=grey><b>OneKey Cheat activated:</b> \n{0}</color>", Text));
					Effect();
				}

				if (Input.GetKeyDown(target))
				{
					Index++;
					if (Index >= Code.Length)
					{
						if (IgnoreCheatProtection == true || CheatsEnabled == true)
						{
							Debug.Log(string.Format("<color=grey><b>Cheat activated:</b> \n{0}</color>", Text));
							Effect();
						}
					}
					else
						return;
				}

				Index = 0;

				target = Code[Index];
				if (Input.GetKeyDown(target))
				{
					Index++;
				}
			}
		}
	}
}
