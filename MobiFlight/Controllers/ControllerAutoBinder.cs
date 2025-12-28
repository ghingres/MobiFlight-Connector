using MobiFlight.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MobiFlight.Controllers
{
    /// <summary>
    /// Performs auto-binding analysis and application for controller serials
    /// </summary>
    public class ControllerAutoBinder
    {
        private readonly List<string> _connectedControllers;
        private readonly Dictionary<string, int> _controllerTypeCount = new Dictionary<string, int>();

        public ControllerAutoBinder(List<string> connectedControllers)
        {
            _connectedControllers = connectedControllers ?? new List<string>();

            // Count controllers by type:name
            foreach (var controllerSerial in _connectedControllers)
            {
                var deviceName = SerialNumber.ExtractDeviceName(controllerSerial);
                var deviceSerialPrefix = SerialNumber.ExtractPrefix(controllerSerial);
                var deviceIdentifier = GetDeviceIdentifier(controllerSerial);

                if (!_controllerTypeCount.ContainsKey(deviceIdentifier))
                    _controllerTypeCount[deviceIdentifier] = 0;
 
                _controllerTypeCount[deviceIdentifier]++;
            }
        }

        /// <summary>
        /// Gets a unique key combining device name and serial prefix for matching
        /// </summary>
        private static string GetDeviceIdentifier(string controllerSerial)
        {
            var deviceName = SerialNumber.ExtractDeviceName(controllerSerial);
            var deviceSerialPrefix = SerialNumber.ExtractPrefix(controllerSerial);
            return $"{deviceSerialPrefix}:{deviceName}";
        }

        /// <summary>
        /// Analyzes binding status for all config items without modifying them
        /// Returns a dictionary mapping original serial -> binding status
        /// </summary>
        public Dictionary<string, (ControllerBindingStatus, string)> AnalyzeBindings(List<IConfigItem> configItems)
        {
            var results = new Dictionary<string, (ControllerBindingStatus, string)>();
            var uniqueSerials = configItems
                .Where(c => !string.IsNullOrEmpty(c.ModuleSerial) && c.ModuleSerial != "-")
                .Select(c => c.ModuleSerial)
                .Distinct()
                .ToList();

            var availableControllers = new List<string>(_connectedControllers);

            foreach (var serial in uniqueSerials)
            {
                var (status, boundController) = AnalyzeSingleBinding(serial, availableControllers);

                results[serial] = (status, boundController);
                if (status == ControllerBindingStatus.Match)
                {
                    // Remove from available controllers to prevent multiple bindings
                    availableControllers.Remove(serial);
                }

                if (status == ControllerBindingStatus.AutoBind)
                {
                    availableControllers.Remove(boundController);
                }
            }

            return results;
        }

        /// <summary>
        /// Applies auto-binding updates to config items based on analysis results
        /// </summary>
        /// <returns>Dictionary mapping original serial -> new serial (only for AutoBound items)</returns>
        public Dictionary<string, string> ApplyAutoBinding(
            List<IConfigItem> configItems,
            Dictionary<string, (ControllerBindingStatus, string)> bindingStatus)
        {
            var serialMappings = bindingStatus.Where((status) => status.Value.Item1 == ControllerBindingStatus.AutoBind)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Item2);

            if (serialMappings.Count == 0) return serialMappings;
            
            // Apply the mappings to config items
            foreach (var item in configItems)
            {
                if (string.IsNullOrEmpty(item.ModuleSerial) || item.ModuleSerial == "-")
                    continue;

                if (serialMappings.ContainsKey(item.ModuleSerial))
                {
                    item.ModuleSerial = serialMappings[item.ModuleSerial];
                }
            }

            return serialMappings;
        }

        private (ControllerBindingStatus status, string claimedController) AnalyzeSingleBinding(string configSerial, List<string> availableControllers)
        {
            // Scenario 1: Exact match
            if (availableControllers.Contains(configSerial))
            {
                return (ControllerBindingStatus.Match, configSerial);
            }

            var deviceTypeName = GetDeviceIdentifier(configSerial);
            var potentialTypeNameMatches = availableControllers
                .Where(c => GetDeviceIdentifier(c) == deviceTypeName)
                .ToList();

            var deviceSerial = SerialNumber.ExtractSerial(configSerial);
            var potentialSerialMatches = availableControllers
                .Where(c => SerialNumber.ExtractSerial(c) == deviceSerial)
                .ToList();

            // Scenario 4: Missing
            if (potentialTypeNameMatches.Count == 0 && potentialSerialMatches.Count == 0)
            {
                return (ControllerBindingStatus.Missing, null);
            }

            var configsWithSameIdentifier = availableControllers
                .Where(s => GetDeviceIdentifier(s) == deviceTypeName)
                .Count();

            // Scenario 5: Multiple matches, need user selection
            if (potentialTypeNameMatches.Count > 1)
            {
                return (ControllerBindingStatus.RequiresManualBind, null);
            }

            // Scenarios 2, 3, 6: Auto-bind
            // - Scenario 2: Serial differs but device name/type match (single match)
            // - Scenario 3: Name differs but serial matches (single match)  
            // - Scenario 6: Multiple matches exist, but multiple configs also exist (1:1 mapping)
            if (potentialTypeNameMatches.Count == 1 || potentialSerialMatches.Count == 1)
            {
                var autoBindSerial = potentialTypeNameMatches.Count == 1 ? potentialTypeNameMatches.First() : potentialSerialMatches.First();
                return (ControllerBindingStatus.AutoBind, autoBindSerial);
            }

            // Fallback
            return (ControllerBindingStatus.Missing, null);
        }

        private string FindNewSerial(string originalSerial)
        {
            var configTypeAndName = GetTypeAndName(originalSerial);
            var potentialMatches = _connectedControllers
                .Where(c => GetTypeAndName(c) == configTypeAndName)
                .ToList();

            return potentialMatches.FirstOrDefault();
        }

        public static string GetTypeAndName(string fullSerial)
        {
            var parts = fullSerial.Split(new[] { SerialNumber.SerialSeparator }, StringSplitOptions.None);
            return parts.Length > 0 ? parts[0].Trim() : fullSerial;
        }
    }
}