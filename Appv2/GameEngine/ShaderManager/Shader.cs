using OpenTK.Graphics.OpenGL4;

namespace KoC.GameEngine.ShaderManager
{
    public struct Shader
    {
        public string filePath;
        public ShaderType shaderType;

        public Shader(string filePath, ShaderType shaderType)
        {
            this.filePath = filePath;
            this.shaderType = shaderType;
        }
    }
}
