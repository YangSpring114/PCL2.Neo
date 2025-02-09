using PCL2.Neo.Animations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PCL2.Neo.Helpers;

/// <summary>
/// 动画帮助类，用来同时执行不同动画。
/// </summary>
public class AnimationHelper
{
    public List<IAnimation> Animations { get; set; }
    public List<Task> Tasks { get; }

    public AnimationHelper() : this(new List<IAnimation>())
    {
    }
    public AnimationHelper(List<IAnimation> animations)
    {
        Animations = animations;
        Tasks = new List<Task>();
    }

    public async Task RunAsync()
    {
        Tasks.Clear();
        foreach (IAnimation animation in Animations)
        {
            Tasks.Add(animation.RunAsync());
        }
        await Task.WhenAll(Tasks);
    }
    public void Cancel()
    {
        foreach (IAnimation animation in Animations)
        {
            animation.Cancel();
        }
    }
    public void CancelAndClear()
    {
        Cancel();
        Animations.Clear();
    }
}