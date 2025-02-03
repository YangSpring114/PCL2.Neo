using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace PCL2.Neo.Models.Minecraft
{
    // TODO: 优化：延时执行获取版本
    public class JavaEntity
    {
        public JavaEntity(string path)
        {
            Path = path.EndsWith("bin\\") ? path : System.IO.Path.Combine(path, "bin\\");
            GetJavaInfo();
        }

        public string Path { set; get; }
        public int Version { set; get; }
        public string JavaExe => Path + "java.exe";
        public string JavaWExe => Path + "javaw.exe";
        public bool IsJdk => File.Exists(Path + "\\javac.exe");

        public bool IsExist()
        {
            return Directory.Exists(Path);
        }

        public bool Is64Bit { get; set; }

        private bool SetVersion(string version)
        {
            var match = Regex.Match(version, @"(\d+)\.");
            Version = match.Success ? int.Parse(match.Groups[1].Value) : 0;

            return match.Success;
        }

        private bool SetBit(string bit)
        {
            if (bit == string.Empty)
            {
                Is64Bit = false;
                return false;
            }
            else
            {
                Is64Bit = "64".Equals(bit);
                return true;
            }
        }

        public void GetJavaInfo()
        {
            //get java output by process
            using var javaProcess = new Process();
            javaProcess.StartInfo = new ProcessStartInfo
            {
                FileName = JavaExe,
                Arguments = "-version",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            javaProcess.Start();
            var output = javaProcess.StandardOutput.ReadToEnd();

            // get java version code
            var javaVersionMatch = Regex.Match(output, """version "([\d._]+)""");
            var version = javaVersionMatch.Success ? javaVersionMatch.Groups[1].Value : string.Empty;
            SetVersion(version);

            // get java bit
            var javaBitMatch = Regex.Match(output, @"\b(\d+)-Bit\b");
            var bit = javaBitMatch.Success ? javaBitMatch.Groups[1].Value : string.Empty;
            SetBit(bit);
        }
    }
}
