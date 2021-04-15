namespace Billing.SharedTestHelpers.Fakes.InsuranceProvider
{
    using AutoBogus;
    using Billing.Core.Entities;

    // or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
    public class FakeInsuranceProvider : AutoFaker<InsuranceProvider>
    {
        public FakeInsuranceProvider()
        {
            // if you want default values on any of your properties (e.g. an int between a certain range or a date always in the past), you can add `RuleFor` lines describing those defaults
            //RuleFor(i => i.ExampleIntProperty, i => i.Random.Number(50, 100000));
            //RuleFor(i => i.ExampleDateProperty, i => i.Date.Past()); 
        }
    }
}