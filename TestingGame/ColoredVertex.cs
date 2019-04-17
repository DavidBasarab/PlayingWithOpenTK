using OpenTK;
using OpenTK.Graphics;

namespace TestingGame
{
	public struct ColoredVertex
	{
		public const int Size = (4 + 4) * 4; // size of struct in bytes

		private readonly Vector4 position;
		private readonly Color4 color;

		public ColoredVertex(Vector4 position, Color4 color)
		{
			this.position = position;
			this.color = color;
		}
	}
}