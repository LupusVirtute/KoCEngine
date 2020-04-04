using System;


namespace KoC.GameEngine.Files.Comparators
{
	public class ObjPrefixParser : IPrefixParser
	{
		public ObjFormatPrefixes prefixId { get; } = ObjFormatPrefixes.Object;

		public bool IsMatch(string[] modArr)
		{
			if (modArr.Length <= 0) throw new Exception("Invalid String cannot check null string");
			return modArr[0].StartsWith("o");
		}
		public static string GetObjectNameFromDataString(string DataString,string defaultObj)
		{
			return DataString[0] == ObjFileStrings.obj ? DataString.Replace(ObjFileStrings.obj + " ", "") : defaultObj;
		}

		public T Parse<T>(string[] modArr)
		{
			throw new NotImplementedException();
		}
	}
}
