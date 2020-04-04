namespace KoC.GameEngine.Files.Comparators
{
	public class NormalsPrefixParser : IPrefixParser
	{
		public ObjFormatPrefixes prefixId { get; } = ObjFormatPrefixes.Normals;

		public bool IsMatch(string[] modArr)
		{
			return modArr[0].StartsWith("vn");
		}

		public T Parse<T>(string[] modArr)
		{
			string type = typeof(T).Name.ToLower();
			return VectorTypeConverter.GetT<T>(modArr, type);
		}
	}
}
