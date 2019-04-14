using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;

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
}