namespace KoC.GameEngine.Files.Comparators
{
    public interface IPrefixParser
    {
        ObjFormatPrefixes prefixId { get; }
        bool IsMatch(string[] modArr);
        T Parse<T>(string[] modArr);
    }
}
