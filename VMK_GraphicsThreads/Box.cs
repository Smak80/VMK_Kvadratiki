using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMK_GraphicsThreads
{
    enum Way
    {
        Right, Down, Left, Up
    }
    public class Box
    {
        private int _size;
        private static Size _containerSize;
        private Random rnd = new Random();
        private int shift;
        private bool _stop = false;
        private Way _direction = Way.Right;
        public Color Color { get; set; }
        public int Size
        {
            get => _size;
            set => _size = Math.Min(100, Math.Max(30, value));
        }

        public Point Location { get; set; }

        public static Size ContainerSize
        {
            get => _containerSize;
            set => _containerSize = value;
        }

        public Box()
        {
            Color = Color.FromArgb(180, rnd.Next(255), rnd.Next(255), rnd.Next(255));
            Location = new Point(0, 0);
            Size = rnd.Next(30, 100);
            shift = rnd.Next(1, 4);
        }

        public void Paint(Graphics g)
        {
            Pen p = new Pen(Color.Black);
            Brush b = new SolidBrush(Color);
            g.FillRectangle(b, Location.X, Location.Y, Size, Size);
            g.DrawRectangle(p, Location.X, Location.Y, Size, Size);
        }

        public void Start()
        {
            _stop = false;
            var t = new Thread(() =>
            {
                while (!_stop)
                {
                    int nX;
                    int nY;
                    switch (_direction)
                    {
                        case Way.Right:
                        {
                            nX = Math.Min(ContainerSize.Width - Size, Math.Max(0, Location.X + shift));
                            nY = Location.Y;
                            if (nX == ContainerSize.Width - Size) _direction = Way.Down;
                            break;
                        }
                        case Way.Left:
                        {
                            nX = Math.Min(ContainerSize.Width - Size, Math.Max(0, Location.X - shift));
                            nY = Location.Y;
                            if (nX == 0) _direction = Way.Up;
                            break;
                        }
                        case Way.Down:
                        {
                            nX = Location.X;
                            nY = Math.Min(ContainerSize.Height - Size, Math.Max(0, Location.Y + shift));
                            if (nY == ContainerSize.Height - Size) _direction = Way.Left;
                            break;
                        }
                        default:
                        {
                            nX = Location.X;
                            nY = Math.Min(ContainerSize.Height - Size, Math.Max(0, Location.Y - shift));
                            if (nY == 0) _direction = Way.Right;
                            break;
                        }
                    }

                    Location = new Point(nX, nY);
                    Thread.Sleep(30);
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void Stop()
        {
            _stop = true;
        }
    }
}
