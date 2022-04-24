using Baloons.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Baloons.ViewModel
{
    public class MainViewModel : ObservableRecipient
    {
        private readonly BaloonManager baloonManager;
        private bool isBaloonBlownUp = false;
        private readonly MediaPlayer mediaPlayer = new();

        private BaloonViewModel? currentBaloon;
        public BaloonViewModel? CurrentBaloon
        {
            get => currentBaloon;
            set => SetProperty(ref currentBaloon, value);
        }

        private ObservableCollection<BaloonViewModel> baloonsSet = new();
        public ObservableCollection<BaloonViewModel> BaloonsSet
        {
            get => baloonsSet;
            set => SetProperty(ref baloonsSet, value);
        }

        private double canvasWidth = 800;
        public double CanvasWidth
        {
            get => canvasWidth;
            set
            {
                baloonManager.CanvasWidth = value;
                SetProperty(ref canvasWidth, value);
            }
        }

        private double canvasHeight = 450;
        public double CanvasHeight
        {
            get => canvasHeight;
            set
            {
                baloonManager.CanvasHeight = value;
                SetProperty(ref canvasHeight, value);
            }
        }

        public MainViewModel()
        {
            baloonManager = new BaloonManager
            {
                CanvasWidth = canvasWidth,
                CanvasHeight = canvasHeight
            };
            currentBaloon = NewBaloon();

            NewBaloonCommand = new RelayCommand(() => ExecuteNewBaloon());
            InflateBaloonCommand = new RelayCommand(() => CurrentBaloon?.Inflate());
            DeflateBaloonCommand = new RelayCommand(() => CurrentBaloon?.Deflate());
            CloseAppCommand = new RelayCommand(() => Application.Current.Shutdown());
        }

        public RelayCommand NewBaloonCommand { get; private set; }
        public RelayCommand InflateBaloonCommand { get; private set; }
        public RelayCommand DeflateBaloonCommand { get; private set; }
        public RelayCommand CloseAppCommand { get; private set; }

        private void ExecuteNewBaloon()
        {
            if (isBaloonBlownUp)
            {
                BaloonsSet.Clear();
                isBaloonBlownUp = false;
            }
            else if (CurrentBaloon != null)
            {
                BaloonsSet.Add(CurrentBaloon);
            }
            mediaPlayer.Stop();
            CurrentBaloon = NewBaloon();
        }

        private BaloonViewModel NewBaloon()
        {
            BaloonViewModel result = new(baloonManager);
            result.BlownUp += BaloonBlownUp;
            return result;
        }

        private void BaloonBlownUp(object? baloonModel, EventArgs e)
        {
            if (isBaloonBlownUp) { return; } 
            isBaloonBlownUp = true;
            if (CurrentBaloon != null)
            {
                BaloonsSet.Add(CurrentBaloon);
                CurrentBaloon = null;
            }

            mediaPlayer.Open(baloonManager.RandomSound);
            mediaPlayer.Play();

            BlowUpEffects blowUpEffect;
            Random random = new();
            int effect = random.Next(3);
            if (effect == 0)
            {
                blowUpEffect = BlowUpEffects.RunOut;
            }
            else if (effect == 1)
            {
                blowUpEffect = BlowUpEffects.FadeOut;
            }
            else
            {
                blowUpEffect = BlowUpEffects.Rainbow;
            }
            if (blowUpEffect == BlowUpEffects.Rainbow)
            {
                BaloonsSet = new ObservableCollection<BaloonViewModel>(BaloonsSet.OrderBy(baloon => baloon.Height).Reverse());
                foreach (BaloonViewModel baloonViewModel in BaloonsSet)
                {
                    baloonViewModel.SetCenter(CanvasWidth / 2, CanvasHeight - 10);
                }
            }
            else if (blowUpEffect == BlowUpEffects.RunOut)
            {
                foreach (BaloonViewModel baloonViewModel in BaloonsSet)
                {
                    baloonViewModel.IsRunningOut = true;
                }
            }
            else if (blowUpEffect == BlowUpEffects.FadeOut)
            {
                foreach (BaloonViewModel baloonViewModel in BaloonsSet)
                {
                    baloonViewModel.IsFadingOut = true;
                }
            }
        }
    }

    enum BlowUpEffects
    {
        Rainbow,
        RunOut,
        FadeOut
    }
}