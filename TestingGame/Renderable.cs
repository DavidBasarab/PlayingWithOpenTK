using System;
using OpenTK.Graphics.OpenGL4;

namespace TestingGame
{
	public abstract class Renderable : IDisposable
	{
		public int Buffer { get; }

		public int Program { get; }

		public int VertexArray { get; }

		public int VerticeCount { get; }

		protected Renderable(int program, int vertexCount)
		{
			Program = program;
			VerticeCount = vertexCount;

			VertexArray = GL.GenVertexArray();
			Buffer = GL.GenBuffer();

			GL.BindVertexArray(VertexArray);
			GL.BindBuffer(BufferTarget.ArrayBuffer, Buffer);
		}

		public void Bind()
		{
			GL.UseProgram(Program);
			GL.BindVertexArray(VertexArray);
		}

		public void Dispose() => Dispose(true);

		protected virtual void Dispose(bool disposing)
		{
			if (!disposing) return;

			GL.DeleteVertexArray(VertexArray);
			GL.DeleteBuffer(Buffer);
		}
	}
}