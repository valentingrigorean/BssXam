using System;
using System.Linq;

namespace Bss.XamCore.Validations
{
    public class OnlyDigitRule : IValidationRule<string>
    {
        public bool AllowSpace { get; set; } = true;

        public string ValidationMessage { get; set; }

        public bool Check(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return value.All(c => char.IsDigit(c) || (AllowSpace && c == ' '));
        }
    }
}
