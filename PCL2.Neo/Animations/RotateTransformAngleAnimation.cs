using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Threading.Tasks;

namespace PCL2.Neo.Animations
{
    public class RotateTransformAngleAnimation : IAnimation
    {
        public Animatable Control { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeSpan Delay { get; set; }
        public double? ValueBefore { get; set; }
        public double ValueAfter { get; set; }
        public Easing Easing { get; set; }

        public RotateTransformAngleAnimation(Animatable control, double valueAfter) : this(
            control, valueAfter, new LinearEasing())
        {
        }
        public RotateTransformAngleAnimation(Animatable control, double valueAfter, Easing easing) : this(
            control, TimeSpan.FromSeconds(1), valueAfter, easing)
        {
        }
        public RotateTransformAngleAnimation(Animatable control, TimeSpan duration, double valueAfter) : this(
            control, duration, valueAfter, new LinearEasing())
        {
        }
        public RotateTransformAngleAnimation(Animatable control, TimeSpan duration, TimeSpan delay, double valueAfter) : this(
            control, duration, delay, valueAfter, new LinearEasing())
        {
        }
        public RotateTransformAngleAnimation(Animatable control, TimeSpan duration, double valueAfter, Easing easing) : this(
            control, duration, control.GetValue(RotateTransform.AngleProperty), valueAfter, easing)
        {
        }
        public RotateTransformAngleAnimation(Animatable control, TimeSpan duration, TimeSpan delay, double valueAfter, Easing easing) : this(
            control, duration, delay, control.GetValue(RotateTransform.AngleProperty), valueAfter, easing)
        {
        }
        public RotateTransformAngleAnimation(Animatable control, double? valueBefore, double valueAfter) : this(
            control, valueBefore, valueAfter, new LinearEasing())
        {
        }
        public RotateTransformAngleAnimation(Animatable control, double? valueBefore, double valueAfter, Easing easing) : this(
            control, TimeSpan.FromSeconds(1), valueBefore, valueAfter, easing)
        {
        }
        public RotateTransformAngleAnimation(Animatable control, TimeSpan duration, double? valueBefore, double valueAfter) : this(
            control, duration, valueBefore, valueAfter, new LinearEasing())
        {
        }
        public RotateTransformAngleAnimation(Animatable control, TimeSpan duration, TimeSpan delay, double? valueBefore, double valueAfter) : this(
            control, duration, delay, valueBefore, valueAfter, new LinearEasing())
        {
        }
        public RotateTransformAngleAnimation(Animatable control, TimeSpan duration, double? valueBefore, double valueAfter, Easing easing) : this(
            control, duration, TimeSpan.Zero, valueBefore, valueAfter, easing)
        {
        }
        public RotateTransformAngleAnimation(Animatable control, TimeSpan duration, TimeSpan delay, double? valueBefore, double valueAfter, Easing easing)
        {
            Control = control;
            Duration = duration;
            Delay = delay;
            ValueBefore = valueBefore;
            ValueAfter = valueAfter;
            Easing = easing;
        }

        public async Task RunAsync()
        {
            var animation = new Animation
            {
                Easing = Easing,
                Duration = Duration,
                Delay = Delay,
                FillMode = FillMode.Both,
                Children =
                {
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter(RotateTransform.AngleProperty, ValueBefore)
                        },
                        Cue = new Cue(0d)
                    },
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter(RotateTransform.AngleProperty, ValueAfter)
                        },
                        Cue = new Cue(1d)
                    }
                }
            };
            await animation.RunAsync(Control);
        }
    }
}