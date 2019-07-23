using LayItOut.Components;
using Moq;
using Shouldly;
using Xunit;

namespace LayItOut.Tests.Components
{
    public class ContainerTests
    {
        class TestableContainer : Container { }

        [Fact]
        public void Stack_can_have_multiple_children()
        {
            var item1 = Mock.Of<IComponent>();
            var item2 = Mock.Of<IComponent>();

            var stack = new TestableContainer();
            stack.AddComponent(item1);
            stack.AddComponent(item2);

            stack.GetChildren().ShouldBe(new[] { item1, item2 });
        }
    }
}