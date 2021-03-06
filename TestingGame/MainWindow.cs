using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using TestingGame.Common;

namespace TestingGame
{
	//http://dreamstatecoding.blogspot.com/2017/02/opengl-4-with-opentk-in-c-part-9.html
	//http://www.songho.ca/opengl/gl_projectionmatrix.html
	public class MainWindow : GameWindow
	{
		private const int DefaultHeight = 720;
		private const int DefaultWidth = 1280;

		private const float StartingZ = -0.75f;

		private float fieldOfView = 60f;

		private ShaderProgramLegacy fillShaderProgramLegacy;

		private Matrix4 projectionMatrix;

		private bool stopped;

		public float AspectRatio => (float)Width / Height;

		private List<BasicCube> Cubes { get; } = new List<BasicCube>();

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
			: base(DefaultWidth, DefaultHeight, GraphicsMode.Default, "TestingGame", GameWindowFlags.Default, DisplayDevice.Default, 4, 5, GraphicsContextFlags.ForwardCompatible) => Title += $": OpenGL Version: {GL.GetString(StringName.Version)}";

		public override void Exit()
		{
			if (stopped) return;

			stopped = true;

			Log.Info("Exit called");

			foreach (var triangle in Triangles) triangle.Dispose();

			fillShaderProgramLegacy.Dispose();
			ModelShaderProgram.Dispose();
			ProjectionProgram.Dispose();

			foreach (var cube in Cubes) cube.Dispose();

			base.Exit();
		}

		protected override void OnLoad(EventArgs e)
		{
			Log.Info("On load");

			var item = new TexturedVertex(Vector4.One, Vector2.One);

			var sizeOfStruct = Marshal.SizeOf(item);

			Log.Debug($"Size Struct <{sizeOfStruct}> | TexturedVertex <{TexturedVertex.Size}>");

			CreateProjection();

			//CreateTriangles();
			CreateCubes();

			var solidProgram = new ShaderProgram();

			solidProgram.AddShader(ShaderType.VertexShader, Path.Combine(DirectoryTools.ExecutingDirectory, @"Shaders\PipeVertexShader.vert"));
			solidProgram.AddShader(ShaderType.FragmentShader, Path.Combine(DirectoryTools.ExecutingDirectory, @"Shaders\PipeVertexShader.vert"));

			CursorVisible = true;
			VSync = VSyncMode.Off;

			CompileShaders();

			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
			GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
			GL.Enable(EnableCap.DepthTest);

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

			var cube = Cubes.FirstOrDefault();

			cube.Render(projectionMatrix);

			//PineWheelCubes();

			GL.PointSize(10);

			SwapBuffers();
		}

		protected override void OnResize(EventArgs e)
		{
			GL.Viewport(0, 0, Width, Height);

			CreateProjection();
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			//GameTime += e.Time;
			var keyState = Keyboard.GetState();

			foreach (var cube in Cubes) cube.Update(keyState, e.Time);

			HandleKeyboard(keyState);
		}

		private void CompileShaders()
		{
			fillShaderProgramLegacy = new ShaderProgramLegacy(Path.Combine(ExecutingDirectory, @"Shaders\fillVertexShader.vert"), Path.Combine(ExecutingDirectory, @"Shaders\fragmentShader.frag"));

			ModelShaderProgram.Initialize();
			ProjectionProgram.Initialize();
		}

		private void CreateCubes()
		{ 
			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Tan));
			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Yellow));
			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Ivory));
			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Cyan));
			Cubes.Add(new BasicCube(new Vector3(0.25f, 0, StartingZ), 0.15f, Color4.Blue));

			//
			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Tan));
			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Yellow));
			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Ivory));
			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Cyan));
			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Blue));
			//
			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Tan));
			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Yellow));
			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Ivory));
			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Cyan));
			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Blue));
			//
			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Tan));
			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Yellow));
			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Ivory));
			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Cyan));
			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Blue));

			// Cubes.Add(new Cube(.2f, Color4.HotPink));
		}

		private void CreateProjection()
		{
			projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(fieldOfView * (float)Math.PI / 180f, AspectRatio, .001f, 4000f);

			//projectionMatrix = Matrix4.CreatePerspectiveOffCenter(0, DefaultWidth, DefaultHeight, 0, 1f, 4000f);
		}

		private void CreateTriangles()
		{
			var triangle = new Triangle
							{
								PointA = new PointF(-0.5f, 0.0f), PointB = new PointF(-1f, -1f), PointC = new PointF(0f, -1f), Color = Color.Purple
							};

			triangle.Initialize();

			Triangles.Add(triangle);

			triangle = new Triangle
						{
							PointA = new PointF(-0.5f, 0.0f), PointB = new PointF(-1f, 1f), PointC = new PointF(0f, 1f), Color = Color.HotPink
						};

			triangle.Initialize();

			Triangles.Add(triangle);

			triangle = new Triangle
						{
							PointA = new PointF(1.0f, 0.5f), PointB = new PointF(0f, 1f), PointC = new PointF(0f, 0f), Color = Color.Green
						};

			triangle.Initialize();

			Triangles.Add(triangle);

			triangle = new Triangle
						{
							PointA = new PointF(1.0f, -0.5f), PointB = new PointF(0f, -1f), PointC = new PointF(0f, 0f), Color = Color.Orange
						};

			triangle.Initialize();

			Triangles.Add(triangle);
		}

		private void HandleKeyboard(KeyboardState keyState)
		{
			if (keyState.IsKeyDown(Key.Escape))
			{
				Log.Warn("Exiting . . . . ");

				Exit();
			}

			if (keyState.IsKeyDown(Key.M)) GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Point);

			if (keyState.IsKeyDown(Key.Comma)) GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

			if (keyState.IsKeyDown(Key.Period)) GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

			if (keyState.IsKeyDown(Key.J))
			{
				fieldOfView = 40f;
				CreateProjection();
			}

			if (keyState.IsKeyDown(Key.K))
			{
				fieldOfView = 50f;
				CreateProjection();
			}

			if (keyState.IsKeyDown(Key.L))
			{
				fieldOfView = 60f;
				CreateProjection();
			}
		}

		private void OnClosed(object sender, EventArgs e) => Exit();

		private void PineWheelCubes()
		{
			var count = 0.0f;

			foreach (var cube in Cubes)
			{
				for (var i = 0; i < 5; i++)
				{
					var k = i + (float)(GameTime * (.05f + .1 * count));

					var translation = Matrix4.CreateTranslation((float)(Math.Sin(k * 5f) * (count + 0.5f)),
																(float)(Math.Cos(k * 5f) * (count + 0.5f)),
																-2.7f);

					var rotations = new Vector4
									{
										X = k * 13.0f + i, Y = k * 13.0f + i, Z = k * 3.0f + i
									};

					cube.Render(projectionMatrix);
				}

				count += 0.3f;
			}
		}

		private void RenderTriangles()
		{
			fillShaderProgramLegacy.Use();

			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

			foreach (var triangle in Triangles) triangle.Render();
		}
	}
}