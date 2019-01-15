using System;

namespace Framework.Core
{
	[Serializable]
	public struct Int2
	{
		public int X;
		public int Y;

		public Int2(int x, int y)
		{
			X = x;
			Y = y;
		}

		public static Int2 operator -(Int2 a, Int2 b)
		{
			a.X -= b.X;
			a.Y -= b.Y;
			return a;
		}

		public static Int2 operator +(Int2 a, Int2 b)
		{
			a.X += b.X;
			a.Y += b.Y;
			return a;
		}

		public static Int2 operator *(Int2 a, int b)
		{
			a.X *= b;
			a.Y *= b;
			return a;
		}

		public static explicit operator UnityEngine.Vector2(Int2 int2)
		{
			return new UnityEngine.Vector2(int2.X, int2.Y);
		}
	}
}
