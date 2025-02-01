using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using PCL2.Neo.Helpers;
using PCL2.Neo.ViewModels;

namespace PCL2.Neo.Views
{
    public partial class MainWindow : Window
    {
        private ThemeHelper _themeHelper;
        private MainWindowViewModel _viewModel;
        
        public MainWindow()
        {
            InitializeComponent();
            
            _viewModel = new MainWindowViewModel(this);
            this.DataContext = _viewModel;
            
            _themeHelper = new ThemeHelper(this);
            _themeHelper.Refresh(); // 刷新主题

            this.TitleBar.BtnTitleClose.Click += (_, _) => _viewModel.Close();
            this.TitleBar.BtnTitleMin.Click += (_, _) => _viewModel.Minimize();

            UpdateClip(); // 圆角
        }

        private void UpdateClip()
        {
            var rect = new Rect(0, 0, this.Width - 36, this.Height - 36);
            this.BorderForm.Clip = new RectangleGeometry(rect, 6, 6);
        }
        
        private void TitleBar_OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                BeginMoveDrag(e);
            }
        }
    }
}