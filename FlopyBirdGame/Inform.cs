using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Numerics;


namespace FlopyBirdGame
{
    static class Menu
    {
        static Texture texC = new Texture(PlayGame.FileSprites, new IntRect(152, 200, 89, 24));
        static Sprite Called;

        static Texture texR = new Texture(PlayGame.FileSprites, new IntRect(384, 73, 34, 9));
        static Sprite Run;

        static Texture texGR = new Texture(PlayGame.FileSprites, new IntRect(254, 71, 92, 25));
        static Sprite GetReady;

        static Color Clarity;
        static bool Definition = true;

        static Menu()
        {
            Called = new Sprite(texC);
            Called.Scale = new Vector2f(4, 4);
            Called.Position = new Vector2f((PlayGame.WIGTH / 2) - ((89 * 4) / 2) - 25, (PlayGame.HEIGHT / 2) - ((24 * 4) / 2) - 200);

            Run = new Sprite(texR);
            Run.Scale = new Vector2f(5, 5);
            Run.Position = new Vector2f((PlayGame.WIGTH / 2) - ((34 * 5) / 2) - 25, ((PlayGame.HEIGHT / 2) - (9 * 5) / 2) + 200);

            texGR.Smooth = true;
            GetReady = new Sprite(texGR);
            GetReady.Position = new Vector2f((PlayGame.WIGTH / 2) - (92 / 2) - 25, ((PlayGame.HEIGHT / 2) - (25 / 2) + 200));

        }

        public static void DrawMenu()
        {
            PlayGame.Window.Draw(Called);
            PlayGame.Window.Draw(Run); 
            PlayGame.Window.Draw(GetReady);
            Blinking();
        }

        //Мигание пробела
        private static void Blinking()
        {
            Clarity = GetReady.Color;
            if(Definition)
            {
                Clarity.A -= 5;
                if (Clarity.A == 100) Definition = false;
                GetReady.Color = Clarity;

                Clarity = Run.Color;
                Clarity.A -= 5;
                Run.Color = Clarity;
            }
            else
            {
                Clarity.A += 5;
                if (Clarity.A == 255) Definition = true;
                GetReady.Color = Clarity;

                Clarity = Run.Color;
                Clarity.A += 5;
                Run.Color = Clarity;
            }
        }
    }


    class Result
    {
        Vector2f Pos = new Vector2f(PlayGame.WIGTH / 10, PlayGame.HEIGHT / 10);
        ushort countDigits;
        BigInteger EarlyCount = 1;
        protected Sprite[] Numbers = new Sprite[10];
        
        protected void DrawResult(Sprite[] numbers, BigInteger count, ushort widthNumber)
        {
            if (count - EarlyCount == 0)
            {
                 EarlyCount *= 10;
                countDigits++;
            }

            int index = 0;
            Pos.X = PlayGame.WIGTH / 10 + (countDigits * (widthNumber + 5));
            for (ushort i = 0; i < countDigits; i++)
            {
                index = (int)(count % 10);
                count /= 10;
                numbers[index].Position = Pos;
                PlayGame.Window.Draw(numbers[index]);
                Pos.X -= widthNumber + 5;
            }
            Pos.X = PlayGame.WIGTH / 10;
        }
    }

    class ResultInGame : Result
    {
        const byte WIGTH = 12;
        const byte HEIGHT = 18;

        public ResultInGame()
        {
            Numbers[0] = new Sprite(new Texture(PlayGame.FileSprites, new IntRect(254, 98, WIGTH, HEIGHT)));
            Numbers[1] = new Sprite(new Texture(PlayGame.FileSprites, new IntRect(238, 80, WIGTH, HEIGHT)));
            for (int i = 2; i < 6; i++)
                Numbers[i] = new Sprite(new Texture(PlayGame.FileSprites, new IntRect(325 + ((i - 2) * 14), 148, WIGTH, HEIGHT)));
            for (int i = 6; i < 10; i++)
                Numbers[i] = new Sprite(new Texture(PlayGame.FileSprites, new IntRect(325 + ((i - 6) * 14), 172, WIGTH, HEIGHT)));

            Numbers[0].Scale = new Vector2f(2, 2);
            for (int i = 1; i < Numbers.Length; i++)
            {
                Numbers[i].Scale = Numbers[0].Scale;
            }
        }

        public void DrawCount(BigInteger Count)
        {
            DrawResult(Numbers, Count, WIGTH * 2);
        }
    }

    class ResultInGameOver : Result
    {
        const byte WIGTH = 6;
        const byte HEIGHT = 7;

        public ResultInGameOver()
        {
            for (int i = 0; i < Numbers.Length; i++)
                if (i % 2 == 0)
                    Numbers[i] = new Sprite(new Texture(PlayGame.FileSprites, new IntRect(279 + ((i / 2) * 9), 171, WIGTH, HEIGHT)));
                else
                    Numbers[i] = new Sprite(new Texture(PlayGame.FileSprites, new IntRect(279 + (((i - 1) / 2) * 9), 180, WIGTH, HEIGHT)));
        }

        public void DrawCount()
        {

        }
    }
}

