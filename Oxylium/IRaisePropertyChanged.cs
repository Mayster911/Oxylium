using System.Runtime.CompilerServices;

namespace Oxylium
{
    public interface IRaisePropertyChanged
    {
        void RaisePropertyChanged([CallerMemberName] string propertyName = "");
    }
}
