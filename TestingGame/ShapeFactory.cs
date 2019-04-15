using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace TestingGame
{
	public class Cube : IDisposable
	{
		private readonly RenderObject faces;
		private readonly RenderObject lines;

		public Cube(float side, Color4 color)
		{
			lines = new RenderObject(ShapeFactory.CreateCubeLines(side));
			faces = new RenderObject(ShapeFactory.CreateSolidCube(side, color));
		}

		public void Dispose()
		{
			lines.Dispose();
			faces.Dispose();
		}

		public void Render()
		{
			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

			faces.Render();

			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

			lines.Render();
		}
	}

	public class ShapeFactory
	{
		public static List<Vertex> CreateCubeLines(float side)
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
								new Vertex(new Vector4(-side, -side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(-side, side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(side, -side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(side, -side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(-side, side, -side, 1.0f), Color4.Black),
								new Vertex(new Vector4(side, side, -side, 1.0f), Color4.Black),

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

		public static List<Vertex> CreateSolidCube(float side, Color4 color)
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