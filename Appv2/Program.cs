﻿
using System.Threading;

namespace KoC
{
    public struct MainC
    {
        public static GameEngine.Game game = new GameEngine.Game();
        static void Main(string[] args)
        {
            game.Run(1,60);
            
        }
    }
}
