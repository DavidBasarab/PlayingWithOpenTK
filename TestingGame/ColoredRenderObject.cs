using System;
using OpenTK.Graphics.OpenGL4;

namespace TestingGame
{
	public class ColoredRenderObject : Renderable
	{
		public ColoredRenderObject(ColoredVertex[] vertices, int program)
			: base(program, vertices.Length)
		{
			GL.NamedBufferStorage(Buffer,
								ColoredVertex.Size * vertices.Length, // Size needed by buffer
								vertices,                             // data to initialize with
								BufferStorageFlags.MapWriteBit);      // at this point we only want to write to the buffer

			GL.VertexArrayAttribBinding(VertexArray, 0, 0);
			GL.EnableVertexArrayAttrib(VertexArray, 0);

			GL.VertexArrayAttribFormat(VertexArray,
										0,                      // attribute index, from the shader location = 0 
										4,                      // size of attribute, vec4
										VertexAttribType.Float, // contains floats
										false,                  // does not need to be normalized as it is already, floats ignore this anyway
										0);                     // relative offset, first item

			GL.VertexArrayAttribBinding(VertexArray, 1, 0);
			GL.EnableVertexArrayAttrib(VertexArray, 1);

			GL.VertexArrayAttribFormat(VertexArray,
										1,                      // attribute index, from the shader location = 1
										4,                      // size of attribute, vec4
										VertexAttribType.Float, // contains float
										false,                  // does not need to be normalized as it is already, floats ignore this flag anyway
										16);                    // relative offset after vec4

			// Link the vertex array and buffer and provide the stride as size of Vertex
			GL.VertexArrayVertexBuffer(VertexArray, 0, Buffer, IntPtr.Zero, ColoredVertex.Size);
		}
	}
}