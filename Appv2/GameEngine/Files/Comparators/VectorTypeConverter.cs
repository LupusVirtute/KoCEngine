using OpenTK;
using System;
using System.Globalization;

namespace KoC.GameEngine.Files.Comparators
{
	public static class VectorTypeConverter
	{
		public static T GetT<T>(string[] modArr,string type)
		{
			CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
			ci.NumberFormat.CurrencyDecimalSeparator = ".";
			char c = type[type.Length - 1];
			switch (c)
			{
				//Vector4
				case '4':
				{
					return (T)Convert.ChangeType(
						new Vector4(
							float.Parse(modArr[1], NumberStyles.Any, ci),
							float.Parse(modArr[2], NumberStyles.Any, ci),
							float.Parse(modArr[3], NumberStyles.Any, ci),
							float.Parse(modArr[4], NumberStyles.Any, ci)),
						typeof(T));
				}
				//Vector3
				case '3':
				{
					return (T)Convert.ChangeType(
						new Vector3(
							float.Parse(modArr[1], NumberStyles.Any, ci),
							float.Parse(modArr[2], NumberStyles.Any, ci),
							float.Parse(modArr[3], NumberStyles.Any, ci)),
						typeof(T));
				}
				//Vector2
				case '2':
				{
					return (T)Convert.ChangeType(
						new Vector2(
							float.Parse(modArr[1], NumberStyles.Any, ci),
							float.Parse(modArr[2], NumberStyles.Any, ci)),
						typeof(T));
				}
				default:
				{
					throw new Exception("Cannot Parse Invalid Type");
				}
			}
		}

	}
}
