using System.Collections.Generic;

namespace KoC.GameEngine.ShaderManager
{
    public class ShaderManager : IShaderManager
    {
		public List<ShaderProgram> ShaderPrograms { get; private set; }
        Dictionary<string, int> shaderDic;
        public ShaderManager(ShaderProgram[] programs)
        {
            ShaderPrograms = new List<ShaderProgram>(programs);
            shaderDic = new Dictionary<string, int>();
            ReloadDictionary();
        }
        private void ReloadDictionary()
        {
            shaderDic.Clear();
            for (int i = 0; i < ShaderPrograms.Count; i++)
            {
                shaderDic.Add(ShaderPrograms[i].Name, i);
            }
        }

		public ShaderProgram this[int id]
        {
            get
            {
                return ShaderPrograms[id];
            }
        }
        public ShaderProgram this[string id]
        {
            get
            {
                return ShaderPrograms[shaderDic[id]];
            }
        }
        public void AddShaderProgram(ShaderProgram shader,string name)
        {
            ShaderPrograms.Add(shader);
            ReloadDictionary();
        }

        public void ReloadShaderProgram(int id)
        {
            ShaderPrograms[id].ReloadShader();
        }

        public void RemoveShaderProgram(int id)
        {
            ShaderPrograms.RemoveAt(id);
            ReloadDictionary();
        }
    }
}
