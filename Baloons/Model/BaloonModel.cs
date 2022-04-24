using System;
using System.Windows;
using System.Windows.Media;

namespace Baloons.Model
{
    public class BaloonModel
    {
        private const int initialRadius = 10;
        private const int stepRadius = 5;

        private const string notes = "CDEFGAB";
        private readonly byte[] notesIds = new byte[] { 0, 2, 4, 5, 7, 9, 11 };
        private const int startOctave = 4, endOctave = 6;
        private const byte startOctaveId = 60;
        private const byte notesInOctave = 12;

        private readonly Random random = new();
        private readonly RandomColor randomColor = new();

        public Point Center { get; set; }
        public int Radius { get; set; }
        public int MaxRadius { get; set; }
        public Brush Color { get; set; }
        public Brush TwineColor { get; set; }
        public string Note => MidiNote();
        public byte NoteId => MidiNoteId();

        public event EventHandler? BlownUp;

        public BaloonModel(double canvasWidth, double canvasHeight)
        {
            int maxDim = (int)Math.Max(canvasWidth, canvasHeight);

            Radius = initialRadius;
            MaxRadius = random.Next(maxDim / 2) + 100;
            Center = new Point(random.Next((int)canvasWidth - initialRadius * 2) + initialRadius, random.Next((int)canvasHeight - initialRadius * 2) + initialRadius);
            Color = new SolidColorBrush(randomColor.SelectedNext());
            TwineColor = new SolidColorBrush(randomColor.SelectedNext());
        }

        public void Blow()
        {
            if (Radius > MaxRadius)
            {
                BlownUp?.Invoke(this, new EventArgs());
            }
            else
            {
                Radius += stepRadius;
            }
        }

        public void Release()
        {
            if (Radius > stepRadius)
            {
                Radius -= stepRadius;
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
            return $"{notes.Substring(note, 1)}{octave}";
        }

        private byte MidiNoteId()
        {
            double factor = (endOctave - startOctave + 1.0) * Radius / MaxRadius;
            byte octave = (byte)Math.Floor(factor);
            byte note = (byte)Math.Floor((factor - Math.Floor(factor)) * notes.Length);
            return (byte)(startOctaveId + octave * notesInOctave + notesIds[note]);
        }
    }
}
