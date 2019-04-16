using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace TestingGame
{
	public class Cube : IDisposable
	{
		private readonly Vector3 currentPostion;
		private readonly RenderObject faces;
		private readonly RenderObject lines;
		private readonly float side;
		private Matrix4 modelView;
		private readonly Vector3 rotation;

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

		public void Update(double gameTime) { }

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