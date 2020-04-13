using KoC.GameEngine.ShaderManager;
using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace KoC.GameEngine.Files.GameFilesParsers
{
    public class ShaderFileParser : IFileParser
    {
        public GameFileType Type
        {
            get
            {
                return GameFileType.Shader;
            }
        }
        public bool IsMatch(GameFileType typee)
        {
            return Type == typee;
        }

        public T Parse<T>(GameFile file)
        {
            if(typeof(T) != typeof(Shader))
            {
                throw new Exception("Cannot pass other type than shader");
            }
            string extension = Path.GetExtension(file.Path);
            ShaderType typeOfShader;
            switch (extension)
            {
                case ".frag":
                    typeOfShader = ShaderType.FragmentShader;
                    break;
                case ".vert":
                    typeOfShader = ShaderType.VertexShader;
                    break;
                case ".tese":
                    typeOfShader = ShaderType.TessEvaluationShader;
                    break;
                case ".tesc":
                    typeOfShader = ShaderType.TessControlShader;
                    break;
                case ".geom":
                    typeOfShader = ShaderType.GeometryShader;
                    break;
                case ".comp":
                    typeOfShader = ShaderType.ComputeShader;
                    break;
                default:
                    throw new Exception("INVALID SHADER TYPE");
            }
            return (T)Convert.ChangeType(new Shader(file.Path,typeOfShader),typeof(T));
        }
    }
}
