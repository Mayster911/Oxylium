using Oxylium;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DemoAfter
{
    public class ViewModelBase : INotifyPropertyChanged, IRaisePropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(propertyName);
        }
    }
}