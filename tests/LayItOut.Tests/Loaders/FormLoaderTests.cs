using System;
using System.Drawing;
using System.IO;
using System.Linq;
using LayItOut.Components;
using LayItOut.Loaders;
using Shouldly;
using Xunit;

namespace LayItOut.Tests.Loaders
{
    public class FormLoaderTests
    {
        [Fact]
        public void It_should_load_form()
        {
            var form = LoadForm("form.xml");
            var hbox = form.Content.ShouldBeOfType<HBox>();

            var panel = hbox.GetChildren().Single().ShouldBeOfType<Panel>();
            panel.Margin.ShouldBe(new Spacer(10));
            panel.Padding.ShouldBe(new Spacer(20, 5));
            panel.BackgroundColor.ShouldBe(Color.Black);
            panel.Border.ShouldBe(Border.Parse("5 #fafaff"));

            var vbox = panel.Inner.ShouldBeOfType<VBox>();
            vbox.Width.ShouldBe(SizeUnit.Unlimited);
            vbox.Height.ShouldBe(SizeUnit.Unlimited);

            var label = vbox.GetChildren().Single().ShouldBeOfType<Label>();
            label.Text.ShouldBe("Hello world!");
            label.TextAlignment.ShouldBe(TextAlignment.Center);
            label.Font.Family.ShouldBe("Aharoni");
        }

        [Fact]
        public void It_should_throw_meaningful_exception_if_cannot_parse_the_element()
        {
            var text = "<Form><unknown /></Form>";
            var loader = new FormLoader();
            var ex = Assert.Throws<InvalidOperationException>(() => loader.Load(new StringReader(text)));
            ex.Message.ShouldBe("Unable to parse element 'unknown' - no corresponding type were registered.");
        }

        private static Form LoadForm(string formName)
        {
            using (var stream = File.OpenRead($"{AppContext.BaseDirectory}\\Loaders\\{formName}"))
            {
                return new FormLoader().Load(stream);
            }
        }
    }
}
