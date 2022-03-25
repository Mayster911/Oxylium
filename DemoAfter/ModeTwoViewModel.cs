using Oxylium;
using System;

namespace DemoAfter
{
    public class ModeTwoViewModel : ViewModelBase
    {
        private readonly NotifyPropertyChangedMediator mediator;

        public ModeTwoViewModel(NotifyPropertyChangedMediator mediator)
        {
            this.mediator = mediator;

            mediator.RegisterQ(this, IsTwoEnabled, IsOneChecked);
            mediator.RegisterQ(this, IsTwoChecked, IsOneChecked);

            mediator.RegisterQ(this, IsThreeEnabled, IsTwoChecked);
            mediator.RegisterQ(this, IsThreeChecked, IsTwoChecked);

            mediator.RegisterQ(this, IsFourEnabled, IsThreeChecked);
            mediator.RegisterQ(this, IsFourChecked, IsThreeChecked);
        }

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
                mediator.OnPropertyChanged(this);
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
                mediator.OnPropertyChanged(this);
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
                mediator.OnPropertyChanged(this);
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
                mediator.OnPropertyChanged(this);
            }
        }

        public bool IsFourEnabled
        {
            get { return _IsThreeChecked; }
        }

    }
}
