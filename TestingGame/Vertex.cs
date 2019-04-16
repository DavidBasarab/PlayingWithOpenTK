using OpenTK;
using OpenTK.Graphics;

// ReSharper disable NotAccessedField.Local

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
}