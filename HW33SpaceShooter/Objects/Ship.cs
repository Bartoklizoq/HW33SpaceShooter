using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace HW33SpaceShooter.Objects
{
    class Ship : BaseObject
    {

        static Image Image { get; } = Image.FromFile("..\\..\\Images/roket.png");

        private static int MoveX, MoveY;

        public static int positionX, positionY;

        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size)
        {

        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(Image, Pos.X, Pos.Y);
        }

        public override void Update()
        {
            Pos.X += MoveX;
            Pos.Y += MoveY;
            positionX = Pos.X;
            positionY = Pos.Y;

            if (Pos.X <= 0) Pos.X = 1;
            if (Pos.Y <= 0) Pos.Y = 1;
            if (Pos.X >= Game.Width - 40) Pos.X = Game.Width - 41;
            if (Pos.Y >= Game.Height - 40) Pos.Y = Game.Height - 41;
        }

        public static void UpdateOnKeyDown(object sender, KeyEventArgs e)
        {
            int speed = 10;

            switch (e.KeyData)
            {
                case Keys.Up:
                    MoveY = -speed;
                    break;
                case Keys.Down:
                    MoveY = speed;
                    break;
                case Keys.Left:
                    MoveX = -speed;
                    break;
                case Keys.Right:
                    MoveX = speed;
                    break;
                case Keys.Space:
                    Game._objsBullets.Add(new Bullet(new Point(Ship.positionX + 65, Ship.positionY + 12),
                            new Point(Ship.positionX + 65, Ship.positionY + 32), new Size(3, 3)));
                    Game._events += Game.NewBullet;
                    break;
                default:
                    break;
            }
        }

        public static void UpdateOnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Up:
                    MoveY = 0;
                    break;
                case Keys.Down:
                    MoveY = 0;
                    break;
                case Keys.Left:
                    MoveX = 0;
                    break;
                case Keys.Right:
                    MoveX = 0;
                    break;
                default:
                    break;
            }
        }
    }
}