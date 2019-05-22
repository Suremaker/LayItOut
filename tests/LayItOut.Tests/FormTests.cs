using System;
using System.Drawing;
using LayItOut.Components;
using Moq;
using Shouldly;
using Xunit;

namespace LayItOut.Tests
{
    public class FormTests
    {
        [Fact]
        public void Form_can_have_content()
        {
            var content = Mock.Of<IComponent>();
            var form = new Form(content);
            form.Content.ShouldBeSameAs(content);
        }

        [Fact]
        public void Form_has_to_have_a_content()
        {
            Assert.Throws<ArgumentNullException>(() => new Form(null));
        }

        [Fact]
        public void LayOut_calls_Measure_then_Arrange_on_content()
        {
            var size = new Size(10, 20);
            var area = new Rectangle(Point.Empty, size);
            var content = new Mock<IComponent>(MockBehavior.Strict);
            var seq = new MockSequence();
            content.InSequence(seq).Setup(x => x.Measure(size));
            content.InSequence(seq).Setup(x => x.Arrange(area));

            var form = new Form(content.Object);
            form.LayOut(size);

            content.Verify(x => x.Arrange(area));
            content.Verify(x => x.Measure(size));
        }
    }
}
