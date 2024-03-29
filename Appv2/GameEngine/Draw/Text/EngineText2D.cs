﻿using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;

namespace KoC.GameEngine.Draw.Text
{
	public class EngineText2D
	{
		KoCFont font;
		string text;
		Matrix4[] matrixArray;
		Vector2 origin;
		public EngineText2D(KoCFont font,string text,Vector2 origin)
		{

			this.origin = origin;
			this.font = font;
			this.text = text;
			matrixArray	= font.CalculateMatrices(text, origin.X, origin.Y);
		}
		public void ChangeText(string text)
		{
			this.text = text;
			matrixArray = font.CalculateMatrices(text, origin.X, origin.Y);
			
		}
		public void ChangePos(Vector2 newOrigin)
		{
			origin = newOrigin;
			matrixArray = font.CalculateMatrices(text, origin.X, origin.Y);
		}
		public void Render(int charOffSetLoc)
		{
			font.BindTexture();

			for(int i = 0; i < matrixArray.Length; i++)
			{
				GL.UniformMatrix4(charOffSetLoc,false,ref matrixArray[i]);
				font[Convert.ToChar(text[i])].RenderCharacter();
			}
		}

	}
}
