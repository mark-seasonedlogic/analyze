using BBIReporting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBIReporting
{
    public class EqualsRule : AlertRuleBase
    {
        public string ComparisonValue { get; set; }

        public EqualsRule(string propertyName, string comparisonValue, Color alertColor, string description, int severity)
            :base(propertyName,alertColor,description, severity) //Pass parameters to the base class
        {
            ComparisonValue = comparisonValue;
        }

        public override bool Evaluate(AndroidDevice device)
        {
            var property = typeof(AndroidDevice).GetProperty(PropertyName);
            if (property == null) return false;

            var value = property.GetValue(device)?.ToString();
            return value != null && value.Equals(ComparisonValue, StringComparison.OrdinalIgnoreCase);
        }
    }
}
