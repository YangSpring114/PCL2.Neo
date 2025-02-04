using System.Threading.Tasks;
using Avalonia.Animation;

namespace PCL2.Neo.Animations;

public interface IAnimation
{
    /// <summary>
    /// 要动画的控件。
    /// </summary>
    Animatable Control { get; set; }
    /// <summary>
    /// 动画类。
    /// </summary>
    Animation Animation { get; }
    /// <summary>
    /// 异步形式执行动画。
    /// </summary>
    Task RunAsync();
}