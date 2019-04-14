using System;
using System.IO;
using System.Reflection;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace TestingGame
{
	public struct Vertex
	{
		public const int Size = (4 + 4) * 4;

		private readonly Vector4 position;
		private readonly Color4 color;

		public Vertex(Vector4 position, Color4 color)
		{
			this.position = position;
			this.color = color;
		}
	}

	public class RenderObject : IDisposable
	{
		private readonly int buffer;

		private readonly int vertexArray;
		private readonly int verticeCount;
		private bool initialized;

		public RenderObject(Vertex[] vertices)
		{
			verticeCount = vertices.Length;

			vertexArray = GL.GenVertexArray();
			buffer = GL.GenBuffer();

			GL.BindVertexArray(vertexArray);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexArray);

			// create first buffer: vertex
			GL.NamedBufferStorage(
								buffer,
								Vertex.Size * vertices.Length,   // the size needed by this buffer
								vertices,                        // data to initialize with
								BufferStorageFlags.MapWriteBit); // at this point we will only write to the buffer

			GL.VertexArrayAttribBinding(vertexArray, 0, 0);
			GL.EnableVertexArrayAttrib(vertexArray, 0);

			GL.VertexArrayAttribFormat(
										vertexArray,
										0,                      // attribute index, from the shader location = 0
										4,                      // size of attribute, vec4
										VertexAttribType.Float, // contains floats
										false,                  // does not need to be normalized as it is already, floats ignore this flag anyway
										0);                     // relative offset, first item

			GL.VertexArrayAttribBinding(vertexArray, 1, 0);

			GL.EnableVertexArrayAttrib(vertexArray, 1);

			GL.VertexArrayAttribFormat(
										vertexArray,
										1,                      // attribute index, from the shader location = 1
										4,                      // size of attribute, vec4
										VertexAttribType.Float, // contains floats
										false,                  // does not need to be normalized as it is already, floats ignore this flag anyway
										16);                    // relative offset after a vec4

			// link the vertex array and buffer and provide the stride as size of Vertex
			GL.VertexArrayVertexBuffer(vertexArray, 0, buffer, IntPtr.Zero, Vertex.Size);

			initialized = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Render()
		{
			GL.BindVertexArray(vertexArray);
			GL.DrawArrays(PrimitiveType.Triangles, 0, verticeCount);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposing) return;

			if (!initialized) return;

			GL.DeleteVertexArray(vertexArray);
			GL.DeleteBuffer(buffer);

			initialized = false;
		}
	}

	//http://dreamstatecoding.blogspot.com/2017/02/opengl-4-with-opentk-in-c-part-5.html
	public class MainWindow : GameWindow
	{
		private int shaderProgram;

		private int vertexArray;

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

		public MainWindow()
			: base(1280, 720, GraphicsMode.Default, "TestingGame", GameWindowFlags.Default, DisplayDevice.Default, 4, 5, GraphicsContextFlags.ForwardCompatible) => Title += $": OpenGL Version: {GL.GetString(StringName.Version)}";

		public override void Exit()
		{
			GL.DeleteVertexArrays(1, ref vertexArray);
			GL.DeleteProgram(shaderProgram);

			base.Exit();
		}

		protected override void OnLoad(EventArgs e)
		{
			Console.WriteLine("On Load");

			CursorVisible = true;
			VSync = VSyncMode.Off;

			shaderProgram = CompileShaders();

			GL.GenVertexArrays(1, out vertexArray);
			GL.BindVertexArray(vertexArray);

			Closed += OnClosed;
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			GameTime += e.Time;

			Title = $"(Vsync: {VSync}) FPS : {1f / e.Time:0}";

			var backColor = Color4.CornflowerBlue;

			GL.ClearColor(backColor);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			DrawRectangle(new Vector4
						{
							X = .1f + (float)Math.Sin(GameTime) * 0.5f,
							Y = .1f + (float)Math.Cos(GameTime) * 0.5f,
							Z = 0.0f,
							W = 1.0f
						});

			DrawRectangle(new Vector4
						{
							X = .4f + (float)Math.Sin(GameTime) * 0.25f,
							Y = .6f + (float)Math.Cos(GameTime) * 0.25f,
							Z = 0.0f,
							W = 1.0f
						});

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

		private void DrawRectangle(Vector4 position)
		{
			GL.UseProgram(shaderProgram);

			GL.DrawArrays(PrimitiveType.Points, 0, 1);
			GL.PointSize(55);

			GL.VertexAttrib1(0, GameTime);

			GL.VertexAttrib4(1, position);
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