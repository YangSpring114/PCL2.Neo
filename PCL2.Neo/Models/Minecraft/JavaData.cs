using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace PCL2.Neo.Models.Minecraft
{
    /// <summary>
    /// 表示Java环境是否存在以及其路径的信息记录。
    /// </summary>
    internal record JavaExist
    {
        /// <summary>
        /// 获取或设置一个值，该值指示Java环境是否存在。
        /// </summary>
        public required bool IsExist { get; set; }

        /// <summary>
        /// 获取或设置Java环境的安装路径。
        /// </summary>
        public required string Path { get; set; }
    }
    /// <summary>
    /// 表示一个Java环境实体，包含有关Java安装位置和其他相关信息。
    /// </summary>
    public class JavaEntity(string path)
    {
        /// <summary>
        /// 获取或设置Java环境的路径，确保以反斜杠结尾。
        /// </summary>
        public string Path = path.EndsWith('\\') ? path : path + '\\';

        /// <summary>
        /// 获取或设置一个值，指示该Java环境是否可用。
        /// </summary>
        public bool IsUseable = true;

        /// <summary>
        /// 私有的可空整型字段，存储Java版本号。
        /// </summary>
        private int? _version;

        /// <summary>
        /// 获取Java环境的主版本号。
        /// </summary>
        /// <returns>返回Java环境的主版本号。</returns>
        public int Version
        {
            get
            {
                if (_version != null)
                {
                    return _version.Value;
                }

                // 使用正则表达式从输出中提取Java版本号
                // get java version code
                var javaVersionMatch = Regex.Match(Output, """version "([\d._]+)""");
                var match = Regex.Match(javaVersionMatch.Success ? javaVersionMatch.Groups[1].Value : string.Empty,
                    @"^(\d+)\.");
                _version = match.Success ? int.Parse(match.Groups[1].Value) : 0;

                if (_version == 1)
                {
                    // 处理Java 8及更早版本的特殊情况
                    // java version 8
                    match = Regex.Match(javaVersionMatch.Groups[1].Value, @"^1\.(\d+)\.");
                    _version = match.Success ? int.Parse(match.Groups[1].Value) : 0;

                    return _version.Value;
                }

                return _version.Value;
            }
        }
        /// <summary>
        /// java.exe的完整路径。
        /// </summary>
        public string JavaExe => Path + "java.exe";
        /// <summary>
        /// javaw.exe的完整路径。
        /// </summary>
        public string JavaWExe => Path + "javaw.exe";

        /// <summary>
        /// 私有字段，存储从Java执行结果中获取的输出信息。
        /// </summary>
        private string? _output;

        /// <summary>
        /// 私有属性，存储从Java执行结果中获取的输出信息。
        /// </summary>
        /// /// <returns>返回Java执行后的输出信息。</returns>
        private string Output
        {
            get
            {
                if (_output != null)
                {
                    return _output;
                }

                _output = RunJava();
                return _output;
            }
        }

        /// <summary>
        /// 判断当前Java环境是否为JRE（Java Runtime Environment）。
        /// </summary>
        public bool IsJre => !File.Exists(Path + "\\javac.exe");
        /// <summary>
        /// 获取或设置一个值，指示该Java环境是否由用户手动导入。
        /// </summary>
        public bool IsUserImport { set; get; }

        /// <summary>
        /// 私有字段，存储当前Java环境是否为64位版本。
        /// </summary>
        private bool? _is64Bit;

        /// <summary>
        /// 获取一个值，指示当前Java环境是否为64位版本。
        /// </summary>
        /// <returns>返回Java环境是否为64位版本。</returns>
        public bool Is64Bit
        {
            get
            {
                if (_is64Bit != null)
                {
                    return _is64Bit.Value;
                }

                var javaBitMatch = Regex.Match(Output, @"\b(\d+)-Bit\b"); // get bit
                _is64Bit = (javaBitMatch.Success ? javaBitMatch.Groups[1].Value : string.Empty) == "64";
                return _is64Bit.Value;
            }
        }

        /// <summary>
        /// 运行Java命令并获取其输出信息。
        /// </summary>
        /// <returns>返回Java命令的输出信息。</returns>
        private string RunJava()
        {
            using var javaProcess = new Process();
            javaProcess.StartInfo = new ProcessStartInfo
            {
                FileName = JavaExe,
                Arguments = "-version",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true, // 这个Java的输出流是tmd stderr！！！// Java的版本信息输出在stderr流中，前面的来自破防的群友(～￣▽￣)～
                RedirectStandardOutput = true
            };
            javaProcess.Start();
            javaProcess.WaitForExit();

            // 检查stderr是否有内容，因为Java的版本信息会输出到stderr
            var output = javaProcess.StandardError.ReadToEnd(); // check stderr have content
            return output != string.Empty ? output : javaProcess.StandardOutput.ReadToEnd(); // 就是tmd stderr
        }
    }
}
