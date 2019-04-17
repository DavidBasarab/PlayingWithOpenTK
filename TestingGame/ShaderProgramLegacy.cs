using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using TestingGame.Common;

namespace TestingGame
{
	public class ShaderProgramLegacy : IDisposable
	{
		private readonly string fragmentShaderPath;
		private readonly string vertexShaderPath;

		private int programId = -77;

		public ShaderProgramLegacy(string vertexShaderPath, string fragmentShaderPath)
		{
			this.vertexShaderPath = vertexShaderPath;
			this.fragmentShaderPath = fragmentShaderPath;

			VerifyFileExists(vertexShaderPath, "vert");
			VerifyFileExists(fragmentShaderPath, "frag");

			LoadShader();
		}

		public void Dispose()
		{
			if (programId == -77) return;

			Log.Info($"Disposing of Program <{programId}>");

			GL.DeleteProgram(programId);
		}

		public void Initialize()
		{
			// Do nothing
		}

		public void Use() => GL.UseProgram(programId);

		private void LoadShader()
		{
			var vertexShader = GL.CreateShader(ShaderType.VertexShader);

			Log.Debug($"Vertex Shader Path := {vertexShaderPath}");
			
			GL.ShaderSource(vertexShader, File.ReadAllText(vertexShaderPath));

			GL.CompileShader(vertexShader);

			var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			
			Log.Debug($"Fragment Shader Path := {fragmentShaderPath}");

			GL.ShaderSource(fragmentShader, File.ReadAllText(fragmentShaderPath));
			GL.CompileShader(fragmentShader);

			programId = GL.CreateProgram();

			GL.AttachShader(programId, vertexShader);
			GL.AttachShader(programId, fragmentShader);

			GL.LinkProgram(programId);

			GL.DetachShader(programId, vertexShader);
			GL.DetachShader(programId, fragmentShader);
			GL.DeleteShader(vertexShader);
			GL.DeleteShader(fragmentShader);
		}

		private static void VerifyFileExists(string path, string extension)
		{
			if (!File.Exists(path)) throw new FileNotFoundException(path);

			var fileExtension = Path.GetExtension(path);

			if (fileExtension.ToLower() == extension.ToLower()) throw new InvalidOperationException($"Extension should be <{extension}>");
		}
	}
}