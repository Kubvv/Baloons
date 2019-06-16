using System;
using System.Windows;
using System.Windows.Media;

namespace Baloons.Model
{
    public class BaloonModel
    {
        readonly int maxRadius;

        public Point Center { get; private set; }
        public int Radius { get; private set; }
        public Brush Color { get; private set; }
        public Brush TwineColor { get; private set; }

        public event EventHandler BlownUp;

        public BaloonModel(double width, double height)
        {
            Random random = new Random();
            maxRadius = random.Next((int)Math.Min(width, height)) / 2 + 40;
            Radius = random.Next(20) + 10;
            Center = new Point(random.Next((int)width - Radius * 2) + Radius, random.Next((int)height - Radius * 2) + Radius);
            Color = new SolidColorBrush(System.Windows.Media.Color.FromArgb((byte)(random.Next(128) + 128), (byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)));
            TwineColor = new SolidColorBrush(System.Windows.Media.Color.FromArgb((byte)(random.Next(128) + 128), (byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)));
        }

        public void Blow()
        {
            if (Radius > maxRadius)
            {
                BlownUp?.Invoke(this, null);
            }
            else
            {
                Radius += 2;
            }
        }

        public void Release()
        {
            if (Radius > 2)
            {
                Radius -= 2;
            }
        }

        internal void SetCenter(double x, double y)
        {
            Center = new Point(x, y);
        }
    }
}
