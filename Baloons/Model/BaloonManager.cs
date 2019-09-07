using System;
using System.Windows;

namespace Baloons.Model
{
    public class BaloonManager
    {
        private readonly Random random = new Random();
        private readonly RandomColor randomColor = new RandomColor();

        public double CanvasWidth { get; set; }
        public double CanvasHeight { get; set; }

        public BaloonModel NewBaloon()
        {
            int maxDim = (int)Math.Max(CanvasWidth, CanvasHeight);
            int radius = random.Next(20) + 10;
            return new BaloonModel
            {
                Radius = radius,
                MaxRadius = random.Next(maxDim / 2) + 100,
                Center = new Point(random.Next((int)CanvasWidth - radius * 2) + radius, random.Next((int)CanvasHeight - radius * 2) + radius),
                Color = randomColor.SelectedBrush(),
                TwineColor = randomColor.SelectedBrush()
            };
        }
    }
}
