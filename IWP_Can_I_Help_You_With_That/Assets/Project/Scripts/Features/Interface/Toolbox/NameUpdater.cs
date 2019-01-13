using UnityEngine;
using UnityEngine.UI;
using Framework.ScriptableObjects.Variables;

namespace IWPCIH.EditorInterfaceObjects.Menu
{
	public class NameUpdater : MonoBehaviour
	{
		public InputField Input;
		public StringReference Name;

		private void Start()
		{
			Input.text = Name.Value;
		}
	}
}