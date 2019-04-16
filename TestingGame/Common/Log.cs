using System;

namespace TestingGame.Common
{
	public static class Log
	{
		public static void Debug(string message) => ConsoleExtensions.WriteWithColor(ConsoleColor.Gray, message);

		public static void Error(string message) => ConsoleExtensions.WriteWithColor(ConsoleColor.Red, message);

		public static void Exception(Exception exception) => exception.PrintToConsole();

		public static void Info(string message) => ConsoleExtensions.WriteWithColor(ConsoleColor.Green, message);

		public static void Warn(string message) => ConsoleExtensions.WriteWithColor(ConsoleColor.Yellow, message);
	}
}