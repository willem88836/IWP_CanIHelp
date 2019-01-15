namespace Framework.Utils
{
	public static class MathUtilities
	{
		/// <summary>
		///		Returns true if v is more than min and less than max.
		/// </summary>
		public static bool LiesBetween(this float v, float min, float max)
		{
			return min < v && v < max;
		}

		/// <summary>
		///		Returns true if v is more than min and less than max.
		/// </summary>
		public static bool LiesBetween(this int v, int min, int max)
		{
			return min < v && v < max;
		}
	}
}
