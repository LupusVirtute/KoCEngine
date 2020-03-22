using KoC.GameEngine.Files;
using System.Threading;

namespace KoC.GameEngine.Loader
{
	public class GameLoader
	{
		public GameLoader(GameFile[] files)
		{
			for(int i =0; i < files.Length;i++)
			{
				Thread thrd = new Thread(() => StaticHolder.fileParser.ParseFile(files[i]));
				thrd.Start();
			}




		}

	}
}
