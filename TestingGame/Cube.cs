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
		private readonly PointF centerPoint;
		private readonly RenderObject faces;
		private readonly RenderObject lines;
		private readonly float side;
		private Matrix4 modelView;

		public Cube(PointF centerPoint, float side, Color4 color)
		{
			this.centerPoint = centerPoint;
			this.side = side;
			lines = new RenderObject(CreateCubeLines(centerPoint, side));
			faces = new RenderObject(CreateSolidCube(centerPoint, side, color));

			modelView = Matrix4.Identity;
		}

		public void Dispose()
		{
			lines.Dispose();
			faces.Dispose();
		}

		public void Render(Matrix4 projectionMatrix, Matrix4 translation, Vector4 rotations)
		{
			// ModelShaderProgram.Use();
			ProjectionProgram.Use();

			// 20 is the location in the shader
			GL.UniformMatrix4(20, false, ref projectionMatrix);

			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

			faces.Bind();
			
			var rotationX = Matrix4.CreateRotationX(rotations.X);
			var rotationY = Matrix4.CreateRotationY(rotations.Y);
			var rotationZ = Matrix4.CreateRotationZ(rotations.Z);

			modelView = rotationX * rotationY * rotationZ * translation;

			GL.UniformMatrix4(21, false, ref modelView);

			faces.Render();

			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

			lines.Bind();

			lines.Render();
		}

		public void Update(double gameTime)
		{
			var k = (float)gameTime * 0.05f;

			var rotationX = Matrix4.CreateRotationX(k * 6.0f);

			var rotationY = Matrix4.CreateRotationY(k * 6.0f);

			var rotationZ = Matrix4.CreateRotationZ(k * 1.5f);

			modelView = Matrix4.CreateTranslation(centerPoint.X, centerPoint.Y, side / 2f);
			modelView *= rotationX;
			modelView *= rotationY;
			modelView *= rotationZ;
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