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

        static float Time;
        static Clock clock = new Clock();

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
            if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
            {
                if (Up || Pos.Y < 0) return;
                Up = true;
                LastDownCoordinate = Pos.Y;
            }
        }

        private static void Drop()
        {
            if (Up)
            {
                Top();
                return;
            }
            if (Pos.Y >= PlayGame.Height - 43) return;

            if (SBird.Rotation <= 80)
            {
                SBird.Rotation += 5;
                Pos.Y += Gravity / 1.4f;
            }
            else
                Pos.Y += Gravity / 1.2f;

            SBird.Position = Pos;
        }

        private static void Top()
        {
            Pos.Y -= 8f;
            SBird.Rotation -= (SBird.Rotation > -20) ? 20 : 0;

            if ((LastDownCoordinate - Pos.Y) > 100)
            {
                Up = false;
                return;
            }
            SBird.Position = Pos;
        }
    }
}
