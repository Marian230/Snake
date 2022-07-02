using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;

namespace Game
{
    public class GameObject
    {
        public virtual Position Pos { get; set; }

        public string Name { get; set; }
        
        protected virtual string Character { get; set; }

        // Defaut color is null, so my Write method will use default foreGround color
        protected virtual ConsoleColor? Color { get; set; } = null;

        protected Game game => Game.Instance;

        public GameObject((int, int) pos, string name, ConsoleColors newColor = null)
        {
            this.Pos = new Position(pos);
            this.Name = name;
        }

        protected virtual void InitializeObject()
        {
        }

        public virtual void Move()
        {
        }

        public virtual void Draw()
        {
            Drawing.Write(this.Pos, this.Character, this.Color);
        }
    }
}