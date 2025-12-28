using MobiFlight.Base;
using System.Collections.Generic;

namespace MobiFlight.Controllers
{
    /// <summary>
    /// High-level service for controller binding operations
    /// </summary>
    public class ControllerBindingService
    {
        private readonly IExecutionManager _executionManager;

        public ControllerBindingService(IExecutionManager executionManager)
        {
            _executionManager = executionManager;
        }

        /// <summary>
        /// Analyzes binding status WITHOUT modifying the project
        /// Returns: Dictionary mapping ModuleSerial -> ControllerBindingStatus
        /// </summary>
        public Dictionary<string, (ControllerBindingStatus, string)> AnalyzeProjectBindings(Project project)
        {
            var connectedControllers = GetAllConnectedControllers();
            var binder = new ControllerAutoBinder(connectedControllers);

            var allResults = new Dictionary<string, (ControllerBindingStatus, string)>();
            var appliedBindingMappings = new Dictionary<string, string>();

            foreach (var configFile in project.ConfigFiles)
            {
                var results = binder.AnalyzeBindings(configFile.ConfigItems, appliedBindingMappings);
                foreach (var kvp in results)
                {
                    var status = kvp.Value.Item1;
                    allResults[kvp.Key] = kvp.Value;
                    if (status != ControllerBindingStatus.AutoBind) continue;

                    appliedBindingMappings[kvp.Key] = kvp.Value.Item2;
                }
            }

            return allResults;
        }

        /// <summary>
        /// Performs auto-binding and modifies config items
        /// Returns: Dictionary mapping ModuleSerial -> ControllerBindingStatus
        /// </summary>
        public Dictionary<string, (ControllerBindingStatus, string)> PerformAutoBinding(Project project)
        {
            var connectedControllers = GetAllConnectedControllers();
            var binder = new ControllerAutoBinder(connectedControllers);

            var allResults = new Dictionary<string, (ControllerBindingStatus, string)>();
            var appliedBindingMappings = new Dictionary<string, string>();

            foreach (var configFile in project.ConfigFiles)
            {
                var results = binder.AnalyzeBindings(configFile.ConfigItems, appliedBindingMappings);
                var serialMappings = binder.ApplyAutoBinding(configFile.ConfigItems, results);

                foreach (var kvp in results)
                {
                    // Only add if not already present (first occurrence wins)
                    if (!allResults.ContainsKey(kvp.Key))
                    {
                        allResults[kvp.Key] = kvp.Value;
                    }
                }

                // Update binding mappings for next config file
                foreach (var mapping in serialMappings)
                {
                    appliedBindingMappings[mapping.Key] = mapping.Value;
                }
            }

            // Update project metadata with new controller serials
            project.DetermineProjectInfos();
            project.ControllerBindings = allResults;

            return allResults;
        }

        private List<string> GetAllConnectedControllers()
        {
            var serials = new List<string>();

            foreach (var module in _executionManager.getMobiFlightModuleCache().GetModules())
            {
                serials.Add($"{module.Name}{SerialNumber.SerialSeparator}{module.Serial}");
            }

            foreach (var joystick in _executionManager.GetJoystickManager().GetJoysticks())
            {
                serials.Add($"{joystick.Name} {SerialNumber.SerialSeparator}{joystick.Serial}");
            }

            foreach (var midiBoard in _executionManager.GetMidiBoardManager().GetMidiBoards())
            {
                serials.Add($"{midiBoard.Name} {SerialNumber.SerialSeparator}{midiBoard.Serial}");
            }

            return serials;
        }
    }
}