using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HW33SpaceShooter.Objects
{
    class Bullet : BaseObject
    {
        static Image Image { get; } = Image.FromFile("..\\..\\Images/bullet23.png");

        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {

        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(Image, Pos);
        }

        public override void Update()
        {
            Pos.X += 20;
            if (Pos.X >= Game.Width) Game._objsBullets.RemoveAt(Game._objsBullets.IndexOf(this));
        }
    }
}
