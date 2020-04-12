using System;

namespace KoC.GameEngine.Utilities._3D
{
	public static class PointManipulation
	{
		/// <summary>
		/// Switches Indices from different types of polygon Faces to simple triangles
		/// </summary>
		/// <param name="indicesPointer">The pointing Array</param>
		/// <returns>Indices of Triangles</returns>
		public static uint[] TriangualizePoints(uint[] indicesPointer)
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
	}
}
