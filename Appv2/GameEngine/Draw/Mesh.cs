using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;

namespace KoC.GameEngine.Draw
{
	/// <summary>
	/// 3D Model Class
	/// </summary>
	public class Mesh : IDisposable
	{
		private bool _initialized;
		private readonly int _vertexArray;
		private readonly int _vertexBuffer;
		private readonly int _uvBuffer;
		private readonly int _normalBuffer;
		private readonly int indexingBuffer;
		private readonly int _verticeCount;
		private int Fl;
		private readonly uint[] indicesArray;
		private readonly uint[] TexturePoints;
		private readonly uint[] NormalPoints;
		private Vector2[] TexVec;
		private Vector3[] Normals;
		Vector3[] VerMatrix;

		/// <summary>
		/// Constructs 3D Mesh Model
		/// </summary>
		/// <param name="x">Vertices</param>
		/// <param name="n">Normals</param>
		/// <param name="tVec">Texture Vectors</param>
		/// <param name="indices">Indices</param>
		/// <param name="tP">Texture Points</param>
		/// <param name="tN">Texture Normals</param>
		public Mesh(Vector3[] x,Vector3[] n,Vector2[] tVec,uint[] indices,uint[] tP,uint[] tN)
		{
			Normals = n;
			TexVec = tVec;
			VerMatrix = x;

			TexturePoints = tP;
			NormalPoints = tN;



			indicesArray = indices;
			_verticeCount = x.Length;

			_vertexArray = GL.GenVertexArray();
			_vertexBuffer = GL.GenBuffer();
			_uvBuffer = GL.GenBuffer();
			_normalBuffer = GL.GenBuffer();
			indexingBuffer = GL.GenBuffer();

			InitVBOAndVAO();
			

			_initialized = true;
		}
		private void InitVBOAndVAO()
		{
			GL.BindVertexArray(_vertexArray);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);

			GL.NamedBufferStorage(
				_vertexBuffer,
				12 * _verticeCount,
				VerMatrix,
				BufferStorageFlags.MapWriteBit
				);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _uvBuffer);

			GL.NamedBufferStorage(
				_uvBuffer,
				8 * Normals.Length,
				TexVec,
				BufferStorageFlags.MapWriteBit
				);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _normalBuffer);

			GL.NamedBufferStorage(
				_normalBuffer,
				12 * Normals.Length,
				Normals,
				BufferStorageFlags.MapWriteBit
				);
			//Vertex Binding Attrib
			GL.VertexArrayAttribBinding(_vertexArray, 0, 0);
			GL.EnableVertexArrayAttrib(_vertexArray, 0);

			GL.VertexArrayAttribFormat(
				_vertexArray,
				0,
				3,
				VertexAttribType.Float,
				false,
				0);
			//UVBuffer Binding Attrib
			GL.VertexArrayAttribBinding(_vertexArray, 1, 1);
			GL.EnableVertexArrayAttrib(_vertexArray, 1);

			GL.VertexArrayAttribFormat(
				_vertexArray,
				1,
				2,
				VertexAttribType.Float,
				false,
				0);
			//Normals Binding Attrib
			GL.VertexArrayAttribBinding(_vertexArray, 2, 2);
			GL.EnableVertexArrayAttrib(_vertexArray, 2);

			GL.VertexArrayAttribFormat(
				_vertexArray,
				1,
				3,
				VertexAttribType.Float,
				false,
				0);

			GL.VertexArrayVertexBuffer(_vertexArray, 0, _vertexBuffer, IntPtr.Zero, 12);
			GL.VertexArrayVertexBuffer(_vertexArray, 1, _uvBuffer, IntPtr.Zero, 8);
			GL.VertexArrayVertexBuffer(_vertexArray, 2, _normalBuffer, IntPtr.Zero, 12);
			Fl = indicesArray.Length;

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexingBuffer);
			GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * Fl, indicesArray, BufferUsageHint.StaticDraw);
		}
		/// <summary>
		/// Renders 3D Mesh
		/// </summary>
		public void Render()
		{
			GL.BindVertexArray(_vertexArray);
			GL.EnableVertexArrayAttrib(_vertexArray, 0);
			GL.EnableVertexArrayAttrib(_vertexArray, 1);
			//GL.EnableVertexArrayAttrib(_vertexArray, 2);
			StaticHolder.CheckGLError();

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexingBuffer);
			StaticHolder.CheckGLError();

			GL.DrawElements(PrimitiveType.Triangles, Fl, DrawElementsType.UnsignedInt, 0);
			StaticHolder.CheckGLError();

			GL.BindBuffer(BufferTarget.ElementArrayBuffer,0);
			StaticHolder.CheckGLError();

			GL.DisableVertexArrayAttrib(_vertexArray,0);
			GL.DisableVertexArrayAttrib(_vertexArray,1);
			//GL.DisableVertexArrayAttrib(_vertexArray,2);
			GL.BindVertexArray(0);
		}
		/// <summary>
		/// Disposes of the VBO and VAO and IBO
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_initialized)
				{
					GL.DeleteVertexArray(_vertexArray);
					GL.DeleteBuffer(_vertexBuffer);
					GL.DeleteBuffer(_uvBuffer);
					GL.DeleteBuffer(_normalBuffer);
					GL.DeleteBuffer(indexingBuffer);
					_initialized = false;
				}
			}

		}
	}

}
