using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace TestingGame
{
	//http://dreamstatecoding.blogspot.com/2017/02/opengl-4-with-opentk-in-c-part-8.html
	//http://www.songho.ca/opengl/gl_projectionmatrix.html
	public class MainWindow : GameWindow
	{
		private const int DefaultHeight = 720;

		private const int DefaultWidth = 1280;
		private float currentX;
		private float currentY;
		private float currentZ = -0.75f;
		private float fieldOfView = 60f;

		private ShaderProgram fillShaderProgram;

		private Matrix4 projectionMatrix;
		private Vector4 rotations = Vector4.Zero;

		private bool stopped;

		public float AspectRatio => (float)Width / Height;

		private List<Cube> Cubes { get; } = new List<Cube>();

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

			Console.WriteLine("Exit called");

			foreach (var triangle in Triangles) triangle.Dispose();

			fillShaderProgram.Dispose();
			ModelShaderProgram.Dispose();
			ProjectionProgram.Dispose();

			foreach (var cube in Cubes) cube.Dispose();

			base.Exit();
		}

		protected override void OnLoad(EventArgs e)
		{
			Console.WriteLine("On Load");

			CreateProjection();

			ModelShaderProgram.Initialize();
			ProjectionProgram.Initialize();

			CreateTriangles();

			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Tan));
			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Yellow));
			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Ivory));
			// Cubes.Add(new Cube(new PointF(0f, 0f), 0.25f, Color4.Cyan));
			Cubes.Add(new Cube(Vector3.Zero, 0.15f, Color4.Blue));

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

			// var translation = Matrix4.CreateTranslation((float)(Math.Sin(k * 5f) * (count + 0.5f)),
			// 											(float)(Math.Cos(k * 5f) * (count + 0.5f)),
			// 											-2.7f);

			// var k = (float)Math.Sin((float)(GameTime * .5f));
			//var k = (float)(GameTime * .5f);

			var translation = Matrix4.CreateTranslation(currentX, currentY, currentZ);

			//rotations.X = k;
			//rotations.Y = k;

			// rotations.Z = k;

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

			//foreach (var cube in Cubes) cube.Update(GameTime);

			HandleKeyboard(e.Time);
		}

		private void CompileShaders()
		{
			fillShaderProgram = new ShaderProgram(Path.Combine(ExecutingDirectory, @"Shaders\fillVertexShader.vert"), Path.Combine(ExecutingDirectory, @"Shaders\fragmentShader.frag"));

			ModelShaderProgram.Initialize();
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

		private void HandleKeyboard(double deltaTime)
		{
			var keyState = Keyboard.GetState();

			if (keyState.IsKeyDown(Key.Escape))
			{
				Console.WriteLine("Exiting . . . ");

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

			if (keyState.IsKeyDown(Key.Q)) rotations.Y += 0.4f * (float)deltaTime;
			if (keyState.IsKeyDown(Key.E)) rotations.Y -= 0.4f * (float)deltaTime;
			
			if (keyState.IsKeyDown(Key.R)) rotations.X += 0.4f * (float)deltaTime;
			if (keyState.IsKeyDown(Key.F)) rotations.X -= 0.4f * (float)deltaTime;

			if (keyState.IsKeyDown(Key.W)) currentZ += 0.2f * (float)deltaTime;
			if (keyState.IsKeyDown(Key.S)) currentZ -= 0.2f * (float)deltaTime;

			if (keyState.IsKeyDown(Key.A)) currentX += 0.2f * (float)deltaTime;
			if (keyState.IsKeyDown(Key.D)) currentX -= 0.2f * (float)deltaTime;

			if (keyState.IsKeyDown(Key.LShift)) currentY += 0.2f   * (float)deltaTime;
			if (keyState.IsKeyDown(Key.LControl)) currentY -= 0.2f * (float)deltaTime;
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
										X = k * 13.0f + i,
										Y = k * 13.0f + i,
										Z = k * 3.0f  + i
									};

					cube.Render(projectionMatrix);
				}

				count += 0.3f;
			}
		}

		private void RenderTriangles()
		{
			fillShaderProgram.Use();

			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

			foreach (var triangle in Triangles) triangle.Render();
		}
	}
}