using Avalonia.Controls;

namespace PCL2.Neo.ViewModels
{
    /// <summary>
    /// 主窗口视图模型类，部分定义，继承自ViewModelBase。用于处理主窗口相关的视图模型逻辑。
    /// Main window view model class, partial definition, inherits from ViewModelBase. Used for handling view model logic related to the main window.
    /// </summary>
    public partial class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// 关联的窗口实例，可以为null。
        /// The associated window instance, can be null.
        /// </summary>
        private Window? _window;

        // 为了设计时的DataContext
        /// <summary>
        /// 无参构造函数，主要用于设计时设置DataContext。不执行任何操作。// 为了设计时的DataContext
        /// Parameterless constructor, mainly used for setting DataContext at design time. Does not perform any operations.
        /// </summary>
        public MainWindowViewModel()
        {
        }
        /// <summary>
        /// 构造函数，接收一个Window实例并将其存储在_window字段中。
        /// Constructor that takes a Window instance and stores it in the _window field.
        /// </summary>
        /// <param name="window">要关联的窗口实例。</param>
        public MainWindowViewModel(Window window)
        {
            this._window = window;
        }

        /// <summary>
        /// 关闭关联的窗口（如果有）。
        /// Closes the associated window if there is one.
        /// </summary>
        public void Close()
        {
            _window?.Close();
        }
        /// <summary>
        /// 将关联的窗口最小化（如果有）。
        /// Minimizes the associated window if there is one.
        /// </summary>
        public void Minimize()
        {
            if (_window is null) return;
            _window.WindowState = WindowState.Minimized;
        }
    }
}
