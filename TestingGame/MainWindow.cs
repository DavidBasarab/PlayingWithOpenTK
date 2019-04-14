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
	public class Triangle : IDisposable
	{
		private RenderObject renderObject;

		public Color Color { get; set; }

		public PointF PointA { get; set; }

		public PointF PointB { get; set; }

		public PointF PointC { get; set; }

		public void Dispose() => renderObject.Dispose();

		public void Initialize()
		{
			Console.WriteLine($"PointA <{PointA}> | PointB <{PointB}> | PointC <{PointC}>");

			var vertices = new List<Vertex>
							{
								CreateVertex(PointA),
								CreateVertex(PointB),
								CreateVertex(PointC)
							};

			renderObject = new RenderObject(vertices);
		}

		public void Render() => renderObject.Render();

		private Vertex CreateVertex(PointF point) => new Vertex(new Vector4(point.X, point.Y, 0.5f, 1.0f), Color);
	}

	//http://dreamstatecoding.blogspot.com/2017/02/opengl-4-with-opentk-in-c-part-5.html
	public class MainWindow : GameWindow
	{
		private int shaderProgram;

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

		private List<Triangle> Triangles { get; } = new List<Triangle>();

		public MainWindow()
			: base(1280, 720, GraphicsMode.Default, "TestingGame", GameWindowFlags.Default, DisplayDevice.Default, 4, 5, GraphicsContextFlags.ForwardCompatible) => Title += $": OpenGL Version: {GL.GetString(StringName.Version)}";

		public override void Exit()
		{
			if (stopped) return;

			stopped = true;

			Console.WriteLine("Exit called");

			foreach (var triangle in Triangles) triangle.Dispose();

			// GL.DeleteVertexArrays(1, ref vertexArray);
			GL.DeleteProgram(shaderProgram);

			base.Exit();
		}

		protected override void OnLoad(EventArgs e)
		{
			Console.WriteLine("On Load");

			CreateTriangles();

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

			foreach (var triangle in Triangles) triangle.Render();

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