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
    static class Bird
    {
        static Texture texture;
        static Sprite SBird;
        static Vector2f Pos;
        static IntRect SIntRect = new IntRect(381, 187, BIRD_W, BIRD_H);

        static float Gravity = 9.8f;
        static bool Up = false;
        static float LastDownCoordinate;
        static float AnimationCounter = 0.05f;

        public const byte BIRD_W = 17;
        public const byte BIRD_H = 12;
        private const byte SHIFT = 26;

        static Bird()
        {
            texture = new Texture(PlayGame.FileSprites);
            texture.Smooth = true;

            SBird = new Sprite(texture, new IntRect(381, 187, BIRD_W, 64));
            SBird.TextureRect = SIntRect;
            SBird.Scale = new Vector2f(3, 3);
            SBird.Position = new Vector2f(PlayGame.Width / 2, PlayGame.Height / 2);

            Pos = SBird.Position;
        }

        public static void DrawBird()
        {
            WingsFlap();
            Drop();
            Jump();
            PlayGame.Window.Draw(SBird);
        }


        private static void WingsFlap()
        {
            if (AnimationCounter > 0.58)
            {
                SBird.TextureRect = SIntRect;
                SIntRect.Top += (SIntRect.Top != 239) ? SHIFT : -2 * SHIFT;
                AnimationCounter = 0;
            }
            AnimationCounter += 0.05f;
        }

        private static void Jump()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
            {
                if (Up || Pos.Y < 0) return;
                Up = true;
                LastDownCoordinate = Pos.Y;
            }
        }

        private static void Top()
        {
            Pos.Y -= 7f;

            if (SBird.Rotation > 0)
                SBird.Rotation -= 8;
            else if (SBird.Rotation != -10) SBird.Rotation -= 10 - Math.Abs(SBird.Rotation);

            if ((LastDownCoordinate - Pos.Y) > 70)
            {
                Up = false;
                return;
            }
            SBird.Position = Pos;
        }

        private static void Drop()
        {
            if (Up)
            {
                Top();
                return;
            }
            if (Pos.Y >= PlayGame.Height - 43) return;


            if (SBird.Rotation <= -7)
            {
                SBird.Rotation += 1f;
            }
            else if(SBird.Rotation <= 10)
            {
                SBird.Rotation += 1.1f;
                Pos.Y += Gravity / 1.3f;
            }
            else if (SBird.Rotation <= 80)
            {
                SBird.Rotation += 3f;
                Pos.Y += Gravity / 1f;
            }
            else
                Pos.Y += Gravity / 0.95f;

            SBird.Position = Pos;
        }
    }
}
