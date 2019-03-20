using System;
using System.Threading.Tasks;
using Humanizer;
using OpenTK;

#pragma warning disable 4014

namespace TestingGame
{
	public class MainWindow : GameWindow { }

	public class ProgramSender
	{
		private static bool running = true;

		public static async Task Main(string[] args)
		{
			Console.CancelKeyPress += OnCancel;
			
			new MainWindow().Run(60);

			//await WaitForExit();
		}

		private static void OnCancel(object sender, ConsoleCancelEventArgs e)
		{
			if (e != null) e.Cancel = true;

			running = false;
		}

		private static async Task WaitForExit()
		{
			Console.WriteLine("Press Control-C to exit . . .");

			while (running) await Task.Delay(10.Milliseconds());

			Console.WriteLine("Exiting . . . .");
		}
	}
}