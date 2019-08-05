using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LayItOut.DocGen
{
    public class MarkdownCompiler
    {
        private readonly string _file;
        private readonly string _name;
        private int _snippets = 0;
        public MarkdownCompiler(string file)
        {
            _file = file;
            _name = Path.GetFileNameWithoutExtension(file);
        }
        public async Task<string> Compile()
        {
            var snippetStart = "```!SNIPPET";
            var snippetEnd = "```";

            var content = File.ReadAllText(_file);
            var builder = new StringBuilder();
            int last = 0;
            int current;
            while ((current = content.IndexOf(snippetStart, last, StringComparison.Ordinal)) >= 0)
            {
                var end = content.IndexOf(snippetEnd, current + snippetStart.Length, StringComparison.Ordinal);
                if (end < 0) throw new InvalidOperationException($"{_file}:{current} Snippet does not have end!");

                var snippet = content.Substring(current + snippetStart.Length, end - current - snippetStart.Length);
                builder.Append(content, last, current);
                last = end + snippetEnd.Length;
                await CompileSnippet(snippet, builder);
            }
            builder.Append(content, last, content.Length - last);
            return builder.ToString();
        }

        private async Task CompileSnippet(string snippet, StringBuilder builder)
        {
            builder.Append("```xml").Append(snippet).AppendLine("```");
            var snippetName = $"{_name}_snippet_{++_snippets}.png";
            await SnippetCompiler.Instance.Compile(snippetName, snippet);
            builder.AppendLine($"![{snippetName}](images/{snippetName})");
        }
    }
}