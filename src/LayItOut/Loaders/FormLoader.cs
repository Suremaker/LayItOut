using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using LayItOut.Attributes;
using LayItOut.Components;

namespace LayItOut.Loaders
{
    public class FormLoader : IDisposable
    {
        private readonly Dictionary<string, Type> _types = new Dictionary<string, Type>();
        private readonly Dictionary<Type, Func<string, Task<object>>> _attributeParsers = new Dictionary<Type, Func<string, Task<object>>>();
        public IAssetLoader AssetLoader { get; }

        public FormLoader(IAssetLoader assetLoader = null)
        {
            AssetLoader = assetLoader ?? new AssetLoader();

            WithTypesFrom(typeof(FormLoader).Assembly);
            _attributeParsers[typeof(AssetSource)] = LoadAsset;
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

        public Task<Form> LoadForm(Stream stream) => LoadForm(new StreamReader(stream));
        public Task<Form> LoadForm(TextReader reader) => DeserializeForm(XDocument.Load(reader).Root);

        public Task<IReadOnlyList<Form>> LoadForms(Stream stream) => LoadForms(new StreamReader(stream));

        public async Task<IReadOnlyList<Form>> LoadForms(TextReader reader)
        {
            var root = XDocument.Load(reader).Root;
            if (root?.Name.LocalName != "Forms")
                throw new InvalidOperationException($"Expected 'Forms' element, but got '{root?.Name.LocalName}'");

            var results = new List<Form>();
            foreach (var element in root.Elements())
                results.Add(await DeserializeForm(element));

            return results;
        }

        private async Task<Form> DeserializeForm(XElement root)
        {
            if (root.Name.LocalName != "Form")
                throw new InvalidOperationException($"Expected '{nameof(Form)}' element, but got '{root.Name.LocalName}'");

            var content = root.Elements().ToArray();
            if (content.Length != 1)
                throw new InvalidOperationException($"Expected '{nameof(Form)}' element with 1 element, got {content.Length}");

            return new Form(await Deserialize(content.Single()));
        }

        private async Task<IComponent> Deserialize(XElement element)
        {
            var type = Resolve(element.Name);
            var item = (IComponent)Activator.CreateInstance(type);
            foreach (var attribute in element.Attributes())
            {
                var prop = type.GetProperty(attribute.Name.LocalName);
                if (prop == null)
                    throw new InvalidOperationException($"Property '{attribute.Name.LocalName}' does not exists on '{type}'");
                prop.SetValue(item, await DeserializeAttribute(prop.PropertyType, attribute.Value));
            }

            switch (item)
            {
                case IContainer container:
                    foreach (var component in element.Elements().Select(Deserialize))
                        container.AddComponent(await component);
                    break;
                case IWrappingComponent containerElement:
                    var task = element.Elements().Select(Deserialize).SingleOrDefault();
                    containerElement.Inner = task != null ? await task : null;
                    break;
                default:
                    if (element.Elements().Any()) throw new InvalidOperationException($"Type '{type}' is not a container, so should not have any inner-elements");
                    break;
            }

            return item;
        }

        private Type Resolve(XName type)
        {
            if (_types.TryGetValue(type.LocalName, out var t))
                return t;
            throw new InvalidOperationException($"Unable to parse element '{type.LocalName}' - no corresponding type were registered");
        }

        private async Task<object> DeserializeAttribute(Type targetType, string value)
        {
            if (_attributeParsers.TryGetValue(targetType, out var parser))
                return await parser(value);
            if (targetType.IsEnum)
                return Enum.Parse(targetType, value.Trim(), true);
            return Convert.ChangeType(value, targetType);
        }
        private async Task<object> LoadAsset(string src) => await AssetLoader.LoadAsync(src);

        public void Dispose()
        {
            AssetLoader.Dispose();
        }
    }
}
