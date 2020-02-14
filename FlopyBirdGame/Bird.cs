using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using static System.Math;
using static FlopyBirdGame.Obstacle;

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

        static Vector2f PointTopRight;
        static Vector2f PointDownRight;
        static Vector2f PointDown;
        static float Rotation;
        const float GRADE = (float)(180 / PI);

        public const byte BIRD_W = 17;
        public const byte BIRD_H = 12;
        const byte SHIFT = 26;

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
            Swing();
            Collision();
            PlayGame.Window.Draw(SBird);
        }

        static void WingsFlap()
        {
            if (AnimationCounter > 0.58)
            {
                SBird.TextureRect = SIntRect;
                SIntRect.Top += (SIntRect.Top != 239) ? SHIFT : -2 * SHIFT;
                AnimationCounter = 0;
            }
            AnimationCounter += 0.05f;
        }

        //При нажатии на клавишу, несколько циклов игры, происходит 
        //измение координаты по y и наклонноса, вверх
        static void Swing()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
            {
                if (Up || Pos.Y < 0) return;
                Up = true; 
                LastDownCoordinate = Pos.Y;
            }
        }

        //Увеличениие высоты на 70 пикселей
        static void Flight()
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
            
        //Усеньшение высоты и наклон вниз
        static void Drop()
        {
            if (Up)
            {
                Flight();
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


        static void Collision()
        {
            if (PosObs.X == 0)
            {
                SBird.Scale = new Vector2f(3, 3);
                return;
            }

             if (ClashOccured(PosObs.Y - 190, PosObs.Y))
            {
                SBird.Scale = new Vector2f(5, 5);
            }
        }

        static bool ClashOccured(float yTop, float yDown)
        {
            PointTopRight = SBird.Position;
            PointDownRight = SBird.Position;
            PointDown = SBird.Position;

            Rotation = (SBird.Rotation < 0) ? -SBird.Rotation : 360 - SBird.Rotation;

            PointTopRight.X += BIRD_W * (float)Cos(Rotation) * GRADE;
            PointDownRight.X += (float)Sqrt((BIRD_W * BIRD_W) + (BIRD_H * BIRD_H)) * (float)Cos(Rotation) * GRADE;
            PointDown.X += BIRD_H * (float)Cos(Rotation) * GRADE;

            PointTopRight.Y += BIRD_W * (float)Sin(Rotation) * GRADE;
            PointDownRight.Y += (float)Sqrt((BIRD_W * BIRD_W) + (BIRD_H * BIRD_H)) * (float)Sin(Rotation) * GRADE;
            PointDown.Y += BIRD_H * (float)Sin(Rotation) * GRADE;

            return false;
        }
    }
}
