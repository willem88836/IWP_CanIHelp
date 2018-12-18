namespace IWPCIH.Explorer
{
	public sealed class OVRExplorer3D : Explorer3D
	{
		private void Update()
		{
			// TODO: Test if this works.
			#if UNITY_EDITOR
				if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.B))
			#else
				if (OVRInput.GetDown(OVRInput.Button.Left))
			#endif
			{
				Back();
			}
		}
	}
}
