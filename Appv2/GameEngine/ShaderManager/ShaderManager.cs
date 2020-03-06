using System.Collections.Generic;

namespace KoC.GameEngine.ShaderManager
{
    public class ShaderManager : IShaderManager
    {
        private List<Shader> shaderPrograms;
        Dictionary<string, int> shaderDic;
        public ShaderManager(Shader[] programs)
        {
            shaderPrograms = new List<Shader>(programs);
            shaderDic = new Dictionary<string, int>();
            ReloadDictionary();
        }
        private void ReloadDictionary()
        {
            shaderDic.Clear();
            for (int i = 0; i < shaderPrograms.Count; i++)
            {
                shaderDic.Add(shaderPrograms[i].Name, i);
            }
        }

        public List<Shader> ShaderPrograms
        {
            get
            {
                return shaderPrograms;
            }
        }
        public Shader this[int id]
        {
            get
            {
                return shaderPrograms[id];
            }
        }
        public Shader this[string id]
        {
            get
            {
                return shaderPrograms[shaderDic[id]];
            }
        }
        public void AddShaderProgram(Shader shader,string name)
        {
            shaderPrograms.Add(shader);
            ReloadDictionary();
        }

        public void ReloadShaderProgram(int id)
        {
            shaderPrograms[id].ReloadShader();
        }

        public void RemoveShaderProgram(int id)
        {
            shaderPrograms.RemoveAt(id);
            ReloadDictionary();
        }
    }
}
