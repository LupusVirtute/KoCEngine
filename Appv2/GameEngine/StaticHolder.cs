using KoC.GameEngine.Draw;
using KoC.GameEngine.Draw.Renderer;
using KoC.GameEngine.Draw.Text;
using KoC.GameEngine.Files;
using KoC.GameEngine.Loader;
using OpenTK.Graphics.OpenGL4;
using System;
namespace KoC.GameEngine
{
    public static class StaticHolder
    {
        public static RenderManager mainRender;
        public static TextureHandler textureHandler;
		public static TextHandler textHandler;
		public static FileParser fileParser;
		public static GameLoader loader;


		public static bool isRenderFreezed = false;
        public static void CheckGLError()
        {
			#if (DEBUG)
				ErrorCode b = GL.GetError();
				if (!b.ToString().Equals("NoError"))
				{
					throw new Exception(b.ToString());
				}
			#else
				ErrorCode b = GL.GetError();
				
			#endif
		}
		public static float GetRatio(float Width,float Height)
		{
			float asp;
			if (Width == 0) asp = Height;
			else if (Height == 0) asp = Width;
			else if (Width > Height) asp = Width / Height;
			else asp = Height / Width;
			return asp;
		}
	}
}
