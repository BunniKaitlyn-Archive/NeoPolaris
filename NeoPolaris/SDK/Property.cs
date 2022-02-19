namespace NeoPolaris.SDK
{
    internal class Property
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Offset { get; set; }
        public int ElementSize { get; set; }

        private void WriteSummary(CodeBuilder builder)
        {
            builder.AppendLine("/// <summary>");
            builder.AppendLine($"/// Name: {Name}");
            builder.AppendLine($"/// Type: {Type}");
            builder.AppendLine($"/// Offset: 0x{Offset:X}");
            builder.AppendLine($"/// Size: 0x{ElementSize:X}");
            builder.AppendLine("/// </summary>");
        }

        private void WriteGetSet(CodeBuilder builder, string type, string readType, int offset)
        {
            builder.SetPropertyIndent();
            builder.AppendLine($"public {type} {Name}");
            builder.AppendLine("{");

            builder.SetGetSetIndent();
            builder.AppendLine("get");
            builder.AppendLine("{");

            builder.SetGetSetInnerIndent();
            builder.AppendLine($"return Read{readType}(0x{offset:X});");

            builder.SetGetSetIndent();
            builder.AppendLine("}");
            builder.AppendLine("set");
            builder.AppendLine("{");

            builder.SetGetSetInnerIndent();
            builder.AppendLine($"Write{readType}(0x{offset:X}, value);");

            builder.SetGetSetIndent();
            builder.AppendLine("}");

            builder.SetPropertyIndent();
            builder.AppendLine("}");
        }
    }
}
