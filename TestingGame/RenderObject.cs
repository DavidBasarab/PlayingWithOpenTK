using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace TestingGame
{
	public class RenderObject : IDisposable
	{
		private readonly int buffer;

		private readonly int vertexArray;
		private readonly int verticeCount;
		private bool initialized;

		public RenderObject(List<Vertex> vertices)
		{
			verticeCount = vertices.Count;

			vertexArray = GL.GenVertexArray();
			buffer = GL.GenBuffer();

			GL.BindVertexArray(vertexArray);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexArray);

			// create first buffer: vertex
			GL.NamedBufferStorage(
								buffer,
								Vertex.Size * vertices.Count,    // the size needed by this buffer
								vertices.ToArray(),              // data to initialize with
								BufferStorageFlags.MapWriteBit); // at this point we will only write to the buffer

			GL.VertexArrayAttribBinding(vertexArray, 0, 0);
			GL.EnableVertexArrayAttrib(vertexArray, 0);

			GL.VertexArrayAttribFormat(
										vertexArray,
										0,                      // attribute index, from the shader location = 0
										4,                      // size of attribute, vec4
										VertexAttribType.Float, // contains floats
										false,                  // does not need to be normalized as it is already, floats ignore this flag anyway
										0);                     // relative offset, first item

			GL.VertexArrayAttribBinding(vertexArray, 1, 0);

			GL.EnableVertexArrayAttrib(vertexArray, 1);

			GL.VertexArrayAttribFormat(
										vertexArray,
										1,                      // attribute index, from the shader location = 1
										4,                      // size of attribute, vec4
										VertexAttribType.Float, // contains floats
										false,                  // does not need to be normalized as it is already, floats ignore this flag anyway
										16);                    // relative offset after a vec4

			// link the vertex array and buffer and provide the stride as size of Vertex
			GL.VertexArrayVertexBuffer(vertexArray, 0, buffer, IntPtr.Zero, Vertex.Size);

			initialized = true;
		}

		public void Bind() => GL.BindVertexArray(vertexArray);

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Render() => GL.DrawArrays(PrimitiveType.Triangles, 0, verticeCount);

		protected virtual void Dispose(bool disposing)
		{
			if (!disposing) return;

			if (!initialized) return;

			GL.DeleteVertexArray(vertexArray);
			GL.DeleteBuffer(buffer);

			initialized = false;
		}
	}
}