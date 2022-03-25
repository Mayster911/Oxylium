﻿using Oxylium;

namespace DemoAfter
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly NotifyPropertyChangedMediator mediator;

        private readonly NotUsefulViewModel notUsefulViewModel;
        private readonly ModeOneViewModel modeOneViewModel;
        private readonly ModeTwoViewModel modeTwoViewModel;

        public MainWindowViewModel()
        {
            notUsefulViewModel = new NotUsefulViewModel();
            modeOneViewModel = new ModeOneViewModel
            {
                AreSettingsEnabledFunc = AreSettingsEnabled,
                UseRubberDuckFunc = GetIsUseRubberDuckSelected
            };
            modeTwoViewModel = new ModeTwoViewModel
            {
                NotifyThreeOrFourChanged = NotifyThreeOrFourChanged
            };

            mediator = new NotifyPropertyChangedMediator();

            mediator.RegisterQ(this, ActiveViewModel, IsUsefulnessEnabled);
            mediator.RegisterQ(this, IsPasswordEnabled, IsUsefulnessEnabled);
            mediator.RegisterQ(this, IsUseRubberDuckEnabled, IsUsefulnessEnabled);

            mediator.RegisterQ(this, IsModeOneSelected, Mode);
            mediator.RegisterQ(this, IsModeTwoSelected, Mode);
            mediator.RegisterQ(this, ActiveViewModel, Mode);
            mediator.RegisterQ(this, IsPasswordEnabled, Mode);
            mediator.RegisterQ(this, IsUseRubberDuckEnabled, Mode);

            mediator.RegisterQ(modeOneViewModel, modeOneViewModel.AreSettingsEnabled, this, (object)PasswordText);
        }

        private bool _IsUsefulnessEnabled;

        public bool IsUsefulnessEnabled
        {
            get { return _IsUsefulnessEnabled; }
            set 
            { 
                _IsUsefulnessEnabled = value;
                mediator.OnPropertyChanged(this);
            }
        }

        private ModesEnum _Mode;

        public ModesEnum Mode
        {
            get { return _Mode; }
            set 
            { 
                _Mode = value; 
                mediator.OnPropertyChanged(this);
            }
        }

        public bool IsModeOneSelected
        {
            get { return _Mode == ModesEnum.ModeOne; }
            set
            {
                if (value)
                    Mode = ModesEnum.ModeOne;
            }
        }

        public bool IsModeTwoSelected
        {
            get { return _Mode == ModesEnum.ModeTwo; }
            set
            {
                if (value)
                    Mode = ModesEnum.ModeTwo;
            }
        }

        public ViewModelBase ActiveViewModel
        {
            get
            {
                if (!_IsUsefulnessEnabled)
                    return notUsefulViewModel;

                if (_Mode == ModesEnum.ModeOne)
                    return modeOneViewModel;

                return modeTwoViewModel;
            }
        }

        private string _PasswordText = string.Empty;

        public string PasswordText
        {
            get { return _PasswordText; }
            set 
            { 
                _PasswordText = value; 
                OnPropertyChanged();
                modeOneViewModel.OnPropertyChanged(nameof(modeOneViewModel.AreSettingsEnabled));
                modeOneViewModel.OnPropertyChanged(nameof(modeOneViewModel.IsCSettingEnabled));
            }
        }

        public bool IsPasswordEnabled
        { 
            get
            {
                return _IsUsefulnessEnabled && _Mode == ModesEnum.ModeOne;
            }
        }

        private bool _IsUseRubberDuckSelected = false;

        public bool IsUseRubberDuckSelected
        {
            get { return _IsUseRubberDuckSelected; }
            set 
            { 
                _IsUseRubberDuckSelected = value; 
                OnPropertyChanged();
                modeOneViewModel.OnPropertyChanged(nameof(modeOneViewModel.IsCSettingEnabled));
            }
        }

        public bool IsUseRubberDuckEnabled
        {
            get 
            {
                if (!_IsUsefulnessEnabled)
                    return false;

                if (_Mode == ModesEnum.ModeOne)
                    return true;

                return !modeTwoViewModel.IsThreeChecked && !modeTwoViewModel.IsFourChecked; 
            }
        }



        private bool AreSettingsEnabled()
        {
            return _PasswordText == "1024";
        }

        private bool GetIsUseRubberDuckSelected()
        {
            return _IsUseRubberDuckSelected;
        }

        private void NotifyThreeOrFourChanged()
        {
            OnPropertyChanged(nameof(IsUseRubberDuckEnabled));
        }
    }
}
