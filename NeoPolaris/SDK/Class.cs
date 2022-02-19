using NeoPolaris.Unreal.Classes;
using System.Collections.Generic;

namespace NeoPolaris.SDK
{
    internal class Class
    {
        public List<string> UsedNamespaces { get; } = new();

        public string Namespace { get; set; }
        public string ClassName { get; set; }

        public string SuperClass { get; set; }

        public int ClassSize { get; set; }

        public Dictionary<string, SDK.Property> Properties { get; } = new();

        public void AddProperty(UProperty property, string className = null, string name = null)
        {
            if (className == null)
                className = property.Class.GetName();
            if (name == null)
                name = property.GetName();

            var isSupported = true;
            var sdkProperty = new SDK.Property
            {
                Name = name,
                Type = className,
                Offset = property.Offset,
                ElementSize = property.ElementSize
            };

            //switch (className)
            {
                //case "AssetObjectProperty":
                //case "MulticastDelegateProperty":
                //case "WeakObjectProperty":
                    isSupported = false;
                    //break;
            }

            if (isSupported)
                Properties.Add(name, sdkProperty);
        }

        public void WriteSummary(CodeBuilder builder)
        {
            builder.SetClassIndent();
            builder.AppendLine("/// <summary>");
            builder.AppendLine($"/// {ClassName} : {SuperClass}");
            builder.AppendLine($"/// Size: 0x{ClassSize:X2}");;
            builder.AppendLine("/// </summary>");
        }

        public void WriteClass(CodeBuilder builder)
        {
            builder.SetClassIndent();

            WriteSummary(builder);

            if (string.IsNullOrEmpty(SuperClass))
                SuperClass = "MemoryObject";

            builder.AppendLine($"internal class {ClassName} : {SuperClass}");
            builder.AppendLine("{");

            builder.SetPropertyIndent();

            foreach (var property in Properties.Values)
            {
                //property.WriteProperty(builder);
                builder.AppendLine(string.Empty);
            }

            builder.AppendLine($"public override int ObjectSize => {ClassSize};");
            
            builder.SetClassIndent();
            builder.AppendLine("}");

            builder.SetNamespaceIndent();
            builder.AppendLine(string.Empty);
        }
    }
}
