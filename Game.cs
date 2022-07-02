using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Game
    {
        private Random MyRandom { get; set; } = new Random();

        public Position ConsoleDimensions { get; set; }

        private List<GameObject> GameObjects { get; set; }


        // in milliseconds
        public int RefreshRate { get; set; } = 65;


        public ConsoleKey? Input { get; set; } = null;

        public bool GameIsOver { get; set; } = false;


        ////// singleton //////
        private Game()
        {
            InitializeGame();
        }

        private static readonly Game instance = new ();
        public static Game Instance => Game.instance;
        //////////////////////


        private void InitializeGame()
        {
            // Set colors of console
            Drawing.InitializeConsole();

            this.ConsoleDimensions = new Position(Console.WindowWidth, Console.WindowHeight);

            this.GameObjects = new List<GameObject>()
            {
                new Snake(this.GetNewPos(), "Me")
                {                                               // Get number of items in Direction enum
                    SnakeDir = (Direction)this.MyRandom.Next(0, Enum.GetNames(typeof(Direction)).Length)
                },
                new Apple(this.GetNewPos(), "Food"),
            };
        }

        public void Play()
        {
            do
            {
                this.GetInput();
                this.CheckSnakeDeath();
                this.Move();
                this.Draw();
                this.CheckSnakeContact();
                
            } while (!this.GameIsOver);
        }

        private void GetInput()
        {
            this.Input = Console.KeyAvailable ? Console.ReadKey(true).Key : null;
        }

        private void CheckSnakeContact()
        {
            Apple apple = this.GameObjects.OfType<Apple>().FirstOrDefault() is Apple app
                ? app
                : throw new Exception("Apple is not in the list");

            this.GameObjects
                .OfType<Snake>()
                .Where(snake => snake.Pos == apple.Pos)
                .ToList()
                .ForEach(snake => {
                    apple.ChangePosition();
                    snake.AddJoint();
                });
        }

        private void CheckSnakeDeath()
        {
            this.GameObjects
                .OfType<Snake>()
                .ToList()
                .ForEach(snake => snake.CheckDeath());
        }

        private void Move()
        {
            this.GameObjects.ForEach(x => x.Move());

            this.CheckSnakeContact();
        }

        private void Draw()
        {
            this.GameObjects.ForEach(x => x.Draw());
            
            Thread.Sleep(this.RefreshRate);
            Drawing.Clear();
        }

        public (int, int) GetNewPos()
        {
            int x = this.MyRandom.Next(this.ConsoleDimensions.X);
            int y = this.MyRandom.Next(this.ConsoleDimensions.Y);

            return (x, y);
        }
    }
}
