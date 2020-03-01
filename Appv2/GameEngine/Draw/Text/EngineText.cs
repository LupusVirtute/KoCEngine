using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace KoC.GameEngine.Draw.Text
{
	public class EngineText
	{
		KoCFont font;
		string text;
		Matrix4[] matrixArray;
		int program;
		float[] origin;
		public EngineText(KoCFont font,string text,int program,float[] origin)
		{
			if(origin.Length != 2) throw new System.ArgumentException("Invalid length of origin", "origin");

			this.origin = origin;
			this.font = font;
			this.text = text;
			this.program = program;
			matrixArray	= font.CalculateMatrices(text,origin[0],origin[1]);
		}
		public void ChangeText(string text)
		{
			this.text = text;
			matrixArray = font.CalculateMatrices(text, origin[0], origin[1]);
			
		}
		public void ChangePos(float[] newOrigin)
		{
			if (newOrigin.Length != 2) throw new System.ArgumentException("Invalid length of origin","newOrigin");
			this.origin = newOrigin;
			matrixArray = font.CalculateMatrices(text, origin[0], origin[1]);
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
