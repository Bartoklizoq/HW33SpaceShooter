using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using HW33SpaceShooter.Objects;

namespace HW33SpaceShooter
{
    static class Game
    {

        public static int gameRound = 0;
        public static int score;
        public static int health;
        public const int baseHealth = 100;


        public static event Action<string> _events;


        static BufferedGraphicsContext context;
        static public BufferedGraphics Buffer { get; private set; }



        static public Random Random { get; } = new Random();
        static public int Width { get; set; }
        static public int Height { get; set; }
        static public Image background = Image.FromFile("..\\..\\Images/sasa.jpg");
        public static Timer timer = new Timer { Interval = 30 };



        public static int Speed
        {
            get { return Random.Next(1, 6); }
        }

        static Game()
        {
            Random = new Random();
        }


        public static int StartX
        {
            get { return Width; }
        }


        public static int StartY
        {
            get { return Random.Next(20, Height - 40); }
        }

        static public void Init(Form form)
        {

            Graphics g;

            context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();

            Width = form.Width - 40;
            Height = form.Height - 60;

            Buffer = context.Allocate(g, new Rectangle(10, 10, Width, Height));

            _events += Status;

            Load();
        }



        public static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }


        public static List<BaseObject> _objsShip;
        public static List<BaseObject> _objsStar;
        public static List<BaseObject> _objsInteraction;
        public static List<BaseObject> _objsAsteroids;
        public static List<BaseObject> _objsBullets;
        public static List<BaseObject> _objsHUD;

        static public void Load()
        {
            score = 0;
            health = baseHealth;

            _objsShip = new List<BaseObject>();
            AddShip();

            _objsStar = new List<BaseObject>();
            AddStar(15);

            _objsAsteroids = new List<BaseObject>();
            AddAsteroid(8);

            _objsBullets = new List<BaseObject>();

            _objsInteraction = new List<BaseObject>();

            AddHealth(1);

            _objsHUD = new List<BaseObject>();
            _objsHUD.Add(new SplashScreen(new Point(0, 0), new Point(0, 0), new Size(0, 0)));
        }

        static public void Draw()
        {

            Buffer.Graphics.DrawImage(background, 0, 0);

            foreach (BaseObject obj in _objsShip)
                obj.Draw();

            foreach (BaseObject obj in _objsStar)
                obj.Draw();

            foreach (BaseObject obj in _objsAsteroids)
                obj.Draw();

            foreach (BaseObject obj in _objsInteraction)
                obj.Draw();

            foreach (BaseObject obj in _objsBullets)
                obj.Draw();

            foreach (BaseObject obj in _objsHUD)
                obj.Draw();

            Buffer.Render();
        }





        static public void Update()
        {

            foreach (BaseObject obj in _objsStar)
                obj.Update();

            foreach (BaseObject obj in _objsShip)
                obj.Update();

            foreach (BaseObject obj in _objsAsteroids)
                obj.Update();

            foreach (BaseObject obj in _objsInteraction)
                obj.Update();

            try
            {
                foreach (BaseObject obj in _objsInteraction)
                {
                    obj.Update();

                    // Bullets & Interaction objects like Health
                    foreach (BaseObject bullet in _objsBullets)
                        if (obj.Collision(bullet))
                        {
                            _objsInteraction.RemoveAt(_objsInteraction.IndexOf(obj));
                            _objsBullets.RemoveAt(_objsBullets.IndexOf(bullet));
                            _events += Attack;
                            score++;

                            if (obj is Health)
                                AddHealth(1);

                            _events += ObjectAdded;
                        }

                    // Ship & Interaction objects like Health
                    foreach (BaseObject ship in _objsShip)
                        if (obj.Collision(ship))
                        {
                            _objsInteraction.RemoveAt(_objsInteraction.IndexOf(obj));
                            if (obj is Health)
                            {
                                health += 10;
                                _events += HealthIncrease;
                                if (health > baseHealth) health = baseHealth;
                                AddHealth(1);
                            }

                            if (health <= 0)
                                GameOver();

                            score++;
                        }
                }

                foreach (BaseObject obj in _objsAsteroids)
                {
                    obj.Update();

                    // Bullets & Asteroids
                    foreach (BaseObject bullet in _objsBullets)
                        if (obj.Collision(bullet))
                        {
                            _objsAsteroids.RemoveAt(_objsAsteroids.IndexOf(obj));
                            _objsBullets.RemoveAt(_objsBullets.IndexOf(bullet));
                            _events += Attack;
                            score++;

                            if (_objsAsteroids.Count == 0)
                                AddAsteroid(8 + gameRound);
                        }

                    // Ship & Asteroids
                    foreach (BaseObject ship in _objsShip)
                        if (obj.Collision(ship))
                        {
                            _objsAsteroids.RemoveAt(_objsAsteroids.IndexOf(obj));
                            health -= 50;
                            _events += Damage;

                            if (_objsAsteroids.Count == 0)
                                AddAsteroid(8 + gameRound);

                            if (health <= 0)
                                GameOver();
                            score++;
                        }
                }
                foreach (BaseObject obj in _objsBullets)
                    obj.Update();

            }
            catch (Exception e)
            {
                // Console.WriteLine(e);
                // throw;
            }
        }

        public static void AddAsteroid(int n)
        {
            if (_objsAsteroids.Count == 0)
                for (int i = 0; i < n; i++)
                {
                    _objsAsteroids.Add(new Asteroid(new Point(StartX, StartY),
                    new Point(Random.Next(Speed), 0),
                    new Size(20, 21)));
                }
            _events += ObjectAdded;
            gameRound++;
        }



        public static void AddStar(int n)
        {
            for (int i = 0; i < n; i++)
            {
                _objsStar.Add(new Star(new Point(StartX, StartY),
                    new Point(Random.Next(Speed), 0),
                    new Size(3, 3)));
            }
            _events += ObjectAdded;
        }


        public static void AddShip()
        {
            _objsShip.Add(new Ship(new Point(20, Height / 2), new Point(0, 0), new Size(50, 26)));
            _events += ObjectAdded;
        }

        public static void AddHealth(int n)
        {
            for (int i = 0; i < n; i++)
            {
                _objsInteraction.Add(new Health(new Point(StartX, StartY),
                    new Point(Random.Next(Speed), 0),
                    new Size(43, 43)));
            }
            _events += ObjectAdded;
        }

        
        public static void ObjectAdded(string message)
        {
            Console.WriteLine($"{DateTime.Now}: {message} added");
        }

        
        public static void Status(string message)
        {
            Console.WriteLine($"{DateTime.Now}: {message} current transport health: ");
        }

        public static void NewBullet(string message)
        {
            Console.WriteLine($"{DateTime.Now}: {message} new bullet");
        }

        public static void TransportDied(string message)
        {
            Console.WriteLine($"{DateTime.Now}: {message} transport died");
        }

        public static void Attack(string message)
        {
            Console.WriteLine($"{DateTime.Now}: {message} attack on object, score: {score}");
        }

        public static void HealthIncrease(string message)
        {
            Console.WriteLine($"{DateTime.Now}: {message} transport healthy increased, current health: {health}");
        }

        public static void Damage(string message)
        {
            Console.WriteLine($"{DateTime.Now}: {message} transport damaged, current health: {health}");
        }



        public static void GameOver()
        {
            _events += TransportDied;
            _objsShip.Clear();
            gameRound = 0;
            Form.ActiveForm.Close();
        }
    }
}
