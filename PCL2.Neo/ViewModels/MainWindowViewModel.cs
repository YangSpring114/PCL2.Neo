using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;

namespace PCL2.Neo.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private Window _window;
        
        public MainWindowViewModel(Window window)
        {
            this._window = window;
        }
        
        [RelayCommand]
        public void Close()
        {
            _window.Close();
        }
        [RelayCommand]
        public void Minimize()
        {
            _window.WindowState = WindowState.Minimized;
        }
    }
}
