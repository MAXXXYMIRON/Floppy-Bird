﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace FlopyBirdGame
{
    static class PlayGame
    {
        public static RenderWindow Window;
        public const ushort Height = 600;
        public const ushort Width = 1200;
        public const string FileSprites = "Flappy Bird Sprites.png";

        private static void Main()
        {
            Window = new RenderWindow(new VideoMode(Width, Height), "Flopy Bird");
            Window.SetVerticalSyncEnabled(true);
            Window.Closed += close;
            Window.Clear();


            while (Window.IsOpen)
            {
                Window.DispatchEvents();
                Window.Clear();

                Background.DrawBack();
                Obstacle.DrawObstacle();
                Bird.DrawBird();


                Window.Display();
            }
        }

        private static void close(object sender, EventArgs e)
        {
            Window.Close();
        }
    }
}
