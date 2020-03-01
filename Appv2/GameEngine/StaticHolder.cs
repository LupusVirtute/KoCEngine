using KoC.GameEngine.Draw;
using KoC.GameEngine.Draw.Renderer;
using OpenTK.Graphics.OpenGL4;
using System;
namespace KoC.GameEngine
{
    public static class StaticHolder
    {
        public static RenderManager mainRender;
        public static TextureHandler textureHandler;
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
	}
}
