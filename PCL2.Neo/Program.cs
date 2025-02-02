using System;
using Avalonia;

namespace PCL2.Neo
{
    internal sealed class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        /// <summary>
        /// 初始化代码。在调用AppMain之前，不要使用任何Avalonia、第三方API或任何依赖SynchronizationContext的代码：因为它们还没有初始化，可能会导致错误。
        /// Initialization code. Don't use any Avalonia, third-party APIs or any SynchronizationContext-reliant code before AppMain is called: things aren't initialized yet and stuff might break.
        /// </summary>
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        /// <summary>
        /// Avalonia配置，请勿移除；也用于可视化设计器。
        /// Avalonia configuration, don't remove; also used by visual designer.
        /// </summary>
        /// <returns>返回配置好的应用程序构建器。</returns>
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}
