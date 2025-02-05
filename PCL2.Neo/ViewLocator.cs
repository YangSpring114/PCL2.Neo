using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using PCL2.Neo.ViewModels;

namespace PCL2.Neo
{
    /// <summary>
    /// 视图定位器类，实现了IDataTemplate接口，用于根据ViewModel动态创建对应的View。
    /// </summary>
    public class ViewLocator : IDataTemplate
    {
        /// <summary>
        /// 根据提供的参数构建并返回一个Control实例。如果参数为null，则返回null。
        /// </summary>
        /// <param name="param">要转换为视图的对象，通常是一个ViewModel。</param>
        /// <returns>返回与ViewModel关联的视图Control实例，如果没有找到匹配的视图，则返回一个显示"Not Found: [视图名称]"文本的TextBlock。</returns>
        public Control? Build(object? param)
        {
            if (param is null)
                return null;

            // 将ViewModel的完整类型名中的"ViewModel"替换为"View"以获取对应的视图类型名。
            var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
            var type = Type.GetType(name);

            if (type != null)
            {
                // 使用反射创建指定类型的实例。
                return (Control)Activator.CreateInstance(type)!;
            }

            // 如果找不到对应的视图类型，则返回一个显示未找到信息的TextBlock。
            return new TextBlock { Text = "Not Found: " + name };
        }
        /// <summary>
        /// 判断提供的数据对象是否匹配该模板。这里判断数据对象是否是ViewModelBase类型或其子类型。
        /// </summary>
        /// <param name="data">要检查的数据对象。</param>
        /// <returns>如果数据对象是ViewModelBase类型或其子类型，则返回true；否则返回false。</returns>
        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }
    }
}
