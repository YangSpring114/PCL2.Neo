using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PCL2.Neo.Helpers;

namespace PCL2.Neo.Views;

/// <summary>
/// 主窗口类
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// 主窗口的构造函数，初始化组件并设置相关事件处理。
    /// Constructor for the main window, initializes components and sets up related event handlers.
    /// </summary>
    public MainWindow()
    {
        // 初始化组件，包括XAML中定义的所有控件和资源。
        // Initializes components, including all controls and resources defined in XAML.
        InitializeComponent();

        // 为导航背景边框的PointerPressed事件绑定处理方法，以便开始拖动窗口。
        // Binds the handler method for the PointerPressed event of the navigation background border to start dragging the window.
        NavBackgroundBorder.PointerPressed += OnNavPointerPressed;

        // 创建ThemeHelper实例并刷新主题，以确保应用了正确的主题样式。
        // Creates an instance of ThemeHelper and refreshes the theme to ensure the correct theme style is applied.
        new ThemeHelper(this).Refresh();

        // 为关闭按钮点击事件绑定处理方法，用于关闭窗口。
        // Binds the handler method for the click event of the close button to close the window.
        BtnTitleClose.Click += (_, _) => Close();

        // 为最小化按钮点击事件绑定处理方法，用于将窗口最小化。
        // Binds the handler method for the click event of the minimize button to minimize the window.
        BtnTitleMin.Click += (_, _) => WindowState = WindowState.Minimized;
    }
    /// <summary>
    /// 当导航背景边框被按下时调用，开始移动拖拽窗口的操作。
    /// Called when the navigation background border is pressed, starts the operation to move and drag the window.
    /// </summary>
    /// <param name="sender">事件发送者。</param>
    /// <param name="e">包含事件数据的PointerPressedEventArgs。</param>
    private void OnNavPointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e) {
        // 开始拖拽操作，允许用户通过鼠标拖动窗口。
        // Starts a drag operation that allows the user to drag the window with the mouse.
        this.BeginMoveDrag(e);
    }
}