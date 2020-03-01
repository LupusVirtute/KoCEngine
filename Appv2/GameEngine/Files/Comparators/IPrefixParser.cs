namespace KoC.GameEngine.Files.Comparators
{
    public interface IPrefixParser
    {
        ObjPrefixes prefixId { get; }
        bool IsMatch(string[] modArr);
        T Parse<T>(string[] modArr);
    }
}
