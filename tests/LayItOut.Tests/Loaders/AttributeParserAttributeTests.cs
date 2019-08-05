using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LayItOut.Components;
using LayItOut.Loaders;
using Shouldly;
using Xunit;

namespace LayItOut.Tests.Loaders
{
    public class AttributeParserAttributeTests
    {
        public class SynchronousFoo
        {
            public string Value { get; }

            private SynchronousFoo(string value)
            {
                Value = value;
            }

            [AttributeParser]
            public static SynchronousFoo Parse(string foo) => new SynchronousFoo(foo);
        }

        public class AsynchronousFoo
        {
            public string Value { get; }

            private AsynchronousFoo(string value)
            {
                Value = value;
            }

            [AttributeParser]
            public static async Task<AsynchronousFoo> Parse(string foo)
            {
                await Task.Delay(100);
                return new AsynchronousFoo(foo);
            }
        }

        public class FooComponent : Component
        {
            public SynchronousFoo Sync { get; set; }
            public AsynchronousFoo Async { get; set; }
        }

        [Fact]
        public async Task It_should_support_synchronous_and_asynchronous_parse_methods()
        {
            var form = await new FormLoader()
                .WithTypesFrom(typeof(FooComponent).Assembly)
                .LoadForm(new StringReader("<Form><FooComponent Sync=\"abc\" Async=\"def\"/></Form>"));

            var foo = form.Content.ShouldBeOfType<FooComponent>();
            foo.Sync.Value.ShouldBe("abc");
            foo.Async.Value.ShouldBe("def");
        }
    }
}
