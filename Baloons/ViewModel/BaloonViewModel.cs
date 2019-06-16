﻿using Baloons.Model;
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
            RaisePropertyChanged("TwineMargin");
            RaisePropertyChanged("IsRunningOut");
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

        internal void Inflate()
        {
            baloon.Blow();
            RaisePropertyChanged("Margin");
            RaisePropertyChanged("Height");
            RaisePropertyChanged("Width");
            RaisePropertyChanged("TwineMargin");
        }

        internal void Deflate()
        {
            baloon.Release();
            RaisePropertyChanged("Margin");
            RaisePropertyChanged("Height");
            RaisePropertyChanged("Width");
            RaisePropertyChanged("TwineMargin");
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