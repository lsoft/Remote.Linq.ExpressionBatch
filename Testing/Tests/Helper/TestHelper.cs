using Xunit.Sdk;

namespace Tests.Helper
{
    public static class TestHelper
    {
        public static void ShouldFail(
            this object o,
            string message
            )
        {
            throw new XunitException(message);
        }
    }
}
