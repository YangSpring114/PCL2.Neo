using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

namespace PCL2.Neo.Models.Minecraft.McVersion
{
    namespace VersionManifest
    {
        public record Latest
        {
            public string Release { get; init; }
            public string Snapshot { get; init; }
        }

        public record VersionInfo
        {
            public string Id { get; init; }
            public string Time { get; init; }
            public string Type { get; init; }
            public string Url { get; init; }
            public string ReleaseTime { get; init; }
        }

        public record VersionManifestData
        {
            public Latest Latest { get; init; }
            public List<VersionInfo> Versions { get; init; }
        }
    }

    namespace VersionData
    {
        public record RulesFeaturesElements
        {
            public string Name { get; init; }
            public bool Value { get; init; }
        }

        public record GameRulesElements
        {
            public string Action { get; init; }
            public RulesFeaturesElements RulesFeature { get; init; }
        }

        public record GameRulesList
        {
            public List<GameRulesElements> Rules { get; init; }
            public List<string> Value { get; init; }
        }

        public record Game
        {
            public List<GameRulesList> GameRules { get; init; }
            public List<string> Value { get; init; }
        }

        public record OsElements
        {
            public string Name { get; init; }
            public string Value { get; init; }
        }

        public record JvmRulesElements
        {
            public string? FirstAction { get; init; }
            public string Action { get; init; }
            public List<OsElements> Os { get; init; }
        }

        public record JvmRulesList
        {
            public List<JvmRulesElements> Rules { get; init; }
            public List<string> Value { get; init; }
        }

        public record Jvm
        {
            public List<JvmRulesList> Rules { get; init; }
            public List<string> Value { get; init; }
        }

        public record Arguments
        {
            public Game Game { get; init; }
            public Jvm Jvm { get; init; }
        }

        public record AssestIndex
        {
            public string Id { get; init; }
            public string Sha1 { get; init; }
            public string Size { get; init; }
            public int TotalSize { get; init; }
            public string Url { get; init; }
        }

        public record DownloadsEneemltValue
        {
            public string Sha1 { get; init; }
            public int Size { get; init; }
            public string Url { get; init; }
        }

        public record Downloads
        {
            public DownloadsEneemltValue Client { get; init; }
            public DownloadsEneemltValue ClientMappings { get; init; }
            public DownloadsEneemltValue Server { get; init; }
            public DownloadsEneemltValue ServerMappings { get; init; }
        }

        public record JavaVersion
        {
            public string Component { get; init; }
            public int MajorVersion { get; init; }
        }

        public record Artifact
        {
            public string Path { get; init; }
            public string Sha1 { get; init; }
            public int Size { get; init; }
            public string Url { get; init; }
        }

        public record LibrariesDownloads
        {
            public Artifact Artifact { get; set; }
            public List<(string, Artifact)>? Classifiers { get; set; }
            public string Name { get; set; }
            public List<(string, string)>? Natives { get; set; }
            public JvmRulesElements? Rules { get; set; }
        }

        public record LibrariesNameUrl
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }

        public record Libraries
        {
            public List<LibrariesDownloads> Downloads { get; set; }
            public List<LibrariesNameUrl> NameUrls { get; set; }
        }

        public record File
        {
            public string Id { get; init; }
            public string Sha1 { get; init; }
            public int Size { get; init; }
            public string Url { get; init; }
        }

        public record Client
        {
            public string Argument { get; init; }
            public File File { get; init; }
            public string Type { get; init; }
        }

        public record Logging
        {
            public Client Client { get; init; }
        }

        public record Temp
        {
            public AssestIndex AssestIndex { get; init; }
            public string Assets { get; init; }
            public byte ComplianceLevel { get; init; }
            public Downloads Downloads { get; init; }
            public string Id { get; init; }
            public JavaVersion JavaVersion { get; init; }
            public Logging Logging { get; init; }
            public string MainClass { get; init; }
            public byte MinimunLauncherVersion { get; init; }
            public string ReleaseTime { get; init; }
            public string Time { get; init; }
            public string Type { get; init; }
        }

        public record VersionData
        {
            public Arguments Arguments { get; init; }
            public AssestIndex AssestIndex { get; init; }
            public string Assets { get; init; }
            public byte ComplianceLevel { get; init; }
            public Downloads Downloads { get; init; }
            public string Id { get; init; }
            public JavaVersion JavaVersion { get; init; }
            public Libraries Libraries { get; init; }
            public Logging Logging { get; init; }
            public string MainClass { get; init; }
            public byte MinimunLauncherVersion { get; init; }
            public string ReleaseTime { get; init; }
            public string Time { get; init; }
            public string Type { get; init; }
        }

        public partial class Version
        {
            private static GameRulesList GameRulesListParser(JsonElement element)
            {
                var rulesList = new GameRulesList { Rules = [], Value = [] };

                var rules = element.GetProperty("rules").EnumerateArray().ElementAt(0);
                //var values = element.GetProperty("value").EnumerateArray();
                var values = element.GetProperty("value");

                var action = rules.GetProperty("action").GetString()!;
                var features = rules.GetProperty("features");

                var ruleElement = new GameRulesElements
                {
                    Action = action,
                    RulesFeature = new RulesFeaturesElements
                    {
                        Name = RulesNameRegex().Match(features.GetRawText()).Groups[1].Value,
                        Value = features.GetRawText().Contains("true")
                    }
                };

                // add values
                switch (values.ValueKind)
                {
                    case JsonValueKind.Array:
                        {
                            foreach (var item in values.EnumerateArray()) rulesList.Value.Add(item.GetString()!);
                            break;
                        }
                    case JsonValueKind.String:
                        rulesList.Value.Add(values.GetString()!);
                        break;
                    default:
                        throw new JsonException("Unknown JsonValueKind");
                }

                rulesList.Rules.Add(ruleElement);

                return rulesList;
            }

            private static Game GameParser(JsonElement element)
            {
                var enummerable = element.EnumerateArray();
                var game = new Game { GameRules = [], Value = [] };

                foreach (var item in enummerable)
                {
                    switch (item.ValueKind)
                    {
                        case JsonValueKind.String:
                            game.Value.Add(item.GetString()!);
                            break;
                        case JsonValueKind.Object:
                            game.GameRules.Add(GameRulesListParser(item));
                            break;
                        default:
                            throw new JsonException("Unknown JsonValueKind");
                    }
                }

                return game;
            }

            private static JvmRulesElements JvmRulesElementsParser(JsonElement element)
            {
                var os = element.GetProperty("os").GetRawText();

                return new JvmRulesElements
                {
                    Action = element.GetProperty("action").GetString()!,
                    Os =
                    [
                        new OsElements()
                        {
                            Name = RulesNameRegex().Match(os).Groups[1].Value,
                            Value = JvmOsValueRegex().Match(os).Groups[1].Value
                        }
                    ]
                };
            }

            private static JvmRulesList JvmRulesListParser(JsonElement element)
            {
                var result = new JvmRulesList { Rules = [], Value = [] };

                var rules = element.GetProperty("rules").EnumerateArray().ElementAt(0);
                var values = element.GetProperty("value");

                result.Rules.Add(JvmRulesElementsParser(rules));

                switch (values.ValueKind)
                {
                    case JsonValueKind.Array:
                        {
                            var valArray = values.EnumerateArray();
                            foreach (var item in valArray) result.Value.Add(item.GetString()!);
                            break;
                        }
                    case JsonValueKind.String:
                        result.Value.Add(values.GetString()!);
                        break;
                    default:
                        throw new JsonException("Unknown JsonValueKind");
                }

                return result;
            }

            private static Jvm JvmParser(JsonElement element)
            {
                var enummerable = element.EnumerateArray();
                var jvm = new Jvm { Rules = [], Value = [] };

                foreach (var item in enummerable)
                {
                    switch (item.ValueKind)
                    {
                        case JsonValueKind.Object:
                            jvm.Rules.Add(JvmRulesListParser(item));
                            break;
                        case JsonValueKind.String:
                            jvm.Value.Add(item.GetString()!);
                            break;
                        default:
                            throw new JsonException("Unknown JsonValueKind");
                    }
                }

                return jvm;
            }

            private static Arguments ArgumentsParser(JsonElement element)
            {
                var game = GameParser(element.GetProperty("game"));
                var jvm = JvmParser(element.GetProperty("jvm"));
                return new Arguments { Game = game, Jvm = jvm };
            }

            private static LibrariesDownloads DowloadParser(JsonElement element)
            {
                var artifact = element.GetProperty("downloads").GetProperty("artifact").GetRawText();
                var name = element.GetProperty("name").GetString()!;

                if (element.TryGetProperty("rules", out var rules))
                {
                    //return new LibrariesDownloads
                    //{
                    //    Artifact = JsonSerializer.Deserialize<Artifact>(artifact)!,
                    //    Name = name,
                    //    Rules = JvmRulesElementsParser(rules.EnumerateArray().ElementAt(0))
                    //};
                    // TODO: add code
                }
                else
                {
                    //return new LibrariesDownloads
                    //{
                    //    Artifact = JsonSerializer.Deserialize<Artifact>(artifact)!, Name = name, Rules = null
                    //};
                    // TODO: add code
                }
            }

            private static Libraries LibrariesParser(JsonElement element)
            {
                var array = element.EnumerateArray();
                var result = new Libraries { Downloads = [], NameUrls = [] };

                foreach (var item in array)
                {
                    if (item.TryGetProperty("downloads", out _))
                    {
                        result.Downloads.Add(DowloadParser(item));
                    }
                    else
                    {
                        result.NameUrls.Add(JsonSerializer.Deserialize<LibrariesNameUrl>(item.GetRawText())!);
                    }
                }

                return result;
            }

            private static Temp RebuildJson(JsonElement element)
            {
                var root = element.EnumerateObject();
                var result = new StringBuilder("{");
                foreach (var pro in root.Where(pro => pro.Name != "arguments" && pro.Name != "libraries"))
                {
                    result.Append($"{pro},");
                }

                result.Remove(result.Length - 1, 1).Append("}");

                var str = result.ToString();
                //Console.WriteLine(str);

                return JsonSerializer.Deserialize<Temp>(str)!;
            }

            public static VersionData Parser(string input)
            {
                var root = JsonDocument.Parse(input).RootElement;
                var arguments = root.GetProperty("arguments");

                var argumentsJson = ArgumentsParser(arguments);
                var rebuild = RebuildJson(root);
                var libraries = LibrariesParser(root.GetProperty("libraries"));

                return new VersionData
                {
                    Arguments = argumentsJson,
                    AssestIndex = rebuild.AssestIndex,
                    Assets = rebuild.Assets,
                    ComplianceLevel = rebuild.ComplianceLevel,
                    Downloads = rebuild.Downloads,
                    Id = rebuild.Id,
                    JavaVersion = rebuild.JavaVersion,
                    Libraries = libraries,
                    Logging = rebuild.Logging,
                    MainClass = rebuild.MainClass,
                    MinimunLauncherVersion = rebuild.MinimunLauncherVersion,
                    ReleaseTime = rebuild.ReleaseTime,
                    Time = rebuild.Time,
                    Type = rebuild.Type
                };
            }

            [GeneratedRegex(@"""([^""]+)""\s*:\s*true")]
            private static partial Regex RulesNameRegex();

            [GeneratedRegex(@"(?<=""name"":\s*)""([^""]+)""")]
            private static partial Regex JvmOsValueRegex();
        }
    }
}
