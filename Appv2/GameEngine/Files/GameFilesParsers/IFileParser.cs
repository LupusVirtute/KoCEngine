using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoC.GameEngine.Files.GameFilesParsers
{
    public interface IFileParser
    {
        bool IsMatch(GameFileType typee);
        void Parse<T>();
        GameFileType type { get;}
    }
}
