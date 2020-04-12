using System.Collections.Generic;

namespace KoC.GameEngine.ShaderManager
{
    public interface IShaderManager
    {
        List<ShaderProgram> ShaderPrograms { get;}
        void AddShaderProgram(ShaderProgram shader,string name);
        void RemoveShaderProgram(int id);
        void ReloadShaderProgram(int id);

    }
}
