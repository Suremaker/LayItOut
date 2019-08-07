using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

        public async Task Compose()
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

                await EmbedLongDescription(type, writer);

                writer.WriteTable(new[] { "Member", "Type", "Description", "Default value" }, ReadMembers(type));
            }

            File.WriteAllText("man\\Components.md", writer.ToString());
        }

        private async Task EmbedLongDescription(Type type, PageWriter writer)
        {
            var file = $"{AppContext.BaseDirectory}\\components\\{type.Name}.md";
            if (File.Exists(file))
                writer.WriteLine("**Sample usage:**").WriteLine().WriteLine(await new MarkdownCompiler(file).Compile());
        }

        private IEnumerable<string[]> ReadMembers(Type type)
        {
            var instance = Activator.CreateInstance(type);
            return type.GetProperties().Where(p => (p.SetMethod?.IsPublic ?? false) && (p.GetMethod?.IsPublic ?? false))
                .OrderBy(p => p.Name)
                .Select(p => new[]
                {
                    p.Name,
                    LinkType(p.PropertyType),
                    p.GetCustomAttribute<DescriptionAttribute>()?.Description,
                    GetPropertyDefaultValue(p, instance)
                });
        }

        private static string GetPropertyDefaultValue(PropertyInfo p, object instance)
        {
            return $"`{p.GetValue(instance) ?? "null"}`";
        }

        private string LinkType(Type type)
        {
            if (type.Assembly == typeof(Component).Assembly)
                return $"[{type.FullName}]({_typesFileLink}#{PageWriter.ToAnchor(type.Name)})";
            return type.FullName;
        }
    }
}