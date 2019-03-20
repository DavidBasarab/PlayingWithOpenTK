using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace TestingGame
{
	public class MainWindow : GameWindow
	{
		public MainWindow()
			: base(1280, 720, GraphicsMode.Default, "TestingGame", GameWindowFlags.Default, DisplayDevice.Default, 4, 0, GraphicsContextFlags.ForwardCompatible)
		{
			Title += $": OpenGL Version: {GL.GetString(StringName.Version)}";
		}

		protected override void OnLoad(EventArgs e)
		{
			Console.WriteLine("On Load");

			CursorVisible = true;
			VSync = VSyncMode.Off;
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			Title = $"(Vsync: {VSync}) FPS : {1f / e.Time:0}";

			var backColor = Color4.SkyBlue;

			GL.ClearColor(backColor);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			SwapBuffers();
		}

		protected override void OnResize(EventArgs e) => GL.Viewport(0, 0, Width, Height);

		protected override void OnUpdateFrame(FrameEventArgs e) => HandleKeyboard();

		private void HandleKeyboard()
		{
			var keyState = Keyboard.GetState();

			if (keyState.IsKeyDown(Key.Escape))
			{
				Console.WriteLine("Exiting . . . ");

				Exit();
			}
		}
	}
}