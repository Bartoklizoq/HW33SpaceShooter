using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HW33SpaceShooter.Objects
{
    class SplashScreen : BaseObject
    {
        public SplashScreen(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawString("Автор: Суханов Александр", new Font(FontFamily.GenericSansSerif, 9, FontStyle.Regular), new SolidBrush(Color.Gainsboro), 10, Game.Height - 40);
            Game.Buffer.Graphics.DrawString($"Score: {Game.score}", new Font(FontFamily.GenericSansSerif, 9, FontStyle.Regular), new SolidBrush(Color.Gainsboro), 10, Game.Height - 30);
            Game.Buffer.Graphics.DrawString($"Health: {Game.health}", new Font(FontFamily.GenericSansSerif, 9, FontStyle.Regular), new SolidBrush(Color.Gainsboro), 10, Game.Height - 20);
        }

        public override void Update()
        {
        }
    }
}
