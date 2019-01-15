namespace Framework.Utils
{
	/// <summary>
	///		Contains utility methods for strings.
	/// </summary>
	public static class StringUtilities
	{
		/// <summary>
		///		Combines the provided objects in to one 
		///		string separating them by the provided char.
		/// </summary>
		public static string Combine(char separator, params object[] elements)
		{
			string s = "";
			foreach (object e in elements)
			{
				s += e.ToString() + separator;
			}
			return s;
		}
	}
}
