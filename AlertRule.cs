using System;
using System.Drawing;

namespace BBIReporting
{
    /* OLD Class Definition

     public class AlertRule
     {
         public Func<AndroidDevice, bool> Condition { get; set; }  // Condition to evaluate the rule
         public string Description { get; set; }  // Optional: For displaying rule description
         public Color AlertColor { get; set; }  // Color to apply if the condition is met

         public AlertRule(Func<AndroidDevice, bool> condition, Color alertColor, string description = "")
         {
             Condition = condition;
             AlertColor = alertColor;
             Description = description;
         }
     }
    */


    public class AlertRule
    {
        public Func<AndroidDevice, bool> Condition { get; set; }
        public Color AlertColor { get; set; }
        public string Description { get; set; }
        public string ComparisonType { get; set; }  // Add ComparisonType property
        public int Severity { get; }  // Lower values mean higher priority
        public AlertRule(Func<AndroidDevice, bool> condition, Color alertColor, string description, string comparisonType)
        {
            Condition = condition;
            AlertColor = alertColor;
            Description = description;
            ComparisonType = comparisonType;
        }
    }


}
