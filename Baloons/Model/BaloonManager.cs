using Commons.Music.Midi;
using System;
using System.IO;
using System.Linq;
using System.Windows.Threading;

namespace Baloons.Model
{
    public class BaloonManager
    {
        private readonly RandomFile randomSound;
        private readonly DispatcherTimer noteTimer = new();
        private byte currentNote = 0;
#pragma warning disable CS0618 // Type or member is obsolete
        private readonly IMidiAccess access;
#pragma warning restore CS0618 // Type or member is obsolete
        private readonly IMidiOutput output;

        public double CanvasWidth { get; set; }
        public double CanvasHeight { get; set; }

        public Uri RandomSound => new(randomSound.ExclusiveNext());

        public BaloonManager()
        {
            randomSound = new RandomFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds"), "*.mp3");
            noteTimer.Tick += NoteTimerTick;
            noteTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            access = MidiAccessManager.Default;
            output = access.OpenOutputAsync(access.Outputs.Last().Id).Result;
            output.Send(new byte[] { 0xC0, GeneralMidi.Instruments.PanFlute }, 0, 2, 0);
        }

        ~BaloonManager()
        {
            output.CloseAsync();
        }

        public BaloonModel NewBaloon()
        {
            BaloonModel baloon = new(CanvasWidth, CanvasHeight);
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

        private void PlayStart()
        {
            output.Send(new byte[] { MidiEvent.NoteOn, currentNote, 127 }, 0, 3, 0);
        }

        private void PlayStop()
        {
            output.Send(new byte[] { MidiEvent.NoteOff, currentNote, 127 }, 0, 2, 0);
        }

        private void PlayNote(BaloonModel baloon)
        {
            noteTimer.Stop();
            if (currentNote != 0)
            {
                PlayStop();
            }
            currentNote = baloon.NoteId;
            PlayStart();
            noteTimer.Start();
        }

        private void NoteTimerTick(object? sender, EventArgs e)
        {
            noteTimer.Stop();
            PlayStop();
            currentNote = 0;
        }
    }
}
