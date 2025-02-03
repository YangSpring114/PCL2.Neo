using System;
using System.Collections.Generic;
using PCL2.Neo.Models.Minecraft.JavaSearcher;

namespace PCL2.Neo.Models.Minecraft
{
    public class Java
    {
        public static List<JavaEntity> SearchJava()
        {
            var javaList = new List<JavaEntity>();

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    javaList.AddRange(PCL2.Neo.Models.Minecraft.JavaSearcher.Windows.SearchJava());
                    break;
                case PlatformID.Unix:
                    javaList.AddRange(Unix.SerachJava());
                    break;
                default:
                    throw new PlatformNotSupportedException();
            }

            return javaList;
        }
    }
}
