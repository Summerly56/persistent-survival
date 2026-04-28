using Content.Server.Solar.EntitySystems;

namespace Content.Server.Solar.Components
{

    /// <summary>
    /// Just a tag for now - This must exist for tracking the nearest star in order for a Solar Console to work on the same grid.
    /// </summary>
    [RegisterComponent]
    [Access(typeof(PowerSolarTrackerSystem))]
    public sealed partial class SolarTrackerComponent : Component
    {

    }
}
