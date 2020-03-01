using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoC.GameEngine.Draw.Textures
{
    public interface ITextureHandler
    {
        Texture2D NoTexture { get;}
        bool AddTexture(Texture2D texture);
        bool RemoveTexture(Texture2D texture);
    }
}
