using System;


namespace KoC.GameEngine.Files.Comparators
{
	public class VerticePrefixParser : IPrefixParser
	{
		public ObjFormatPrefixes prefixId { get; } = ObjFormatPrefixes.Vertice;
		public bool IsMatch(string[] modArr)
		{
			if (modArr.Length <= 0) throw new Exception("Invalid String cannot check null string");
			return modArr[0].Length == 1 && modArr[0][0] == 'v';
		}
		public T Parse<T>(string[] modArr)
		{
			string type = typeof(T).Name.ToLower();
			return VectorTypeConverter.GetT<T>(modArr, type);
		}
	}
}
