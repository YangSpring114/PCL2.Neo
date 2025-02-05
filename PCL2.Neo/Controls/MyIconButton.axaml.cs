using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using PCL2.Neo.Models;
using PCL2.Neo.Utils;

namespace PCL2.Neo.Controls;

[PseudoClasses(":color", ":white", ":black", ":red", ":custom")]
public class MyIconButton : TemplatedControl
{
    private Path? _pathIcon;
    private Border? _panBack;
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _pathIcon = e.NameScope.Find<Path>("PathIcon")!;
        _panBack = e.NameScope.Find<Border>("PanBack")!;
        
        // 事件
        //this.PointerEntered += (_, _) => RefreshColor();
        //this.PointerExited += (_, _) => RefreshColor();
        //this.PointerReleased += OnPointerReleased;
        //this.AddHandler(PointerReleasedEvent, OnPointerReleased, RoutingStrategies.Bubble, true);
        this.Loaded += (_, _) => RefreshColor();
        
        // 初始化
        _pathIcon.Data = Geometry.Parse(Logo);
        _pathIcon.RenderTransform = new ScaleTransform{ ScaleX = LogoScale, ScaleY = LogoScale };
        
        SetPseudoClass();
    }
    
    protected override void OnPointerReleased(PointerReleasedEventArgs pointerReleasedEventArgs)
    {
        if (pointerReleasedEventArgs.InitialPressMouseButton == MouseButton.Left)
        {
            OnClick();
            // 这里缺少动画
            // 这里缺少 PCL 的 Event
        }
    }

    public int Uuid = CoreUtils.GetUuid();
    
    public static readonly StyledProperty<string> LogoProperty = AvaloniaProperty.Register<MyIconButton, string>(
        nameof(Logo));
    public string Logo
    {
        get => GetValue(LogoProperty);
        set
        {
            SetValue(LogoProperty, value);
            if (_pathIcon != null)
            {
                _pathIcon.Data = Geometry.Parse(value);
            }
        }
    }
    public static readonly StyledProperty<double> LogoScaleProperty = AvaloniaProperty.Register<MyIconButton, double>(
        nameof(LogoScale),
        1);
    public double LogoScale
    {
        get => GetValue(LogoScaleProperty);
        set
        {
            SetValue(LogoScaleProperty, value);
            if (_pathIcon != null)
            {
                _pathIcon.RenderTransform = new ScaleTransform{ ScaleX = value, ScaleY = value };
            }
        }
    }
    public enum IconThemes
    {
        Color,
        White,
        Black,
        Red,
        Custom
    }

    public static readonly StyledProperty<IconThemes> IconThemeProperty = AvaloniaProperty.Register<MyIconButton, IconThemes>(
        nameof(IconTheme),
        IconThemes.Color);
    public IconThemes IconTheme
    {
        get => GetValue(IconThemeProperty);
        set
        {
            SetValue(IconThemeProperty, value);
            SetPseudoClass();
        }
    }

    public new static readonly StyledProperty<IBrush> ForegroundProperty = AvaloniaProperty.Register<MyIconButton, IBrush>(
        nameof(Foreground));

    public new IBrush Foreground
    {
        get => GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }
    
    public static readonly StyledProperty<IBrush> ForegroundInnerProperty = AvaloniaProperty.Register<MyIconButton, IBrush>(
        nameof(ForegroundInner));

    public IBrush ForegroundInner
    {
        get => GetValue(ForegroundInnerProperty);
        set => SetValue(ForegroundInnerProperty, value);
    }

    public new static readonly StyledProperty<IBrush> BackgroundProperty = AvaloniaProperty.Register<MyIconButton, IBrush>(
        nameof(Background));

    public new IBrush Background
    {
        get => GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }
    
    public static readonly StyledProperty<string> EventTypeProperty = AvaloniaProperty.Register<MyIconButton, string>(
        nameof(EventType));

    public string EventType
    {
        get => GetValue(EventTypeProperty);
        set => SetValue(EventTypeProperty, value);
    }

    public static readonly StyledProperty<string> EventDataProperty = AvaloniaProperty.Register<MyIconButton, string>(
        nameof(EventData));

    public string EventData
    {
        get => GetValue(EventDataProperty);
        set => SetValue(EventDataProperty, value);
    }
    
    public static readonly RoutedEvent<RoutedEventArgs> ClickEvent = RoutedEvent.Register<MyIconButton, RoutedEventArgs>(
        nameof(Click),
        RoutingStrategies.Bubble);
    
    public event EventHandler<RoutedEventArgs> Click
    {
        add => AddHandler(ClickEvent, value);
        remove => RemoveHandler(ClickEvent, value);
    }
    
    protected virtual void OnClick()
    {
        RaiseEvent(new RoutedEventArgs(ClickEvent));
    }
    
    /// <summary>
    /// 初始化颜色。
    /// </summary>
    private void RefreshColor()
    {
        if (_pathIcon is null || _panBack is null) return;
        switch (IconTheme)
        {
            case IconThemes.Color:
                _pathIcon.Fill = Application.Current!.Resources["ColorBrush5"] as SolidColorBrush;
                break;
            case IconThemes.White:
                _pathIcon.Fill = (SolidColorBrush)new MyColor(234, 242, 254);
                break;
            case IconThemes.Red:
                _pathIcon.Fill = (SolidColorBrush)new MyColor(160, 255, 76, 76);
                break;
            case IconThemes.Black:
                _pathIcon.Fill = (SolidColorBrush)new MyColor(160, 0, 0, 0);
                break;
            case IconThemes.Custom:
                _pathIcon.Fill = (SolidColorBrush)new MyColor(160, (SolidColorBrush)Foreground);
                break;
        }
        _panBack.Background = (SolidColorBrush)new MyColor(0, 255, 255, 255);
    }
    private void SetPseudoClass()
    {
        switch (IconTheme)
        {
            case IconThemes.Color:
                PseudoClasses.Set(":color", true);
                break;
            case IconThemes.White:
                PseudoClasses.Set(":white", true);
                break;
            case IconThemes.Black:
                PseudoClasses.Set(":black", true);
                break;
            case IconThemes.Red:
                PseudoClasses.Set(":red", true);
                break;
            case IconThemes.Custom:
                PseudoClasses.Set(":custom", true);
                break;
        }
    }
}