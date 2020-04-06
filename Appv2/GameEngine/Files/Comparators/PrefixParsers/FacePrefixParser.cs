using System;


namespace KoC.GameEngine.Files.Comparators
{
	public class FacePrefixParser : IPrefixParser
	{
		public int lastIndx;
		public ObjFormatPrefixes prefixId { get; } = ObjFormatPrefixes.Face;

		public bool IsMatch(string[] modArr)
		{
			if (modArr.Length <= 0) throw new Exception("Invalid String cannot check null string");
			return modArr[0].StartsWith("f");

		}

		public T Parse<T>(string[] modArr)
		{
			if (typeof(T).Name != typeof(uint[][]).Name) 
				throw new Exception("This Type is not handled here");

			uint[] verticePointerArray = new uint[modArr.Length - 1];
			uint[] textureCoordsArray = new uint[modArr.Length - 1];
			uint[] normalPointerArray = new uint[modArr.Length - 1];
			for (int b = 1, xyy = modArr.Length; b < xyy; b++)
			{
				string[] f = modArr[b].Split('/');
				verticePointerArray[b - 1] = Convert.ToUInt32(int.Parse(f[0]) - lastIndx);
				textureCoordsArray[b - 1] = Convert.ToUInt32(int.Parse(f[1]) - lastIndx);
				normalPointerArray[b - 1] = Convert.ToUInt32(int.Parse(f[2]) - lastIndx);
			}
			uint[][] UltimatePointer = new uint[3][]
			{
				verticePointerArray,
				textureCoordsArray,
				normalPointerArray
			};
			return (T)Convert.ChangeType(UltimatePointer, typeof(T));
		}
	}
}
