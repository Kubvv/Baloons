using Baloons.Model;
using GalaSoft.MvvmLight;
using System;
using System.Windows;
using System.Windows.Media;

namespace Baloons.ViewModel
{
    public class BaloonViewModel : ViewModelBase
    {
        private readonly BaloonModel baloon;

        public event EventHandler BlownUp;

        public BaloonViewModel(double width, double height)
        {
            baloon = new BaloonModel(width, height);
            baloon.BlownUp += BaloonBlownUp;
            RaisePropertyChanged("Margin");
            RaisePropertyChanged("Height");
            RaisePropertyChanged("Width");
            RaisePropertyChanged("Color");
        }

        public Thickness Margin => new Thickness(baloon.Center.X - baloon.Radius, baloon.Center.Y - baloon.Radius, 0, 0);
        public int Height => baloon.Radius * 2;
        public int Width => baloon.Radius * 2;
        public Brush Color => baloon.Color;

        internal void Blow()
        {
            baloon.Blow();
            RaisePropertyChanged("Margin");
            RaisePropertyChanged("Height");
            RaisePropertyChanged("Width");
        }

        internal void Release()
        {
            baloon.Release();
            RaisePropertyChanged("Margin");
            RaisePropertyChanged("Height");
            RaisePropertyChanged("Width");
        }

        private void BaloonBlownUp(object baloonModel, EventArgs e)
        {
            BlownUp?.Invoke(baloonModel, e);
        }

        internal void SetCenter(double x, double y)
        {
            baloon.SetCenter(x, y);
        }
    }
}