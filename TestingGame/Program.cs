using System;
using System.Threading.Tasks;
using Humanizer;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

#pragma warning disable 4014

namespace TestingGame
{
	public class MainWindow : GameWindow
	{
		public MainWindow()
			: base(1280, 720, GraphicsMode.Default, "TestingGame", GameWindowFlags.Default, DisplayDevice.Default, 4, 0, GraphicsContextFlags.ForwardCompatible)
		{
			Title += $": OpenGL Version: {GL.GetString(StringName.Version)}";
		}

		protected override void OnResize(EventArgs e) => GL.Viewport(0, 0, Width, Height);

		protected override void OnLoad(EventArgs e)
		{
			Console.WriteLine("On Load");

			CursorVisible = true;
		}
	}

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