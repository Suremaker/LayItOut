using System;
using System.Drawing;
using LayItOut.Components;
using LayItOut.Tests.TestHelpers;
using Shouldly;
using Xunit;
using Image = LayItOut.Components.Image;

namespace LayItOut.Tests.Components
{
    public class TextBoxTests
    {
        [Fact]
        public void It_should_not_allow_non_label_children()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => new TextBox().AddComponent(new Image()));
            ex.Message.ShouldBe("Only Label and it's descendants are supported, got: Image");
        }

        [Fact]
        public void Measure_should_use_measure_all_content()
        {
            var box = new Size(20, 30);
            var font1 = new Font(FontFamily.GenericSerif, 14);
            var font2 = new Font(FontFamily.GenericSerif, 16);

            var textBox = new TextBox();
            textBox.AddComponent(new Label { Text = "foo bar", Font = font1 });
            textBox.AddComponent(new Link { Text = "baz", Font = font2 });

            textBox.Measure(box, TestRenderingContext.Instance);
            textBox.DesiredSize.ShouldBe(new Size("foo bar baz".Length, (int)Math.Ceiling(font2.GetHeight())));
        }
    }
}