using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;

namespace KoC.GameEngine.Draw
{

    public struct Obj3D
	{
        private float[] rotationAx;
        private Vector3 origin;
        private Matrix4 objMatrix;
		private Texture2D tex;
        private Mesh mesh;
        private float scaler;

        public Obj3D(ref Mesh _Mesh, Vector3 worldPos,string textureFilePath)
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
				tex = new Texture2D(TextureTarget.Texture2D, Files.FileParser.LoadBitMap(textureFilePath),Path.GetFileNameWithoutExtension(textureFilePath));
			}
			else
			{
				tex = StaticHolder.textureHandler.NoTexture;
			}
			ReloadMatrix();
        }
        public Obj3D(ref Mesh _Mesh,Vector3 worldPos,ref Texture2D texture)
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
            tex = texture;
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
		/// <br/> Uses pre OpenGL 4.3 uniform pass<br/>
		/// Use only when OpenGL version is pre 4.3
		/// </summary>
		[Obsolete("Use only for OpenGL Pre 4.3")]
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

}
