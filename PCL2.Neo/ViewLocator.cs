using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using PCL2.Neo.ViewModels;

namespace PCL2.Neo
{
    /// <summary>
    /// 视图定位器类，实现了IDataTemplate接口，用于根据ViewModel动态创建对应的View。
    /// View locator class that implements the IDataTemplate interface, used for dynamically creating corresponding Views based on ViewModels.
    /// </summary>
    public class ViewLocator : IDataTemplate
    {
        /// <summary>
        /// 根据提供的参数构建并返回一个Control实例。如果参数为null，则返回null。
        /// Builds and returns a Control instance based on the provided parameter. Returns null if the parameter is null.
        /// </summary>
        /// <param name="param">要转换为视图的对象，通常是一个ViewModel。</param>
        /// <returns>返回与ViewModel关联的视图Control实例，如果没有找到匹配的视图，则返回一个显示"Not Found: [视图名称]"文本的TextBlock。</returns>
        public Control? Build(object? param)
        {
            if (param is null)
                return null;

            // 将ViewModel的完整类型名中的"ViewModel"替换为"View"以获取对应的视图类型名。
            // Replaces "ViewModel" with "View" in the full type name of the ViewModel to get the corresponding view type name.
            var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
            var type = Type.GetType(name);

            if (type != null)
            {
                // 使用反射创建指定类型的实例。
                // Creates an instance of the specified type using reflection.
                return (Control)Activator.CreateInstance(type)!;
            }

            // 如果找不到对应的视图类型，则返回一个显示未找到信息的TextBlock。
            // If no matching view type is found, returns a TextBlock displaying not found information.
            return new TextBlock { Text = "Not Found: " + name };
        }
        /// <summary>
        /// 判断提供的数据对象是否匹配该模板。这里判断数据对象是否是ViewModelBase类型或其子类型。
        /// Determines whether the provided data object matches this template. Here it checks if the data object is of type ViewModelBase or its subtypes.
        /// </summary>
        /// <param name="data">要检查的数据对象。</param>
        /// <returns>如果数据对象是ViewModelBase类型或其子类型，则返回true；否则返回false。</returns>
        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }
    }
}
