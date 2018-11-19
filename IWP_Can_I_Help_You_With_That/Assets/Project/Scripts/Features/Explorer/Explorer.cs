using System.Collections.Generic;
using UnityEngine;

namespace IWPCIH.Explorer
{
	public class Explorer : MonoBehaviour
	{
		public class ViewActions
		{
			public enum UpdateType { None, All, OnlySelf, AllExceptSelf }

			public ExplorerView View;
			public UpdateType Type = UpdateType.None;
		}

		public List<ViewActions> Actions;


		private void Start()
		{
			string path = "";

			foreach(ViewActions action in Actions)
			{
				Update(action, path);
			}
		}

		private void OnObjectInvoke(ExplorerView parent, ExplorerViewObject invoked)
		{
			ViewActions va = Actions.Find((ViewActions v) => v.View == parent);

			if (va == null)
				return;

			Invoke(va);
		}

		private void Invoke(ViewActions va)
		{
			string path = va.View.Path;


			if (va.Type == ViewActions.UpdateType.None)
				return;

			if (va.Type == ViewActions.UpdateType.OnlySelf)
			{
				Update(va, path);
				return;
			}

			foreach (ViewActions action in Actions)
			{
				if (va.Type == ViewActions.UpdateType.AllExceptSelf && action == va)
					continue;

				Update(va, path);
			}
		}

		private void Update(ViewActions va, string path)
		{
			va.View.OnObjectInvoke += OnObjectInvoke;
			va.View.Initialize(path);
		}
	}
}