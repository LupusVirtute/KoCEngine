using System.Type;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;
namespace KoC.GameEngine.Files.GameFilesParsers 
{
	public class TextureFileParser : IFileParser
	{
		public GameFileType Type {
			get {
				return GameFileType.Texture2D;
			}
		}
		public bool IsMatch(GameFileType typee){
			return Type == typee;
		}
		public T Parse<T>(GameFile gameFile){
			if(typeof(T) != typeof(Texture2D)){
				#if DEBUG
					throw new Exception("Error couldn't parse other type than Texture2D");
				#else
					return;
				#endif
			}

			return (T)Convert.ChangeType(ParseTexture2D(gameFile),T);
		}

		public Texture2D ParseTexture2D(GameFile gameFile){
			Bitmap bitmap = FileParser.LoadBitMap(gameFile.Path);
			string textureName = Path.GetFileName(gameFile.Path);
			return new Texture2D(TextureTarget.Texture2D, bitmap,textureName);
		}

	}



}
