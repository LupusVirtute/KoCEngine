using System;


namespace KoC.GameEngine.Files.Comparators
{
	public enum ObjPrefixes
	{
		Object = 0,
		Vertice = 1,
		TextureCoordinates = 2,
		Face = 3,
		Material = 4,
		Normals = 5,
		SmoothShading = 6

	}
	public class VerticePrefixParser : IPrefixParser
	{
		public ObjPrefixes prefixId { get; } = ObjPrefixes.Vertice;
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
	public class TextureCoordsPrefixParser : IPrefixParser
	{
		public ObjPrefixes prefixId { get; } = ObjPrefixes.TextureCoordinates;
		public bool IsMatch(string[] modArr)
		{
			if (modArr.Length <= 0) throw new Exception("Invalid String cannot check null string");
			return modArr[0].StartsWith("vt");
		}

		public T Parse<T>(string[] modArr)
		{
			string type = typeof(T).Name.ToLower();
			return VectorTypeConverter.GetT<T>(modArr, type);
		}
	}
	public class FacePrefixParser : IPrefixParser
	{
		public int lastIndx;
		public ObjPrefixes prefixId { get; } = ObjPrefixes.Face;

		public bool IsMatch(string[] modArr)
		{
			if (modArr.Length <= 0) throw new Exception("Invalid String cannot check null string");
			return modArr[0].StartsWith("f");

		}

		public T Parse<T>(string[] modArr)
		{
			if (typeof(T).Name != typeof(uint[][]).Name) throw new Exception("This Type is not handled here");
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
	public class ObjPrefixParser : IPrefixParser
	{
		public ObjPrefixes prefixId { get; } = ObjPrefixes.Object;

		public bool IsMatch(string[] modArr)
		{
			if (modArr.Length <= 0) throw new Exception("Invalid String cannot check null string");
			return modArr[0].StartsWith("o");
		}

		public T Parse<T>(string[] modArr)
		{
			throw new NotImplementedException();
		}
	}
	public class NormalsPrefixParser : IPrefixParser
	{
		public ObjPrefixes prefixId { get; } = ObjPrefixes.Normals;

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
	public class MaterialPrefixParser : IPrefixParser
	{
		public ObjPrefixes prefixId { get; } = ObjPrefixes.Material;

		public bool IsMatch(string[] modArr)
		{
			return modArr[0].StartsWith("usemtl");
		}

		public T Parse<T>(string[] modArr)
		{
			throw new NotImplementedException();
		}
	}
	public class SmoothShadingPrefixParser : IPrefixParser
	{
		public ObjPrefixes prefixId { get; } = ObjPrefixes.SmoothShading;

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
