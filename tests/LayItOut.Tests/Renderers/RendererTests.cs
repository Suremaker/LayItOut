using System.Drawing;
using LayItOut.Components;
using LayItOut.Rendering;
using LayItOut.Tests.TestHelpers;
using Moq;
using Xunit;

namespace LayItOut.Tests.Renderers
{
    public class RendererTests
    {
        class TestableRenderer : Renderer<object>
        {
            public void Render(object graphics, Form form) => base.Render(graphics, form.Content);
        }

        [Fact]
        public void Render_should_traverse_through_child_hierarchy_and_render_all_which_size_is_greater_than_0()
        {
            var panelRenderer = new Mock<IComponentRenderer<object, Panel>>();
            var hboxRenderer = new Mock<IComponentRenderer<object, HBox>>();
            var componentRenderer = new Mock<IComponentRenderer<object, Component>>();
            var renderer = new TestableRenderer();
            renderer.RegisterRenderer(panelRenderer.Object);
            renderer.RegisterRenderer(hboxRenderer.Object);
            renderer.RegisterRenderer(componentRenderer.Object);

            var hbox = new HBox();
            var vbox = new VBox();
            var component1 = new Component { Width = 1, Height = 1 };
            var component2 = new Component { Width = 1, Height = 1 };
            var component3 = new Component { Width = 1, Height = 1 };
            var component4 = new Component { Width = 1, Height = 1 };
            var panel1 = new Panel { Width = 10, Height = 10, Inner = component1 };
            var panel2 = new Panel { Width = 0, Height = 10, Inner = component2 };
            var panel3 = new Panel { Width = 10, Height = 0, Inner = component3 };
            var panel4 = new Panel { Width = 2, Height = 2, Inner = component4 };
            hbox.AddComponent(vbox);
            hbox.AddComponent(panel1);

            vbox.AddComponent(panel2);
            vbox.AddComponent(panel3);
            vbox.AddComponent(panel4);

            var form = new Form(hbox);
            form.LayOut(new Size(int.MaxValue, int.MaxValue), TestRendererContext.Instance);
            var g = new object();

            renderer.Render(g, form);

            hboxRenderer.Verify(x => x.Render(g, hbox), Times.Once);
            panelRenderer.Verify(x => x.Render(g, panel1), Times.Once);
            panelRenderer.Verify(x => x.Render(g, panel2), Times.Never);
            panelRenderer.Verify(x => x.Render(g, panel3), Times.Never);
            panelRenderer.Verify(x => x.Render(g, panel4), Times.Once);

            componentRenderer.Verify(x => x.Render(g, component1), Times.Once);
            componentRenderer.Verify(x => x.Render(g, component2), Times.Never);
            componentRenderer.Verify(x => x.Render(g, component3), Times.Never);
            componentRenderer.Verify(x => x.Render(g, component4), Times.Once);
        }
    }
}
