using System;
using System.Threading.Tasks;

#pragma warning disable 4014

namespace MongoGame
{
	//https://www.gamefromscratch.com/post/2015/06/15/MonoGame-Tutorial-Creating-an-Application.aspx
	public class ProgramSender
	{
		private static bool running = true;

		[STAThread]
		public static void Main(string[] args)
		{
			Console.WriteLine("This is going to play around with MongoGame");

			using (var game = new Game1()) game.Run();

			//Console.CancelKeyPress += OnCancel;

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

			while (running) await Task.Delay(TimeSpan.FromMilliseconds(10));

			Console.WriteLine("Exiting . . . .");
		}
	}
}