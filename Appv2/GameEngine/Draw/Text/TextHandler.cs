using KoC.GameEngine.ShaderManager;
using OpenTK;
using System.Collections.Generic;
using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;

namespace KoC.GameEngine.Draw.Text
{
    public class TextHandler : IDisposable
    {
        List<EngineText2D> texts = new List<EngineText2D>();
        List<KoCFont> fonts = new List<KoCFont>();
        KoCFont defaultFont;
        public TextHandler(EngineText2D[] texts)
        {
            this.texts = new List<EngineText2D>(texts);
            Font f = new Font("Consolas",64f);
            defaultFont = new KoCFont(f);
        }
        public TextHandler(EngineText2D[] texts,Font f)
        {
            this.texts = new List<EngineText2D>(texts);
            defaultFont = new KoCFont(f);
        }
        public void Dispose()
        {
            defaultFont.Dispose();
            for(int i=0; i < fonts.Length;i++)
                fonts[i].Dispose();
        }
        /// <summary>
        /// Changes used default Font <br/>
        /// First and Last are used for range of chars to copy to usable chars
        /// </summary>
        /// <param name="f">Font to use</param>
        /// <param name="first">first char</param>
        /// <param name="last">end char</param>
        public void ChangeDefaultFont(Font f,int first = 32,int last = 127)
        {
            defaultFont = new KoCFont(f,first,last);
        }
        public void AddTextUsingDefaultFont(string text,Vector2 origin)
        {
            EngineText2D engTxT2D = new EngineText2D(defaultFont,text,origin);
            AddText(engTxT2D);
        }
        public void AddText(EngineText2D TxT)
        {
            texts.Add(TxT);
        }
        public void RemoveText(int id)
        {
            texts.RemoveAt(id);
        }
        public void RenderText(int charOffSetLoc,int gSamplerLocation)
        {
            GL.Uniform1(gSamplerLocation, 0);
            for (int i = 0; i < texts.Count; i++)
            {
                texts[i].Render(charOffSetLoc);
            }
        }
    }
}
