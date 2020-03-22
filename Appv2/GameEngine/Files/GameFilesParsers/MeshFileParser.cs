using KoC.GameEngine.Draw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoC.GameEngine.Files.GameFilesParsers
{
    public class MeshFileParser : IFileParser
    {
        public GameFileType type
        {
            get
            {
                return GameFileType.Mesh;
            }
        }
        public bool IsMatch(GameFileType typee)
        {
            return type == typee;
        }

        public void Parse<T>()
        {
            throw new NotImplementedException();
        }
    }
}
