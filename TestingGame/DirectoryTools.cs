using System;
using System.IO;
using System.Reflection;

namespace TestingGame
{
	public static class DirectoryTools
	{
		public static string ExecutingDirectory
		{
			get
			{
				var codeBase = Assembly.GetExecutingAssembly().CodeBase;

				var uri = new UriBuilder(codeBase);

				var path = Uri.UnescapeDataString(uri.Path);

				return Path.GetDirectoryName(path);
			}
		}

		public static void CreateDirectory(string directory)
		{
			if (IsValidDir(directory)) return;

			Directory.CreateDirectory(directory);
		}

		public static bool DoesFileExist(string fileNamePath) => !string.IsNullOrEmpty(fileNamePath) && File.Exists(fileNamePath);

		public static string FormatDirectory(string directory)
		{
			if (directory.EndsWith(@"\") || directory.EndsWith("/")) return directory;

			if (directory.IndexOf("/") >= 0) directory += "/";
			else directory += @"\";

			return directory;
		}

		internal static void CreateFile(string fullFilePath)
		{
			if (DoesFileExist(fullFilePath)) return;

			var file = File.Create(fullFilePath);

			// Flush and Close the file, otherwise it cannot be used
			file.Flush();
			file.Close();
		}

		private static bool IsValidDir(string fileNamePath)
		{
			var intLast = fileNamePath.LastIndexOf("/");

			if (intLast == -1) intLast = fileNamePath.LastIndexOf(@"\");

			var intCount = fileNamePath.Length - intLast;

			var strDirPath = fileNamePath.Remove(intLast, intCount);

			return Directory.Exists(strDirPath);
		}
	}
}