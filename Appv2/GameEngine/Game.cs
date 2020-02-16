using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using OpenTK.Graphics;
using KoC.GameEngine.Files;
using KoC.GameEngine.Draw;
using System.Runtime;
using System.Collections.Generic;
using System.Threading;

namespace KoC.GameEngine
{
	public sealed class Game : GameWindow
	{
		private bool powerLimiter;
		private Mesh[] m;
		public RenderManager mainRender;
		//Constructor
		public Game() : base(
				500,
				500,
				GraphicsMode.Default,
				"The Game Project",
				GameWindowFlags.Default,
				DisplayDevice.Default,
				4,
				5,
				GraphicsContextFlags.ForwardCompatible
			)
		{
			Title += ": OpenGL Version: " + GL.GetString(StringName.Version);
			powerLimiter = true;
		}
		/// <summary>
		/// Switches Power Saving Mode if on can cause stutter.<br/>
		/// But if off and user have bad drivers it can create performance issues
		/// 
		/// </summary>
		public void SwitchPowerLimiter()
		{
			powerLimiter = !powerLimiter;
		}
		public bool IsPowerSaved()
		{ 
			return powerLimiter; 
		}
		public void SwitchVSync(VSyncMode vsync)
		{
			VSync = vsync;
		}

		//Overrides
		#region Overrides
		protected override void OnLoad(EventArgs e)
		{
			Thread.Sleep(1);
			CursorVisible = true;
			int[] shaders = new int[2];
			shaders[0] = FileCompiler.CompileShader(ShaderType.VertexShader, @"Shaders\verShader.vert");
			shaders[1] = FileCompiler.CompileShader(ShaderType.FragmentShader, @"Shaders\fragShader.frag");

			int _program = FileCompiler.CreateProgram(shaders);
			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
			//GL.FrontFace(FrontFaceDirection.Cw);
			//GL.CullFace(CullFaceMode.Front);

			GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Less);

			Closed += OnClosed;
			m = FileParser.ParseFile("iron_anvil.obj");
			//m = FileParser.ParseFile("C:/Users/Marcin/Desktop/cube.obj");
			//m[0].Move(1.0f,2.0f,0.0f);
			GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
			GC.Collect();
			List<D3Obj> d3objli = new List<D3Obj>();
			for(int i = 0; i < m.Length; i++)
			{
				d3objli.Add(
					new D3Obj(
						ref m[i],
						new Vector3(0f,0f,-10f),
						string.Empty
						)
					);
			}
			Vector3 CameraPos		= new Vector3(1.0f, 1.0f, -10.0f);
			Vector3 CameraTarget	= new Vector3(0.45f, 0.0f, 1.0f);
			Vector3 CameraUp		= new Vector3(0.0f, 1.0f, 0.0f);
			mainRender = new RenderManager(d3objli, new Player.Camera(CameraPos, CameraTarget, CameraUp),_program);
			mainRender.ReloadProjections(Width,Height);
			base.OnLoad(e);
		}
		private void OnClosed(object s, EventArgs e)
		{
			Exit();
		}
		public override void Exit()
		{
			mainRender.Delete();
			for (int i = 0, l = m.Length; i < l; i++)
			{
				m[i].Dispose();

			}
			base.Exit();
		}
		protected override void OnResize(EventArgs e)
		{
			float asp;
			GL.Viewport(this.ClientRectangle);
			if (Width == 0) asp = Height;
			else if (Height == 0) asp = Width;
			else if (Width > Height) asp = Width / Height;
			else asp = Height / Width;
			mainRender.ReloadProjections(asp);
			base.OnResize(e);
		}
		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			InputHandler.HandleKeyboard();
			base.OnUpdateFrame(e);
		}
		protected override void OnRenderFrame(FrameEventArgs e)
		{
			if(powerLimiter) Thread.Sleep(15);

			Title = $"(Vsync : {VSync}) (FPS : {1f / e.Time:0})";

			mainRender.RenderCall();

			SwapBuffers();
			
			//Error Catch
			#if (DEBUG)
				ErrorCode b = GL.GetError();
				if (!b.ToString().Equals("NoError"))
				{
					throw new Exception(b.ToString());
				}
			#endif
		}
		#endregion Overrides
	}
}
