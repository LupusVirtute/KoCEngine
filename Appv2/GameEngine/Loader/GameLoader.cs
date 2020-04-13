using KoC.GameEngine.Files;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace KoC.GameEngine.Loader
{
	public class GameLoader
	{
		public bool[] fileReady;
		public bool AreAllFilesReady()
		{
			for(int i =0; i < fileReady.Length; i++)
			{
				if (!fileReady[i])
					return false;
			}
			return true;
		}
		public GameFile[] LoadDefaults()
		{
			string[][] shaderFilesLocations = new string[LoaderSettings.ShaderLocation.Length][];
			string[][] meshFilesLocations = new string[LoaderSettings.MeshLocations.Length][];
			
			for(int i = 0; i < LoaderSettings.ShaderLocation.Length;i++)
				shaderFilesLocations[i] = Directory.GetFiles(LoaderSettings.ShaderLocation[i]);
			for(int i = 0; i < LoaderSettings.MeshLocations.Length;i++)
				meshFilesLocations[i] = Directory.GetFiles(LoaderSettings.MeshLocations[i]);

			List<GameFile> gameFiles = new List<GameFile>(); 
			for(int i =0; i < shaderFilesLocations.GetLength(0);i++)
				for(int j = 0; i < shaderFilesLocations.GetLength(1);i++)
					gameFiles.Add(new GameFile(shaderFilesLocations[i][j],GameFileType.Shader));


			for (int i = 0; i < meshFilesLocations.GetLength(0); i++)
				for (int j = 0; i < meshFilesLocations.GetLength(1); i++)
					gameFiles.Add(new GameFile(meshFilesLocations[i][j], GameFileType.Mesh));

			return gameFiles.ToArray();
		}
		public GameLoader()
		{
			LoadGameFiles(LoadDefaults());
		}
		public GameLoader(GameFile[] files)
		{
			LoadGameFiles(files);
		}
		public void LoadGameFiles(GameFile[] files)
		{
			fileReady = new bool[files.Length];
			for (int i = 0; i < files.Length; i++)
			{
				Thread thrd = new Thread(() => StaticHolder.fileParser.ParseFile(files[i], i));
				thrd.Start();
			}
		}

	}
}
