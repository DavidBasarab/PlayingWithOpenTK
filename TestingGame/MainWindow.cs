using System;
using System.IO;
using System.Reflection;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace TestingGame
{
	public class MainWindow : GameWindow
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

		private int program;

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

			program = CompileShaders();

			Closed += OnClosed;
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

		private int CompileShaders()
		{
			var vertexShader = GL.CreateShader(ShaderType.VertexShader);

			GL.ShaderSource(vertexShader, File.ReadAllText(Path.Combine(ExecutingDirectory, "Shaders\vertexShader.vert")));

			GL.CompileShader(vertexShader);

			var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);

			GL.ShaderSource(fragmentShader, File.ReadAllText(Path.Combine(ExecutingDirectory, "Shaders\fragmentShader.frag")));
			GL.CompileShader(fragmentShader);

			var program = GL.CreateProgram();

			GL.AttachShader(program, vertexShader);
			GL.AttachShader(program, fragmentShader);

			GL.LinkProgram(program);

			GL.DetachShader(program, vertexShader);
			GL.DetachShader(program, fragmentShader);
			GL.DeleteShader(vertexShader);
			GL.DeleteShader(fragmentShader);

			return program;
		}

		private void HandleKeyboard()
		{
			var keyState = Keyboard.GetState();

			if (keyState.IsKeyDown(Key.Escape))
			{
				Console.WriteLine("Exiting . . . ");

				Exit();
			}
		}

		private void OnClosed(object sender, EventArgs e) { Exit(); }
	}
}