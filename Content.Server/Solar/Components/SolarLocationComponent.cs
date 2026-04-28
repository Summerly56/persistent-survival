using Content.Server.Solar.EntitySystems;

namespace Content.Server.Solar.Components
{

    [RegisterComponent]
    [Access(typeof(SolarPositioningSystem))]
    public sealed partial class SolarLocationComponent : Component
    {
        /// <summary>
        /// This is set to true when the SolarLocationComponent's angle & vel
        /// are randomized, and is used to prevent resetting these when loading a save
        /// </summary>
        [DataField]
        public bool WasInitialized = false;

        /// <summary>
        /// The current sun angle.
        /// </summary>
        [DataField]
        public Angle TowardsSun { get; set; } = Angle.Zero;

        /// <summary>
        /// The current sun angular velocity. (This is changed in Initialize)
        /// </summary>
        [DataField]
        public Angle SunAngularVelocity { get; set; } = Angle.Zero;
    }
}
