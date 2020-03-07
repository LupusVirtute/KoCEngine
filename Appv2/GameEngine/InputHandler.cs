using OpenTK;
using OpenTK.Input;
using System;

namespace KoC.GameEngine
{
	public class InputHandler
	{
		static float speed = .1f;
		static float deltaTime = 0f;
		public static void onKeyUp(object sender,KeyboardKeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.W:
				{
					deltaTime = 0f;
					break;
				}
				case Key.D:
				{
					deltaTime = 0f;
					break;
				}
				case Key.A:
				{
					deltaTime = 0f;
					break;
				}
				case Key.S:
				{
					deltaTime = 0f;
					break;
				}
				case Key.Space:
				{
					deltaTime = 0f;
					break;
				}
				case Key.LShift:
				{
					deltaTime = 0f;
					break;
				}
			}
		}
        public static void onKeyDown(object sender,KeyboardKeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Escape:
				{
					MainC.game.Exit();
					break;
				}
				case Key.W:
				{
					deltaTime += .1f;
					Vector3 vec3Move = new Vector3(0.0f,0.0f,-1.0f);
					StaticHolder.mainRender.GetCamera.CameraMove(vec3Move, speed * deltaTime);
					break;
				}
				case Key.D:
				{
					deltaTime += .1f;
					Vector3 vec3Move = new Vector3(1.0f, 0.0f,0.0f);
					StaticHolder.mainRender.GetCamera.CameraMove(vec3Move, speed * deltaTime);
					break;
				}
				case Key.A:
				{
					deltaTime += .1f;

					Vector3 vec3Move = new Vector3(-1.0f, 0.0f,0.0f);
					StaticHolder.mainRender.GetCamera.CameraMove(vec3Move, speed * deltaTime);
					break;
				}
				case Key.S:
				{
					deltaTime += .1f;
					Vector3 vec3Move = new Vector3(0.0f,0.0f,1.0f);
					StaticHolder.mainRender.GetCamera.CameraMove(vec3Move, speed * deltaTime);
					break;
				}
				case Key.Space:
				{
					deltaTime += .1f;
					Vector3 vec3Move = new Vector3(0.0f, 1.0f, 0.0f);
					StaticHolder.mainRender.GetCamera.CameraMove(vec3Move, speed * deltaTime);
					break;
				}
				case Key.LShift:
				{
					deltaTime += .1f;
					Vector3 vec3Move = new Vector3(0.0f, -1.0f, .0f);
					StaticHolder.mainRender.GetCamera.CameraMove(vec3Move, speed * deltaTime);
					break;
				}

			}
        }
	}
}
