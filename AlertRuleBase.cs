using System;
using System.Collections.Generic;
using System.Drawing;

namespace BBIReporting
{
    public abstract class AlertRuleBase
    {
        public string PropertyName { get; }
        public Color AlertColor { get; }
        public string Description { get; }
        public int Severity { get; }

        // New properties for composite rules
        public string LogicalOperator { get; set; }
        public List<AlertRuleBase> Conditions { get; set; }

        protected AlertRuleBase(string propertyName, Color alertColor, string description, int severity)
        {
            PropertyName = propertyName;
            AlertColor = alertColor;
            Description = description;
            Severity = severity;
        }

        // Method to be overridden by derived classes for individual rule logic
        public abstract bool Evaluate(AndroidDevice device);

        // Method to evaluate composite rules
        public virtual bool EvaluateCompositeRule(AndroidDevice device)
        {
            if (Conditions == null || Conditions.Count == 0)
                throw new InvalidOperationException("No conditions for composite rule.");

            bool result = LogicalOperator == "AND";

            foreach (var condition in Conditions)
            {
                if (LogicalOperator == "AND")
                {
                    result &= condition.Evaluate(device);
                }
                else if (LogicalOperator == "OR")
                {
                    result |= condition.Evaluate(device);
                }

                if (LogicalOperator == "AND" && !result) return false;
                if (LogicalOperator == "OR" && result) return true;
            }

            return result;
        }
    }
}
