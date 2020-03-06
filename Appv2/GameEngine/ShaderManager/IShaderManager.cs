using System.Collections.Generic;

namespace KoC.GameEngine.ShaderManager
{
    public interface IShaderManager
    {
        List<Shader> ShaderPrograms { get;}
        void AddShaderProgram(Shader shader,string name);
        void RemoveShaderProgram(int id);
        void ReloadShaderProgram(int id);

    }
}
