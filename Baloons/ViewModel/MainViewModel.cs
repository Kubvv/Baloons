using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Windows;

namespace Baloons.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private bool isBaloonBlownUp = false;

        private BaloonViewModel currentBaloon;
        public BaloonViewModel CurrentBaloon { get => currentBaloon; set => Set(ref currentBaloon, value); }

        private ObservableCollection<BaloonViewModel> baloons = new ObservableCollection<BaloonViewModel>();
        public ObservableCollection<BaloonViewModel> Baloons { get => baloons; set => Set(ref baloons, value); }

        private double canvasWidth = 800;
        public double CanvasWidth { get => canvasWidth; set => Set(ref canvasWidth, value); }

        private double canvasHeight = 450;
        public double CanvasHeight { get => canvasHeight; set => Set(ref canvasHeight, value); }

        public MainViewModel()
        {
            ExecuteNewBaloon();
        }

        private RelayCommand newBaloonCommand;
        public RelayCommand NewBaloonCommand => newBaloonCommand ?? (newBaloonCommand = new RelayCommand(() => ExecuteNewBaloon()));

        private void ExecuteNewBaloon()
        {
            if (isBaloonBlownUp)
            {
                Baloons.Clear();
                CurrentBaloon = null;
                isBaloonBlownUp = false;
            }
            if (CurrentBaloon != null)
            {
                Baloons.Add(CurrentBaloon);
            }
            CurrentBaloon = new BaloonViewModel(CanvasWidth, CanvasHeight);
            CurrentBaloon.BlownUp += BaloonBlownUp;
        }

        private RelayCommand inflateBaloonCommand;
        public RelayCommand InflateBaloonCommand => inflateBaloonCommand ?? (inflateBaloonCommand = new RelayCommand(() => CurrentBaloon?.Inflate()));

        private RelayCommand deflateBaloonCommand;
        public RelayCommand DeflateBaloonCommand => deflateBaloonCommand ?? (deflateBaloonCommand = new RelayCommand(() => CurrentBaloon?.Deflate()));

        private RelayCommand closeAppCommand;
        public RelayCommand CloseAppCommand => closeAppCommand ?? (closeAppCommand = new RelayCommand(() => Application.Current.Shutdown()));

        private BlowUpEffects blowUpEffect = BlowUpEffects.Rainbow;

        private void BaloonBlownUp(object baloonModel, EventArgs e)
        {
            isBaloonBlownUp = true;
            Baloons.Add(CurrentBaloon);
            CurrentBaloon = null;

            if (blowUpEffect == BlowUpEffects.Rainbow)
            {
                Baloons = new ObservableCollection<BaloonViewModel>(Baloons.OrderBy(baloon => baloon.Height).Reverse());
                foreach (BaloonViewModel baloonViewModel in Baloons)
                {
                    baloonViewModel.SetCenter(CanvasWidth / 2, CanvasHeight - 10);
                }
            }
            else if (blowUpEffect == BlowUpEffects.RunOut)
            {
                foreach (BaloonViewModel baloonViewModel in Baloons)
                {
                    baloonViewModel.IsRunningOut = true;
                }
            }
            SystemSounds.Beep.Play();
        }
    }

    enum BlowUpEffects
    {
        Rainbow,
        RunOut
    }
}