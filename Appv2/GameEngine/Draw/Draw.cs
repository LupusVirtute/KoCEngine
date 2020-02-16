using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace KoC.GameEngine.Draw
{
	public struct Texture2D
	{
		private readonly int _texture;
		private readonly TextureTarget textureTarget;

		public Texture2D(TextureTarget x, System.Drawing.Bitmap bitmap)
		{
			textureTarget = x;
			_texture = GL.GenTexture();
			GL.BindTexture(x, _texture);
			bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
			GL.TextureParameter(_texture,TextureParameterName.TextureMinFilter,(int)TextureMinFilter.Linear);
			GL.TextureParameter(_texture,TextureParameterName.TextureMagFilter,(int)TextureMagFilter.Linear);
			GL.TextureParameter(_texture, TextureParameterName.TextureWrapS, (int)All.Repeat);
			GL.TextureParameter(_texture, TextureParameterName.TextureWrapT, (int)All.Repeat);
			GL.TexImage2D(x, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			GL.TexSubImage2D(x,0,0,0,bitmapData.Width,bitmapData.Height,OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,PixelType.UnsignedByte,bitmapData.Scan0);
			bitmap.UnlockBits(bitmapData);
			bitmap.Dispose();
			GL.BindTexture(x,0);
		}
		public void Bind(TextureUnit texU)
		{
			GL.ActiveTexture(texU);
			GL.BindTexture(textureTarget,_texture);
		}

	}
	public struct Vertex
	{
		public const int Size = 12;

		private readonly Vector3 _position;
		public Vertex(Vector4 position)
		{
			_position = new Vector3(position);
		}
		public Vertex(Vector3 position)
		{
			_position = position;

		}
	}
	public struct D3Obj
	{
        private float[] rotationAx;
        private Vector3 origin;
        private Matrix4 objMatrix;
		private Texture2D tex;
        private Mesh mesh;
        private float scaler;

        public D3Obj(ref Mesh _Mesh, Vector3 worldPos,string textureFilePath)
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
			if (!string.IsNullOrEmpty(textureFilePath))
			{
				tex = new Texture2D(TextureTarget.Texture2D, Files.FileParser.LoadBitMap(textureFilePath));

			}
			else
			{
				tex = new Texture2D(TextureTarget.Texture2D,Files.FileParser.LoadBitMap("DefaultTexture\\NoTexture.bmp"));
			}
			ReloadMatrix();
        }
		/// <summary>
		/// Heavier performance Rendering Method use only when necessary and with different program shaders
		/// </summary>
		/// <param name="prog">program to use</param>
		public void RenderObjP(int prog)
        {
			GL.UseProgram(prog);
            GL.UniformMatrix4(22, false, ref objMatrix);
            mesh.Render();
        }
		public void RenderObj(int gSampLoc)
		{
			GL.UniformMatrix4(22,false,ref objMatrix);
			GL.Uniform1(gSampLoc, 0);
			tex.Bind(TextureUnit.Texture0);
			mesh.Render();
		}
		/// <summary>
		/// <strong>Deprecated*</strong><br/> Uses pre OpenGL 4.3 uniform pass<br/>
		/// Use only when OpenGL version is pre 4.3
		/// </summary>
		public void RenderObjOld(int prog)
		{
			int loc = GL.GetUniformLocation(prog, "modelView");
			GL.UniformMatrix4(loc, false, ref objMatrix);
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
			Matrix4.CreateRotationX(rotationAx[0]) *
			Matrix4.CreateRotationY(rotationAx[1]) *
			Matrix4.CreateRotationZ(rotationAx[2]) *
			Matrix4.CreateScale(scaler) *
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
				8 * n.Length,
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

			GL.VertexArrayAttribBinding(_vertexArray, 0, 0);
			GL.EnableVertexArrayAttrib(_vertexArray, 0);
			GL.VertexArrayAttribFormat(
				_vertexArray,
				0,
				3,
				VertexAttribType.Float,
				false,
				0);
			GL.VertexArrayAttribBinding(_vertexArray, 1,1);
			GL.EnableVertexArrayAttrib(_vertexArray, 1);
			GL.VertexArrayAttribFormat(
				_vertexArray,
				1,
				2,
				VertexAttribType.Float,
				false,
				0);
			ErrorCode b = GL.GetError();
			GL.VertexArrayAttribBinding(_vertexArray, 2,2);
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
			ErrorCode b = GL.GetError();
			GL.EnableVertexArrayAttrib(_vertexArray, 1);
			b = GL.GetError();
			GL.EnableVertexArrayAttrib(_vertexArray, 2);
			b = GL.GetError();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexingBuffer);
			b = GL.GetError();
			GL.DrawElements(PrimitiveType.Triangles, Fl, DrawElementsType.UnsignedInt,0);
			b = GL.GetError();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer,0);
			b = GL.GetError();
			GL.DisableVertexArrayAttrib(_vertexArray,0);
			b = GL.GetError();
			GL.DisableVertexArrayAttrib(_vertexArray,1);
			b = GL.GetError();
			GL.DisableVertexArrayAttrib(_vertexArray,2);
			b = GL.GetError();

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
					GL.DeleteBuffer(_vertexBuffer);
					GL.DeleteBuffer(indexingBuffer);
					_initialized = false;
				}
			}

		}
	}

}
