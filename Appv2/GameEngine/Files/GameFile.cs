namespace KoC.GameEngine.Files
{
    public class GameFile
    {
        string path;
        public string Path
        {
            get
            {
                return path;
            }
        }
        public GameFileType FileType
        {
            get
            {
                return fileType;
            }
        }
        GameFileType fileType;
        public GameFile(string path, GameFileType fileType)
        {
            this.path = path;
            this.fileType = fileType;
        }
    }
}
