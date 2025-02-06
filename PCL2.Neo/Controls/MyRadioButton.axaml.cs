using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using PCL2.Neo.Helpers;
using PCL2.Neo.Models;
using PCL2.Neo.Utils;

namespace PCL2.Neo.Controls;

[PseudoClasses(":white", ":highlight", ":checked", ":pressed")]
public class MyRadioButton : TemplatedControl
{
    private Path? _shapeLogo;
    private TextBlock? _labText;
    private Border? _panBack;
    
    private bool _isMouseDown = false;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _shapeLogo = e.NameScope.Find<Path>("ShapeLogo")!;
        _labText = e.NameScope.Find<TextBlock>("LabText")!;
        _panBack = e.NameScope.Find<Border>("PanBack")!;
        
        this.Loaded += (_, _) => RefreshColor();
        
        _shapeLogo.Data = Geometry.Parse(Logo);
        _shapeLogo.RenderTransform = new ScaleTransform { ScaleX = LogoScale, ScaleY = LogoScale };
        _labText.Text = Text;
        
        SetPseudoClass();
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        if (Checked) return;
        if (e.InitialPressMouseButton == MouseButton.Left)
        {
            _isMouseDown = false;
            SetCheck();
            SetPseudoClass();
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (Checked) return;
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            _isMouseDown = true;
            SetPseudoClass();
        }
    }

    public int Uuid = CoreUtils.GetUuid();
    
    public static readonly StyledProperty<string> LogoProperty = AvaloniaProperty.Register<MyRadioButton, string>(
        nameof(Logo));

    public string Logo
    {
        get => GetValue(LogoProperty);
        set
        {
            SetValue(LogoProperty, value);
            if (_shapeLogo != null)
            {
                _shapeLogo.Data = Geometry.Parse(value);
            }
        }
    }

    public static readonly StyledProperty<double> LogoScaleProperty = AvaloniaProperty.Register<MyRadioButton, double>(
        nameof(LogoScale),
        1);

    public double LogoScale
    {
        get => GetValue(LogoScaleProperty);
        set
        {
            SetValue(LogoScaleProperty, value);
            if (_shapeLogo != null)
            {
                _shapeLogo.RenderTransform = new ScaleTransform { ScaleX = value, ScaleY = value };
            }
        }
    }
    
    public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<MyRadioButton, string>(
        nameof(Text),
        string.Empty);

    public string Text
    {
        get => GetValue(TextProperty);
        set
        {
            SetValue(TextProperty, value);
            if (_labText != null)
            {
                _labText.Text = value;
            }
        }
    }

    public enum ColorState
    {
        White,
        HighLight
    }
    
    public static readonly StyledProperty<ColorState> ColorTypeProperty = 
        AvaloniaProperty.Register<MyRadioButton, ColorState>(nameof(ColorType));

    public ColorState ColorType
    {
        get => GetValue(ColorTypeProperty);
        set
        {
            SetValue(ColorTypeProperty, value);
            SetPseudoClass();
        }
    }

    public new static readonly StyledProperty<IBrush> BackgroundProperty = AvaloniaProperty.Register<MyRadioButton, IBrush>(
        nameof(Background));

    public new IBrush Background
    {
        get => GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    public new static readonly StyledProperty<IBrush> ForegroundProperty = AvaloniaProperty.Register<MyRadioButton, IBrush>(
        nameof(Foreground));

    public new IBrush Foreground
    {
        get => GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }
    
    public static readonly StyledProperty<bool> CheckedProperty = AvaloniaProperty.Register<MyRadioButton, bool>(
        nameof(Checked));

    public bool Checked
    {
        get => GetValue(CheckedProperty);
        set
        {
            SetValue(CheckedProperty, value);
            SetCheck();
            SetPseudoClass();
        }
    }

    private void SetCheck()
    {
        if (this.Parent is Panel parent)
        {
            foreach (var child in parent.Children)
            {
                if (child is MyRadioButton radioButton && radioButton != this)
                {
                    radioButton.Checked = false;
                }
            }
        }
    }
    
    private void SetPseudoClass()
    {
        PseudoClasses.Set(":checked", Checked);
        PseudoClasses.Set(":pressed", _isMouseDown);
        
        switch (ColorType)
        {
            case ColorState.White:
                PseudoClasses.Set(":white", true);
                break;
            case ColorState.HighLight:
                PseudoClasses.Set(":highlight", true);
                break;
        }
    }
    
    private void RefreshColor()
    {
        if (_shapeLogo is null || _labText is null) return;
        switch (ColorType)
        {
            case ColorState.White:
                if (Checked)
                {
                    _panBack!.Background = (SolidColorBrush)new MyColor(255, 255, 255);
                    _shapeLogo.Fill = (IBrush?)Application.Current!.Resources["ColorBrush3"];
                    _labText.Foreground = (IBrush?)Application.Current!.Resources["ColorBrush3"];
                }
                else
                {
                    _panBack!.Background = (SolidColorBrush)ThemeHelper.ColorSemiTransparent;
                    _shapeLogo.Fill = (SolidColorBrush)new MyColor(255, 255, 255);
                    _labText.Foreground = (SolidColorBrush)new MyColor(255, 255, 255);
                }
                break;
            case ColorState.HighLight:
                if (Checked)
                {
                    _panBack!.Background = (IBrush?)Application.Current!.Resources["ColorBrush3"];
                    _shapeLogo.Fill = (SolidColorBrush)new MyColor(255, 255, 255);
                    _labText.Foreground = (SolidColorBrush)new MyColor(255, 255, 255);
                }
                else
                {
                    _panBack!.Background = (SolidColorBrush)ThemeHelper.ColorSemiTransparent;
                    _shapeLogo.Fill = (IBrush?)Application.Current!.Resources["ColorBrush3"];
                    _labText.Foreground = (IBrush?)Application.Current!.Resources["ColorBrush3"];
                }
                break;
        }
    }
}