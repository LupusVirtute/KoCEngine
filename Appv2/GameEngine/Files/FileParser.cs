using KoC.GameEngine.Draw;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime;
using System.Text;


namespace KoC.GameEngine.Files
{
	public class FileParser
	{
		/// <summary>
		/// Loads Image or texture in BitmapData
		/// </summary>
		/// <param name="filePath">Path to file</param>
		/// <returns>BitmapData of image</returns>
		public static Bitmap LoadBitMap(string filePath)
		{
			Bitmap bitMap = new Bitmap(filePath);
			return bitMap;
		}
		/// <summary>
		/// Parses a file to a known model Structure
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static Mesh[] ParseFile(string filePath)
		{
			try
			{

				string filex = Path.GetExtension(filePath);
				if (filex.Contains("obj"))
				{
					return ParseObjFile(filePath);
				}
				else
				{
					throw new IOException("Can't Parse the " + filex + " File Format");
				}
			}
			catch (FileNotFoundException ex)
			{
				Console.WriteLine(ex.ToString());
				throw;
			}
		}
		/// <summary>
		/// Switches Indices from different types of polygon Faces to simple triangles
		/// </summary>
		/// <param name="indicesPointer">The pointing Array</param>
		/// <returns>Indices of Triangles</returns>
		private static uint[] TriangualizeIndices(uint[] indicesPointer)
		{
			int l = indicesPointer.Length;
			if (l <= 2)
			{
				throw new Exception("Invalid length of indice Array");
			}
			else if (l % 4 == 0)
			{
				uint[] newIndiceArray = new uint[(l / 2) * 3];
				uint newIndiceArrayPointer = 0;
				for (int i = 0; i < l; i += 4, newIndiceArrayPointer += 2)
				{
					newIndiceArray[0 + newIndiceArrayPointer * 3] = indicesPointer[i];
					newIndiceArray[1 + newIndiceArrayPointer * 3] = indicesPointer[i + 1];
					newIndiceArray[2 + newIndiceArrayPointer * 3] = indicesPointer[i + 2];
					newIndiceArray[0 + ((newIndiceArrayPointer + 1) * 3)] = indicesPointer[i];
					newIndiceArray[1 + ((newIndiceArrayPointer + 1) * 3)] = indicesPointer[i + 2];
					newIndiceArray[2 + ((newIndiceArrayPointer + 1) * 3)] = indicesPointer[i + 3];
				}
				return newIndiceArray;
			}
			else if (l % 3 == 0)
			{
				uint[] newIndiceArray = new uint[l];
				uint newIndiceArrayPointer = 0;
				for (int i = 0; i < l; i += 3, newIndiceArrayPointer++)
				{
					newIndiceArray[0 + newIndiceArrayPointer * 3] = indicesPointer[i];
					newIndiceArray[1 + newIndiceArrayPointer * 3] = indicesPointer[i + 1];
					newIndiceArray[2 + newIndiceArrayPointer * 3] = indicesPointer[i + 2];
				}
				return newIndiceArray;
			}//Triangle Fan Just not as whole VAO
			else
			{
				uint[] newIndiceArray = new uint[(l / 2) * 3];
				uint newIndiceArrayPointer = 0;
				newIndiceArray[0 + newIndiceArrayPointer * 3] = indicesPointer[0];
				newIndiceArray[1 + newIndiceArrayPointer * 3] = indicesPointer[1];
				newIndiceArray[2 + newIndiceArrayPointer * 3] = indicesPointer[2];
				newIndiceArrayPointer++;
				for (int i = 2; i < l; i++, newIndiceArrayPointer++)
				{
					newIndiceArray[0 + (newIndiceArrayPointer * 3)] = indicesPointer[0];
					newIndiceArray[1 + (newIndiceArrayPointer * 3)] = indicesPointer[i - 1];
					newIndiceArray[2 + (newIndiceArrayPointer * 3)] = indicesPointer[i];
				}
				return newIndiceArray;
			}
		}
		public static Mesh[] ParseObjFile(string filePath)
		{

			List<Mesh> meshList = new List<Mesh>();
			string DataString = string.Empty;
			int lastindx = 1;
			string nextObj = string.Empty;

			CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
			ci.NumberFormat.CurrencyDecimalSeparator = ".";

			using (StreamReader str = new StreamReader(filePath))
			{
				while((DataString = str.ReadLine()) != null)
				{
					if (DataString[0] == '#')
					{
						continue;
					}
					string name = string.Empty;
					List<Vector3> normals = new List<Vector3>();
					List<Vector2> texcoords = new List<Vector2>();
					List<Vector3> Vlist = new List<Vector3>();
					List<uint> verListP = new List<uint>();
					List<uint> texCoorP = new List<uint>();
					List<uint> normalP = new List<uint>();
					if (DataString[0] == 'o' || !string.IsNullOrEmpty(nextObj))
					{
						name = DataString[0] == 'o' ? DataString.Replace("o ", "") : nextObj;
						while(true)
						{
							DataString = str.ReadLine();
							if (DataString == null)
							{
								uint[] verticesPointer = verListP.ToArray();
								verListP.Clear();
								uint[] texCoordsP = texCoorP.ToArray();
								texCoorP.Clear();
								uint[] normalPointers = normalP.ToArray();
								normalP.Clear();

								meshList.Add(new Mesh(
									Vlist.ToArray(),
									normals.ToArray(),
									texcoords.ToArray(),
									TriangualizeIndices(verticesPointer),
									TriangualizeIndices(texCoordsP),
									TriangualizeIndices(normalPointers)
									));

								lastindx += Vlist.Count;
								nextObj = string.Empty;
								break;
							}
							string[] modArr = DataString.Split(' ');
							DataString = string.Empty;
							if (modArr[0].Length >= 1 && !modArr[0].Contains("#"))
							{
								if (modArr[0].Length == 1 && modArr[0][0] == 'v')
								{
									Vlist.Add(
										new Vector3(
											float.Parse(modArr[1], NumberStyles.Any, ci),
											float.Parse(modArr[2], NumberStyles.Any, ci),
											float.Parse(modArr[3], NumberStyles.Any, ci)
											)
										);
								}
								else if (modArr[0].StartsWith("vt"))
								{
									texcoords.Add(
									new Vector2(
										float.Parse(modArr[1], NumberStyles.Any, ci),
										float.Parse(modArr[2], NumberStyles.Any, ci)
										)
									);
								}
								else if (modArr[0].StartsWith("vn"))
								{
									normals.Add(
									new Vector3(
										float.Parse(modArr[1], NumberStyles.Any, ci),
										float.Parse(modArr[2], NumberStyles.Any, ci),
										float.Parse(modArr[3], NumberStyles.Any, ci)
										)
									);
								}
								else if (modArr[0].StartsWith("usemtl"))
								{
								}
								else if (modArr[0].StartsWith("s"))
								{
								}
								else if (modArr[0].StartsWith("f"))
								{
									uint[] x = new uint[modArr.Length - 1];
									uint[] x1 = new uint[modArr.Length - 1];
									uint[] x2 = new uint[modArr.Length - 1];
									for (int b = 1, xyy = modArr.Length; b < xyy; b++)
									{
										string[] f = modArr[b].Split('/');
										x[b - 1] = Convert.ToUInt32(int.Parse(f[0]) - lastindx);
										x1[b - 1] = Convert.ToUInt32(int.Parse(f[1]) - lastindx);
										x2[b - 1] = Convert.ToUInt32(int.Parse(f[2]) - lastindx);
									}
									for (int i2 = 0, l2 = x.Length; i2 < l2; i2++)
									{
										verListP.Add(x[i2]);
										texCoorP.Add(x1[i2]);
										normalP.Add(x2[i2]);
									}
								}
								else if (modArr[0].StartsWith("o"))
								{
									meshList.Add(new Mesh(
											Vlist.ToArray(),
											normals.ToArray(),
											texcoords.ToArray(),
											TriangualizeIndices(verListP.ToArray()),
											TriangualizeIndices(texCoorP.ToArray()),
											TriangualizeIndices(normalP.ToArray())
										));
									lastindx += Vlist.Count;
									nextObj = modArr[1];
									break;
								}
							}
						}
					}
				}
			}
			
			// TODO:
			// Save mesh in new File Format
			return meshList.ToArray();
		}
	}
}
