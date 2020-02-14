using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Drawing.Imaging;

namespace KoC.GameEngine.Draw
{	
	public struct Texture
	{
		private readonly int _texture;

		public Texture(TextureTarget x, BitmapData y)
		{
			_texture = GL.GenTexture();
			GL.BindTexture(x, _texture);
			GL.TexImage2D(x, 0, PixelInternalFormat.Rgb, y.Width, y.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Rgb, PixelType.Byte, y.Scan0);
		}

	}
	public struct Vertex
	{
		public const int Size = 28;

		private readonly Vector3 _position;
		private readonly Color4 _color4;
		public Vertex(Vector4 position, Color4 color)
		{
			_position = new Vector3(position);
			_color4 = color;
		}
		public Vertex(Vector3 position, Color4 color)
		{
			_position = position;
			_color4 = color;

		}
	}
	public struct D3Obj
	{
        private float[] rotationAx;
        private Vector3 origin;
        private Matrix4 objMatrix;
        private Mesh mesh;
        private float scaler;

        public D3Obj(ref Mesh _Mesh, Vector3 worldPos)
        {
            mesh = _Mesh;
            origin = worldPos;
            objMatrix = Matrix4.Identity;
            origin = new Vector3();
            scaler = 1.0f;
            origin.X = 0.0f;
            origin.Y = 0.0f;
            origin.Z = -5.0f;
            rotationAx = new float[3];

        }
        public void RenderObj()
        {
            GL.UniformMatrix4(21, false, ref objMatrix);
            mesh.Render();
        }

        /// <summary>
        /// Multiplies the Rotation Matrix of this 3D Mesh.
        /// Rotation Array checks for 3 first indexs and
        /// checks their sign(+/-) and decides if angle should be added to rotationAxis or not.
        /// if rot == 0 then rotationAxis stays as it was
        /// </summary>
        /// <param name="angle">The angle to rotate</param>
        /// <param name="rot">Rotation Array </param>
        public void Rotate(double angle, sbyte[] rot)
        {
            if (rot[0] > 0)
            {
                rotationAx[0] = (float)Math.Abs((rotationAx[0] + angle) % 360);
            }
            else if (rot[0] < 0)
            {
                rotationAx[0] = (float)Math.Abs((rotationAx[0] - angle) % 360);
            }
            if (rot[1] > 0)
            {
                rotationAx[1] = (float)Math.Abs((rotationAx[1] + angle) % 360);
            }
            else if (rot[1] < 0)
            {
                rotationAx[1] = (float)Math.Abs((rotationAx[1] - angle) % 360);
            }
            if (rot[2] > 0)
            {
                rotationAx[2] = (float)Math.Abs((rotationAx[2] + angle) % 360);
            }
            else if (rot[2] < 0)
            {
                rotationAx[2] = (float)Math.Abs((rotationAx[2] - angle) % 360);
            }
            ReloadMatrix();
        }
        public void Move(float x, float y, float z)
        {
            origin.X += x;
            origin.Y += y;
            origin.Z += z;
            ReloadMatrix();
        }
        public void Scale(float sc)
        {
            scaler = sc;
            ReloadMatrix();
        }
        private void ReloadMatrix()
        {
            objMatrix =
            Matrix4.CreateScale(scaler) *
            Matrix4.CreateRotationX(rotationAx[0]) *
            Matrix4.CreateRotationY(rotationAx[1]) *
            Matrix4.CreateRotationZ(rotationAx[2]) *
            Matrix4.CreateTranslation(origin);
        }
    }
	/// <summary>
	/// 3D Model Class
	/// </summary>
	public class Mesh : IDisposable
	{
		private bool _initialized;
		private readonly int _vertexArray;
		private readonly int _buffer;
		private readonly int indexingBuffer;
		private readonly int _verticeCount;
		private int Fl;
		private readonly uint[] indicesArray;
		private readonly uint[] TexturePoints;
		private readonly uint[] NormalPoints;
		private Vector2[] TexVec;
		private Vector3[] Normals;
		Vertex[] VerMatrix;
		
		
		
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
			TexturePoints = tP;
			NormalPoints = tN;

			float[,] colorve = new float[,]
			{
				{1.0f,0.0f,0.0f },
				{0.0f,0.0f,1.0f },
				{0.0f,1.0f,0.0f },
				{0.0f,1.0f,1.0f },
				{1.0f,1.0f,0.0f }
			};

			VerMatrix = new Vertex[x.Length];
			for(int i = 0,l= x.Length; i < l; i++)
			{
				VerMatrix[i] = new Vertex(x[i],new Color4(colorve[i%5,0], colorve[i % 5, 1], colorve[i % 5, 2],1.0f));
			}
			indicesArray = indices;
			_verticeCount = x.Length;

			_vertexArray = GL.GenVertexArray();
			_buffer = GL.GenBuffer();

			
			GL.BindVertexArray(_vertexArray);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _buffer);

			GL.NamedBufferStorage(
				_buffer,
				Vertex.Size * _verticeCount,
				VerMatrix,
				BufferStorageFlags.MapWriteBit
				);

			GL.VertexArrayAttribBinding(_vertexArray, 0, 0);
			GL.EnableVertexArrayAttrib(_vertexArray, 0);
			GL.VertexArrayAttribFormat(
				_vertexArray,
				0,
				3,
				VertexAttribType.Float,
				false,
				0);
			GL.VertexArrayAttribBinding(_vertexArray, 1,0);
			GL.EnableVertexArrayAttrib(_vertexArray, 1);
			GL.VertexArrayAttribFormat(
				_vertexArray,
				1,
				4,
				VertexAttribType.Float,
				false,
				12);
			GL.VertexArrayVertexBuffer(_vertexArray, 0, _buffer, IntPtr.Zero, 28);
			Fl = indicesArray.Length;

			indexingBuffer = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexingBuffer);
			GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * Fl, indicesArray, BufferUsageHint.StaticDraw);


			_initialized = true;
		}
		/// <summary>
		/// Renders 3D Mesh
		/// </summary>
		public void Render()
		{
			GL.EnableVertexArrayAttrib(_vertexArray, 0);
			GL.EnableVertexArrayAttrib(_vertexArray, 1);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexingBuffer);
			GL.DrawElements(PrimitiveType.Triangles, Fl, DrawElementsType.UnsignedInt,0);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer,0);
			GL.DisableVertexArrayAttrib(_vertexArray,0);
			GL.DisableVertexArrayAttrib(_vertexArray,1);
		}
		~Mesh()
		{
			Dispose();
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
					GL.DeleteBuffer(_buffer);
					GL.DeleteBuffer(indexingBuffer);
					_initialized = false;
				}
			}

		}
	}

}
