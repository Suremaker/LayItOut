using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LayItOut.Components;
using Component = LayItOut.Components.Component;
using IComponent = LayItOut.Components.IComponent;
using IContainer = LayItOut.Components.IContainer;

namespace LayItOut.DocGen
{
    class ComponentPageComposer
    {
        private readonly string _typesFileLink;

        private static readonly Type[] Interfaces = new[] { typeof(IContainer), typeof(IWrappingComponent), typeof(IComponent) };

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
                .GroupBy(GetInterface)
                .OrderByDescending(x => Array.IndexOf(Interfaces, x.Key))
                .ToArray();

            var writer = new PageWriter();
            foreach (var group in components)
            {
                writer.WriteLine($"* {group.Key.Name}");
                writer.WriteTableOfContent(group.Select(x => x.Name), 1);
            }

            foreach (var group in components)
            {
                writer.WriteHeader(group.Key.Name, 2);
                writer.WriteDescription(group.Key.GetCustomAttribute<DescriptionAttribute>());
                writer.WriteLine();

                foreach (var type in group)
                {
                    writer.WriteHeader(type.Name, 3);
                    writer.WriteLine($"Implements: `{GetInterface(type).Name}`").WriteLine();
                    writer.WriteDescription(type.GetCustomAttribute<DescriptionAttribute>());

                    await EmbedLongDescription(type, writer);

                    writer.WriteHeader($"{type.Name} members", 3);

                    writer.WriteTable(new[] { "Member", "Type", "Default value", "Description" }, ReadMembers(type));
                }

            }
            File.WriteAllText("man\\Components.md", writer.ToString());
        }

        private Type GetInterface(Type type) => Interfaces.First(i => i.IsAssignableFrom(type));

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
                    GetPropertyDefaultValue(p, instance),
                    p.GetCustomAttribute<DescriptionAttribute>()?.Description
                });
        }

        private static string GetPropertyDefaultValue(PropertyInfo p, object instance)
        {
            return $"`{p.GetValue(instance) ?? "null"}`";
        }

        private string LinkType(Type type)
        {
            if (type.Assembly == typeof(Component).Assembly)
                return $"[{type.Name}]({_typesFileLink}#{PageWriter.ToAnchor(type.Name)})";
            return type.FullName;
        }
    }
}