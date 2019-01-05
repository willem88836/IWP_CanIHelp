using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Framework.Zipping
{
	/// <summary>
	///		zips and unzips files within one directory together.
	///		Note: Not nested.
	/// </summary>
	public class SimpleZipper
	{
		private const int LONGLENGTH = 58;
		private const int SHORTLENGTH = 52;

		private string directory;
		private string target;


		public SimpleZipper(string directory, string target)
		{
			this.directory = directory;
			this.target = target;
		}


		#region Zipping

		public FileInfo Zip()
		{
			string[] files = Directory.GetFiles(directory);

			long[] fileLengths = new long[files.Length];
			short[] nameLengths = new short[files.Length];

			long dataLength = 0;
			for (int i = 0; i < files.Length; i++)
			{
				string file = files[i];

				FileInfo info = new FileInfo(file);
				if (info != null)
				{
					long l = info.Length;
					dataLength += l;
					fileLengths[i] = l;
					nameLengths[i] = (short)ObjectToByteArray(Path.GetFileName(file)).Length;
				}
			}

			// stores all names. 
			List<byte> nameDataList = new List<byte>();
			foreach(string name in files)
			{
				nameDataList.AddRange(ObjectToByteArray(Path.GetFileName(name)));
			}
			byte[] nameData = nameDataList.ToArray();

			// stores all formatting data.
			long formatLength = LONGLENGTH					// this variable
				+ (files.Length * LONGLENGTH);				// the lengths of the files

			dataLength += formatLength
				+ (nameLengths.Length * SHORTLENGTH)        // the lengths of the lengths of the names
				+ (nameData.Length);                        // the lengths of the names.
			
			// the data.
			byte[] zipData = new byte[dataLength];

			// stores the format info.
			int insertIndex = 0;
			insertIndex = StoreData(ObjectToByteArray(formatLength), 0, ref zipData);

			// stores all the file lengths.
			foreach (long l in fileLengths)
			{
				insertIndex = StoreData(ObjectToByteArray(l), insertIndex, ref zipData);
			}

			// stores all filenamelength data
			foreach (short l in nameLengths)
			{
				insertIndex = StoreData(ObjectToByteArray(l), insertIndex, ref zipData);
			}

			// stores all filenames
			insertIndex = StoreData(nameData, insertIndex, ref zipData);

			// stores all files
			foreach (string file in files)
			{
				byte[] fileData = File.ReadAllBytes(file);
				foreach (byte b in fileData)
				{
					zipData[insertIndex] = b;
					insertIndex++;
				}
			}

			File.WriteAllBytes(target, zipData);
			return new FileInfo(target);
		}

		private int StoreData(byte[] data, int startIndex, ref byte[] receiver)
		{
			for (int i = 0; i < data.Length; i++)
			{
				receiver[i + startIndex] = data[i];
			}
			return startIndex + data.Length;
		}

		// Convert an object to a byte array
		private byte[] ObjectToByteArray(object obj)
		{
			if (obj == null)
				return null;

			BinaryFormatter bf = new BinaryFormatter();
			MemoryStream ms = new MemoryStream();
			bf.Serialize(ms, obj);

			return ms.ToArray();
		}

		#endregion


		#region Unzip

		public DirectoryInfo Unzip()
		{
			byte[] data = File.ReadAllBytes(target);

			// Determines the number of files that is in the zip.
			long dataIndex = 0;
			long fileCount = ((long)ByteArrayToObject(SubData(data, ref dataIndex, LONGLENGTH))) / LONGLENGTH - 1;

			// Collects file sizes.
			long[] fileSizes = new long[fileCount];
			for(int i = 0; i < fileCount; i++)
			{
				fileSizes[i] = (long)ByteArrayToObject(SubData(data, ref dataIndex, LONGLENGTH));
			}

			// Collects name sizes.
			short[] namelengths = new short[fileCount];
			for(int i = 0; i < fileCount; i++)
			{
				namelengths[i] = (short)ByteArrayToObject(SubData(data, ref dataIndex, SHORTLENGTH));
			}

			// Collects names. 
			string[] fileNames = new string[fileCount];
			for(int i = 0; i < fileCount; i++)
			{
				short length = namelengths[i];
				fileNames[i] = (string)ByteArrayToObject(SubData(data, ref dataIndex, length));
			}

			// Unzips the individual files.
			for (int i = 0; i < fileSizes.Length; i++)
			{
				byte[] item = SubData(data, ref dataIndex, fileSizes[i]);
				string extractPath = Path.Combine(directory, fileNames[i]);
				File.WriteAllBytes(extractPath, item);
			}

			return new DirectoryInfo(directory);
		}

		private byte[] SubData(byte[] allBytes, ref long startIndex, long length)
		{
			byte[] subData = new byte[length];
			for(int i = 0; i < length; i++)
			{
				subData[i] = allBytes[i + startIndex];
			}
			startIndex += length;
			return subData;
		}

		// Convert a byte array to an Object
		private object ByteArrayToObject(byte[] arrBytes)
		{
			MemoryStream memStream = new MemoryStream();
			BinaryFormatter binForm = new BinaryFormatter();
			memStream.Write(arrBytes, 0, arrBytes.Length);
			memStream.Seek(0, SeekOrigin.Begin);
			object obj = (object)binForm.Deserialize(memStream);
			return obj;
		}

		#endregion	
	}
}
