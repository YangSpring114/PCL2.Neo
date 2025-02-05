using Avalonia.Controls;

namespace PCL2.Neo.ViewModels
{
    /// <summary>
    /// 主窗口视图模型类，部分定义，继承自ViewModelBase。用于处理主窗口相关的视图模型逻辑。
    /// </summary>
    public partial class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// 关联的窗口实例，可以为null。
        /// </summary>
        private Window? _window;

        // 为了设计时的DataContext
        /// <summary>
        /// 无参构造函数，主要用于设计时设置DataContext。不执行任何操作。// 为了设计时的DataContext
        /// </summary>
        public MainWindowViewModel()
        {
        }
        /// <summary>
        /// 构造函数，接收一个Window实例并将其存储在_window字段中。
        /// </summary>
        /// <param name="window">要关联的窗口实例。</param>
        public MainWindowViewModel(Window window)
        {
            this._window = window;
        }

        /// <summary>
        /// 关闭关联的窗口（如果有）。
        /// </summary>
        public void Close()
        {
            _window?.Close();
        }
        /// <summary>
        /// 将关联的窗口最小化（如果有）。
        /// </summary>
        public void Minimize()
        {
            if (_window is null) return;
            _window.WindowState = WindowState.Minimized;
        }
    }
}
