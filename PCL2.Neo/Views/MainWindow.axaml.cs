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
    /// </summary>
    public MainWindow()
    {
        // 初始化组件，包括XAML中定义的所有控件和资源。
        InitializeComponent();

        // 为导航背景边框的PointerPressed事件绑定处理方法，以便开始拖动窗口。
        NavBackgroundBorder.PointerPressed += OnNavPointerPressed;

        // 创建ThemeHelper实例并刷新主题，以确保应用了正确的主题样式。
        new ThemeHelper(this).Refresh();

        // 为关闭按钮点击事件绑定处理方法，用于关闭窗口。
        BtnTitleClose.Click += (_, _) => Close();

        // 为最小化按钮点击事件绑定处理方法，用于将窗口最小化。
        BtnTitleMin.Click += (_, _) => WindowState = WindowState.Minimized;
    }
    /// <summary>
    /// 当导航背景边框被按下时调用，开始移动拖拽窗口的操作。
    /// </summary>
    /// <param name="sender">事件发送者。</param>
    /// <param name="e">包含事件数据的PointerPressedEventArgs。</param>
    private void OnNavPointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e) {
        // 开始拖拽操作，允许用户通过鼠标拖动窗口。
        this.BeginMoveDrag(e);
    }
}