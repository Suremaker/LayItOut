using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;

namespace LayItOut.DocGen
{
    class PageWriter
    {
        private readonly StringBuilder _builder = new StringBuilder();

        public PageWriter WriteTableOfContent(IEnumerable<string> names, int level = 0)
        {
            foreach (var name in names)
            {
                var link = ToAnchor(name);
                for (int i = 0; i < level; ++i)
                    _builder.Append("  ");
                _builder.AppendLine($"* [{name}](#{link})");
            }
            _builder.AppendLine();
            return this;
        }

        public PageWriter WriteHeader(string title, int level)
        {
            _builder.AppendLine($"{new string('#', level)} {title}").AppendLine();
            return this;
        }

        public PageWriter WriteDescription(DescriptionAttribute attribute)
        {
            if (!string.IsNullOrWhiteSpace(attribute?.Description))
                _builder.AppendLine(attribute.Description).AppendLine();
            return this;
        }

        public PageWriter Write(string text)
        {
            _builder.Append(text);
            return this;
        }

        public PageWriter WriteLine(string text)
        {
            _builder.AppendLine(text);
            return this;
        }

        public PageWriter WriteLine()
        {
            _builder.AppendLine();
            return this;
        }

        public PageWriter WriteTable(string[] headers, IEnumerable<string[]> rows)
        {
            _builder.AppendLine();
            _builder.Append('|').Append(string.Join('|', headers)).Append('|').AppendLine();
            foreach (var _ in headers) _builder.Append("|--");
            _builder.AppendLine("|");
            foreach (var row in rows)
            {
                foreach (var cell in row)
                    _builder.Append('|').Append(cell);
                _builder.AppendLine("|");
            }

            _builder.AppendLine();
            return this;
        }

        public override string ToString() => _builder.ToString();

        public static string ToAnchor(string name)
        {
            return Regex.Replace(name, "[^0-9a-zA-Z ]", "").Replace(' ', '-').ToLowerInvariant();
        }
    }
}