using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;

namespace TestingGame
{
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