using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LayItOut.Attributes;
using LayItOut.Components;
using LayItOut.Loaders;
using Shouldly;
using Xunit;

namespace LayItOut.Tests.Loaders
{
    public class FormLoaderTests
    {
        [Fact]
        public async Task LoadForm_should_load_form()
        {
            var form = await LoadForm("form.xml");
            var hbox = form.Content.ShouldBeOfType<HBox>();

            var panel = hbox.GetChildren().Single().ShouldBeOfType<Panel>();
            panel.Margin.ShouldBe(new Spacer(10));
            panel.Padding.ShouldBe(new Spacer(20, 5));
            panel.BackgroundColor.ShouldBe(Color.Black);
            panel.Border.ShouldBe(Border.Parse("5 #fafaff"));

            var vbox = panel.GetChildren().Single().ShouldBeOfType<VBox>();
            vbox.Width.ShouldBe(SizeUnit.Unlimited);
            vbox.Height.ShouldBe(SizeUnit.Unlimited);

            var label = vbox.GetChildren().Single().ShouldBeOfType<Label>();
            label.Text.ShouldBe("Hello world!");
            label.TextAlignment.ShouldBe(TextAlignment.Center);
            label.Font.Family.ShouldBe("Aharoni");
        }

        [Fact]
        public async Task LoadForms_should_load_multiple_forms()
        {
            var forms = await LoadForms("forms.xml");
            forms.Count.ShouldBe(2);
            forms[0].Content.ShouldBeOfType<Panel>().BackgroundColor.ShouldBe(Color.Yellow);
            forms[1].Content.ShouldBeOfType<Panel>().BackgroundColor.ShouldBe(Color.Green);
        }

        [Fact]
        public async Task LoadForm_should_throw_meaningful_exception_if_cannot_parse_the_element()
        {
            var text = "<Form><unknown /></Form>";
            var loader = new FormLoader();
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => loader.LoadForm(new StringReader(text)));
            ex.Message.ShouldBe("Unable to parse element 'unknown' - no corresponding type were registered");
        }

        [Fact]
        public async Task LoadForm_should_throw_meaningful_exception_if_cannot_parse_form_element()
        {
            var text = "<Something/>";
            var loader = new FormLoader();
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => loader.LoadForm(new StringReader(text)));
            ex.Message.ShouldBe("Expected 'Form' element, but got 'Something'");
        }

        [Fact]
        public async Task LoadForms_should_throw_meaningful_exception_if_cannot_parse_forms_element()
        {
            var text = "<Something/>";
            var loader = new FormLoader();
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => loader.LoadForms(new StringReader(text)));
            ex.Message.ShouldBe("Expected 'Forms' element, but got 'Something'");
        }

        [Fact]
        public async Task LoadForms_should_support_no_forms()
        {
            var text = "<Forms/>";
            var loader = new FormLoader();
            Assert.Empty(await loader.LoadForms(new StringReader(text)));
        }

        private static async Task<Form> LoadForm(string formName)
        {
            using (var stream = File.OpenRead($"{AppContext.BaseDirectory}\\Loaders\\{formName}"))
            {
                return await new FormLoader().LoadForm(stream);
            }
        }
        private static async Task<IReadOnlyList<Form>> LoadForms(string formName)
        {
            using (var stream = File.OpenRead($"{AppContext.BaseDirectory}\\Loaders\\{formName}"))
            {
                return await new FormLoader().LoadForms(stream);
            }
        }
    }
}
