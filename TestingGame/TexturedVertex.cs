using OpenTK;

namespace TestingGame
{
	public struct TexturedVertex
	{
		private readonly Vector4 position;
		private readonly Vector2 textureCoordinate;

		public const int Size = (4 + 2) * 4; // Size of Struct in Bytes

		public TexturedVertex(Vector4 position, Vector2 textureCoordinate)
		{
			this.position = position;
			this.textureCoordinate = textureCoordinate;
		}
	}
}