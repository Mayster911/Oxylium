using Demo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBefore
{
    public class ModeTwoViewModel : ViewModelBase
    {
        public Action? NotifyThreeOrFourChanged { get; set; }

        private bool _IsOneChecked;

        public bool IsOneChecked
        {
            get { return _IsOneChecked; }
            set 
            { 
                _IsOneChecked = value;
                if (!value)
                {
                    _IsTwoChecked = false;
                    _IsThreeChecked = false;
                    _IsFourChecked = false;
                }
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(IsTwoEnabled)); 
                OnPropertyChanged(nameof(IsThreeEnabled)); 
                OnPropertyChanged(nameof(IsFourEnabled)); 
                OnPropertyChanged(nameof(IsTwoChecked));
                OnPropertyChanged(nameof(IsThreeChecked));
                OnPropertyChanged(nameof(IsFourChecked));
            }
        }

        private bool _IsTwoChecked;

        public bool IsTwoChecked
        {
            get { return IsOneChecked && _IsTwoChecked; }
            set 
            { 
                _IsTwoChecked = value;
                if (!value)
                {
                    _IsThreeChecked = false;
                    _IsFourChecked = false;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsTwoEnabled));
                OnPropertyChanged(nameof(IsThreeEnabled));
                OnPropertyChanged(nameof(IsFourEnabled));
                OnPropertyChanged(nameof(IsThreeChecked));
                OnPropertyChanged(nameof(IsFourChecked));
            }
        }

        public bool IsTwoEnabled
        {
            get { return _IsOneChecked; }
        }

        private bool _IsThreeChecked;

        public bool IsThreeChecked
        {
            get { return IsTwoChecked && _IsThreeChecked; }
            set 
            { 
                _IsThreeChecked = value;
                if (!value)
                {
                    _IsFourChecked = false;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsTwoEnabled));
                OnPropertyChanged(nameof(IsThreeEnabled));
                OnPropertyChanged(nameof(IsFourEnabled));
                OnPropertyChanged(nameof(IsFourChecked));
                NotifyThreeOrFourChanged?.Invoke();
            }
        }

        public bool IsThreeEnabled
        {
            get { return _IsTwoChecked; }
        }

        private bool _IsFourChecked;

        public bool IsFourChecked
        {
            get { return IsThreeChecked && _IsFourChecked; }
            set 
            { 
                _IsFourChecked = value;
                OnPropertyChanged(); 
                NotifyThreeOrFourChanged?.Invoke();
            }
        }

        public bool IsFourEnabled
        {
            get { return _IsThreeChecked; }
        }

    }
}
