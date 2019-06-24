using System.Reactive.Concurrency;
using Bss.XamForms.Validations;
using Xunit;
using ReactiveUI.Legacy;

namespace Bss.XamFormsTests.Validations
{
    public class ValidatableObjectTests
    {
        [Fact]
        public void TestIfAutovalidate()
        {
            var email = new ValidatableObject<string>();

            email.Validations.Add(new EmailRule { ValidationMessage = "Invalid Email" });
            email.AutoValidate = true;

            Assert.False(email.IsValid, "Should be false");
            email.Value = "abc@abc.com";
            Assert.True(email.IsValid, "Should be true");
        }

        [Fact]
        public void TestIfIsInvalidAndHaveErrors()
        {
            var email = new ValidatableObject<string>();

            email.Validations.Add(new EmailRule { ValidationMessage = "Invalid Email" });

            email.Value = "abc@abom";

            email.Validate();

            Assert.False(email.IsValid, "IsValid should be false");
            Assert.True(email.Errors.Count == 1, "Errors.Count == 1");
        }

        [Fact]
        public void TestIfIsValidAndHaveNoErrors()
        {
            var email = new ValidatableObject<string>();

            email.Validations.Add(new EmailRule { ValidationMessage = "Invalid Email" });

            email.Value = "abc@abc.com";

            email.Validate();

            Assert.True(email.IsValid, "IsValid should be true");
            Assert.True(email.Errors.Count == 0, "Errors.Count == 0");
        }

        [Fact]
        public void TestIfIsValidAsObservable()
        {
            var email = new ValidatableObject<string>();

            email.Validations.Add(new EmailRule { ValidationMessage = "Invalid Email" });

            var isValid = email.IsValidAsObservable.CreateCollection(ImmediateScheduler.Instance);
            email.Value = "";

            email.Value = "abc@abc.com";

            Assert.False(isValid[0], "Should be false");
            Assert.False(isValid[1], "Should be false");
            Assert.True(isValid[2], "Should be true");
        }
    }
}