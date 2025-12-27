namespace MobiFlight.Controllers
{
    /// <summary>
    /// Represents the binding status of a controller
    /// </summary>
    public enum ControllerBindingStatus
    {
        /// <summary>
        /// Controller is connected with exact serial match (Scenario 1)
        /// </summary>
        Match,

        /// <summary>
        /// Controller was automatically bound (Scenarios 2, 3, 6)
        /// </summary>
        AutoBind,

        /// <summary>
        /// Controller is not connected (Scenario 4)
        /// </summary>
        Missing,

        /// <summary>
        /// Multiple controllers found, requires manual selection (Scenario 5)
        /// </summary>
        RequiresManualBind
    }
}