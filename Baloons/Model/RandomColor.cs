using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Baloons.Model
{
    public class RandomColor
    {
        private readonly Random random = new Random();
        private readonly List<Color> selectedColors;

        public RandomColor()
        {
            selectedColors = new List<Color>
            {
                Colors.Aqua,
                Colors.Beige,
                Colors.Blue,
                Colors.Brown,
                Colors.Crimson,
                Colors.DarkGreen,
                Colors.DarkRed,
                Colors.DeepPink,
                Colors.DeepSkyBlue,
                Colors.ForestGreen,
                Colors.Green,
                Colors.Gold,
                Colors.Indigo,
                Colors.Lavender,
                Colors.LightBlue,
                Colors.LightGreen,
                Colors.LightSkyBlue,
                Colors.Lime,
                Colors.Magenta,
                Colors.Olive,
                Colors.Orange,
                Colors.Pink,
                Colors.Purple,
                Colors.Red,
                Colors.RoyalBlue,
                Colors.Salmon,
                Colors.Silver,
                Colors.SkyBlue,
                Colors.Turquoise,
                Colors.Violet,
                Colors.Yellow
            };
        }

        public Color Next()
        {
            return Color.FromArgb(RandomOpacity, (byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
        }

        public Color SelectedNext()
        {
            int randomColor = random.Next(selectedColors.Count);
            Color color = selectedColors[randomColor];
            color.A = RandomOpacity;
            return color;
        }

        public byte RandomOpacity => (byte)(random.Next(128) + 128);
    }
}
