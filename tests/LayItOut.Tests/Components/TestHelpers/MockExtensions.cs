using System.Drawing;
using LayItOut.Components;
using Moq;

namespace LayItOut.Tests.Components.TestHelpers
{
    static class MockExtensions
    {
        public static Mock<IComponent> WithDesiredSize(this Mock<IComponent> mock, Size desiredSize)
        {
            mock.Setup(x => x.DesiredSize).Returns(desiredSize);
            return mock;
        }
    }
}