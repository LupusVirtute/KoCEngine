using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;
namespace KoC.GameEngine.Draw.Text
{
	public struct Character : IDisposable
	{
		private int VAO;
		private int VBO;
		private int VBOTexCoords;
		private char _c;
		public char c {
			get{
				return _c;
			}
		}

		Font font;
		public Character(char charToDraw,Font font,float[] textureCoords)
		{
			this.font = font;
			_c = charToDraw;
			//VAO and VBO's Init
			float[] charTable = new float[12]
			{
				0.0f,0.0f,
				0.0f,1.0f,
				1.0f,0.0f,

				1.0f,1.0f,
				0.0f,1.0f,
				1.0f,0.0f
			};
			VAO = GL.GenVertexArray();
			VBO = GL.GenBuffer();
			VBOTexCoords = GL.GenBuffer();

			GL.BindVertexArray(VAO);
			GL.BindBuffer(BufferTarget.ArrayBuffer,VBO);
			GL.BufferData(BufferTarget.ArrayBuffer,48,charTable,BufferUsageHint.StaticRead);

			GL.EnableVertexArrayAttrib(VAO, 0);
			GL.VertexAttribPointer(0,2,VertexAttribPointerType.Float,false,0,charTable);
			GL.DisableVertexArrayAttrib(VAO, 0);

			GL.BindBuffer(BufferTarget.ArrayBuffer,VBOTexCoords);
			GL.BufferData(BufferTarget.ArrayBuffer,sizeof(float)*textureCoords.Length,textureCoords,BufferUsageHint.StaticRead);

			GL.EnableVertexArrayAttrib(VAO,1);
			GL.VertexAttribPointer(1,2,VertexAttribPointerType.Float,false,0,textureCoords);


			GL.DisableVertexArrayAttrib(VAO, 1);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
		}

		public void RenderCharacter()
		{
			GL.BindVertexArray(VAO);
			GL.BindBuffer(BufferTarget.ArrayBuffer,VBO);
			GL.EnableVertexArrayAttrib(VAO, 0);
			GL.EnableVertexArrayAttrib(VAO, 1);
			
			GL.DrawArrays(PrimitiveType.Triangles, 0,6);
			
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.DisableVertexArrayAttrib(VAO, 0);
			GL.DisableVertexArrayAttrib(VAO, 1);
			GL.BindVertexArray(0);
		}
		public void Dispose()
		{
			GL.DeleteBuffer(VBO);
			GL.DeleteVertexArray(VAO);
			GC.SuppressFinalize(this);
		}
	}
}
