using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using LayItOut.Attributes;
using LayItOut.Loaders;

namespace LayItOut.DocGen
{
    class TypesPageComposer : IPageComposer
    {
        private static readonly IReadOnlyDictionary<Type, MethodInfo> ParseMethods = LoadParseMethods();
        public string Compose()
        {
            var types = typeof(SizeUnit).Assembly.GetTypes().Where(t => t.Namespace == typeof(SizeUnit).Namespace).OrderBy(x => x.Name).ToArray();
            var writer = new PageWriter();
            writer.WriteTableOfContent(types.Select(x => x.Name));

            foreach (var type in types)
            {
                writer.WriteHeader(type.Name);
                writer.WriteDescription(type.GetCustomAttribute<DescriptionAttribute>());

                if (type.IsEnum)
                    writer.Write("Enum values: ").WriteLine(string.Join(", ", Enum.GetNames(type).Select(n => $"`{n}`")));
                writer.WriteLine();

                if (ParseMethods.TryGetValue(type, out var method))
                {
                    var parseDescription = method.GetCustomAttribute<DescriptionAttribute>();
                    if (parseDescription != null)
                    {
                        writer.WriteLine("**Parse rules:**").WriteLine().WriteDescription(parseDescription).WriteLine();
                    }
                }
            }

            return writer.ToString();
        }

        private static Dictionary<Type, MethodInfo> LoadParseMethods()
        {
            return typeof(SizeUnit).Assembly
                .GetTypes()
                .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public))
                .Where(m => m.GetCustomAttribute<AttributeParserAttribute>() != null)
                .ToDictionary(x => x.ReturnType);
        }
    }
}