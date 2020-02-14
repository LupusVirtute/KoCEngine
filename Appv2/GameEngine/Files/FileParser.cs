using KoC.GameEngine.Draw;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
		public static BitmapData LoadImage(string filePath)
		{
			Bitmap x = new Bitmap(filePath);
			BitmapData x2 = x.LockBits(new Rectangle(0, 0, x.Width, x.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
			x.UnlockBits(x2);
			return x2;
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
		/// <param name="x">The pointing Array</param>
		/// <returns>Indices of Triangles</returns>
		private static uint[,] TriangualizeIndices(uint[] x)
		{
			int l = x.Length;
			if (l <= 1)
			{
				throw new Exception("Invalid length of indice Array");
			}
			else if (l % 4 == 0)
			{
				uint[,] x2D = new uint[l / 2, 3];
				uint x2Didx = 0;
				for (int i = 0; i < l; i += 4, x2Didx += 2)
				{
					x2D[x2Didx, 0] = x[i];
					x2D[x2Didx, 1] = x[i + 1];
					x2D[x2Didx, 2] = x[i + 2];
					x2D[x2Didx + 1, 0] = x[i];
					x2D[x2Didx + 1, 1] = x[i + 2];
					x2D[x2Didx + 1, 2] = x[i + 3];
				}
				return x2D;
			}
			else if (l % 3 == 0)
			{
				uint[,] x2D = new uint[l / 3, 3];
				uint x2Didx = 0;
				for (int i = 0; i < l; i += 3, x2Didx++)
				{
					x2D[x2Didx, 0] = x[i];
					x2D[x2Didx, 1] = x[i + 1];
					x2D[x2Didx, 2] = x[i + 2];
				}
				return x2D;
			}//Triangle Fan Just not as whole VAO
			else
			{
				uint[,] x2D = new uint[l / 2, 3];
				uint x2Didx = 0;
				x2D[x2Didx, 0] = x[0];
				x2D[x2Didx, 1] = x[1];
				x2D[x2Didx, 2] = x[2];
				x2Didx++;
				for (int i = 2; i < l; i++, x2Didx++)
				{
					x2D[x2Didx, 0] = x[0];
					x2D[x2Didx, 1] = x[i - 1];
					x2D[x2Didx, 2] = x[i];
				}
				return x2D;
			}
		}
		public static Mesh[] ParseObjFile(string filePath)
		{

			List<Mesh> meshList = new List<Mesh>();
			string[] strArr = File.ReadAllLines(filePath);
			int lastindx = 1;
			for (int i = 0; i < strArr.Length; i++)
			{
				if (0 >= strArr[i].Length)
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
				if (strArr[i][0] == 'o')
				{
					name = strArr[i].Replace("o ", "");
					i++;
					for (; i < strArr.Length; i++)
					{
						if (strArr.Length == i+1)
						{
							uint[] x = verListP.ToArray();
							uint[] y = texCoorP.ToArray();
							uint[] z = normalP.ToArray();
							uint[,] x2 = TriangualizeIndices(x);
							x = new uint[x2.Length];
							int ii = 0;
							for (int i2 = 0, l2 = x2.GetLength(0); i2 < l2; i2++, ii += 3)
							{
								x[ii] = x2[i2, 0];
								x[ii + 1] = x2[i2, 1];
								x[ii + 2] = x2[i2, 2];
							}
							x2 = TriangualizeIndices(y);
							y = new uint[x2.Length];
							ii = 0;
							for (int i2 = 0, l2 = x2.GetLength(0); i2 < l2; i2++, ii += 3)
							{
								y[ii] = x2[i2, 0];
								y[ii + 1] = x2[i2, 1];
								y[ii + 2] = x2[i2, 2];
							}
							x2 = TriangualizeIndices(z);
							z = new uint[x2.Length];
							ii = 0;
							for (int i2 = 0, l2 = x2.GetLength(0); i2 < l2; i2++, ii += 3)
							{
								z[ii] = x2[i2, 0];
								z[ii + 1] = x2[i2, 1];
								z[ii + 2] = x2[i2, 2];
							}
							meshList.Add(new Mesh(
								Vlist.ToArray(),
								normals.ToArray(),
								texcoords.ToArray(),
								x,
								y,
								z
								));

							lastindx += Vlist.Count;
							i--;
							break;
						}
						string[] modArr = strArr[i].Split(' ');
						if (modArr[0].Length >= 1 && !modArr[0].Contains("#"))
						{
							if (modArr[0].Length == 1 && modArr[0][0] == 'v')
							{
								Vlist.Add(
									new Vector3(
										float.Parse(modArr[1].Replace('.', ',')),
										float.Parse(modArr[2].Replace('.', ',')),
										float.Parse(modArr[3].Replace('.', ','))
										)
									);
							}
							else if (modArr[0].StartsWith("vt"))
							{
								texcoords.Add(
								new Vector2(
									float.Parse(modArr[1].Replace('.', ',')),
									float.Parse(modArr[2].Replace('.', ','))
									)
								);
							}
							else if (modArr[0].StartsWith("vn"))
							{
								normals.Add(
								new Vector3(
									float.Parse(modArr[1].Replace('.', ',')),
									float.Parse(modArr[2].Replace('.', ',')),
									float.Parse(modArr[3].Replace('.', ','))
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
								uint[] x = verListP.ToArray();
								uint[] y = texCoorP.ToArray();
								uint[] z = normalP.ToArray();
								uint[,] x2 = TriangualizeIndices(x);
								x = new uint[x2.Length];
								int ii = 0;
								for (int i2 = 0, l2 = x2.GetLength(0); i2 < l2; i2++, ii += 3)
								{
									y[ii] = x2[i2, 0];
									y[ii + 1] = x2[i2, 1];
									y[ii + 2] = x2[i2, 2];
								}
								x2 = TriangualizeIndices(y);
								y = new uint[x2.Length];
								ii = 0;
								for (int i2 = 0, l2 = x2.GetLength(0); i2 < l2; i2++, ii += 3)
								{
									x[ii] = x2[i2, 0];
									x[ii + 1] = x2[i2, 1];
									x[ii + 2] = x2[i2, 2];
								}
								x2 = TriangualizeIndices(z);
								z = new uint[x2.Length];
								ii = 0;
								for (int i2 = 0, l2 = x2.GetLength(0); i2 < l2; i2++, ii += 3)
								{
									z[ii] = x2[i2, 0];
									z[ii + 1] = x2[i2, 1];
									z[ii + 2] = x2[i2, 2];
								}
								meshList.Add(new Mesh(
									Vlist.ToArray(),
									normals.ToArray(),
									texcoords.ToArray(),
									x,
									y,
									z
									));
								lastindx += Vlist.Count;
								i--;
								break;
							}
						}
					}
				}
			}
			return meshList.ToArray();
		}
	}
}
