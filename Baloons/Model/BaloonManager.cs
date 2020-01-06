using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Toub.Sound.Midi;

namespace Baloons.Model
{
    public class BaloonManager
    {
        private readonly Random random = new Random();
        private readonly RandomColor randomColor = new RandomColor();
        private readonly RandomFile randomSound;
        private readonly DispatcherTimer noteTimer = new DispatcherTimer();
        private string currentNote;

        public double CanvasWidth { get; set; }
        public double CanvasHeight { get; set; }

        public Uri RandomSound => new Uri(randomSound.ExclusiveNext());

        public BaloonManager()
        {
            randomSound = new RandomFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds"), "*.mp3");
            noteTimer.Tick += NoteTimerTick;
            noteTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            MidiPlayer.OpenMidi();
            MidiPlayer.Play(new ProgramChange(0, 1, GeneralMidiInstruments.PanFlute));
        }

        ~BaloonManager()
        {
            MidiPlayer.CloseMidi();
        }

        public BaloonModel NewBaloon()
        {
            int maxDim = (int)Math.Max(CanvasWidth, CanvasHeight);
            int maxRadius = random.Next(maxDim / 2) + 100;
            int radius = random.Next(maxRadius / 5) + 10;
            BaloonModel baloon = new BaloonModel
            {
                MaxRadius = maxRadius,
                Radius = radius,
                Center = new Point(random.Next((int)CanvasWidth - radius * 2) + radius, random.Next((int)CanvasHeight - radius * 2) + radius),
                Color = new SolidColorBrush(randomColor.SelectedNext()),
                TwineColor = new SolidColorBrush(randomColor.SelectedNext())
            };
            PlayNote(baloon);
            return baloon;
        }

        internal void Blow(BaloonModel baloon)
        {
            baloon.Blow();
            PlayNote(baloon);
        }

        internal void Release(BaloonModel baloon)
        {
            baloon.Release();
            PlayNote(baloon);
        }

        private void PlayNote(BaloonModel baloon)
        {
            noteTimer.Stop();
            if (currentNote != null) MidiPlayer.Play(new NoteOff(0, 1, currentNote, 127));
            currentNote = baloon.Note;
            MidiPlayer.Play(new NoteOn(0, 1, currentNote, 127));
            noteTimer.Start();
        }

        private void NoteTimerTick(object sender, EventArgs e)
        {
            noteTimer.Stop();
            MidiPlayer.Play(new NoteOff(0, 1, currentNote, 127));
            currentNote = null;
        }
    }
}
