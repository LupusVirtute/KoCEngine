using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;

namespace KoC.GameEngine.Draw.Text
{
	public class EngineText2D
	{
		KoCFont font;
		string text;
		Matrix4[] matrixArray;
		int program;
		Vector2 origin;
		public EngineText2D(KoCFont font,string text,int program,Vector2 origin)
		{

			this.origin = origin;
			this.font = font;
			this.text = text;
			this.program = program;
			matrixArray	= font.CalculateMatrices(text, origin.X, origin.Y);
		}
		public void ChangeText(string text)
		{
			this.text = text;
			matrixArray = font.CalculateMatrices(text, origin.X, origin.Y);
			
		}
		public void ChangePos(Vector2 newOrigin)
		{
			if (newOrigin.Length != 2) throw new System.ArgumentException("Invalid length of origin","newOrigin");
			origin = newOrigin;
			matrixArray = font.CalculateMatrices(text, origin.X, origin.Y);
		}
		public void Render()
		{
			int loc = GL.GetUniformLocation(program,"charOffset");
			for(int i = 0; i < matrixArray.Length; i++)
			{
				GL.UniformMatrix4(loc,false,ref matrixArray[i]);
				font[System.Convert.ToChar(text[i])].RenderCharacter();
			}
		}

	}
}
