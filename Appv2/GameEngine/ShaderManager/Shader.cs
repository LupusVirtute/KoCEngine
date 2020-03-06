namespace KoC.GameEngine.ShaderManager
{
    public struct Shader
    {
        int programID;
        string name;
        public string Name
        {
            get
            {
                return name;
            }
        }
        ShaderCompile[] compileShader;
        public Shader(ShaderCompile[] shader, string name)
        {
            this.name = name;
            compileShader = shader;
            int[] shaderID = new int[shader.Length];
            for (int i = 0; i < shader.Length; i++)
            {
                shaderID[i] = Files.FileCompiler.CompileShader(shader[i].shaderType,shader[i].filePath);
            }
            programID = Files.FileCompiler.CreateProgram(shaderID);
        }
        /// <summary>
        /// Reloads Shader
        /// <br/><b>FREEZES RENDERING</b>
        /// </summary>
        public void ReloadShader()
        {
            StaticHolder.FreezeRender = true;
            int[] shaderID = new int[compileShader.Length];
            for (int i = 0; i < compileShader.Length; i++)
            {
                shaderID[i] = Files.FileCompiler.CompileShader(compileShader[i].shaderType, compileShader[i].filePath);
            }
            programID = Files.FileCompiler.CreateProgram(shaderID);

        }
    }
}
