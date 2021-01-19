using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HW33SpaceShooter.Objects
{
    public abstract class BaseObject : ICollision
    {
        public delegate void Message();

        protected Point Pos;
        protected Point Dir;
        protected Size Size;


        public Point Position
        {
            get { return Pos; }
            set { Pos = value; }
        }

        public BaseObject(Point pos, Point dir, Size size)
        {
            Pos = pos;
            Dir = dir;
            Size = size;
        }

        abstract public void Draw();

        public abstract void Update();

        public Rectangle Rect => new Rectangle(Pos, Size);

        public bool Collision(ICollision obj)
        {
            return obj.Rect.IntersectsWith(this.Rect);
        }
    }
}
