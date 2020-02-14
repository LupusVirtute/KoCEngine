using OpenTK.Input;

namespace KoC.GameEngine
{
	public class InputHandler
	{

        public static void HandleKeyboard()
		{
			KeyboardState keystate = Keyboard.GetState();
			if (keystate.IsKeyDown(Key.Escape))
			{
				MainC.game.Exit();
			}


        }
	}
}
