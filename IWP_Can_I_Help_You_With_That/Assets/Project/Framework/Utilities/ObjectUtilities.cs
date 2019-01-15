using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Framework.Utils
{
	/// <summary>
	///		Contains utility methods for objects.
	/// </summary>
	public static class ObjectUtilities
	{
		/// <summary>
		///		Convert an object to a byte array.
		/// </summary>
		public static byte[] ToByteArray(this object obj)
		{
			if (obj == null)
				return null;

			// TODO: this is memory wastefull. Make an attempt to not create a new object every time this is called.
			BinaryFormatter bf = new BinaryFormatter();
			MemoryStream ms = new MemoryStream();
			bf.Serialize(ms, obj);

			return ms.ToArray();
		}

		/// <summary>
		///		Convert a byte array to an Object.
		/// </summary>
		public static object ToObject(this byte[] byteArray)
		{
			// TODO: this is memory wastefull. Make an attempt to not create a new object every time this is called.
			MemoryStream memStream = new MemoryStream();
			BinaryFormatter binForm = new BinaryFormatter();
			memStream.Write(byteArray, 0, byteArray.Length);
			memStream.Seek(0, SeekOrigin.Begin);
			object obj = binForm.Deserialize(memStream);
			return obj;
		}
	}
}