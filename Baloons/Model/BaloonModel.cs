using System;
using System.Windows;
using System.Windows.Media;

namespace Baloons.Model
{
    public class BaloonModel
    {
        public Point Center { get; set; }
        public int Radius { get; set; }
        public int MaxRadius { get; set; }
        public Brush Color { get; set; }
        public Brush TwineColor { get; set; }

        public event EventHandler BlownUp;

        public void Blow()
        {
            if (Radius > MaxRadius)
            {
                BlownUp?.Invoke(this, null);
            }
            else
            {
                Radius += 5;
            }
        }

        public void Release()
        {
            if (Radius > 5)
            {
                Radius -= 5;
            }
        }

        public void SetCenter(double x, double y)
        {
            Center = new Point(x, y);
        }
    }
}
