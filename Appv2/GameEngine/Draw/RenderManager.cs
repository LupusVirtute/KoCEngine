using KoC.GameEngine.Player;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace KoC.GameEngine.Draw
{
	public sealed class RenderManager
	{
		private List<D3Obj> Objects = new List<D3Obj>();
		private float _FOV;
		public float FOV {
			get
			{
				return _FOV;
			}
			set
			{
				if(value > 179)
				{
					Console.WriteLine("FOV can't be Higher than 179");
				}
				else _FOV = value;
			}
		}
		public float renderDistance;
		private ICamera camera;
		private Matrix4 projection;
		private Matrix4 ortho;
		private bool _bortho = false;
		private int _program;
		private readonly int loc = 20;
		private readonly int camLoc = 21;
		private readonly int gSampLoc;
		public RenderManager()
		{
			FOV = 90.0f;
			renderDistance = 100.0f;
			_program = 0;
			ReloadProjections(MainC.game.Width / MainC.game.Height);
		}
		public RenderManager(List<D3Obj> Objects,ICamera camera,int program = 0)
		{
			this.Objects = Objects;
			this.camera = camera;
			_program = program;
			renderDistance = 100.0f;
			FOV = 90.0f;
			if (GL.GetInteger(GetPName.MajorVersion) < 4 && GL.GetInteger(GetPName.MinorVersion) < 3)
			{
				if (GL.IsProgram(_program))
				{
					loc = GL.GetUniformLocation(_program, "projection");
					camLoc = GL.GetUniformLocation(_program, "camera");
				}
			}
			if (GL.IsProgram(_program))
			{

				gSampLoc = GL.GetUniformLocation(_program,"gSampler");
			}
			ReloadProjections(MainC.game.Width / MainC.game.Height);
		}
		public void SetProgram(int program)
		{
			if (GL.IsProgram(program))
				_program = program;
			else throw new Exception("Incorrect Program Shader");
		}
		public void ReloadProjections(float Width, float Height)
		{
			projection = Matrix4.CreatePerspectiveFieldOfView(QuickMaths.DegreeToRadian(_FOV), Width / Height, .1f, renderDistance);
			ortho = Matrix4.CreateOrthographic(10f, 10f, 100f, -100f);
		}
		public void ReloadProjections(float asp)
		{
			projection = Matrix4.CreatePerspectiveFieldOfView(QuickMaths.DegreeToRadian(_FOV), asp, .1f, renderDistance);
			ortho = Matrix4.CreateOrthographic(10f, 10f, 100f, -100f);
		}
		public void SwitchOrtho()
		{
			_bortho = !_bortho;
		}
		public bool IsOrtho()
		{
			return _bortho;
		}
		public void RenderCall()
		{
			Color4 backColor = new Color4(0, 0, 80, 255);
			GL.ClearColor(backColor);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			//Switch ortho or perspectiveFOV

			if (GL.IsProgram(_program)) GL.UseProgram(_program);
			if (_bortho)
			{
				GL.UniformMatrix4(loc, false, ref ortho);
			}
			else
			{
				GL.UniformMatrix4(loc, false, ref projection);
			}

			GL.UniformMatrix4(camLoc, false, ref camera.GetCameraMatrix());
			for (int i = 0; i < Objects.Count; i++)
			{
				Objects[i].RenderObj(gSampLoc);
			}
			//UI - TODO
			GL.UniformMatrix4(loc, false, ref ortho);

		}
		public void Delete()
		{
			GL.DeleteProgram(_program);
		}
	}
}
