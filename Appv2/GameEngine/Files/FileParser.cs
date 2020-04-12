using KoC.GameEngine.Draw;
using KoC.GameEngine.Files.Comparators;
using KoC.GameEngine.Files.GameFilesParsers;
using KoC.GameEngine.ShaderManager;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;

namespace KoC.GameEngine.Files
{
	public class FileParser
	{
		public void ParseFile(GameFile file,int readyIndex)
		{
			IFileParser[] parsers =
			{
				new MeshFileParser()
			};
			IFileParser result = parsers.FirstOrDefault(o => o.IsMatch(file.FileType));
			switch (file.FileType)
			{
				case GameFileType.Mesh:
					result.Parse<Mesh>(file);
					break;
				case GameFileType.Shader:
					result.Parse<Shader>(file);
					break;
				case GameFileType.Texture2D:
					result.Parse<Texture2D>(file);
					break;
				case GameFileType.SceneInfo:
					break;
				case GameFileType.PlayerInfo:
					break;
				case GameFileType.Save:
					break;
				case GameFileType.ObjectInfo:
					break;
				default:
					break;
			}

			StaticHolder.loader.fileReady[readyIndex] = true;

		}
		/// <summary>
		/// Loads Image or texture in BitmapData
		/// </summary>
		/// <param name="filePath">Path to file</param>
		/// <returns>BitmapData of image</returns>
		public static Bitmap LoadBitMap(string filePath)
		{
			Bitmap bitMap = new Bitmap(filePath);
			return bitMap;
		}

	}
}
