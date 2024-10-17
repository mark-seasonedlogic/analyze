using System;
using System.Collections.Generic;
using System.Drawing;

namespace BBIReporting
{
    public class RuleFactory
    {
        public static List<AlertRuleBase> CreateRules(List<AlertRuleConfig> configs)
        {
            var rules = new List<AlertRuleBase>();

            foreach (var config in configs)
            {
                var rule = CreateRule(config);
                if (rule != null)
                {
                    rules.Add(rule);
                }
            }

            return rules;
        }

        private static AlertRuleBase CreateRule(AlertRuleConfig config)
        {
            if (config.Conditions != null && config.Conditions.Count > 0)
            {
                // Create a composite rule
                var conditions = new List<AlertRuleBase>();
                foreach (var conditionConfig in config.Conditions)
                {
                    conditions.Add(CreateRule(conditionConfig));
                }

                return new CompositeAlertRule(conditions, config.LogicalOperator, config.AlertColor, config.Description, config.Severity);
            }

            // Create individual rules
            string propertyName = config.PropertyName;
            string comparisonType = config.ComparisonType;
            string comparisonValue = config.ComparisonValue;

            switch (comparisonType)
            {
                case "Equals":
                    return new EqualsRule(propertyName, comparisonValue, config.AlertColor, config.Description, config.Severity);
                case "DateGreaterThan":
                    return new DateGreaterThanRule(propertyName, () => DateTime.Now.AddDays(-int.Parse(comparisonValue)), config.AlertColor, config.Description, config.Severity);
                case "GreaterThan":
                    return new GreaterThanRule(propertyName, comparisonValue, config.AlertColor, config.Description, config.Severity);
                case "LessThan":
                    return new LessThanRule(propertyName, comparisonValue, config.AlertColor, config.Description, config.Severity);
                default:
                    throw new NotImplementedException($"Unsupported rule type: {comparisonType}");
            }
        }
    }
}
