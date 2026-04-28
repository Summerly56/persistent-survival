namespace Content.Shared.MiningFluid.Components;

[Serializable]
[DataDefinition]
public partial struct VariableFluidDefinition
{
    [DataField("prob")]
    public float Probability { get; set; } = 1f;

    [DataField]
    public float Moles { get; set; } = 1f;
}
