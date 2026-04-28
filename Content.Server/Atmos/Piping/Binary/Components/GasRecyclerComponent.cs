namespace Content.Server.Atmos.Piping.Binary.Components
{
    [RegisterComponent]
    public sealed partial class GasRecyclerComponent : Component
    {
        [ViewVariables(VVAccess.ReadOnly)]
        [DataField("reacting")]
        public Boolean Reacting { get; set; } = false;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("inlet")]
        public string InletName { get; set; } = "inlet";

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("outlet")]
        public string OutletName { get; set; } = "outlet";

        [DataField("recipes"), ViewVariables(VVAccess.ReadWrite)]
        public List<string> EnabledRecipes { get; set; } = new();

        [DataField("containerSlot"), ViewVariables(VVAccess.ReadWrite)]
        public string ContainerSlotId { get; set; } = "beaker_slot";
    }
}
