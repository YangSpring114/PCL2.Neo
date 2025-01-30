using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using PCL2.Neo.Models;
using PCL2.Neo.Utils;

namespace PCL2.Neo.Controls;

public class MyIconButton : TemplatedControl
{
    private Path? _pathIcon;
    private Border? _panBack;
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _pathIcon = e.NameScope.Find<Path>("PathIcon")!;
        _panBack = e.NameScope.Find<Border>("PanBack")!;
        
        this.PointerEntered += (_, _) => RefreshColor();
        this.PointerExited += (_, _) => RefreshColor();
        this.Loaded += (_, _) => RefreshColor();
        
        _pathIcon.Data = Geometry.Parse(Logo);
        _pathIcon.RenderTransform = new ScaleTransform{ ScaleX = LogoScale, ScaleY = LogoScale };
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
        nameof(IconTheme));
    public IconThemes IconTheme
    {
        get => GetValue(IconThemeProperty);
        set
        {
            SetValue(IconThemeProperty, value);
            RefreshColor();
        }
    }

    private void RefreshColor()
    {
        if (_pathIcon is null || _panBack is null) return;
        if (IsLoaded)
        {
            if (_panBack.Background is null)
            {
                _panBack.Background = (SolidColorBrush)new MyColor(0,255,255,255);
            }
            
            
            if (_pathIcon.Fill is null)
            {
                switch (IconTheme)
                {
                    case IconThemes.Red: 
                        _pathIcon.Fill = (SolidColorBrush)new MyColor(160, 255, 76, 76);
                        break;
                    case IconThemes.Black:
                        _pathIcon.Fill = (SolidColorBrush)new MyColor(160, 0, 0, 0);
                        break;
                }
            }

            if (IsPointerOver)
            {
                switch (IconTheme)
                {
                    case IconThemes.Color:
                        _pathIcon.Fill = (IBrush?)Application.Current!.Resources["ColorBrush2"];
                        break;
                    case IconThemes.White:
                        _panBack.Background = (SolidColorBrush)new MyColor(50, 255, 255, 255);
                        break;
                    case IconThemes.Red:
                        _pathIcon.Fill = (SolidColorBrush)new MyColor(76, 76, 76);
                        break;
                    case IconThemes.Black:
                        _pathIcon.Fill = (SolidColorBrush)new MyColor(230, 0, 0, 0);
                        break;
                }
            }
            else
            {
                switch (IconTheme)
                {
                    case IconThemes.Color:
                        _pathIcon.Fill = (IBrush?)Application.Current!.Resources["ColorBrush4"];
                        _panBack.Background = (SolidColorBrush)new MyColor(0, 255, 255, 255);
                        break;
                    case IconThemes.White:
                        _pathIcon.Fill = (IBrush?)Application.Current!.Resources["ColorBrush8"];
                        _panBack.Background = (SolidColorBrush)new MyColor(0, 255, 255, 255);
                        break;
                    case IconThemes.Red:
                        _pathIcon.Fill = (SolidColorBrush)new MyColor(160, 255, 76, 76);
                        _panBack.Background = (SolidColorBrush)new MyColor(0, 255, 255, 255);
                        break;
                    case IconThemes.Black:
                        _pathIcon.Fill = (SolidColorBrush)new MyColor(160, 0, 0, 0);
                        _panBack.Background = (SolidColorBrush)new MyColor(0, 255, 255, 255);
                        break;
                }
            }
        }
        else
        {
            switch (IconTheme)
            {
                case IconThemes.Color:
                    _pathIcon.Fill = Application.Current!.Resources["ColorBrush5"] as SolidColorBrush;
                    break;
                case IconThemes.White:
                    _pathIcon.Fill = Application.Current!.Resources["ColorBrush8"] as SolidColorBrush;
                    break;
                case IconThemes.Red:
                    _pathIcon.Fill = (SolidColorBrush)new MyColor(160, 255, 76, 76);
                    break;
                case IconThemes.Black:
                    _pathIcon.Fill = (SolidColorBrush)new MyColor(160, 0, 0, 0);
                    break;
            }
            _panBack.Background = (SolidColorBrush)new MyColor(0, 255, 255, 255);
        }
    }
}