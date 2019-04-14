using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace TestingGame
{
	//http://dreamstatecoding.blogspot.com/2017/02/opengl-4-with-opentk-in-c-part-5.html
	public class MainWindow : GameWindow
	{
		private int shaderProgram;

		private string ExecutingDirectory
		{
			get
			{
				var codeBase = Assembly.GetExecutingAssembly().CodeBase;

				var uri = new UriBuilder(codeBase);

				var path = Uri.UnescapeDataString(uri.Path);

				return Path.GetDirectoryName(path);
			}
		}

		private double GameTime { get; set; }

		private List<RenderObject> RenderObjects { get; } = new List<RenderObject>();

		public MainWindow()
			: base(1280, 720, GraphicsMode.Default, "TestingGame", GameWindowFlags.Default, DisplayDevice.Default, 4, 5, GraphicsContextFlags.ForwardCompatible) => Title += $": OpenGL Version: {GL.GetString(StringName.Version)}";

		public override void Exit()
		{
			Console.WriteLine("Exit called");

			foreach (var renderObject in RenderObjects) renderObject.Dispose();

			// GL.DeleteVertexArrays(1, ref vertexArray);
			GL.DeleteProgram(shaderProgram);

			base.Exit();
		}

		protected override void OnLoad(EventArgs e)
		{
			Console.WriteLine("On Load");

			var vertices = new List<Vertex>
							{
								new Vertex(new Vector4(-.25f, 0.25f, 0.5f, 1.0f), Color4.HotPink),
								new Vertex(new Vector4(0.0f, -0.25f, 0.5f, 1.0f), Color4.Gray),
								new Vertex(new Vector4(0.25f, 0.25f, 0.5f, 1.0f), Color4.Yellow)
							};

			RenderObjects.Add(new RenderObject(vertices));

			CursorVisible = true;
			VSync = VSyncMode.Off;

			shaderProgram = CompileShaders();

			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
			GL.PatchParameter(PatchParameterInt.PatchVertices, 3);

			Closed += OnClosed;
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			GameTime += e.Time;

			Title = $"(Vsync: {VSync}) FPS : {1f / e.Time:0}";

			var backColor = Color4.CornflowerBlue;

			GL.ClearColor(backColor);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.UseProgram(shaderProgram);

			foreach (var renderObject in RenderObjects) renderObject.Render();

			SwapBuffers();
		}

		protected override void OnResize(EventArgs e) => GL.Viewport(0, 0, Width, Height);

		protected override void OnUpdateFrame(FrameEventArgs e) => HandleKeyboard();

		private int CompileShaders()
		{
			var vertexShader = GL.CreateShader(ShaderType.VertexShader);

			Console.WriteLine($"ExecutingDirectory <{ExecutingDirectory}>");

			var vertexShaderPath = Path.Combine(ExecutingDirectory, @"Shaders\vertexShader.vert");

			Console.WriteLine($"Vertex Shader Path := {vertexShaderPath}");

			GL.ShaderSource(vertexShader, File.ReadAllText(vertexShaderPath));

			GL.CompileShader(vertexShader);

			var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);

			GL.ShaderSource(fragmentShader, File.ReadAllText(Path.Combine(ExecutingDirectory, @"Shaders\fragmentShader.frag")));
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

		private void OnClosed(object sender, EventArgs e) => Exit();
	}
}