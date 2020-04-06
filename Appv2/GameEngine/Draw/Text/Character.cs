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
		public float[] texCoords;

		Font font;
		public Character(char charToDraw,Font font,float[] textureCoords)
		{
			texCoords = textureCoords;
			this.font = font;
			_c = charToDraw;
			//VAO and VBO's Init
			float[] charTable = new float[12]
			{
				-0.5f,  0.5f, // Top-left
				 0.5f,  0.5f, // Top-right
				 0.5f, -0.5f, // Bottom-right
				 0.5f, -0.5f, // Bottom-right
				-0.5f, -0.5f, // Bottom-left
				-0.5f,  0.5f  // Top-left
			};
			VAO = GL.GenVertexArray();
			VBO = GL.GenBuffer();
			VBOTexCoords = GL.GenBuffer();

			GL.BindVertexArray(VAO);
			GL.BindBuffer(BufferTarget.ArrayBuffer,VBO);
			GL.NamedBufferStorage(VBO,sizeof(float)*charTable.Length,charTable, BufferStorageFlags.MapWriteBit);



			GL.BindBuffer(BufferTarget.ArrayBuffer,VBOTexCoords);
			GL.NamedBufferStorage(VBOTexCoords,sizeof(float)*textureCoords.Length,textureCoords,BufferStorageFlags.MapWriteBit);


			GL.VertexArrayAttribFormat(
				VAO,
				0,
				2,
				VertexAttribType.Float,
				false,
				0);
			GL.VertexArrayAttribFormat(
				VAO,
				1,
				2,
				VertexAttribType.Float,
				false,
				0);

			GL.VertexArrayVertexBuffer(VAO, 0, VBO, IntPtr.Zero, 8);
			GL.VertexArrayVertexBuffer(VAO, 1, VBOTexCoords, IntPtr.Zero, 8);

			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
		}

		public void RenderCharacter()
		{
			GL.BindVertexArray(VAO);
			GL.EnableVertexArrayAttrib(VAO, 0);
			GL.EnableVertexArrayAttrib(VAO, 1);
			
			GL.DrawArrays(PrimitiveType.Triangles, 0,6);
			
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
