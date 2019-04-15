using System;
using System.IO;
using System.Reflection;
using OpenTK.Graphics.OpenGL4;

namespace TestingGame
{
	public static class ModelShaderProgram
	{
		private static readonly Lazy<ShaderProgram> instance = new Lazy<ShaderProgram>(() => new ShaderProgram(Path.Combine(ExecutingDirectory, @"Shaders\ModelVertexShader.vert"), Path.Combine(ExecutingDirectory, @"Shaders\fragmentShader.frag")));

		private static ShaderProgram Instance => instance.Value;

		private static string ExecutingDirectory
		{
			get
			{
				var codeBase = Assembly.GetExecutingAssembly().CodeBase;

				var uri = new UriBuilder(codeBase);

				var path = Uri.UnescapeDataString(uri.Path);

				return Path.GetDirectoryName(path);
			}
		}

		public static void Dispose() => Instance.Dispose();

		public static void Initialize() => Instance.Initialize();

		public static void Use() => Instance.Use();
	}

	public class ShaderProgram : IDisposable
	{
		private readonly string fragmentShaderPath;
		private readonly string vertexShaderPath;

		private int programId = -77;

		public ShaderProgram(string vertexShaderPath, string fragmentShaderPath)
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

			Console.WriteLine($"Disposing of Program <{programId}>");

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

			Console.WriteLine($"Vertex Shader Path := {vertexShaderPath}");

			GL.ShaderSource(vertexShader, File.ReadAllText(vertexShaderPath));

			GL.CompileShader(vertexShader);

			var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);

			Console.WriteLine($"Fragment Shader Path := {fragmentShaderPath}");

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