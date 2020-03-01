using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
namespace KoC.GameEngine.Draw.Text
{
	public class KoCFont : IDisposable
	{
		private Character[] characters;
		private float scaler;
		private Vector3 origin;
		private float avgWidth;
		private float avgWidthNormalized;
		private int last;
		private int first;
		Font font;
		public Character this[char c]
		{
			get
			{
				if(c == '\0')
				{
					return characters[0];
				}
				return characters[GetCharIndex(c)];
			}
		}
		/// <summary>
		/// Constructs KoC Font value type
		/// </summary>
		/// <param name="font"></param>
		/// <param name="first">First Character to include</param>
		/// <param name="last">Last Character to include</param>
		public KoCFont(Font font, int first = 32, int last = 127)
		{
			this.first = first;
			this.last = last;
			this.font = font;
			Image bmp = new Bitmap(1, 1);

			Graphics g = Graphics.FromImage(bmp);
			scaler = 1.0f;
			origin = new Vector3();
			this.font = font;
			string charsToDraw;

			{
				StringBuilder chars = new StringBuilder();
				for (int i = first; i < last; i++)
				{
					chars.Append(Convert.ToChar(i));
				}
				charsToDraw = chars.ToString();
				chars.Clear();
			}
			SizeF size = g.MeasureString(charsToDraw, font, new PointF(0f, 0f), StringFormat.GenericDefault);

			bmp = new Bitmap((int)size.Width - 5, (int)size.Height - 5);
			avgWidth = (float)bmp.Width / charsToDraw.Length;
			avgWidthNormalized = 1f / bmp.Width;
			g = Graphics.FromImage(bmp);

			PointF rect = new PointF(-5, -5);

			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.PixelOffsetMode = PixelOffsetMode.None;
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;

			g.DrawString(charsToDraw, font, new SolidBrush(Color.Black), rect);
			Texture2D texture = new Texture2D(TextureTarget.Texture2D, (Bitmap)bmp, font.Name);
			StaticHolder.textureHandler.AddTexture(texture);
			StaticHolder.CheckGLError();

			List<Character> charList = new List<Character>();
			for (int i = 0; i < charsToDraw.Length; i++)
			{
				charList.Add(
					new Character(charsToDraw[i], font,new float[4] { avgWidthNormalized*i, 1.0f,avgWidthNormalized*i+avgWidthNormalized,0.0f })
				);
			}
			StaticHolder.CheckGLError();
			characters = charList.ToArray();
		}
		public KoCFont(Character[] characters, string name)
		{
			this.characters = characters;
			font = new Font(name, 32f);
		}
		public Matrix4[] CalculateMatrices(string text,float originX,float originY)
		{
			List<Matrix4> matrix4s = new List<Matrix4>();
			int newLines = 1;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] == '\n' || text[i] == '\r')
				{
					newLines++;
					matrix4s.Add(SetPosition((float)i * scaler +originX, (float)newLines * scaler+originY));
				}
				else
				{
					matrix4s.Add(SetPosition((float)i * scaler+originX,(float) newLines * scaler+originY));
				}
			}
			return matrix4s.ToArray();
		}
		public int GetCharIndex(char c)
		{
			if (c == '\0') c = '\n';
			else if (c == '\u0001') c = '1';
			else if (!IsCharValid(c))
			{
				throw new ArgumentException("Invalid Character", "c");
			}
			return Convert.ToInt32(c) - first;
		}
		public bool IsCharValid(char c)
		{
			return first <= Convert.ToInt32(c) && Convert.ToInt32(c) <= last;
		}
		/// <summary>
		/// Sets Matrix of Position of Character by X and Y coords
		/// </summary>
		/// <param name="x">X Coord</param>
		/// <param name="y">Y Coord</param>
		/// <returns>Matrix</returns>
		public Matrix4 SetPosition(float x, float y)
		{
			origin.X = x;
			origin.Y = y;
			return ReloadMatrix();
		}
		public Matrix4 Scale(float sc)
		{
			scaler = sc;
			return ReloadMatrix();
		}
		private Matrix4 ReloadMatrix()
		{
			return
			Matrix4.CreateScale(scaler) *
			Matrix4.CreateTranslation(origin);
		}
		public void Dispose()
		{
			StaticHolder.textureHandler[font.Name].Dispose();
			for (int i = 0; i < characters.Length; i++)
			{
				characters[i].Dispose();
			}
		}
	}
}
