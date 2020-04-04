using KoC.GameEngine.Draw;
using KoC.GameEngine.Files.Comparators;
using KoC.GameEngine.Files.GameFilesParsers;
using KoC.GameEngine.ShaderManager;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;

namespace KoC.GameEngine.Files
{
	public class FileParser
	{
		public void ParseFile(GameFile file,int readyIndex)
		{
			IFileParser[] parsers =
			{
				new MeshFileParser()
			};
			IFileParser result = parsers.FirstOrDefault(o => o.IsMatch(file.FileType));
			switch (file.FileType)
			{
				case GameFileType.Mesh:
					result.Parse<Mesh>();
					break;
				case GameFileType.Shader:
					result.Parse<ShaderCompile>();
					break;
				case GameFileType.Texture2D:
					result.Parse<Texture2D>();
					break;
				case GameFileType.SceneInfo:
					break;
				case GameFileType.PlayerInfo:
					break;
				case GameFileType.Save:
					break;
				case GameFileType.ObjectInfo:
					break;
				default:
					break;
			}

			StaticHolder.loader.fileReady[readyIndex] = true;

		}
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
		public static Mesh[] ParseMeshFile(string filePath)
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
		private static uint[] TriangualizePoints(uint[] indicesPointer)
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
			FacePrefixParser fPrefixParser = new FacePrefixParser();
			List<IPrefixParser> prefixParsers = new List<IPrefixParser>
			{
				new VerticePrefixParser(),
				new TextureCoordsPrefixParser(),
				fPrefixParser,
				new NormalsPrefixParser(),
				new ObjPrefixParser(),
				new MaterialPrefixParser(),
				new SmoothShadingPrefixParser()
			};
			List<Mesh> meshList = new List<Mesh>();
			string DataString = string.Empty;
			fPrefixParser.lastIndx = 1;
			string nextObj = string.Empty;

			CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
			ci.NumberFormat.CurrencyDecimalSeparator = ".";

			using (StreamReader str = new StreamReader(filePath))
			{
				while ((DataString = str.ReadLine()) != null)
				{
					if (DataString[0] == ObjFileStrings.comment)
					{
						continue;
					}
					string name = string.Empty;
					List<Vector3> normals = new List<Vector3>();
					List<Vector2> textureCoords = new List<Vector2>();
					List<Vector3> verticesList = new List<Vector3>();
					List<uint> verListPoint = new List<uint>();
					List<uint> textureCoordsPoint = new List<uint>();
					List<uint> normalPoint = new List<uint>();
					if (DataString[0] == ObjFileStrings.obj || !string.IsNullOrEmpty(nextObj))
					{
						name = DataString[0] == ObjFileStrings.obj ? DataString.Replace(ObjFileStrings.obj+" ", "") : nextObj;
						while (true)
						{
							DataString = str.ReadLine();
							if (DataString == null)
							{
								uint[] verticesPointer = verListPoint.ToArray();
								verListPoint.Clear();
								uint[] texCoordsP = textureCoordsPoint.ToArray();
								textureCoordsPoint.Clear();
								uint[] normalPointers = normalPoint.ToArray();
								normalPoint.Clear();

								meshList.Add(new Mesh(
									verticesList.ToArray(),
									normals.ToArray(),
									textureCoords.ToArray(),
									TriangualizePoints(verticesPointer),
									TriangualizePoints(texCoordsP),
									TriangualizePoints(normalPointers)
									));

								fPrefixParser.lastIndx += verticesList.Count;
								nextObj = string.Empty;
								break;
							}
							string[] modArr = DataString.Split(' ');
							DataString = string.Empty;
							//Checks if is there any need for checking values
							if (modArr[0].Length >= 1 && !modArr[0].Contains($"{ObjFileStrings.comment}"))
							{
								var parser = prefixParsers.FirstOrDefault(p => p.IsMatch(modArr));
								//To just stop this loop running we can break it legs :)
								bool legs = false;
								switch (parser.prefixId)
								{
									case ObjPrefixes.Vertice:
									{

										verticesList.Add(
												parser.Parse<Vector3>(modArr)
											);
										break;
									}
									case ObjPrefixes.Face:
									{
										uint[][] FaceArray = parser.Parse<uint[][]>(modArr);
										for (int i2 = 0, l2 = FaceArray[0].Length; i2 < l2; i2++)
										{
											verListPoint.Add(FaceArray[0][i2]);
											textureCoordsPoint.Add(FaceArray[1][i2]);
											normalPoint.Add(FaceArray[2][i2]);
										}
										break;
									}
									case ObjPrefixes.Material:
									{

										break;
									}
									case ObjPrefixes.TextureCoordinates:
									{
										textureCoords.Add(
											parser.Parse<Vector2>(modArr)
										);
										break;
									}
									case ObjPrefixes.Normals:
									{
										normals.Add(
											parser.Parse<Vector3>(modArr)
										);
										break;
									}
									case ObjPrefixes.Object:
									{
										meshList.Add(new Mesh(
											verticesList.ToArray(),
											normals.ToArray(),
											textureCoords.ToArray(),
											TriangualizePoints(verListPoint.ToArray()),
											TriangualizePoints(textureCoordsPoint.ToArray()),
											TriangualizePoints(normalPoint.ToArray())
										));
										fPrefixParser.lastIndx += verticesList.Count;
										nextObj = modArr[1];
										//we are telling legs to prepare for being breaked
										legs = true;
										break;
									}
									case ObjPrefixes.SmoothShading:
									{

										break;
									}
									default:
									{
										throw new Exception("Unrecognized Instruction");
									}
								}
								//Break the legs
								if (legs) break;
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
