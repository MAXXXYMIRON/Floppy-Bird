using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using static System.Math;
using static FlopyBirdGame.Obstacle;
using System.Numerics;

namespace FlopyBirdGame
{
    static class Bird
    {
        //Вывод картинки
        static Texture texture;
        static Sprite SBird;
        static IntRect SIntRect = new IntRect(381, 187, BIRD_W, BIRD_H);

        //Анимация падения, взмахов и поворотов
        static float Gravity = 9.8f;
        static bool Up = false;//Флаг для указания взлета или падения
        static float LastDownCoordinate;//Последняя координата по Y перед взлетом
        static float AnimationCounter = 0.05f;//Скорость анимации
        const byte SHIFT = 26;//Для анимации взмахов
        static Vector2f Pos;//Для изменения позиции

        //Поиск коллизий
        static float PointTopRight;//Координата по Y правой верхней точки
        static float PointDownRight;//Координата по Y правой нижней точки
        static float PointDown;//Координата по Y нижней точки
        static float Rotation;//Угол поворота спрайта
        const float GRADE = (float)(PI / 180); //Константа для перевода в радианы
        const byte BIRD_W = 17; //Ширина спрайта
        const byte BIRD_H = 12; //Высота спрайта
        static readonly float BIRD_D = (float)Sqrt((BIRD_W * BIRD_W) + (BIRD_H * BIRD_H));//Диагональ

        //Геймплей
        public static bool Die;
        public static BigInteger Count = 0;
        

        static Bird()
        {
            texture = new Texture(PlayGame.FileSprites);
            texture.Smooth = true;

            SBird = new Sprite(texture, new IntRect(381, 187, BIRD_W, 64));
            SBird.TextureRect = SIntRect;
            SBird.Scale = new Vector2f(3, 3);
            SBird.Position = new Vector2f((PlayGame.Width / 2) - (BIRD_W * 3), PlayGame.Height / 2);

            Pos = SBird.Position;
        }

        public static void DrawBird()
        {
            Collision();
            Drop();
            if (!Die)
            {
                WingsFlap();
                Swing();
            }
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
        //измение координаты по Y и наклона носа вверх
        static void Swing()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
            {
                if (Up || SBird.Position.Y < 50) return;
                Up = true; 
                LastDownCoordinate = Pos.Y;
            }
        }

        //Увеличениие высоты на 70 пикселей
        //подъем носа вверх
        static void Flight()
        {
            Pos.Y -= 7f;

            if (SBird.Rotation > 0)
                SBird.Rotation -= 8;
            else if (SBird.Rotation != -10) SBird.Rotation -= 10 - Abs(SBird.Rotation);

            //Перестать подниматься при изменении высоты на 70
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
            //Если мы поднимаемся, то не выполнять падение
            if (Up)
            {
                Flight();
                return;
            }

            PointsCoordinate("PDR");
            if (PointDownRight >= PlayGame.Height - 40)
            {
                Die = true;
                return;
            }

            //Падение
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

        //Проверка на сотлкновение с препядствием
        static void Collision()
        {
            if (PosObs.X == 0 || Die) return;
            if (ClashOccured(PosObs.Y - 190, PosObs.Y))
                 Die = true;
        }

        //Поиск точки задевшей препядствие
        static bool ClashOccured(float yTop, float yDown)
        {
            PointsCoordinate();

             if (SBird.Position.Y < yTop || SBird.Position.Y > yDown) return true;
            if (PointTopRight < yTop || PointTopRight > yDown) return true;
            if (PointDownRight < yTop || PointDownRight > yDown) return true;
            if (PointDown < yTop || PointDown > yDown) return true;

            return false;
        }

        //Поиск координат вершин спрайта по Y
        static void PointsCoordinate()
        {
            PointTopRight = SBird.Position.Y;
            PointDownRight = PointTopRight;
            PointDown = PointTopRight;

            Rotation = (SBird.Rotation < 0) ? 360 - SBird.Rotation : SBird.Rotation;

            PointTopRight += BIRD_W * (float)Sin(Rotation * GRADE);
            PointDownRight += BIRD_D * (float)Sin((Rotation + 35.5f) * GRADE);
            PointDown += BIRD_H * (float)Sin((Rotation + 90) * GRADE);
        }

        //Поиск координат вершин спрайта по Y
        //PTR - правая верхняя
        //PDR - правая нижняя
        //PD - нижняя
        static void PointsCoordinate(string Dot)
        {
            switch (Dot)
            {
                case ("PTR"):
                    {
                        Rotation = (SBird.Rotation < 0) ? 360 - SBird.Rotation : SBird.Rotation;
                        PointTopRight = SBird.Position.Y;
                        PointTopRight += BIRD_W * (float)Sin(Rotation * GRADE);
                    }
                    break;
                case ("PDR"):
                    {
                        Rotation = (SBird.Rotation < 0) ? 360 - SBird.Rotation : SBird.Rotation;
                        PointDownRight = SBird.Position.Y;
                        PointDownRight += BIRD_D * (float)Sin((Rotation + 35.5f) * GRADE);
                    }
                    break;
                case ("PD"):
                    {
                        Rotation = (SBird.Rotation < 0) ? 360 - SBird.Rotation : SBird.Rotation;
                        PointDown = SBird.Position.Y;
                        PointDown += BIRD_H * (float)Sin((Rotation + 90) * GRADE);
                    }
                    break;
            }

        }
    }
}
