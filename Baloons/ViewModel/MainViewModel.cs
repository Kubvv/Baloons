using Baloons.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Windows;
using Toub.Sound.Midi;

namespace Baloons.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly BaloonManager baloonManager;
        private bool isBaloonBlownUp = false;

        private BaloonViewModel currentBaloon;
        public BaloonViewModel CurrentBaloon { get => currentBaloon; set => Set(ref currentBaloon, value); }

        private ObservableCollection<BaloonViewModel> baloonsSet = new ObservableCollection<BaloonViewModel>();
        public ObservableCollection<BaloonViewModel> BaloonsSet { get => baloonsSet; set => Set(ref baloonsSet, value); }

        private double canvasWidth = 800;
        public double CanvasWidth
        {
            get => canvasWidth;
            set
            {
                baloonManager.CanvasWidth = value;
                Set(ref canvasWidth, value);
            }
        }

        private double canvasHeight = 450;
        public double CanvasHeight
        {
            get => canvasHeight;
            set
            {
                baloonManager.CanvasHeight = value;
                Set(ref canvasHeight, value);
            }
        }

        public MainViewModel()
        {
            baloonManager = new BaloonManager
            {
                CanvasWidth = canvasWidth,
                CanvasHeight = canvasHeight
            };
            MidiPlayer.OpenMidi();
        }

        ~MainViewModel()
        {
            MidiPlayer.CloseMidi();
        }

        private RelayCommand newBaloonCommand;
        public RelayCommand NewBaloonCommand => newBaloonCommand ?? (newBaloonCommand = new RelayCommand(() => ExecuteNewBaloon()));

        private void ExecuteNewBaloon()
        {
            if (isBaloonBlownUp)
            {
                BaloonsSet.Clear();
                CurrentBaloon = null;
                isBaloonBlownUp = false;
            }
            if (CurrentBaloon != null)
            {
                BaloonsSet.Add(CurrentBaloon);
            }
            CurrentBaloon = new BaloonViewModel(baloonManager);
            CurrentBaloon.BlownUp += BaloonBlownUp;
        }

        private RelayCommand inflateBaloonCommand;
        public RelayCommand InflateBaloonCommand => inflateBaloonCommand ?? (inflateBaloonCommand = new RelayCommand(() => CurrentBaloon?.Inflate()));

        private RelayCommand deflateBaloonCommand;
        public RelayCommand DeflateBaloonCommand => deflateBaloonCommand ?? (deflateBaloonCommand = new RelayCommand(() => CurrentBaloon?.Deflate()));

        private RelayCommand closeAppCommand;
        public RelayCommand CloseAppCommand => closeAppCommand ?? (closeAppCommand = new RelayCommand(() => Application.Current.Shutdown()));

        private void BaloonBlownUp(object baloonModel, EventArgs e)
        {
            SoundPlayer soundPlayer = new SoundPlayer(Baloons.Properties.Resources.cannon);
            soundPlayer.Play();

            BlowUpEffects blowUpEffect;
            Random random = new Random();
            int effect = random.Next(3);
            if(effect == 0)
            {
                blowUpEffect = BlowUpEffects.RunOut;
            }
            else if(effect == 1)
            {
                blowUpEffect = BlowUpEffects.FadeOut;
            }
            else
            {
                blowUpEffect = BlowUpEffects.Rainbow;
            }

            isBaloonBlownUp = true;
            BaloonsSet.Add(CurrentBaloon);
            CurrentBaloon = null;

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