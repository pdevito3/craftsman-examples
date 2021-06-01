namespace Ordering.IntegrationTests
{
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;
    using static TestFixture;

    public class TestBase
    {
        [SetUp]
        public async Task TestSetUp()
        {
            await ResetState();
        }
    }
}