using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using LayItOut.Components;

namespace LayItOut.Loaders
{
    public class FormLoader : IDisposable
    {
        private readonly Dictionary<string, Type> _types = new Dictionary<string, Type>();
        private readonly Dictionary<Type, Func<string, object>> _attributeParsers = new Dictionary<Type, Func<string, object>>();
        public IFontLoader FontParser { get; }
        public IBitmapLoader BitmapLoader { get; }

        public FormLoader(IFontLoader fontParser = null, IBitmapLoader bitmapLoader = null)
        {
            FontParser = fontParser ?? new FontParser();
            BitmapLoader = bitmapLoader ?? new BitmapLoader();

            WithTypesFrom(typeof(FormLoader).Assembly);
            _attributeParsers[typeof(Font)] = LoadFont;
            _attributeParsers[typeof(Bitmap)] = LoadBitmap;
        }

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


        public Form Load(Stream stream) => Load(new StreamReader(stream));
        public Form Load(TextReader reader)
        {
            var doc = XDocument.Load(reader);

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
            if (_types.TryGetValue(type.LocalName, out var t))
                return t;
            throw new InvalidOperationException($"Unable to parse element '{type.LocalName}' - no corresponding type were registered.");
        }

        private object DeserializeAttribute(Type targetType, string value)
        {
            if (_attributeParsers.TryGetValue(targetType, out var parser))
                return parser(value);
            return Convert.ChangeType(value, targetType);
        }
        private object LoadBitmap(string src) => BitmapLoader.Load(src);
        private object LoadFont(string font) => FontParser.Parse(font);

        public void Dispose()
        {
            BitmapLoader.Dispose();
            FontParser.Dispose();
        }
    }
}
