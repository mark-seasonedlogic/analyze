using System;
using System.Drawing;

namespace BBIReporting
{
    public class DateGreaterThanRule : AlertRuleBase
    {
        public readonly Func<DateTime> _comparisonDateProvider;

        public DateGreaterThanRule(string propertyName, Func<DateTime> comparisonDateProvider, Color alertColor, string description, int severity)
            : base(propertyName, alertColor, description, severity)
        {
            _comparisonDateProvider = comparisonDateProvider;
        }

        public override bool Evaluate(AndroidDevice device)
        {
            var propertyValue = typeof(AndroidDevice).GetProperty(PropertyName)?.GetValue(device)?.ToString();

            if (DateTime.TryParse(propertyValue, out var deviceDate))
            {
                var comparisonDate = _comparisonDateProvider();
                return deviceDate < comparisonDate;
            }

            return false;
        }
    }
}
