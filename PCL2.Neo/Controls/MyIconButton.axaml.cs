using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using PCL2.Neo.Utils;

namespace PCL2.Neo.Controls;

public class MyIconButton : TemplatedControl
{
    private Path _pathIcon;
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _pathIcon = this.FindControl<Path>("PathIcon");
    }
    
    public int Uuid = CoreUtils.GetUuid();
    
    public static readonly StyledProperty<string> LogoProperty = AvaloniaProperty.Register<MyIconButton, string>(nameof(Logo));
    public string Logo
    {
        get => GetValue(LogoProperty);
        set
        {
            if (_pathIcon != null)
            {
                _pathIcon.Data = Geometry.Parse(value);
            }
            SetValue(LogoProperty, value);
        }
    }
    public static readonly StyledProperty<double> LogoScaleProperty = AvaloniaProperty.Register<MyIconButton, double>(nameof(LogoScale), 1);
    public double LogoScale
    {
        get => GetValue(LogoScaleProperty);
        set
        {
            if (_pathIcon != null)
            {
                _pathIcon.RenderTransform = new ScaleTransform{ ScaleX = value, ScaleY = value };
            }
            SetValue(LogoScaleProperty, value);
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

    public static readonly StyledProperty<IconThemes> IconThemeProperty = AvaloniaProperty.Register<MyIconButton, IconThemes>(nameof(IconTheme), IconThemes.Color);
    public IconThemes IconTheme
    {
        get => GetValue(IconThemeProperty);
        set => SetValue(IconThemeProperty, value);
    }

    private void RefreshAnimation()
    {
        
    }
}