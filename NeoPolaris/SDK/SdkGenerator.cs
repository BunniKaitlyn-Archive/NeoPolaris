using NeoPolaris.Unreal.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NeoPolaris.SDK
{
    internal class SdkGenerator
    {
        public Dictionary<string, List<SdkClass>> Classes { get; } = new();

        private string[] GetIgnores()
        {
            var types = new List<string>();
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                types.Add(type.Name);
            }

            return types.ToArray();
        }

        private SdkClass ProcessClass(UClass clazz)
        {
            var sdkClass = new SdkClass();

            sdkClass.Namespace = clazz.Outer.GetName();
            sdkClass.ClassName = clazz.GetName(true);

            if (clazz.SuperField != null)
            {
                sdkClass.SuperClass = clazz.SuperField.GetName(true);

                var namespaze = clazz.SuperField.Cast<UClass>().Outer.GetName();
                if (!sdkClass.UsedNamespaces.Contains(namespaze))
                    sdkClass.UsedNamespaces.Add(namespaze);
            }

            sdkClass.ClassSize = clazz.PropertySize;

            var child = clazz.Children;
            while (child != null)
            {
                if (child.IsA<UProperty>())
                    sdkClass.AddProperty(child.Cast<UProperty>());

                child = child.Next;
            }

            return sdkClass;
        }

        private void WriteFiles()
        {
            foreach (var item in Classes)
            {
                if (item.Value.Count == 0)
                    continue;

                var usedNamespaces = new List<string>();
                foreach (var clazz in item.Value)
                {
                    foreach (var usedNamespace in clazz.UsedNamespaces)
                    {
                        if (!usedNamespaces.Contains(usedNamespace))
                            usedNamespaces.Add(usedNamespace);
                    }
                }

                var builder = new SdkBuilder();

                builder.SetNamespaceIndent();

                foreach (var usedNamespace in usedNamespaces)
                {
                    builder.AppendLine($"using {usedNamespace};");
                }

                builder.AppendLine(string.Empty);

                builder.AppendLine($"namespace {item.Key}");
                builder.AppendLine("{");

                builder.SetNamespaceIndent();

                foreach (var clazz in item.Value)
                {
                    clazz.WriteClass(builder);
                }

                builder.SetNamespaceIndent();
                builder.AppendLine("}");

                File.WriteAllText(Path.Combine(App.SdkPath, $"{item.Key.Split('/').Last()}.cs"), builder.ToString());
            }
        }

        public void Generate()
        {
            var ignores = GetIgnores();

            for (var i = 0; i < App.Instance.Objects.Count; i++)
            {
                var obj = App.Instance.Objects[i];
                if (obj != null)
                {
                    if (obj.IsA<UClass>() && !obj.IsA("BlueprintGeneratedClass") && !ignores.Contains(obj.GetName(true)))
                    {
                        Console.WriteLine($"Generating class {obj.GetName(true)}");

                        var clazz = ProcessClass(obj.Cast<UClass>());
                        if (!Classes.ContainsKey(clazz.Namespace))
                            Classes.Add(clazz.Namespace, new());

                        Classes[clazz.Namespace].Add(clazz);
                    }
                    else if (obj.IsA<UScriptStruct>() && !ignores.Contains(obj.GetName(true)))
                    {
                        //Console.WriteLine($"Generating struct {obj.GetName(true)}");
                    }
                }
            }

            WriteFiles();
        }
    }
}
