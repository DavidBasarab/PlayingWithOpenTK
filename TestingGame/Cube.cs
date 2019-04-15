using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace TestingGame
{
	public class Cube : IDisposable
	{
		private readonly RenderObject faces;
		private readonly RenderObject lines;
		private Matrix4 modelView;

		public Cube(PointF centerPoint, float side, Color4 color)
		{
			lines = new RenderObject(CreateCubeLines(centerPoint, side));
			faces = new RenderObject(CreateSolidCube(centerPoint, side, color));
		}

		public void Dispose()
		{
			lines.Dispose();
			faces.Dispose();
		}

		public void Render()
		{
			ModelShaderProgram.Use();

			// 20 is the location in the shader
			GL.UniformMatrix4(20, false, ref modelView);

			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

			faces.Render();

			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

			lines.Render();
		}

		public void Update(double gameTime)
		{
			var k = (float)gameTime * 0.05f;

			var r1 = Matrix4.CreateRotationX(k * 6.0f);
			var r2 = Matrix4.CreateRotationY(k * 6.0f);
			var r3 = Matrix4.CreateRotationZ(k * 1.5f);

			modelView = r1 * r2 * r3;
		}

		private static List<Vertex> CreateCubeLines(PointF centerPoint, float side)
		{
			side = side / 2f; // half side - and other half

			var vertices = new List<Vertex>
							{
								// Face
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + -side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + -side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + -side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + side, side, 1.0f), Color4.Black),

								// Face
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y + -side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y + side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y + -side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y + -side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y + side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y + side, side, 1.0f), Color4.Black),

								// Face
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + -side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + -side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + -side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + -side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + -side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + -side, side, 1.0f), Color4.Black),

								// Face
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + side, side, 1.0f), Color4.Black),

								// Face
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + -side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + -side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + -side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + side, -side, 1.0f), Color4.Black),

								// Face
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + -side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + -side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + -side, side, 1.0f), Color4.Black),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + side, side, 1.0f), Color4.Black)
							};

			return vertices;
		}

		private static List<Vertex> CreateSolidCube(PointF centerPoint, float side, Color4 color)
		{
			side = side / 2f; // half side - and other half

			var vertices = new List<Vertex>
							{
								// Face
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + -side, -side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + -side, side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + side, -side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + side, -side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + -side, side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + side, side, 1.0f), color),

								// Face
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y + -side, -side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y + side, -side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y + -side, side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y + -side, side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y + side, -side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y + side, side, 1.0f), color),

								// Face
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + -side, -side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + -side, -side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + -side, side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + -side, side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + -side, -side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + -side, side, 1.0f), color),

								// Face
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + side, -side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + side, side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + side, -side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + side, -side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + side, side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + side, side, 1.0f), color),

								// Face
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + -side, -side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + side, -side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + -side, -side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + -side, -side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + side, -side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + side, -side, 1.0f), color),

								// Face
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + -side, side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + -side, side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + side, side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + -side, centerPoint.Y + side, side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + -side, side, 1.0f), color),
								new Vertex(new Vector4(centerPoint.X + side, centerPoint.Y  + side, side, 1.0f), color)
							};

			return vertices;
		}
	}
}