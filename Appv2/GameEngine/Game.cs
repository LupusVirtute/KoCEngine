#if (DEBUG)
#undef DEBUG
#define DEBUG
#else
#define DEBUG
#endif
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using OpenTK.Graphics;
using KoC.GameEngine.Files;
using KoC.GameEngine.Draw;
using System.Runtime;

namespace KoC.GameEngine
{
	public sealed class Game : GameWindow
	{
		private bool _bortho = true;
		private int _program;
		private Mesh[] m;
		Mesh meshr;
		private Matrix4 _modelView;
		private Matrix4 projection;
		private Matrix4 ortho;
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
		}

		public void SwitchOrtho(){
			_bortho = !_bortho;
		}
		public bool IsOrtho(){
			return _bortho;
		}

		//Overrides
		#region Overrides
		public override WindowState WindowState { get => base.WindowState; set => base.WindowState = value; }
		protected override void OnLoad(EventArgs e)
		{
			projection = Matrix4.CreatePerspectiveFieldOfView(QuickMaths.DegreeToRadian(120.0d),Width/Height,.1f,1000.0f);
			ortho = Matrix4.CreateOrthographicOffCenter(0f,Width,0f,Height,0.1f, 1000.0f);

			CursorVisible = true;
			int[] shaders = new int[2];
			shaders[0] = FileCompiler.CompileShader(ShaderType.VertexShader, @"C:\Users\Marcin\source\repos\Appv2\Appv2\Shaders\verShader.vert");
			shaders[1] = FileCompiler.CompileShader(ShaderType.FragmentShader, @"C:\Users\Marcin\source\repos\Appv2\Appv2\Shaders\fragShader.frag");

			_program = FileCompiler.CreateProgram(shaders);
			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
			GL.CullFace(CullFaceMode.Front);

			GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
			GL.Enable(EnableCap.DepthTest);

			Closed += OnClosed;
			m = FileParser.ParseFile("iron_anvil.obj");
			//m = FileParser.ParseFile("C:/Users/Marcin/Desktop/cube.obj");
			//m[0].Move(1.0f,2.0f,0.0f);
			ref Mesh meshr = ref m[0];
			this.meshr = m[0];
			GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
			GC.Collect();
		}
		private void OnClosed(object s, EventArgs e)
		{
			Exit();
		}
		public override void Exit()
		{
			GL.DeleteProgram(_program);
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
			projection = Matrix4.CreatePerspectiveFieldOfView(QuickMaths.DegreeToRadian(120.0d), asp, 1.0f, 100.0f);
			ortho = Matrix4.CreateOrthographicOffCenter(0f, Width, 0f, Height, 0.1f, 1000.0f);

		}
		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			InputHandler.HandleKeyboard();
		}
		double _time;
		bool switchM = false;
		protected override void OnRenderFrame(FrameEventArgs e)
		{
			_time += e.Time;
			if((int)_time % 360 == 0)
			{
				switchM = !switchM;
			}
			Title = $"(Vsync : {VSync}) (FPS : {1f / e.Time:0})";

			Color4 backColor = new Color4(0, 0, 80, 255);
			GL.ClearColor(backColor);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			Matrix4 tr = Matrix4.CreateTranslation(0.0f, 0.0f, -10f);
			_modelView = tr;

			//Switch ortho or projection
			
			GL.UseProgram(_program);
			if (!_bortho)
			{
				GL.UniformMatrix4(20, false, ref ortho);
			}
			else
			{
				GL.UniformMatrix4(20, false, ref projection);
			}

			GL.UniformMatrix4(21, false, ref _modelView);
			/*
			 * if (!switchM) meshr.Move(0.0001f, 0.0001f, -0.0001f);
			else meshr.Move(-0.0001f, -0.0001f, 0.0001f);
			m[0].Rotate(0.01, new sbyte[3] { 1, 1, 1 });
			*/
			m[0].Render();
			//UI - TODO

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
