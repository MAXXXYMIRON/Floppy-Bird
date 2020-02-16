using SFML.Graphics;
using SFML.System;

namespace FlopyBirdGame
{
    static class Background
    {
        static Texture tex1;
        static Sprite FirstBackground;
        static Sprite SecondBackground;

        static Vector2f Pos;
        static Background()
        {
            tex1 = new Texture(PlayGame.FileSprites, new IntRect(3, 0, 143, 255));
            tex1.Smooth = true;

            FirstBackground = new Sprite(tex1);
            FirstBackground.Scale = new Vector2f((float)PlayGame.WIGTH / 143, (float)PlayGame.HEIGHT / 255);

            SecondBackground = new Sprite(tex1);
            SecondBackground.Scale = FirstBackground.Scale;
        }

        public static void DrawBack()
        {
            TransPosition();
            PlayGame.Window.Draw(FirstBackground);
            PlayGame.Window.Draw(SecondBackground);
        }

        static void TransPosition()
        {
            Pos.X = (Pos.X == -PlayGame.WIGTH) ? 0 : Pos.X;

            Pos.X -= 5;
            FirstBackground.Position = Pos;
            
            Pos.X += PlayGame.WIGTH;
            SecondBackground.Position = Pos;

            Pos.X -= PlayGame.WIGTH;
        }
    }
}
