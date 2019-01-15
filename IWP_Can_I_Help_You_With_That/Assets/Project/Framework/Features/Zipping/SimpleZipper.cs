using System.Collections.Generic;
using System.IO;
using Framework.Utils;

namespace Framework.Zipping
{
	/// <summary>
	///		Zips a set of files or directory into a singular file. 
	///		Not nested.
	/// </summary>
	public class SimpleZipper
	{
		private const int LONGLENGTH = 58;
		private const int SHORTLENGTH = 52;


		#region Zipping

		/// <summary>
		///		Zips all files within the provided 
		///		directory into the provided extraction path.
		/// </summary>
		public FileInfo Zip(string directory, string extractionPath)
		{
			string[] files = Directory.GetFiles(directory);
			return Zip(files, extractionPath);
		}

		/// <summary>
		///		Zips all provided files into the provided extraction path.
		/// </summary>
		public FileInfo Zip(string[] files, string extractionPath)
		{
			long[] fileLengths = new long[files.Length];
			short[] nameLengths = new short[files.Length];
			List<byte> nameDataList = new List<byte>();

			long dataLength = 0;
			for (int i = 0; i < files.Length; i++)
			{
				string file = files[i];
				FileInfo info = new FileInfo(file);

				long l = info.Length;
				dataLength += l;
				fileLengths[i] = l;

				byte[] fileData = ObjectUtilities.ToByteArray(Path.GetFileName(file));
				nameDataList.AddRange(fileData);
				nameLengths[i] = (short)fileData.Length;
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
			InsertData(formatLength.ToByteArray(), ref insertIndex, ref zipData);

			// stores all the file lengths.
			foreach (long l in fileLengths)
			{
				InsertData(l.ToByteArray(), ref insertIndex, ref zipData);
			}

			// stores all filenamelength data
			foreach (short l in nameLengths)
			{
				InsertData(l.ToByteArray(), ref insertIndex, ref zipData);
			}

			// stores all filenames
			InsertData(nameData, ref insertIndex, ref zipData);

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

			File.WriteAllBytes(extractionPath, zipData);
			return new FileInfo(extractionPath);
		}

		/// <summary>
		///		Zips all provided files into the provided extraction path.
		/// </summary>
		public FileInfo Zip(List<string> files, string extractionPath)
		{
			// TODO: Make this less tacky.
			return Zip(files.ToArray(), extractionPath);
		}

		/// <summary>
		///		Inserts the data array into the provided receiver array. 
		/// </summary>
		private void InsertData(byte[] data, ref int startIndex, ref byte[] receiver)
		{
			for (int i = 0; i < data.Length; i++)
			{
				receiver[i + startIndex] = data[i];
			}
			startIndex += data.Length;
		}


		#endregion


		#region Unzipping

		/// <summary>
		///		Unzips all files contained in the 
		///		zip into the provided extraction directory.
		/// </summary>
		public DirectoryInfo Unzip(string zip, string extractionDirectory)
		{
			byte[] data = File.ReadAllBytes(zip);

			// Determines the number of files that is in the zip.
			long dataIndex = 0;
			long fileCount = ((long)SubData(data, ref dataIndex, LONGLENGTH).ToObject()) / LONGLENGTH - 1;

			// Collects file sizes.
			long[] fileSizes = new long[fileCount];
			for(int i = 0; i < fileCount; i++)
			{
				fileSizes[i] = (long)SubData(data, ref dataIndex, LONGLENGTH).ToObject();
			}

			// Collects name sizes.
			short[] namelengths = new short[fileCount];
			for(int i = 0; i < fileCount; i++)
			{
				namelengths[i] = (short)SubData(data, ref dataIndex, SHORTLENGTH).ToObject();
			}

			// Collects names. 
			string[] fileNames = new string[fileCount];
			for(int i = 0; i < fileCount; i++)
			{
				short length = namelengths[i];
				fileNames[i] = (string)SubData(data, ref dataIndex, length).ToObject();
			}

			// Unzips the individual files.
			for (int i = 0; i < fileSizes.Length; i++)
			{
				byte[] item = SubData(data, ref dataIndex, fileSizes[i]);
				string extractPath = Path.Combine(extractionDirectory, fileNames[i]);
				File.WriteAllBytes(extractPath, item);
			}

			return new DirectoryInfo(extractionDirectory);
		}

		/// <summary>
		///		Returns a data subset of the complete data set.
		/// </summary>
		private byte[] SubData(byte[] data, ref long startIndex, long length)
		{
			byte[] subData = new byte[length];
			for(int i = 0; i < length; i++)
			{
				subData[i] = data[i + startIndex];
			}
			startIndex += length;
			return subData;
		}


		#endregion	
	}
}
