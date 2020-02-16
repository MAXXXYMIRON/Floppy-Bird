﻿using System;
using SFML.Graphics;
using SFML.System;
using System.Numerics;

namespace FlopyBirdGame
{
    static class Obstacle
    {
        //Просто земля
        static Texture textureG = new Texture(PlayGame.FileSprites, new IntRect(215, 10, 168, 56));
        static Sprite Ground;
        static Vector2f PosG;
        
        //Препядствия
        const byte OBSTACLE_W = 26;
        const byte OBSTACLE_H = 160;
        static Texture textureU = new Texture(PlayGame.FileSprites, new IntRect(152, 3, OBSTACLE_W, OBSTACLE_H));
        static Texture textureD = new Texture(PlayGame.FileSprites, new IntRect(180, 3, OBSTACLE_W, OBSTACLE_H));
        static Sprite[] Up = new Sprite[4];
        static Sprite[] Down = new Sprite[4];

        //Позиция предствия на игрком
        public static Vector2f PosCenterObs { get; private set; } = new Vector2f(0, 0);
        //Кол-во препядствий прошедших над игроком
        public static BigInteger Count = 0;

        //Разъемы для прохождения препядствия
        static Vector2f Pos;
        static Random random = new Random();
        static byte RandTop = 10, RandDown = 35;

        static Obstacle()
        {
            textureD.Smooth = true;
            textureU.Smooth = true;
            textureG.Smooth = true;

            Ground = new Sprite(textureG);
            Ground.Scale = new Vector2f(((float)PlayGame.WIGTH / 168) * 2, 2);
            PosG.Y = PlayGame.HEIGHT - 50;
            Ground.Position = PosG;

            //Наложить текстуры на спрайты препядствий
            for (byte i = 0; i < Up.Length; i++)
            {
                Up[i] = new Sprite(textureU);
                Down[i] = new Sprite(textureD);
            }

            //Увеличить размер текстур в 2.5 раза
            Up[0].Scale = new Vector2f(2.5f, 2.5f);
            Down[0].Scale = Up[0].Scale;
            for (byte i = 1; i < Up.Length; i++)
            {
                Up[i].Scale = Up[0].Scale;
                Down[i].Scale = Up[0].Scale;
            }

            //Задать расстояние м/у препядствиями
            Pos.X = PlayGame.WIGTH / 4 - (PlayGame.WIGTH / 4) / 2;
            Up[0].Position = Pos;
            Down[0].Position = Pos;
            for (byte i = 1; i < Down.Length; i++)
            {
                Pos = Up[i - 1].Position;
                Pos.X += PlayGame.WIGTH / 4;

                Up[i].Position = Pos;
                Down[i].Position = Pos;
            }

            //Сдвинуть за пределы экрана препядствия
            //Чтобы они не появились на экране сразу
            for (byte i = 0; i < Down.Length; i++)
            {
                Pos = Up[i].Position;
                Pos.X += PlayGame.WIGTH;
                Up[i].Position = Pos;
                Down[i].Position = Pos;
            }

            //Задать область для прохождения препядствия
            for (byte i = 0; i < Down.Length; i++)
            {
                Pos.X = Up[i].Position.X;
                Pos.Y = -(random.Next(RandTop, RandDown) / 10f) * 100;
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
                //Спустить нижнее препядствие относительно верхнего
                Pos.Y += (OBSTACLE_H * 2.5f) + 190;
                Down[i].Position = Pos;

                if(!Bird.Die) CenterObstacle(i);

                PlayGame.Window.Draw(Up[i]);
                PlayGame.Window.Draw(Down[i]);
            }
            if (PosG.X == -PlayGame.WIGTH) PosG.X = 0;
            PosG.X -= 5;
            Ground.Position = PosG;
            PlayGame.Window.Draw(Ground);
        }

        //Сдвинуть препядствие
        static void MovePillars(ref Sprite Pillar)
        {
            if(Pillar.Position.X > -OBSTACLE_W * 2.5f)
            {
                Pos = Pillar.Position;
                Pos.X -= 5;
                Pillar.Position = Pos;
            }
        }

        //Задать область для прохождения препядствия,
        //если препядствие за пределами видимости
        //Вернуть препядствие в начало пути
        static void HeightPillars(ref Sprite Pillar)
        {
            if (Pillar.Position.X <= -OBSTACLE_W * 2.5f)
            {
                Pos.X = PlayGame.WIGTH;
                Pos.Y = -(random.Next(RandTop, RandDown) / 10f) * 100;
                Pillar.Position = Pos;
            }
        }

        //Возвращать позицию препядствия, которое проходит мимо игрока
        private static void CenterObstacle(byte i)
        {
            if (Up[i].Position.X <= PlayGame.WIGTH / 2 && Up[i].Position.X > ((PlayGame.WIGTH / 2) - ((OBSTACLE_W * 2.5f) + 50)))
            {
                PosCenterObs = Down[i].Position;
                return;
            }
            else if (Up[i].Position.X == ((PlayGame.WIGTH / 2) - ((OBSTACLE_W * 2.5f) + 50)))
            {
                Count++;
                PosCenterObs -= PosCenterObs;
                return;
            }
        }
    }
}
