using System;

namespace TestingGame.Common
{
	public static class Log
	{
		public static void Debug(string message) => ConsoleExtensions.WriteLineWithColor(ConsoleColor.Gray, message);

		public static void Error(string message) => ConsoleExtensions.WriteLineWithColor(ConsoleColor.Red, message);

		public static void Exception(Exception exception) => exception.PrintToConsole();

		public static void Info(string message) => ConsoleExtensions.WriteLineWithColor(ConsoleColor.Green, message);

		public static void Warn(string message) => ConsoleExtensions.WriteLineWithColor(ConsoleColor.Yellow, message);
	}
}