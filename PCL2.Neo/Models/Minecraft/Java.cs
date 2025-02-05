using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PCL2.Neo.Models.Minecraft.JavaSearcher;

namespace PCL2.Neo.Models.Minecraft
{
    public class Java
    {
        /// <summary>
        /// 搜索系统中安装的Java环境。
        /// </summary>
        /// <returns>返回一个包含系统中找到的所有Java环境实体的列表。</returns>
        /// <exception cref="PlatformNotSupportedException">当运行平台不被支持时抛出此异常。</exception>
        public static async Task<List<JavaEntity>> SearchJava()
        {
            // 根据操作系统类型搜索Java
            var javaList = new List<JavaEntity>();

            switch (Environment.OSVersion.Platform)
            {
                // 在Windows平台上搜索Java，并将结果添加到javaList列表中
                case PlatformID.Win32NT:
                    javaList.AddRange(await PCL2.Neo.Models.Minecraft.JavaSearcher.Windows.SearchJava());
                    break;
                // 在Unix平台上搜索Java，并将结果添加到javaList列表中
                case PlatformID.Unix:
                    javaList.AddRange(Unix.SerachJava());
                    break;
                // 如果是其他平台，抛出不支持的平台异常
                default:
                    throw new PlatformNotSupportedException();
            }

            return javaList;
        }
    }
}
