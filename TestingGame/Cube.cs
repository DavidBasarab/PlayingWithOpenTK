using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace TestingGame
{
	public class Cube : IDisposable
	{
		private readonly RenderObject faces;
		private readonly RenderObject lines;
		private readonly float side;
		private Vector3 currentPostion;
		private Matrix4 modelView;
		private Vector3 rotation;

		public Cube(Vector3 currentPostion, float side, Color4 color)
		{
			this.currentPostion = currentPostion;

			rotation = Vector3.Zero;

			this.side = side;

			lines = new RenderObject(CreateCubeLines(side));
			faces = new RenderObject(CreateSolidCube(side, color));

			modelView = Matrix4.Identity;
		}

		public void Dispose()
		{
			lines.Dispose();
			faces.Dispose();
		}

		public void Render(Matrix4 projectionMatrix)
		{
			// ModelShaderProgram.Use();
			ProjectionProgram.Use();

			// 20 is the location in the shader
			GL.UniformMatrix4(20, false, ref projectionMatrix);

			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

			faces.Bind();

			var translation = Matrix4.CreateTranslation(currentPostion);

			var rotationX = Matrix4.CreateRotationX(rotation.X);
			var rotationY = Matrix4.CreateRotationY(rotation.Y);
			var rotationZ = Matrix4.CreateRotationZ(rotation.Z);

			modelView = rotationX * rotationY * rotationZ * translation;

			GL.UniformMatrix4(21, false, ref modelView);

			faces.Render();

			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

			lines.Bind();

			lines.Render();
		}

		public void Update(KeyboardState keyState, double deltaTime)
		{
			if (keyState.IsKeyDown(Key.Q)) rotation.Y += 0.4f * (float)deltaTime;
			if (keyState.IsKeyDown(Key.E)) rotation.Y -= 0.4f * (float)deltaTime;

			if (keyState.IsKeyDown(Key.R)) rotation.X += 0.4f * (float)deltaTime;
			if (keyState.IsKeyDown(Key.F)) rotation.X -= 0.4f * (float)deltaTime;

			if (keyState.IsKeyDown(Key.W)) currentPostion.Z += 0.2f * (float)deltaTime;
			if (keyState.IsKeyDown(Key.S)) currentPostion.Z -= 0.2f * (float)deltaTime;

			if (keyState.IsKeyDown(Key.A)) currentPostion.X -= 0.2f * (float)deltaTime;
			if (keyState.IsKeyDown(Key.D)) currentPostion.X += 0.2f * (float)deltaTime;

			if (keyState.IsKeyDown(Key.LShift)) currentPostion.Y += 0.2f   * (float)deltaTime;
			if (keyState.IsKeyDown(Key.LControl)) currentPostion.Y -= 0.2f * (float)deltaTime;
		}

		private static List<Vertex> CreateCubeLines(float side)
		{
			side = side / 2f; // half side - and other half

			var vertices = new List<Vertex>
							{
								// Face
								new Vertex(new Vector4(-side, -side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(-side, -side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(-side, side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(-side, side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(-side, -side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(-side, side, side, 1.0f), Color4.Black),

								// Face
								new Vertex(new Vector4(side, -side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(side, side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(side, -side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(side, -side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(side, side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(side, side, side, 1.0f), Color4.Black),

								// Face
								new Vertex(new Vector4(-side, -side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(side, -side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(-side, -side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(-side, -side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(side, -side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(side, -side, side, 1.0f), Color4.Black),

								// Face
								new Vertex(new Vector4(-side, side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(-side, side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(side, side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(side, side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(-side, side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(side, side, side, 1.0f), Color4.Black),

								// Face
								new Vertex(new Vector4(+-side, -side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(+-side, side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(+side, -side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(+side, -side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(+-side, side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(+side, side, -side, 1.0f), Color4.Black),

								// Face
								new Vertex(new Vector4(-side, -side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(side, -side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(-side, side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(-side, side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(side, -side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(side, side, side, 1.0f), Color4.Black)
							};

			return vertices;
		}

		private static List<Vertex> CreateSolidCube(float side, Color4 color)
		{
			side = side / 2f; // half side - and other half

			var vertices = new List<Vertex>
							{
								// Face
								new Vertex(new Vector4(-side, -side, -side, 1.0f), color),
								new Vertex(new Vector4(-side, -side, side, 1.0f), color),
								new Vertex(new Vector4(-side, side, -side, 1.0f), color),
								new Vertex(new Vector4(-side, side, -side, 1.0f), color),
								new Vertex(new Vector4(-side, -side, side, 1.0f), color),
								new Vertex(new Vector4(-side, side, side, 1.0f), color),

								// Face
								new Vertex(new Vector4(side, -side, -side, 1.0f), color),
								new Vertex(new Vector4(side, side, -side, 1.0f), color),
								new Vertex(new Vector4(side, -side, side, 1.0f), color),
								new Vertex(new Vector4(side, -side, side, 1.0f), color),
								new Vertex(new Vector4(side, side, -side, 1.0f), color),
								new Vertex(new Vector4(side, side, side, 1.0f), color),

								// Face
								new Vertex(new Vector4(-side, -side, -side, 1.0f), color),
								new Vertex(new Vector4(side, -side, -side, 1.0f), color),
								new Vertex(new Vector4(-side, -side, side, 1.0f), color),
								new Vertex(new Vector4(-side, -side, side, 1.0f), color),
								new Vertex(new Vector4(side, -side, -side, 1.0f), color),
								new Vertex(new Vector4(side, -side, side, 1.0f), color),

								// Face
								new Vertex(new Vector4(-side, side, -side, 1.0f), color),
								new Vertex(new Vector4(-side, side, side, 1.0f), color),
								new Vertex(new Vector4(side, side, -side, 1.0f), color),
								new Vertex(new Vector4(side, side, -side, 1.0f), color),
								new Vertex(new Vector4(-side, side, side, 1.0f), color),
								new Vertex(new Vector4(side, side, side, 1.0f), color),

								// Face
								new Vertex(new Vector4(-side, -side, -side, 1.0f), color),
								new Vertex(new Vector4(-side, side, -side, 1.0f), color),
								new Vertex(new Vector4(side, -side, -side, 1.0f), color),
								new Vertex(new Vector4(side, -side, -side, 1.0f), color),
								new Vertex(new Vector4(-side, side, -side, 1.0f), color),
								new Vertex(new Vector4(side, side, -side, 1.0f), color),

								// Face
								new Vertex(new Vector4(-side, -side, side, 1.0f), color),
								new Vertex(new Vector4(side, -side, side, 1.0f), color),
								new Vertex(new Vector4(-side, side, side, 1.0f), color),
								new Vertex(new Vector4(-side, side, side, 1.0f), color),
								new Vertex(new Vector4(side, -side, side, 1.0f), color),
								new Vertex(new Vector4(side, side, side, 1.0f), color)
							};

			return vertices;
		}
	}
}