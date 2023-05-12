using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMK_GraphicsThreads
{
    public class Animator
    {
        private List<Box> _boxes;
        private bool _stop = false;
        private Graphics _mainGraphics;
        public Graphics MainGraphics
        {
            get => _mainGraphics;
            set
            {
                _mainGraphics = value;
                ContainerSize = _mainGraphics.VisibleClipBounds.Size.ToSize();
                _bufferedGraphics = BufferedGraphicsManager.Current.Allocate(_mainGraphics, new Rectangle(new Point(0, 0), ContainerSize));
            }
        }

        private BufferedGraphics _bufferedGraphics;
        private Graphics _bg => _bufferedGraphics.Graphics;

        public Size ContainerSize
        {
            get => Box.ContainerSize;
            set => Box.ContainerSize = value;
        }

        public Animator(Graphics g)
        {
            MainGraphics = g;
            _boxes = new List<Box>();
        }

        public void AddNewBox()
        {
            var b = new Box();
            _boxes.Add(b);
            b.Start();
        }

        public void Start()
        {
            _stop = false;
            var t = new Thread(() =>
            {
                while (!_stop)
                {
                    CreateFrame(_bg);
                    try
                    {
                        _bufferedGraphics.Render();
                    }
                    catch
                    {
                    }
                    Thread.Sleep(30);
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        private void CreateFrame(Graphics g)
        {
            g.Clear(Color.White);
            var b = new List<Box>(_boxes);
            foreach (var box in b)
            {
                box.Paint(g);
            }
        }

        public void Stop()
        {
            _stop = true;
        }
    }
}
