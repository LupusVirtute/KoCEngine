using System;


namespace KoC.GameEngine.Files.Comparators
{
	public class MaterialPrefixParser : IPrefixParser
	{
		public ObjFormatPrefixes prefixId { get; } = ObjFormatPrefixes.Material;

		public bool IsMatch(string[] modArr)
		{
			return modArr[0].StartsWith("usemtl");
		}

		public T Parse<T>(string[] modArr)
		{
			throw new NotImplementedException();
		}
	}
}
