using KoC.GameEngine.Draw;
using KoC.GameEngine.Files.Comparators.FileFormatParsers;
using System;
using System.IO;

namespace KoC.GameEngine.Files.GameFilesParsers
{
	public class MeshFileParser : IFileParser
    {
        public GameFileType Type
        {
            get
            {
                return GameFileType.Mesh;
            }
        }
        public bool IsMatch(GameFileType typee)
        {
            return Type == typee;
        }

        public T Parse<T>(GameFile file)
        {
			if (typeof(T) != typeof(Mesh))
			{
				throw new Exception("Type Exception Expected Type: Mesh");
			}
			return (T)Convert.ChangeType(ParseMeshFile(file.Path),typeof(T));
        }
		/// <summary>
		/// Parses a file to a known model Structure
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		private static Mesh[] ParseMeshFile(string filePath)
		{
			try
			{

				string filex = Path.GetExtension(filePath);
				switch(filex)
				{
					case ".obj":
					{
					
						return ObjectFileFormatParse.ParseObjFile(filePath);
					}
					default:
					{
						throw new IOException("Can't Parse the " + filex + " File Format");
					}
				}
			}
			catch (FileNotFoundException ex)
			{
				Console.WriteLine(ex.ToString());
				throw;
			}
		}
	}
}
