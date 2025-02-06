using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Media;
using Avalonia.Styling;
using System;

namespace PCL2.Neo.Animations
{
    public class RotateTransformAngleAnimation : IAnimation
    {
        public Animatable Control { get; set; }
        public Animation Animation { get; }
        public TimeSpan Duration { get; set; }
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
        public RotateTransformAngleAnimation(Animatable control, TimeSpan duration, double valueAfter, Easing easing) : this(
            control, duration, control.GetValue(RotateTransform.AngleProperty), valueAfter, easing)
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
        public RotateTransformAngleAnimation(Animatable control, TimeSpan duration, double? valueBefore, double valueAfter, Easing easing)
        {
            Control = control;
            Duration = duration;
            ValueBefore = valueBefore;
            ValueAfter = valueAfter;
            Easing = easing;
            Animation = new Animation
            {
                Easing = easing,
                Duration = duration,
                Children =
                {
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter(RotateTransform.AngleProperty, valueBefore)
                        },
                        Cue = new Cue(0d)
                    },
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter(RotateTransform.AngleProperty, valueAfter)
                        },
                        Cue = new Cue(1d)
                    }
                }
            };
        }

        public async void RunAsync()
        {
            await Animation.RunAsync(Control);
        }
    }
}