using System;
using System.IO;
using System.Reflection;

namespace TestingGame
{
	public static class ProjectionProgram
	{
		private static readonly Lazy<ShaderProgram> instance = new Lazy<ShaderProgram>(() => new ShaderProgram(Path.Combine(ExecutingDirectory, @"Shaders\PipeVertexShader.vert"), Path.Combine(ExecutingDirectory, @"Shaders\fragmentShader.frag")));

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

		private static ShaderProgram Instance => instance.Value;

		public static void Dispose() => Instance.Dispose();

		public static void Initialize() => Instance.Initialize();

		public static void Use() => Instance.Use();
	}
}