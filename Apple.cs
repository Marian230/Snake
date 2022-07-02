using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Apple : GameObject
    {
        protected override string Character => "*";
        
        public Apple((int, int) pos, string name) : base(pos, name)
        {
        }

        public void ChangePosition()
        {
            this.Pos = new Position(this.game.GetNewPos());
        }
    }
}