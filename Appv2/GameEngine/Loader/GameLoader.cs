using KoC.GameEngine.Files;
using System.Threading;

namespace KoC.GameEngine.Loader
{
	public class GameLoader
	{
		public bool[] fileReady;
		public GameLoader(GameFile[] files)
		{
			fileReady = new bool[files.Length];
			for(int i =0; i < files.Length;i++)
			{
				Thread thrd = new Thread(() => StaticHolder.fileParser.ParseFile(files[i],i));
				thrd.Start();
			}
		}

	}
}
