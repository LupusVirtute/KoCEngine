using OpenTK.Graphics.OpenGL4;

namespace KoC.GameEngine.ShaderManager
{
    public struct ShaderCompile
    {
        public string filePath;
        public ShaderType shaderType;

        public ShaderCompile(string filePath, ShaderType shaderType)
        {
            this.filePath = filePath;
            this.shaderType = shaderType;
        }
    }
}
