using Oxylium;

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
            mediator = new NotifyPropertyChangedMediator();

            notUsefulViewModel = new NotUsefulViewModel();
            modeOneViewModel = new ModeOneViewModel(mediator)
            {
                AreSettingsEnabledFunc = AreSettingsEnabled,
                UseRubberDuckFunc = GetIsUseRubberDuckSelected
            };
            modeTwoViewModel = new ModeTwoViewModel(mediator);

            // What's better?
            // 1. It's no longer necessary to pass a notifier function to ModeTwoViewModel - the mediator refreshes the registered properties
            // 2. All inter-property dependencies are located in the constructors instead of the properties, even if the dependencies are inter-viewmodel
            // 3. ModeTwoViewModel now leverages a chain of notifications for it's checkboxes - 6 lines of registration replaced 15 OnPropertyChanged calls.

            mediator.RegisterQ(this, ActiveViewModel,           IsUsefulnessEnabled);
            mediator.RegisterQ(this, IsPasswordEnabled,         IsUsefulnessEnabled);
            mediator.RegisterQ(this, IsUseRubberDuckEnabled,    IsUsefulnessEnabled);

            mediator.RegisterQ(this, IsModeOneSelected,         Mode);
            mediator.RegisterQ(this, IsModeTwoSelected,         Mode);
            mediator.RegisterQ(this, ActiveViewModel,           Mode);
            mediator.RegisterQ(this, IsPasswordEnabled,         Mode);
            mediator.RegisterQ(this, IsUseRubberDuckEnabled,    Mode);

            mediator.RegisterBetweenQ(modeOneViewModel, modeOneViewModel.AreSettingsEnabled,    this, PasswordText);
            mediator.RegisterBetweenQ(modeOneViewModel, modeOneViewModel.IsCSettingEnabled,     this, PasswordText);

            mediator.RegisterBetweenQ(modeOneViewModel, modeOneViewModel.IsCSettingEnabled,     this, IsUseRubberDuckSelected);

            mediator.RegisterBetweenQ(this, IsUseRubberDuckEnabled, modeTwoViewModel, modeTwoViewModel.IsThreeChecked);
            mediator.RegisterBetweenQ(this, IsUseRubberDuckEnabled, modeTwoViewModel, modeTwoViewModel.IsFourChecked);
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
                mediator.OnPropertyChanged(this);
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
                mediator.OnPropertyChanged(this);
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
    }
}
