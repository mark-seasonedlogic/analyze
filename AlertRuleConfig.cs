using System.Collections.Generic;
using System.Drawing;

namespace BBIReporting
{
    public class AlertRuleConfig
    {
        // Basic properties for a single rule
        public string PropertyName { get; set; }
        public string ComparisonType { get; set; }
        public string ComparisonValue { get; set; }
        public Color AlertColor { get; set; }
        public string Description { get; set; }
        public int Severity { get; set; }

        // New properties for composite rules
        public string LogicalOperator { get; set; }  // AND, OR, etc.
        public List<AlertRuleConfig> Conditions { get; set; }  // Nested conditions for composite rules

        public AlertRuleConfig()
        {
            Conditions = new List<AlertRuleConfig>();
        }
    }
}
