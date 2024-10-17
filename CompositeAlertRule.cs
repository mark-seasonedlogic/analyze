using System.Collections.Generic;
using System.Drawing;

namespace BBIReporting
{
    public class CompositeAlertRule : AlertRuleBase
    {
        public CompositeAlertRule(List<AlertRuleBase> conditions, string logicalOperator, Color alertColor, string description, int severity)
            : base(null, alertColor, description, severity)  // No specific property name for composite rule
        {
            Conditions = conditions;
            LogicalOperator = logicalOperator;
        }

        public override bool Evaluate(AndroidDevice device)
        {
            return EvaluateCompositeRule(device);
        }

        // Method to concatenate descriptions for tooltips
        public string GetCompositeDescription()
        {
            string combinedDescription = string.Join($" {LogicalOperator} ", Conditions.ConvertAll(c => c.Description));
            return combinedDescription;
        }

        // Pass the color to the DataGrid
        public Color GetAlertColor()
        {
            return AlertColor;  // Pass the composite alert color
        }
    }
}
