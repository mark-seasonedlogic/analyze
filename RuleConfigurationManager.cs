using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;

namespace BBIReporting
{


    public static class RuleConfigurationManager
    {
        private const string ConfigFilePath = "AlertRulesConfig.json";

        // Load alert rules from the configuration file
        // Load alert rules from the configuration file using a factory-based approach
        public static List<AlertRuleBase> LoadRules()
        {
            try
            {
                if (!File.Exists(ConfigFilePath))
                {
                    Debug.Print("Config file not found: {0}", ConfigFilePath);
                    return new List<AlertRuleBase>();
                }

                string json = File.ReadAllText(ConfigFilePath);

                // Deserialize into a list of AlertRuleConfig first
                var configs = JsonConvert.DeserializeObject<List<AlertRuleConfig>>(json);

                // Use the factory to create the appropriate AlertRuleBase derived classes
                return RuleFactory.CreateRules(configs);  // RuleFactory maps config to appropriate derived rule classes
            }
            catch (Exception ex)
            {
                Debug.Print("Failed to load rules: {0}", ex.Message);
                return new List<AlertRuleBase>();
            }
        }
        // Save alert rules to the configuration file
        public static void SaveRules(List<AlertRuleConfig> configs)
        {
            try
            {
                // Use JsonConvert from Newtonsoft.Json to serialize the object
                string json = JsonConvert.SerializeObject(configs, Formatting.Indented);
                File.WriteAllText(ConfigFilePath, json);
                Debug.Print("Rules successfully saved to {0}", ConfigFilePath);
            }
            catch (Exception ex)
            {
                Debug.Print("Failed to save rules: {0}", ex.Message);
            }
        }

    }
}
