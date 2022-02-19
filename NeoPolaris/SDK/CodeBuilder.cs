using System.Text;

namespace NeoPolaris.SDK
{
    internal class CodeBuilder
    {
        public StringBuilder Builder { get; } = new();

        public int Indent { get; private set; }

        public void SetNamespaceIndent()
            => Indent = 0;
        public void SetClassIndent()
            => Indent = 1;
        public void SetPropertyIndent()
            => Indent = 2;
        public void SetGetSetIndent()
            => Indent = 3;
        public void SetGetSetInnerIndent()
            => Indent = 4;

        private string GetIndent()
        {
            var result = string.Empty;
            for (var i = 0; i < Indent; i++)
                result += '\t';
            return result;
        }

        public void AppendLine(string value)
            => Builder.AppendLine($"{GetIndent()}{value}");

        public override string ToString()
            => Builder.ToString();
    }
}
