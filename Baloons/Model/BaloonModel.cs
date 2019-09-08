using System;
using System.Windows;
using System.Windows.Media;
using Toub.Sound.Midi;

namespace Baloons.Model
{
    public class BaloonModel
    {
        const string notes = "CDEFGAB";
        const int startOctave = 2, endOctave = 7;

        public Point Center { get; set; }
        public int Radius { get; set; }
        public int MaxRadius { get; set; }
        public Brush Color { get; set; }
        public Brush TwineColor { get; set; }
        public NoteOn Note => new NoteOn(0, 1, MidiNote(), 127);

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

        private string MidiNote()
        {
            double factor = (endOctave - startOctave + 1.0) * Radius / MaxRadius;
            int octave = (int)Math.Floor(factor) + startOctave;
            int note = (int)Math.Floor((factor - Math.Floor(factor)) * notes.Length);
            string result = $"{notes.Substring(note, 1)}{octave}";
            return result;
        }
    }
}
