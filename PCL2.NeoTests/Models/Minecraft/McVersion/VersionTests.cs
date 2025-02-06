using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;
using System.IO;
using PCL2.Neo.Models.Minecraft.McVersion.VersionData;

namespace PCL2.Neo.Models.Minecraft.McVersion.VersionData.Tests
{
    [TestClass()]
    public class VersionTests
    {
        [TestMethod()]
        public void ParserTest()
        {
            var fileInfo = System.IO.File.ReadAllText(
                @"C:\Users\WhiteCAT\Desktop\Games\PCL2\.minecraft\versions\1.19.2-Fabric 0.14.10\1.19.2-Fabric 0.14.10.json");
            Console.WriteLine(Version.Parser(fileInfo));
        }
    }
}