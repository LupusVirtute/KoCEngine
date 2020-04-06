using KoC.GameEngine.Draw.Text;
using KoC.GameEngine.Files;
using KoC.GameEngine.Player;
using KoC.GameEngine.ShaderManager;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace KoC.GameEngine.Draw.Renderer
{
	public sealed class RenderManager : IRenderer
	{
		private List<Obj3D> Objects = new List<Obj3D>();
		private float _FOV;
		private EngineText2D text;
		public float FOV
		{
			get
			{
				return _FOV;
			}
			set
			{
				if (value > 179)
				{
					throw new ArgumentException("Invalid value for FOV");
				}
				else _FOV = value;
			}
		}
		public float RenderDistance
		{
			get
			{
				return renderDistance;
			}
			set
			{
				if (value <= 0.1f)
				{
					throw new ArgumentException("Invalid value for render distance");
				}
				else renderDistance = value;
			}
		}
		public ICamera GetCamera
		{
			get
			{
				return camera;
			}
			set
			{
				if(value == null)
				{
					throw new ArgumentNullException();
				}
				camera = value;
			}
		}


		private float renderDistance;
		private ICamera camera;
		private Matrix4 projection;
		private Matrix4 ortho;
		private bool _bortho = false;
		private int _program;
		private int _TxTProgram;
		private readonly int loc = 20;
		private readonly int camLoc = 21;
		private readonly int gSampLoc;
		public Camera camx;
		public RenderManager()
		{
			ShaderCompile shc = new ShaderCompile("Shaders/TxtFragShader.frag",ShaderType.FragmentShader);
			ShaderCompile shc2 = new ShaderCompile("Shaders/TxtVerShader.vert",ShaderType.VertexShader);
			Shader sh = new Shader(new ShaderCompile[2] { shc,shc2},"TxtShader");

			KoCFont font = new KoCFont(new System.Drawing.Font("Consolas", 6f));
			text = new EngineText2D(font,"YES FINALLY I CAN JERK OFF TO THIS",_TxTProgram,new Vector2(32f,32f));
			
			FOV = 90.0f;
			renderDistance = 100.0f;
			_program = 0;
			ReloadProjections(MainC.game.Width / MainC.game.Height);
		}
		public RenderManager(List<Obj3D> Objects, ICamera camera, int program = 0)
		{
			this.Objects = Objects;
			this.camera = camera;
			_program = program;
			camx = (Camera)camera;
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

				gSampLoc = GL.GetUniformLocation(_program, "gSampler");
			}
			int[] shaders = new int[2];
			shaders[0] = FileCompiler.CompileShader(ShaderType.VertexShader, "Shaders/TxtVerShader.vert");
			shaders[1] = FileCompiler.CompileShader(ShaderType.FragmentShader, "Shaders/TxtFragShader.frag");
			_TxTProgram = FileCompiler.CreateProgram(shaders);
			StaticHolder.CheckGLError();

			text = new EngineText2D(new KoCFont(new System.Drawing.Font("Consolas", 6f)), "YES FINALLY I CAN JERK OFF TO THIS", _TxTProgram, new Vector2(0f,0f));
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

			//Switch ortho or perspectiveFOV

			if (GL.IsProgram(_program)) GL.UseProgram(_program);
			if (!_bortho)
			{
				GL.UniformMatrix4(loc, false, ref ortho);
			}
			else
			{
				GL.UniformMatrix4(loc, false, ref projection);
			}

			Matrix4 CamMatrix = camera.GetCameraMatrix;
			GL.UniformMatrix4(21, false,ref CamMatrix);
			for (int i = 0; i < Objects.Count; i++)
			{
				Objects[i].RenderObj(gSampLoc);
			}
			StaticHolder.CheckGLError();

			//UI - TODO
			GL.UseProgram(_TxTProgram);
			GL.UniformMatrix4(loc, false, ref ortho);
			text.Render();
			StaticHolder.CheckGLError();

		}
		public void Delete()
		{
			GL.DeleteProgram(_program);
		}
	}
}
