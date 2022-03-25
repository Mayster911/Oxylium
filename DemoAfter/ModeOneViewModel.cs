using System;

namespace DemoAfter
{
    public class ModeOneViewModel : ViewModelBase
    {
        public Func<bool>? AreSettingsEnabledFunc { get; set; }
        public Func<bool>? UseRubberDuckFunc { get; set; }

        public bool AreSettingsEnabled
        {
            get { return AreSettingsEnabledFunc?.Invoke() ?? false; }
        }

        private bool _IsAChecked;

        public bool IsAChecked
        {
            get { return _IsAChecked; }
            set { _IsAChecked = value; OnPropertyChanged(); }
        }

        private bool _IsBChecked;

        public bool IsBChecked
        {
            get { return _IsBChecked; }
            set { _IsBChecked = value; OnPropertyChanged(); }
        }

        private bool _IsCChecked;

        public bool IsCChecked
        {
            get { return _IsCChecked; }
            set { _IsCChecked = value; OnPropertyChanged(); }
        }

        public bool IsCSettingEnabled
        {
            get
            {
                return (AreSettingsEnabledFunc?.Invoke() ?? false) && (UseRubberDuckFunc?.Invoke() ?? false);
            }
        }
    }
}
