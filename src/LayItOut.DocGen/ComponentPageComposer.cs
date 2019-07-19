using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Component = LayItOut.Components.Component;

namespace LayItOut.DocGen
{
    class ComponentPageComposer
    {
        private readonly string _typesFileLink;

        public ComponentPageComposer(string typesFileLink)
        {
            _typesFileLink = typesFileLink;
        }

        public void Compose()
        {
            var components = typeof(Component).Assembly
                .GetTypes()
                .Where(t => typeof(Component).IsAssignableFrom(t) && !t.IsAbstract)
                .OrderBy(t => t.Name)
                .ToArray();

            var writer = new PageWriter();
            writer.WriteTableOfContent(components.Select(x => x.Name));

            foreach (var type in components)
            {
                writer.WriteHeader(type.Name);
                writer.WriteDescription(type.GetCustomAttribute<DescriptionAttribute>());

                EmbedLongDescription(type, writer);

                writer.WriteTable(new[] { "Member", "Type", "Description" }, ReadMembers(type));
            }

            File.WriteAllText("man\\Components.md", writer.ToString());
        }

        private void EmbedLongDescription(Type type, PageWriter writer)
        {
            var file = $"{AppContext.BaseDirectory}\\components\\{type.Name}.md";
            if (File.Exists(file))
                writer.WriteLine("**Sample usage:**").WriteLine().WriteLine(new MarkdownCompiler(file).Compile());
        }

        private IEnumerable<string[]> ReadMembers(Type type)
        {
            return type.GetProperties().Where(p => (p.SetMethod?.IsPublic ?? false) && (p.GetMethod?.IsPublic ?? false))
                .OrderBy(p => p.Name)
                .Select(p => new[]
                {
                    p.Name,
                    LinkType(p.PropertyType),
                    p.GetCustomAttribute<DescriptionAttribute>()?.Description
                });
        }

        private string LinkType(Type type)
        {
            if (type.Assembly == typeof(Component).Assembly)
                return $"[{type.FullName}]({_typesFileLink}#{PageWriter.ToAnchor(type.Name)})";
            return type.FullName;
        }
    }
}