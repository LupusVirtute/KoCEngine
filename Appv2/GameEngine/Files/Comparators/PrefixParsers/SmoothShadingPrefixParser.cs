using System;


namespace KoC.GameEngine.Files.Comparators
{
	public class SmoothShadingPrefixParser : IPrefixParser
	{
		public ObjFormatPrefixes prefixId { get; } = ObjFormatPrefixes.SmoothShading;

		public bool IsMatch(string[] modArr)
		{
			return modArr[0].StartsWith("s");
		}

		public T Parse<T>(string[] modArr)
		{
			throw new NotImplementedException();
		}
	}
}
