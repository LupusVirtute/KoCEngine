using OpenTK.Graphics.OpenGL4;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace KoC.GameEngine.Draw
{
	public class Texture2D : IDisposable
	{
		public string name;
		private readonly int _texture = -32;
		private readonly TextureTarget textureTarget;

		public Texture2D(TextureTarget x, Bitmap bitmap,string name,bool wrapS = true,bool filter = true)
		{
			textureTarget = x;
			_texture = GL.GenTexture();
			GL.BindTexture(x, _texture);
			bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
			if (filter)
			{
				GL.TextureParameter(_texture, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
				GL.TextureParameter(_texture, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			}
			if (wrapS)
			{
				GL.TextureParameter(_texture, TextureParameterName.TextureWrapS, (int)All.Repeat);
				GL.TextureParameter(_texture, TextureParameterName.TextureWrapT, (int)All.Repeat);
			}
			GL.TexImage2D(x, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			GL.TexSubImage2D(x,0,0,0,bitmapData.Width,bitmapData.Height,OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,PixelType.UnsignedByte,bitmapData.Scan0);
			bitmap.UnlockBits(bitmapData);
			bitmap.Dispose();
			GL.BindTexture(x,0);
		}
		public bool IsNull()
		{
			return GL.IsTexture(_texture);
		}
		public void Dispose()
		{
			GL.DeleteTexture(_texture);
			GC.SuppressFinalize(this);

		}
		public void Bind(TextureUnit texU)
		{
			GL.ActiveTexture(texU);
			GL.BindTexture(textureTarget,_texture);
		}

	}

}
