using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;
using TestingGame.Common;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

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
										0                       // relative offset, first item that is why this is 0
									);

			GL.VertexArrayAttribBinding(VertexArray, 1, 0);
			GL.EnableVertexArrayAttrib(VertexArray, 1);

			GL.VertexArrayAttribFormat(VertexArray,
										1,                      // attribute index, from shader location 1
										2,                      // size of attribute, vec2
										VertexAttribType.Float, // contains float
										false,                  // does not need to be normalized as it is already, floats ignore this flag anyway
										16);                    // relative offset after vec4

			// link the vertex array and buffer and provide the stride as size of Vertex
			GL.VertexArrayVertexBuffer(VertexArray, 0, Buffer, IntPtr.Zero, TexturedVertex.Size);

			texture = InitTextures(filename);
		}

		private int InitTextures(string filename)
		{
			var data = LoadTexture(filename, out var width, out var height);

			GL.CreateTextures(TextureTarget.Texture2D, 1, out int textureId);

			GL.TextureStorage2D(textureId,
								1,                         // Levels of mipmapping
								SizedInternalFormat.Rg32f, // format of texture
								width,
								height
								);

			GL.BindTexture(TextureTarget.Texture2D, textureId);

			GL.TextureSubImage2D(textureId,
								0, // this is level 0
								0, // x offset
								0, // y offset
								width,
								height,
								PixelFormat.Rgba,
								PixelType.Float,
								data);

			// Data not needed from here on, OpenGL has the data
			return textureId;
		}

		private float[] LoadTexture(string filename, out int width, out int height)
		{
			float[] r;

			using (var bmp = (Bitmap)Image.FromFile(filename))
			{
				width = bmp.Width;
				height = bmp.Height;

				r = new float[width * height * 4];

				var index = 0;

				BitmapData data = null;

				try
				{
					data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

					unsafe
					{
						var ptr = (byte*)data.Scan0;

						var remain = data.Stride - data.Width * 3;

						for (var i = 0; i < data.Height; i++)
						{
							for (var j = 0; j < data.Width; j++)
							{
								r[index++] = ptr[2] / 255f;
								r[index++] = ptr[1] / 255f;
								r[index++] = ptr[0] / 255f;
								r[index++] = 1f;

								ptr += 3;
							}

							ptr += remain;
						}
					}
				}
				catch (Exception ex) { Log.Exception(ex); }
				finally
				{
					if (data != null) bmp.UnlockBits(data);
				}
			}

			return r;
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