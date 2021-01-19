using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HW33SpaceShooter.Objects
{
    class Health : BaseObject
    {
        static Image Image { get; } = Image.FromFile("..\\..\\Images/firstap.png");

        public Health(Point pos, Point dir, Size size) : base(pos, dir, size)
        {

        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(Image, Pos);
        }

        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            // Pos.Y = Pos.Y + Dir.Y;

            if (Pos.X <= 0 || Pos.X >= Game.Width)
            {
                Pos.X = Game.StartX;
                Pos.Y = Game.StartY;
                do
                {
                    Dir.X = Game.Random.Next(-Game.Speed, Game.Speed);
                } while (Dir.X == 0 && Dir.Y == 0);
            }
        }
    }
}
