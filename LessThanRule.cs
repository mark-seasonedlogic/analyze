using BBIReporting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBIReporting
{
    public class LessThanRule : AlertRuleBase
    {
        public string ComparisonValue { get; set; }

        public LessThanRule(string propertyName, string comparisonValue, Color alertColor, string description, int severity)
            : base(propertyName, alertColor, description, severity)
        {
            ComparisonValue = comparisonValue;
        }

        public override bool Evaluate(AndroidDevice device)
        {
            var property = typeof(AndroidDevice).GetProperty(PropertyName);
            if (property == null) return false;

            var value = property.GetValue(device)?.ToString();
            if (value == null) return false;

            if (double.TryParse(value, out var numericValue) && double.TryParse(ComparisonValue, out var comparisonValue))
            {
                return numericValue < comparisonValue;
            }

            return false;
        }
    }
}
