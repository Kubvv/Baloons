using Baloons.Model;
using GalaSoft.MvvmLight;
using System;
using System.Windows;
using System.Windows.Media;
using Toub.Sound.Midi;

namespace Baloons.ViewModel
{
    public class BaloonViewModel : ViewModelBase
    {
        private readonly BaloonModel baloon;

        public event EventHandler BlownUp;

        public BaloonViewModel(BaloonManager baloonManager)
        {
            baloon = baloonManager.NewBaloon();
            baloon.BlownUp += BaloonBlownUp;
            RaisePropertyChanged("Margin");
            RaisePropertyChanged("Height");
            RaisePropertyChanged("Width");
            RaisePropertyChanged("Color");
            RaisePropertyChanged("TwineMargin");
            MidiPlayer.Play(baloon.Note);
        }

        Thickness margin = new Thickness(0);
        public Thickness Margin
        {
            get
            {
                margin.Left = baloon.Center.X - baloon.Radius;
                margin.Top = baloon.Center.Y - baloon.Radius;
                return margin;
            }
        }

        Thickness twineMargin = new Thickness(0);
        public Thickness TwineMargin
        {
            get
            {
                twineMargin.Left = baloon.Radius;
                return twineMargin;
            }
        }

        public int Height => baloon.Radius * 2;
        public int Width => baloon.Radius * 2;
        public Brush Color => baloon.Color;
        public Brush TwineColor => baloon.TwineColor;

        bool isRunningOut = false;
        public bool IsRunningOut
        {
            get => isRunningOut;
            set => Set(ref isRunningOut, value);
        }

        bool isFadingOut = false;
        public bool IsFadingOut
        {
            get => isFadingOut;
            set => Set(ref isFadingOut, value);
        }

        public void Inflate()
        {
            baloon.Blow();
            RaisePropertyChanged("Margin");
            RaisePropertyChanged("Height");
            RaisePropertyChanged("Width");
            RaisePropertyChanged("TwineMargin");
            MidiPlayer.Play(baloon.Note);
        }

        public void Deflate()
        {
            baloon.Release();
            RaisePropertyChanged("Margin");
            RaisePropertyChanged("Height");
            RaisePropertyChanged("Width");
            RaisePropertyChanged("TwineMargin");
            MidiPlayer.Play(baloon.Note);
        }

        public void SetCenter(double x, double y)
        {
            baloon.SetCenter(x, y);
            RaisePropertyChanged("Margin");
        }

        private void BaloonBlownUp(object baloonModel, EventArgs e)
        {
            BlownUp?.Invoke(baloonModel, e);
        }
    }
}