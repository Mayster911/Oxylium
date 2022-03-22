using Demo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBefore
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly NotUsefulViewModel notUsefulViewModel;
        private readonly ModeOneViewModel modeOneViewModel;
        private readonly ModeTwoViewModel modeTwoViewModel;

        public MainWindowViewModel()
        {
            notUsefulViewModel = new NotUsefulViewModel();
            modeOneViewModel = new ModeOneViewModel();
            modeTwoViewModel = new ModeTwoViewModel();
        }

        private bool _IsUsefulnessEnabled;

        public bool IsUsefulnessEnabled
        {
            get { return _IsUsefulnessEnabled; }
            set { _IsUsefulnessEnabled = value; OnPropertyChanged(); }
        }

        private ModesEnum _Mode;

        public ModesEnum Mode
        {
            get { return _Mode; }
            set { _Mode = value; OnPropertyChanged(); }
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
            set { _PasswordText = value; OnPropertyChanged(); }
        }

        public bool IsPasswordEnabled
        { 
            get
            {
                return _IsUsefulnessEnabled && _Mode == ModesEnum.ModeOne;
            }
        }

        private bool _UseRubberDuck = false;

        public bool UseRubberDuck
        {
            get { return _UseRubberDuck; }
            set { _UseRubberDuck = value; OnPropertyChanged(); }
        }
    }
}
