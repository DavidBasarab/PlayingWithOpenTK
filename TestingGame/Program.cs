using System;
using System.Threading.Tasks;
using Humanizer;

#pragma warning disable 4014

namespace TestingGame
{
	//http://dreamstatecoding.blogspot.com/2017/01/opengl-4-with-opentk-in-c-part-2.html
	public class ProgramSender
	{
		private static bool running = true;

		[STAThread]
		public static void Main(string[] args)
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