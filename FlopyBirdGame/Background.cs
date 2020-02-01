using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace FlopyBirdGame
{
    static class Background
    {
        static Texture Back;
        static Sprite FirstBack;
        static Sprite SecondBack;

        static Vector2f Pos;

        const ushort BACK_W = 143;
        const ushort BACK_H = 255;

        static Background()
        {
            Back = new Texture(PlayGame.FileSprites, new IntRect(3, 0, BACK_W, BACK_H));
            Back.Smooth = true;

            FirstBack = new Sprite(Back);
            FirstBack.Scale = new Vector2f((float)PlayGame.Width / BACK_W, (float)PlayGame.Height / BACK_H);

            SecondBack = new Sprite(Back);
            SecondBack.Scale = new Vector2f((float)PlayGame.Width / BACK_W, (float)PlayGame.Height / BACK_H);

            Pos = FirstBack.Position;
        }

        public static void DrawBack()
        {
            TransPosition();
            PlayGame.Window.Draw(FirstBack);
            PlayGame.Window.Draw(SecondBack);
        }

        static void TransPosition()
        {
            Pos.X = (Pos.X == -PlayGame.Width) ? 0 : Pos.X;

            Pos.X -= 5;
            FirstBack.Position = Pos;

            Pos.X += PlayGame.Width;
            SecondBack.Position = Pos;

            Pos = FirstBack.Position;
        }
    }
}
