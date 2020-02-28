using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace FlopyBirdGame
{
    static class PlayGame
    {
        public static RenderWindow Window;
        public const ushort HEIGHT = 600;
        public const ushort WIGTH = 1200;
        public const string FileSprites = "Flappy Bird Sprites.png";

        static ResultInGame result = new ResultInGame();
        static ResultInGameOver resultWin = new ResultInGameOver();
        static byte Mode;

        private static void Main()
        {
            Window = new RenderWindow(new VideoMode(WIGTH, HEIGHT), "Flopy Bird");
            Window.SetVerticalSyncEnabled(true);
            Window.Closed += close;
            Window.Clear();

            //Меню игры
            while(Window.IsOpen && Mode == 0)
            {
                Window.DispatchEvents();
                Window.Clear();

                Background.DrawBack();
                Obstacle.DrawGround();
                //Bird.DrawBirdNotDrop();
                Menu.DrawMenu();

                if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
                    Mode = 1;
                if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
                    Mode = 2;

                Window.Display();
            }

            //Геймплей
            while (Window.IsOpen)
            {
                //Игра
                if (Mode == 1)
                {
                    Obstacle.Point.SoundBuffer = Obstacle.PointBuffer;
                    Bird.Wing.SoundBuffer = Bird.WingBuffer;
                    Bird.SDie.SoundBuffer = Bird.DieBuffer;

                    Bird.Pos.X = (WIGTH / 2) - (Bird.BIRD_W * 3);

                    while (Window.IsOpen && !Bird.Die)
                    {
                        Window.DispatchEvents();
                        Window.Clear();

                        Background.DrawBack();
                        Obstacle.DrawObstacle();
                        result.DrawCount(Obstacle.Count);
                        Bird.DrawBird();


                        Window.Display();
                    }
                    Bird.SDie.Play(); 
                }
                else
                {
                    Obstacle.Point.SoundBuffer = Obstacle.FastPointBuffer;
                    Bird.Wing.SoundBuffer = Bird.FastWingBuffer;
                    Bird.SDie.SoundBuffer = Bird.FastDieBuffer;

                    Bird.Pos.X = (WIGTH / 4) - (Bird.BIRD_W * 3);

                    while (Window.IsOpen && !Bird.Die)
                    {
                        Window.DispatchEvents();
                        Window.Clear();

                        Background.DrawBack();
                        Obstacle.DrawOneOstacle();
                        result.DrawCount(Obstacle.Count);
                        Bird.DrawBird();


                        Window.Display();
                    }
                    Bird.SDie.Play();
                }

                //Падение на землю
                while (Window.IsOpen && !Bird.OnGround)
                {
                    Window.DispatchEvents();
                    Window.Clear();

                    Background.DrawBack();
                    Obstacle.DrawNotMove();
                    Bird.DrawBird();

                    Window.Display();
                }

                Mode = 0;

                //Меню результата
                while (Window.IsOpen && Mode == 0)
                {
                    Window.DispatchEvents();
                    Window.Clear();

                    Background.DrawBack();
                    Obstacle.DrawNotMove();
                    Bird.DrawBird();
                    resultWin.DrawResultWindow(Obstacle.Count);

                    if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
                        Mode = 1;
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
                        Mode = 2;

                    Window.Display();
                }

                //Сброс состояния игрока и препядствий
                Bird.ClearBird();
                Obstacle.ObstacleClear();
            }
        }

        private static void close(object sender, EventArgs e)
        {
            Window.Close();
        }
    }
}
