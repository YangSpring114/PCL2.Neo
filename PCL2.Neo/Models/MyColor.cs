using System;
using Avalonia.Media;
using Color = Avalonia.Media.Color;

namespace PCL2.Neo.Models;

/// <summary>
/// 提供，操作颜色的类
/// </summary>
public class MyColor
{
    /// <summary>
    /// 获取或设置颜色的透明度值。默认值为255（完全不透明）。
    /// Gets or sets the transparency value of the color. The default value is 255 (fully opaque).
    /// </summary>
    public double A
    {
        get => _a;
        set => _a = Clamp(value, 0, 255);
    }
    private double _a = 255;

    /// <summary>
    /// 获取或设置颜色的红色成分值。
    /// Gets or sets the red component value of the color.
    /// </summary>
    public double R
    {
        get => _r;
        set => _r = Clamp(value, 0, 255);
    }
    private double _r;

    /// <summary>
    /// 获取或设置颜色的绿色成分值。
    /// Gets or sets the green component value of the color.
    /// </summary>
    public double G
    {
        get => _g;
        set => _g = Clamp(value, 0, 255);
    }
    private double _g;

    /// <summary>
    /// 获取或设置颜色的蓝色成分值。
    /// Gets or sets the blue component value of the color.
    /// </summary>
    public double B
    {
        get => _b;
        set => _b = Clamp(value, 0, 255);
    }
    private double _b;

    // 类型转换
    /// <summary>
    /// 隐式类型转换操作符，将字符串转换为MyColor对象。需要实现从字符串到颜色的解析逻辑。
    /// Implicit type conversion operator that converts a string to a MyColor object. Requires implementation of logic to parse from string to color.
    /// </summary>
    /// <param name="str">表示颜色的字符串。</param>
    public static implicit operator MyColor(string str)
    {
        return new MyColor(str);
    }

    /// <summary>
    /// 隐式类型转换操作符，将系统默认的Color对象转换为MyColor对象。
    /// Implicit type conversion operator that converts a system's default Color object into a MyColor object.
    /// </summary>
    /// <param name="col">要转换的Color对象。</param>
    public static implicit operator MyColor(Color col)
    {
        return new MyColor(col);
    }

    /// <summary>
    /// 隐式类型转换操作符，将MyColor对象转换回系统默认的Color对象。
    /// Implicit type conversion operator that converts a MyColor object back to the system's default Color object.
    /// </summary>
    /// <param name="conv">要转换的MyColor对象。</param>
    public static implicit operator Color(MyColor conv)
    {
        return Color.FromArgb((byte)Clamp(conv.A, 0, 255), (byte)Clamp(conv.R, 0, 255), (byte)Clamp(conv.G, 0, 255), (byte)Clamp(conv.B, 0, 255));
    }

    /// <summary>
    /// 隐式类型转换操作符，将MyColor对象转换为System.Drawing.Color对象。
    /// Implicit type conversion operator that converts a MyColor object to a System.Drawing.Color object.
    /// </summary>
    /// <param name="conv">要转换的MyColor对象。</param>
    public static implicit operator System.Drawing.Color(MyColor conv)
    {
        return System.Drawing.Color.FromArgb((byte)Clamp(conv.A, 0, 255), (byte)Clamp(conv.R, 0, 255), (byte)Clamp(conv.G, 0, 255), (byte)Clamp(conv.B, 0, 255));
    }

    /// <summary>
    /// 隐式类型转换操作符，将SolidColorBrush对象转换为MyColor对象。
    /// Implicit type conversion operator that converts a SolidColorBrush object into a MyColor object.
    /// </summary>
    /// <param name="bru">要转换的SolidColorBrush对象。</param>
    public static implicit operator MyColor(SolidColorBrush bru)
    {
        return new MyColor(bru.Color);
    }
    /// <summary>
    /// 隐式类型转换操作符，将MyColor对象转换为SolidColorBrush对象。
    /// Implicit type conversion operator that converts a MyColor object into a SolidColorBrush object.
    /// </summary>
    /// <param name="conv">要转换的MyColor对象。</param>
    public static implicit operator SolidColorBrush(MyColor conv)
    {
        return new SolidColorBrush(Color.FromArgb((byte)Clamp(conv.A, 0, 255), (byte)Clamp(conv.R, 0, 255), (byte)Clamp(conv.G, 0, 255), (byte)Clamp(conv.B, 0, 255)));
    }
    /// <summary>
    /// 隐式类型转换操作符，将Brush对象转换为MyColor对象。需要具体实现根据Brush类型进行转换的逻辑。
    /// Implicit type conversion operator that converts a Brush object into a MyColor object. Requires specific implementation logic for conversion based on Brush type.
    /// </summary>
    /// <param name="bru">要转换的Brush对象。</param>
    public static implicit operator MyColor(Brush bru)
    {
        return new MyColor(bru);
    }
    /// <summary>
    /// 隐式类型转换操作符，将MyColor对象转换为Brush对象。通常转换为目标框架支持的Brush类型，如SolidColorBrush。
    /// Implicit type conversion operator that converts a MyColor object into a Brush object. Typically converts to a supported Brush type in the target framework, such as SolidColorBrush.
    /// </summary>
    /// <param name="conv">要转换的MyColor对象。</param>
    public static implicit operator Brush(MyColor conv)
    {
        return new SolidColorBrush(Color.FromArgb((byte)Clamp(conv.A, 0, 255), (byte)Clamp(conv.R, 0, 255), (byte)Clamp(conv.G, 0, 255), (byte)Clamp(conv.B, 0, 255)));
    }

    // 颜色运算

    /// <summary>
    /// 定义两个MyColor对象相加的操作符。
    /// Defines the operator for adding two MyColor objects.
    /// </summary>
    /// <param name="a">第一个MyColor对象。</param>
    /// <param name="b">第二个MyColor对象。</param>
    /// <returns>返回一个新的MyColor对象，其各组件值为两操作数对应组件值之和。</returns>
    public static MyColor operator +(MyColor a, MyColor b)
    {
        return new MyColor { A = a.A + b.A, B = a.B + b.B, G = a.G + b.G, R = a.R + b.R };
    }

    /// <summary>
    /// 定义两个MyColor对象相减的操作符。
    /// Defines the operator for subtracting two MyColor objects.
    /// </summary>
    /// <param name="a">第一个MyColor对象。</param>
    /// <param name="b">第二个MyColor对象。</param>
    /// <returns>返回一个新的MyColor对象，其各组件值为两操作数对应组件值之差。</returns>
    public static MyColor operator -(MyColor a, MyColor b)
    {
        return new MyColor { A = a.A - b.A, B = a.B - b.B, G = a.G - b.G, R = a.R - b.R };
    }
    /// <summary>
    /// 定义一个MyColor对象与一个双精度数值相乘的操作符。
    /// Defines the operator for multiplying a MyColor object by a double value.
    /// </summary>
    /// <param name="a">MyColor对象。</param>
    /// <param name="b">双精度数值。</param>
    /// <returns>返回一个新的MyColor对象，其各组件值为原组件值乘以给定的数值。</returns>
    public static MyColor operator *(MyColor a, double b)
    {
        return new MyColor { A = a.A * b, B = a.B * b, G = a.G * b, R = a.R * b };
    }
    /// <summary>
    /// 定义一个MyColor对象除以一个双精度数值的操作符。
    /// Defines the operator for dividing a MyColor object by a double value.
    /// </summary>
    /// <param name="a">MyColor对象。</param>
    /// <param name="b">双精度数值。</param>
    /// <returns>返回一个新的MyColor对象，其各组件值为原组件值除以给定的数值。</returns>
    public static MyColor operator /(MyColor a, double b)
    {
        return new MyColor { A = a.A / b, B = a.B / b, G = a.G / b, R = a.R / b };
    }
    /// <summary>
    /// 重载等于操作符，比较两个MyColor对象是否相等。
    /// Overloads the equality operator to compare whether two MyColor objects are equal.
    /// </summary>
    /// <param name="a">第一个MyColor对象。</param>
    /// <param name="b">第二个MyColor对象。</param>
    /// <returns>如果两个对象的所有组件值都相等，则返回true；否则返回false。</returns>
    public static bool operator ==(MyColor a, MyColor b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
        return a.A == b.A && a.R == b.R && a.G == b.G && a.B == b.B;
    }
    /// <summary>
    /// 重载不等于操作符，比较两个MyColor对象是否不相等。
    /// Overloads the inequality operator to compare whether two MyColor objects are not equal.
    /// </summary>
    /// <param name="a">第一个MyColor对象。</param>
    /// <param name="b">第二个MyColor对象。</param>
    /// <returns>如果两个对象的任意组件值不相等，则返回true；否则返回false。</returns>
    public static bool operator !=(MyColor a, MyColor b)
    {
        return !(a == b);
    }

    // 构造函数
    /// <summary>
    /// 默认构造函数，初始化一个新的MyColor实例。默认透明度值为255（完全不透明），其他颜色组件值未初始化。
    /// Default constructor that initializes a new instance of MyColor. The default transparency value is 255 (fully opaque), other color component values are uninitialized.
    /// </summary>
    public MyColor()
    {
    }
    /// <summary>
    /// 将系统默认的Color对象转换为此自定义的MyColor对象。
    /// Color conversion function used to convert a system's default Color object into this custom MyColor object.
    /// </summary>
    /// <param name="col">需要转换的Color对象。</param>
    public MyColor(Color col)
    {
        A = col.A;
        R = col.R;
        G = col.G;
        B = col.B;
    }
    /// <summary>
    /// 将Hex字符串转换为此自定义的MyColor对象。
    /// Color conversion function used to convert a Hex string into this custom MyColor object.
    /// </summary>
    /// <param name="hexString">需要转换的Hex字符串。</param>
    public MyColor(string hexString)
    {
        Color stringColor = Color.Parse(hexString);
        A = stringColor.A;
        R = stringColor.R;
        G = stringColor.G;
        B = stringColor.B;
    }

    /// <summary>
    /// 创建一个具有指定透明度和基于另一个MyColor对象的颜色组件的新MyColor对象。
    /// Constructor that creates a new MyColor object with specified transparency and color components based on another MyColor object.
    /// </summary>
    /// <param name="newA">新的透明度值。</param>
    /// <param name="col">基于此对象的颜色组件值。</param>
    public MyColor(double newA, MyColor col)
    {
        A = newA;
        R = col.R;
        G = col.G;
        B = col.B;
    }
    /// <summary>
    /// 创建一个具有指定红色、绿色和蓝色组件的新MyColor对象，默认透明度值为255（完全不透明）。
    /// Constructor that creates a new MyColor object with specified red, green, and blue components, the default transparency value is 255 (fully opaque).
    /// </summary>
    /// <param name="newR">新的红色组件值。</param>
    /// <param name="newG">新的绿色组件值。</param>
    /// <param name="newB">新的蓝色组件值。</param>
    public MyColor(double newR, double newG, double newB)
    {
        A = 255;
        R = newR;
        G = newG;
        B = newB;
    }
    /// <summary>
    /// 创建一个具有指定透明度、红色、绿色和蓝色组件的新MyColor对象。
    /// Constructor that creates a new MyColor object with specified transparency, red, green, and blue components.
    /// </summary>
    /// <param name="newA">新的透明度值。</param>
    /// <param name="newR">新的红色组件值。</param>
    /// <param name="newG">新的绿色组件值。</param>
    /// <param name="newB">新的蓝色组件值。</param>
    public MyColor(double newA, double newR, double newG, double newB)
    {
        A = newA;
        R = newR;
        G = newG;
        B = newB;
    }
    /// <summary>
    /// 通过Brush对象创建一个新的MyColor对象。
    /// Constructor that creates a new MyColor object from a Brush object.
    /// </summary>
    /// <param name="brush">用于创建新MyColor对象的Brush对象。</param>
    public MyColor(Brush brush)
    {
        SolidColorBrush solidBrush = (SolidColorBrush)brush;
        Color color = solidBrush.Color;
        A = color.A;
        R = color.R;
        G = color.G;
        B = color.B;
    }
    /// <summary>
    /// 通过SolidColorBrush对象创建一个新的MyColor对象。
    /// Constructor that creates a new MyColor object from a SolidColorBrush object.
    /// </summary>
    /// <param name="brush">用于创建新MyColor对象的SolidColorBrush对象。</param>
    public MyColor(SolidColorBrush brush)
    {
        Color color = brush.Color;
        A = color.A;
        R = color.R;
        G = color.G;
        B = color.B;
    }

    // HSL转换
    /// <summary>
    /// 根据给定的参数计算色调值。
    /// Calculates the hue value based on the given parameters.
    /// </summary>
    /// <param name="v1">第一个中间值。</param>
    /// <param name="v2">第二个中间值。</param>
    /// <param name="vH">色调值，范围从0到1。</param>
    /// <returns>返回计算出的色调值。</returns>
    public double Hue(double v1, double v2, double vH)
    {
        if (vH < 0) vH += 1;
        if (vH > 1) vH -= 1;
        if (vH < 0.16667) return v1 + (v2 - v1) * 6 * vH;
        if (vH < 0.5) return v2;
        if (vH < 0.66667) return v1 + (v2 - v1) * (4 - vH * 6);
        return v1;
    }
    /// <summary>
    /// 将HSL颜色模型转换为当前MyColor对象的颜色值（RGB）。
    /// Converts a color from the HSL color model to the current MyColor object's color values (RGB).
    /// </summary>
    /// <param name="sH">色调值，范围从0到360。</param>
    /// <param name="sS">饱和度值，范围从0到100。</param>
    /// <param name="sL">亮度值，范围从0到100。</param>
    /// <returns>返回当前MyColor对象，包含转换后的颜色值。</returns>
    public MyColor FromHsl(double sH, double sS, double sL)
    {
        if (sS == 0)
        {
            R = sL * 2.55;
            G = R;
            B = R;
        }
        else
        {
            double h = sH / 360;
            double s = sS / 100;
            double l = sL / 100;
            s = l < 0.5 ? s * l + l : s * (1.0 - l) + l;
            l = 2 * l - s;
            R = 255 * Hue(l, s, h + 1 / 3.0);
            G = 255 * Hue(l, s, h);
            B = 255 * Hue(l, s, h - 1 / 3.0);
        }
        A = 255;
        return this;
    }
    /// <summary>
    /// 使用改进的HSL算法将颜色转换为当前MyColor对象的颜色值（RGB），考虑了亮度调整。
    /// Converts a color from an enhanced HSL algorithm to the current MyColor object's color values (RGB), considering brightness adjustments.
    /// </summary>
    /// <param name="sH">色调值，范围从0到360。</param>
    /// <param name="sS">饱和度值，范围从0到100。</param>
    /// <param name="sL">亮度值，范围从0到100。</param>
    /// <returns>返回当前MyColor对象，包含转换后的颜色值。</returns>
    public MyColor FromHsl2(double sH, double sS, double sL)
    {
        if (sS == 0)
        {
            R = sL * 2.55;
            G = R;
            B = R;
        }
        else
        {
            sH = (sH + 3600000) % 360;
            double[] cent = {
                +0.1, -0.06, -0.3, // 0, 30, 60
                -0.19, -0.15, -0.24, // 90, 120, 150
                -0.32, -0.09, +0.18, // 180, 210, 240
                +0.05, -0.12, -0.02, // 270, 300, 330
                +0.1, -0.06}; // 最后两位与前两位一致，加是变亮，减是变暗
            double center = sH / 30.0;
            int intCenter = (int)Math.Floor(center);  // 亮度片区编号
            center = 50 - (
                (1 - center + intCenter) * cent[intCenter] + (center - intCenter) * cent[intCenter + 1]
            ) * sS;

            sL = sL < center ? sL / center : 1 + (sL - center) / (100 - center);
            sL *= 50;
            FromHsl(sH, sS, sL);
        }
        A = 255;
        return this;
    }

    /// <summary>
    /// 将当前MyColor对象的属性值转换为字符串表示形式。
    /// Converts the property values of the current MyColor object to a string representation.
    /// </summary>
    /// <returns>返回一个包含透明度（A）、红色（R）、绿色（G）和蓝色（B）组件值的字符串。</returns>
    public override string ToString()
    {
        return $"({A},{R},{G},{B})";
    }

    /// <summary>
    /// 重写Equals方法，用于比较两个对象是否相等。这里通过强制转换并使用已定义的==操作符进行比较。
    /// Overrides the Equals method to compare whether two objects are equal. Here it compares by casting and using the defined == operator.
    /// </summary>
    /// <param name="obj">要与此实例进行比较的对象。</param>
    /// <returns>如果指定的对象与此实例表示相同的颜色，则返回true；否则返回false。</returns>
    public override bool Equals(object obj)
    {
        return this == (MyColor)obj;
    }

    /// <summary>
    /// 静态方法，用于将一个值限制在指定的最小值和最大值之间。
    /// Static method used to clamp a value between a specified minimum and maximum value.
    /// </summary>
    /// <param name="value">要限制的值。</param>
    /// <param name="min">允许的最小值。</param>
    /// <param name="max">允许的最大值。</param>
    /// <returns>返回被限制在[min, max]范围内的值。</returns>
    public static double Clamp(double value, double min, double max)
    {
        return Math.Min(Math.Max(value, min), max);
    }
}