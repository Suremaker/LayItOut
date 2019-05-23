using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using Xunit;

namespace LayItOut.Tests
{
    public class AlignmentTests
    {
        [Fact]
        public void Parse_should_properly_interpret_values()
        {
            Alignment.Parse("center").ShouldBe(new Alignment(VerticalAlignment.Center, HorizontalAlignment.Center));
            Alignment.Parse("ToP  lEfT").ShouldBe(new Alignment(VerticalAlignment.Top, HorizontalAlignment.Left));
            Alignment.Parse(" BOTTOM right ").ShouldBe(new Alignment(VerticalAlignment.Bottom, HorizontalAlignment.Right));
            Alignment.Parse("center CENTER").ShouldBe(new Alignment(VerticalAlignment.Center, HorizontalAlignment.Center));
            Assert.Throws<ArgumentException>(() => Alignment.Parse("foo")).Message.ShouldStartWith("Provided value is not a valid Alignment: foo");
        }

        [Fact]
        public void ToString_should_return_alignment()
        {
            new Alignment(VerticalAlignment.Bottom, HorizontalAlignment.Right).ToString().ShouldBe("Bottom Right");
            new Alignment(VerticalAlignment.Center, HorizontalAlignment.Center).ToString().ShouldBe("Center");
        }

        [Fact]
        public void Predefined_fields_should_have_proper_values()
        {
            Alignment.Center.ShouldBe(new Alignment(VerticalAlignment.Center, HorizontalAlignment.Center));
            Alignment.TopLeft.ShouldBe(new Alignment(VerticalAlignment.Top, HorizontalAlignment.Left));
        }
    }
}
