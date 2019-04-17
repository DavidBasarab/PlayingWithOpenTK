using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using TestingGame.Common;

namespace TestingGame
{
	public class ShaderProgram : IDisposable
	{
		private readonly List<int> shaders = new List<int>();

		public int ProgramId { get; }

		public ShaderProgram() => ProgramId = GL.CreateProgram();

		public void AddShader(ShaderType shaderType, string path)
		{
			var shaderSource = File.ReadAllText(path);

			var shader = GL.CreateShader(shaderType);

			GL.ShaderSource(shader, shaderSource);

			GL.CompileShader(shader);

			var info = GL.GetShaderInfoLog(shader);

			if (!string.IsNullOrWhiteSpace(info)) Log.Debug($"GL.CompileShader <{shaderType}> had info log: {info}");

			shaders.Add(shader);
		}

		public void Dispose() => Dispose(true);

		public void Link()
		{
			foreach (var shader in shaders) GL.AttachShader(ProgramId, shader);

			GL.LinkProgram(ProgramId);

			var info = GL.GetProgramInfoLog(ProgramId);

			if (!string.IsNullOrWhiteSpace(info)) Log.Debug($"GL.LinkProgram had info log: {info}");

			foreach (var shader in shaders)
			{
				GL.DetachShader(ProgramId, shader);
				GL.DeleteShader(shader);
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing) GL.DeleteProgram(ProgramId);
		}
	}
}