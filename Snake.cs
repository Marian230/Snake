using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Snake : GameObject
    {
        public override Position Pos => SnakeJoints.First();

        private List<Position> SnakeJoints { get; set; } = new List<Position>(1);

        protected override string Character => "0";

        public Direction SnakeDir { get; set; }

        private int SnakeSize { get; set; }

        public Dictionary<ConsoleKey, Direction> KeyMap { get; set; }

        public Snake((int, int) pos, string name) : base(pos, name)
        {
            this.SnakeJoints.Add(new Position(pos));

            this.InitializeObject();
        }

        protected override void InitializeObject()
        {
            this.KeyMap = new Dictionary<ConsoleKey, Direction>()
            {
                { ConsoleKey.W, Direction.Up },
                { ConsoleKey.A, Direction.Left },
                { ConsoleKey.S, Direction.Down },
                { ConsoleKey.D, Direction.Right },
            };
        }
        
        public void AddJoint()
        {
            this.SnakeJoints.Add(new Position(this.SnakeJoints[this.SnakeSize]));
            this.SnakeSize++;
        }

        public override void Move()
        {
            this.MoveAllJoints();
            this.DirectionChange();
            this.MoveSnake();
        }

        public void CheckDeath()
        {
            for (int i = 2; i < this.SnakeJoints.Count; i++)
                if (this.SnakeJoints[0] == this.SnakeJoints[i])
                {
                    this.game.GameIsOver = true;
                    break;
                }
        }

        private void MoveAllJoints()
        {
            for (int i = this.SnakeJoints.Count - 1; i > 0; i--)
                this.SnakeJoints[i].Set(this.SnakeJoints[i - 1].Get());
        }

        private void DirectionChange()
        {                                       // check if key without any binding was pressed
            if (this.game.Input == null || !this.KeyMap.TryGetValue((ConsoleKey)this.game.Input, out _))
                return;

            Direction newDir = this.KeyMap[(ConsoleKey)this.game.Input];

            if      (this.CheckDir(newDir) && newDir == Direction.Left)
                this.SnakeDir = newDir;
            else if (this.CheckDir(newDir) && newDir == Direction.Right)
                this.SnakeDir = newDir;
            else if (this.CheckDir(newDir) && newDir == Direction.Up)
                this.SnakeDir = newDir;
            else if (this.CheckDir(newDir) && newDir == Direction.Down)
                this.SnakeDir = newDir;
        }

        // Preventing snake from going other way around
        private bool CheckDir(Direction dir)
        {
            if      ((dir == Direction.Left) && (this.SnakeDir == Direction.Right))
                return false;
            else if ((dir == Direction.Up) && (this.SnakeDir == Direction.Down))
                return false;
            else if ((dir == Direction.Right) && (this.SnakeDir == Direction.Left))
                return false;
            else if ((dir == Direction.Down) && (this.SnakeDir == Direction.Up))
                return false;

            return true;
        }

        private void MoveSnake()
        {
            if (this.SnakeDir == Direction.Left)
            {
                this.Pos.X--;
                if (this.Pos.X < 0)
                    this.Pos.X = this.game.ConsoleDimensions.X - 1;
            }
            else if (this.SnakeDir == Direction.Right)
            {
                this.Pos.X++;
                if (this.Pos.X >= this.game.ConsoleDimensions.X)
                    this.Pos.X = 0;
            }
            else if (this.SnakeDir == Direction.Up)
            {
                this.Pos.Y--;
                if (this.Pos.Y < 0)
                    this.Pos.Y = this.game.ConsoleDimensions.Y - 1;
            }
            else if (this.SnakeDir == Direction.Down)
            {
                this.Pos.Y++;
                if (this.Pos.Y >= this.game.ConsoleDimensions.Y)
                    this.Pos.Y = 0;
            }
        }

        public override void Draw()
        {
            this.SnakeJoints.ForEach(joint 
                => Drawing.Write(joint, this.Character, this.Color));
        }
    }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
    }
}  