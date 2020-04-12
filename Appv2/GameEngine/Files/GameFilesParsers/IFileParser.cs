namespace KoC.GameEngine.Files.GameFilesParsers
{
    public interface IFileParser
    {
        bool IsMatch(GameFileType typee);
        T Parse<T>(GameFile file);
        GameFileType Type { get;}
    }
}
