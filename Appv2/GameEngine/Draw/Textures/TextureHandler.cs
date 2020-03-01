using KoC.GameEngine.Draw.Textures;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace KoC.GameEngine.Draw
{
    public class TextureHandler : ITextureHandler
    {
        private Texture2D noTexture = new Texture2D(TextureTarget.Texture2D, Files.FileParser.LoadBitMap("DefaultTexture\\NoTexture.bmp"), "NullTexture");
        public Texture2D NoTexture
        { 
            get
            {
                return noTexture;
            }
        }
        private List<Texture2D> textures = new List<Texture2D>();
        public Texture2D GetTexture2DByName(string name)
        {
            if (string.IsNullOrEmpty(name)) return noTexture;
            for(int i = 0; i < textures.Count; i++)
            {
                if (textures[i].name == name) return textures[i];
            }
            return noTexture;
        }
        public Texture2D this[string name]
        {
            get
            {
                return GetTexture2DByName(name);
            }
        }
        /// <summary>
        /// Adds texture to texture pool
        /// </summary>
        /// <param name="texture"></param>
        /// <returns>
        /// Success:
        ///     true - Success
        ///     false - failed
        /// </returns>
        public bool AddTexture(Texture2D texture)
        {
            if (texture.IsNull()) return false;
            textures.Add(texture);
            return true;
        }
        public bool RemoveTexture(Texture2D texture)
        {
            if (texture.IsNull()) return false;
            textures.Remove(texture);
            return true;
        }
    }

}
