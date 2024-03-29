namespace Billing.IntegrationTests
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

            // close to equivalency required to reconcile precision differences between EF and Postgres
            AssertionOptions.AssertEquivalencyUsing(options =>
              options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation)).WhenTypeIs<DateTime>()
            );
            AssertionOptions.AssertEquivalencyUsing(options =>
              options.Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation)).WhenTypeIs<DateTimeOffset>()
            );
        }
    }
}