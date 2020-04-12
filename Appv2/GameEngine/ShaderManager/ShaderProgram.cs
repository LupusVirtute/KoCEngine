namespace KoC.GameEngine.ShaderManager
{
    public struct ShaderProgram
    {
        public int ProgramID { get; private set; }
		public string Name { get; private set; }
		Shader[] compileShader;
        public ShaderProgram(Shader[] shader, string name)
        {
            this.Name = name;
            compileShader = shader;
            int[] shaderID = new int[shader.Length];
            for (int i = 0; i < shader.Length; i++)
            {
                shaderID[i] = Files.FileCompiler.CompileShader(shader[i].shaderType,shader[i].filePath);
            }
            ProgramID = Files.FileCompiler.CreateProgram(shaderID);
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
            ProgramID = Files.FileCompiler.CreateProgram(shaderID);

        }
    }
}
