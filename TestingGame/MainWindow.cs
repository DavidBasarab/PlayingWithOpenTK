using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace TestingGame
{
	public class ShaderProgram : IDisposable
	{
		private readonly string fragmentShaderPath;
		private readonly string vertexShaderPath;

		private int programId = -77;

		public ShaderProgram(string vertexShaderPath, string fragmentShaderPath)
		{
			this.vertexShaderPath = vertexShaderPath;
			this.fragmentShaderPath = fragmentShaderPath;

			VerifyFileExists(vertexShaderPath, "vert");
			VerifyFileExists(fragmentShaderPath, "frag");

			LoadShader();
		}

		public void Dispose()
		{
			if (programId != -77) GL.DeleteProgram(programId);
		}

		public void Use() => GL.UseProgram(programId);

		private void LoadShader()
		{
			var vertexShader = GL.CreateShader(ShaderType.VertexShader);

			Console.WriteLine($"Vertex Shader Path := {vertexShaderPath}");

			GL.ShaderSource(vertexShader, File.ReadAllText(vertexShaderPath));

			GL.CompileShader(vertexShader);

			var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			
			Console.WriteLine($"Fragment Shader Path := {fragmentShaderPath}");

			GL.ShaderSource(fragmentShader, File.ReadAllText(fragmentShaderPath));
			GL.CompileShader(fragmentShader);

			programId = GL.CreateProgram();

			GL.AttachShader(programId, vertexShader);
			GL.AttachShader(programId, fragmentShader);

			GL.LinkProgram(programId);

			GL.DetachShader(programId, vertexShader);
			GL.DetachShader(programId, fragmentShader);
			GL.DeleteShader(vertexShader);
			GL.DeleteShader(fragmentShader);
		}

		private static void VerifyFileExists(string path, string extension)
		{
			if (!File.Exists(path)) throw new FileNotFoundException(path);

			var fileExtension = Path.GetExtension(path);

			if (fileExtension.ToLower() == extension.ToLower()) throw new InvalidOperationException($"Extension should be <{extension}>");
		}
	}

	//http://dreamstatecoding.blogspot.com/2017/02/opengl-4-with-opentk-in-c-part-6.html
	public class MainWindow : GameWindow
	{
		private ShaderProgram fillShaderProgram;
		private bool stopped;

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

		private List<Triangle> Triangles { get; } = new List<Triangle>();

		public MainWindow()
			: base(1280, 720, GraphicsMode.Default, "TestingGame", GameWindowFlags.Default, DisplayDevice.Default, 4, 5, GraphicsContextFlags.ForwardCompatible) => Title += $": OpenGL Version: {GL.GetString(StringName.Version)}";

		public override void Exit()
		{
			if (stopped) return;

			stopped = true;

			Console.WriteLine("Exit called");

			foreach (var triangle in Triangles) triangle.Dispose();

			fillShaderProgram.Dispose();

			base.Exit();
		}

		protected override void OnLoad(EventArgs e)
		{
			Console.WriteLine("On Load");

			CreateTriangles();
			var cubeVertices = ShapeFactory.CreateSolidCube(1.0f, Color4.Red);

			RenderObjects.Add(new RenderObject(cubeVertices));

			CursorVisible = true;
			VSync = VSyncMode.Off;

			CompileShaders();

			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
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

			RenderTriangles();

			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

			foreach (var renderObject in RenderObjects) renderObject.Render();

			SwapBuffers();
		}

		protected override void OnResize(EventArgs e) => GL.Viewport(0, 0, Width, Height);

		protected override void OnUpdateFrame(FrameEventArgs e) => HandleKeyboard();

		private void CompileShaders() { fillShaderProgram = new ShaderProgram(Path.Combine(ExecutingDirectory, @"Shaders\fillVertexShader.vert"), Path.Combine(ExecutingDirectory, @"Shaders\fragmentShader.frag")); }

		private void CreateTriangles()
		{
			var triangle = new Triangle
							{
								PointA = new PointF(-0.5f, 0.0f),
								PointB = new PointF(-1f, -1f),
								PointC = new PointF(0f, -1f),
								Color = Color.Purple
							};

			triangle.Initialize();

			Triangles.Add(triangle);

			triangle = new Triangle
						{
							PointA = new PointF(-0.5f, 0.0f),
							PointB = new PointF(-1f, 1f),
							PointC = new PointF(0f, 1f),
							Color = Color.HotPink
						};

			triangle.Initialize();

			Triangles.Add(triangle);

			triangle = new Triangle
						{
							PointA = new PointF(1.0f, 0.5f),
							PointB = new PointF(0f, 1f),
							PointC = new PointF(0f, 0f),
							Color = Color.Green
						};

			triangle.Initialize();

			Triangles.Add(triangle);

			triangle = new Triangle
						{
							PointA = new PointF(1.0f, -0.5f),
							PointB = new PointF(0f, -1f),
							PointC = new PointF(0f, 0f),
							Color = Color.Orange
						};

			triangle.Initialize();

			Triangles.Add(triangle);
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

		private void RenderTriangles()
		{
			fillShaderProgram.Use();

			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

			foreach (var triangle in Triangles) triangle.Render();
		}
	}
}