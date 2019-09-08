using System;
using System.Windows;
using Toub.Sound.Midi;

namespace Baloons.Model
{
    public class BaloonManager
    {
        private readonly Random random = new Random();
        private readonly RandomColor randomColor = new RandomColor();
        private readonly RandomMusic randomMusic = new RandomMusic();

        public double CanvasWidth { get; set; }
        public double CanvasHeight { get; set; }

        public Uri RandomSound => randomMusic.RandomSound();

        public BaloonManager()
        {
            MidiPlayer.OpenMidi();
        }

        ~BaloonManager()
        {
            MidiPlayer.CloseMidi();
        }

        public BaloonModel NewBaloon()
        {
            int maxDim = (int)Math.Max(CanvasWidth, CanvasHeight);
            int radius = 10;
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
