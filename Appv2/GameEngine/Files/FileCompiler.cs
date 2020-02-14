using OpenTK.Graphics.OpenGL4;
using System.Diagnostics;
using System.IO;

namespace KoC.GameEngine.Files
{
	class FileCompiler
	{
		/// <summary>
		/// Creates and Compiles shader and returns it's int
		/// </summary>
		/// <param name="type">Type of shader</param>
		/// <param name="shaderPath">Shader Path</param>
		/// <returns>int shader</returns>
		public static int CompileShader(ShaderType type,string shaderPath)
		{
			int shader = GL.CreateShader(type);
			GL.ShaderSource(shader, File.ReadAllText(shaderPath));
			GL.CompileShader(shader);

			string info = GL.GetShaderInfoLog(shader);
			if (!string.IsNullOrWhiteSpace(info))
			{
				throw new System.Exception($"Error while compiling shader:\n  ShaderType:\n {type} Info: {info}");
			}
			return shader;
		}
		/// <summary>
		/// Creates the program with the following shaders
		/// </summary>
		/// <param name="shaders">The shaders you want to attach to program</param>
		/// <returns>int program if error then return = -1</returns>
		public static int CreateProgram(int[] shaders)
		{
			int program = GL.CreateProgram();
			int l = shaders.Length;
			for (int i=0;i < l; i++)
			{
				GL.AttachShader(program,shaders[i]);
			}
			GL.LinkProgram(program);
			for(int i = 0; i < l; i++)
			{
				GL.DetachShader(program,shaders[i]);
				GL.DeleteShader(shaders[i]);
			}
			string info = GL.GetProgramInfoLog(program);
			if (!string.IsNullOrWhiteSpace(info))
			{
				GL.DeleteProgram(program);
				program = -1;
				throw new System.Exception($"Program couldn't get compiled because of error: {info}");
			}
			return program;
		}
	}
}
