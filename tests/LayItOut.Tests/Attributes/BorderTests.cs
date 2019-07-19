using System.Drawing;
using LayItOut.Attributes;
using Shouldly;
using Xunit;

namespace LayItOut.Tests.Attributes
{
    public class BorderTests
    {
        [Fact]
        public void Parse_should_create_border()
        {
            Border.Parse("").ShouldBe(Border.NotSet);

            Border.Parse("10 red").ShouldBe(new Border(10, Color.Red));
        }

        [Fact]
        public void AsSpacer_should_convert_border()
        {
                new Border(10, Color.Aqua)
                .AsSpacer().ShouldBe(new Spacer(10));
        }
    }
}
