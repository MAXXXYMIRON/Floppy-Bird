using System;
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
    static class Obstacle
    {
        const byte OBSTACLE_W = 26;
        const byte OBSTACLE_H = 160;
        static Texture textureU = new Texture(PlayGame.FileSprites, new IntRect(152, 3, OBSTACLE_W, OBSTACLE_H));
        static Texture textureD = new Texture(PlayGame.FileSprites, new IntRect(180, 3, OBSTACLE_W, OBSTACLE_H));


        static Vector2f Pos;

        static Sprite[] Up = new Sprite[5];
        static Sprite[] Down = new Sprite[5];

        static Random random = new Random();

        static Obstacle()
        {
            for (byte i = 0; i < Up.Length; i++)
            {
                Up[i] = new Sprite(textureU);
                Down[i] = new Sprite(textureD);
            }

            Up[0].Scale = new Vector2f(2.5f, 2.5f);
            Down[0].Scale = Up[0].Scale;
            for (byte i = 1; i < Up.Length; i++)
            {
                Up[i].Scale = Up[0].Scale;
                Down[i].Scale = Up[0].Scale;
            }

            Pos.X = PlayGame.Width / 5 - (PlayGame.Width / 5) / 2;
            Up[0].Position = Pos;
            Down[0].Position = Pos;
            for (byte i = 1; i < Down.Length; i++)
            {
                Pos = Up[i - 1].Position;
                Pos.X += PlayGame.Width / 5;

                Up[i].Position = Pos;
                Down[i].Position = Pos;
            }

            for (byte i = 0; i < Down.Length; i++)
            {
                Pos = Up[i].Position;
                Pos.X += PlayGame.Width;
                Up[i].Position = Pos;
                Down[i].Position = Pos;
            }

            for (byte i = 0; i < Down.Length; i++)
            {
                Pos.X = Up[i].Position.X;
                Pos.Y = -(random.Next(0, 40) / 10f) * 100;
                Up[i].Position = Pos;
            }
        }

        public static void DrawObstacle()
        {
            for (byte i = 0; i < Up.Length; i++)
            {
                MovePillars(ref Up[i]);
                Down[i].Position = Pos;

                HeightPillars(ref Up[i]);
                Pos.Y += OBSTACLE_H * 3 + 100;
                Down[i].Position = Pos;

                PlayGame.Window.Draw(Up[i]);
                PlayGame.Window.Draw(Down[i]);
            }
        }

        static void MovePillars(ref Sprite Pillar)
        {
            if(Pillar.Position.X > -OBSTACLE_W * 2.5f)
            {
                Pos = Pillar.Position;
                Pos.X -= 5;
                Pillar.Position = Pos;
            }
        }

        static void HeightPillars(ref Sprite Pillar)
        {
            if (Pillar.Position.X <= -OBSTACLE_W * 2.5f)
            {
                Pos.X = PlayGame.Width;
                Pos.Y = -(random.Next(0, 40) / 10f) * 100;
                Pillar.Position = Pos;
            }
        }
    }
}
