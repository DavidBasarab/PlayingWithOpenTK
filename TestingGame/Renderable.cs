using System;
using OpenTK.Graphics.OpenGL4;

namespace TestingGame
{
	public class TexturedRenderObject : Renderable
	{
		private int texture;

		public TexturedRenderObject(TexturedVertex[] vertices, int program, string filename) : base(program, vertices.Length)
		{
			// Create first buffer: vertex
			GL.NamedBufferStorage(Buffer,
								TexturedVertex.Size * vertices.Length, // Size needed by this buffer
								vertices,                              // Data to initialize with
								BufferStorageFlags.MapWriteBit         // At this point we only write to the buffer
								);

			GL.VertexArrayAttribBinding(VertexArray, 0, 0);
			GL.EnableVertexArrayAttrib(VertexArray, 0);

			GL.VertexArrayAttribFormat(VertexArray,
										0,                      // attribute index, from the shader location = 0
										4,                      // size of attribute, vec4
										VertexAttribType.Float, // contains floats
										false,                  // does not need to be normalized as it is already, floats ignore this flag anyway
										0
									);
		}
	}

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