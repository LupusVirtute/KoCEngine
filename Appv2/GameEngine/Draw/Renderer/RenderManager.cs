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
				camera = value ?? throw new ArgumentNullException();
			}
		}


		private float renderDistance;
		private ICamera camera;
		private Matrix4 projection;
		private Matrix4 ortho;
		private bool _bortho = false;
		private ShaderProgram mainProgramShader;
		private ShaderProgram mainTextProgramShader;
		private readonly int projectionLocation = 20;
		private readonly int camLoc = 21;
		private readonly int gSampLoc;
		public Camera camx;
		public RenderManager()
		{
			Shader shc = new Shader("Shaders/TxtFragShader.frag",ShaderType.FragmentShader);
			Shader shc2 = new Shader("Shaders/TxtVerShader.vert",ShaderType.VertexShader);

			mainProgramShader = new ShaderProgram(new Shader[2] { shc,shc2},"TxtShader");
			
			FOV = 90.0f;
			renderDistance = 100.0f;

			ReloadProjections(MainC.game.Width / MainC.game.Height);
		}
		public RenderManager(List<Obj3D> Objects, ICamera camera, ShaderProgram mainProgram)
		{
			this.Objects = Objects;
			this.camera = camera;
			mainProgramShader = mainProgram;
			camx = (Camera)camera;
			renderDistance = 100.0f;
			FOV = 90.0f;
			if (GL.GetInteger(GetPName.MajorVersion) < 4 && GL.GetInteger(GetPName.MinorVersion) < 3)
			{
				if (GL.IsProgram(mainProgram.ProgramID))
				{
					projectionLocation = GL.GetUniformLocation(mainProgram.ProgramID, "projection");
					camLoc = GL.GetUniformLocation(mainProgram.ProgramID, "camera");
				}
			}
			if (GL.IsProgram(mainProgram.ProgramID))
				gSampLoc = GL.GetUniformLocation(mainProgram.ProgramID, "gSampler");
				

			Shader[] shaders = new Shader[2];
			shaders[0] = new Shader("Shaders/TxtVerShader.vert", ShaderType.VertexShader);
			shaders[1] = new Shader("Shaders/TxtFragShader.frag", ShaderType.FragmentShader);
			
			mainTextProgramShader = new ShaderProgram(shaders,"TextShader");


			ReloadProjections(MainC.game.Width / MainC.game.Height);
		}
		public void SetShaderProgram(ShaderProgram program)
		{
			if (GL.IsProgram(program.ProgramID))
				this.mainProgramShader = program;
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

			if (GL.IsProgram(mainProgramShader.ProgramID)) 
				GL.UseProgram(mainProgramShader.ProgramID);
#if GAME3D
			GL.UniformMatrix4(projectionLocation, false, ref projection);
#else
			GL.UniformMatrix4(projectionLocation, false, ref ortho);
#endif
			Matrix4 CamMatrix = camera.GetCameraMatrix;
			
			GL.UniformMatrix4(camLoc, false,ref CamMatrix);

			for (int i = 0; i < Objects.Count; i++)
			{
				Objects[i].RenderObj(gSampLoc);
			}
			//UI - TODO
			GL.UseProgram(mainTextProgramShader.ProgramID);

			GL.UniformMatrix4(projectionLocation, false, ref ortho);
			
			StaticHolder.textHandler.RenderText(
				GL.GetUniformLocation(mainTextProgramShader.ProgramID, "charOffset"),
				GL.GetUniformLocation(mainTextProgramShader.ProgramID, "gSampler")
			);

#if DEBUG
			StaticHolder.CheckGLError();
#endif
		}
		public void Delete()
		{
			GL.DeleteProgram(mainProgramShader.ProgramID);
			GL.DeleteProgram(mainTextProgramShader.ProgramID);
		}
	}
}
