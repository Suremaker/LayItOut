using System.Drawing;
using Shouldly;
using Xunit;

namespace LayItOut.Tests
{
    public class BorderTests
    {
        [Fact]
        public void Parse_should_create_border()
        {
            Border.Parse("10 red").ShouldBe(new Border(new BorderLine(10, Color.Red)));

            Border.Parse("15 red; 10 blue").ShouldBe(new Border(
                new BorderLine(15, Color.Red),
                new BorderLine(10, Color.Blue)));

            Border.Parse("15 red; 10 blue; 20 yellow; 30 green").ShouldBe(new Border(
                new BorderLine(15, Color.Red),
                new BorderLine(10, Color.Blue),
                new BorderLine(20, Color.Yellow),
                new BorderLine(30, Color.Green)));

            Border.Parse("15 red;;;30 green").ShouldBe(new Border(
                new BorderLine(15, Color.Red),
                BorderLine.NotSet,
                BorderLine.NotSet,
                new BorderLine(30, Color.Green)));
        }

        [Fact]
        public void AsSpacer_should_convert_border()
        {
            new Border(
                new BorderLine(10, Color.Aqua),
                new BorderLine(11, Color.Aqua),
                new BorderLine(12, Color.Aqua),
                new BorderLine(13, Color.Aqua))
                .AsSpacer().ShouldBe(new Spacer(10, 11, 12, 13));
        }
    }
}
