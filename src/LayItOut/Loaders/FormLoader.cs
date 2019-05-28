using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using LayItOut.Components;

namespace LayItOut.Loaders
{
    public class FormLoader
    {
        private readonly Dictionary<string, Type> _types = new Dictionary<string, Type>();
        private readonly Dictionary<Type, Func<string, object>> _attributeParsers = new Dictionary<Type, Func<string, object>>();

        public FormLoader WithTypesFrom(params Assembly[] assemblies)
        {
            foreach (var t in assemblies.Distinct().SelectMany(a => a.GetTypes()))
            {
                if (t.IsClass && !t.IsAbstract && typeof(IComponent).IsAssignableFrom(t))
                    _types[t.Name] = t;

                foreach (var parseMethod in AttributeParserAttribute.FindParseMethods(t))
                    _attributeParsers[parseMethod.type] = parseMethod.method;
            }

            return this;
        }

        public FormLoader()
        {
            WithTypesFrom(typeof(FormLoader).Assembly);
        }

        public Form Load(Stream stream)
        {
            var doc = XDocument.Load(stream);

            if (doc.Root?.Name.LocalName != "Form")
                throw new InvalidOperationException($"Expected {nameof(Form)}, but got {doc.Root?.Name.LocalName}");

            var content = doc.Root.Elements().ToArray();
            if (content.Length != 1)
                throw new InvalidOperationException($"Expected {nameof(Form)} with 1 element, got {content.Length}");

            return new Form(Deserialize(content.Single()));
        }

        private IComponent Deserialize(XElement element)
        {
            var type = Resolve(element.Name);
            var item = (IComponent)Activator.CreateInstance(type);
            foreach (var attribute in element.Attributes())
            {
                var prop = type.GetProperty(attribute.Name.LocalName);
                if (prop == null)
                    throw new InvalidOperationException($"Property '{attribute.Name.LocalName}' does not exists on '{type}'");
                prop.SetValue(item, DeserializeAttribute(prop.PropertyType, attribute.Value));
            }

            switch (item)
            {
                case IContainer container:
                    foreach (var component in element.Elements().Select(Deserialize))
                        container.AddComponent(component);
                    break;
                case IWrappingComponent containerElement:
                    containerElement.Inner = element.Elements().Select(Deserialize).SingleOrDefault();
                    break;
                default:
                    if (element.Elements().Any()) throw new InvalidOperationException($"Type '{type}' is not a container, so should not have any inner-elements.");
                    break;
            }

            return item;
        }

        private Type Resolve(XName type)
        {
            return _types[type.LocalName];
        }

        private object DeserializeAttribute(Type targetType, string value)
        {
            if (_attributeParsers.TryGetValue(targetType, out var parser))
                return parser(value);
            return Convert.ChangeType(value, targetType);
        }
    }
}
