using Content.Shared.Shuttles.Systems;
using Content.Shared.Tag;
using Robust.Shared.Prototypes;
using Robust.Shared.GameStates;

namespace Content.Shared.Shuttles.Components;

/// <summary>
/// Handles what a grid should look like on radar.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedShuttleSystem))]
public sealed partial class IFFComponent : Component
{
    public static readonly Color SelfColor = Color.MediumSpringGreen;

    /// <summary>
    /// Default color to use for IFF if no component is found.
    /// </summary>
    public static readonly Color IFFColor = Color.Gold;

    public static readonly ProtoId<TagPrototype> SortTagShip = "IFFShip";
    public static readonly ProtoId<TagPrototype> SortTagStation = "IFFStation";
    private static readonly ProtoId<TagPrototype>[] ShipSortTags = { SortTagShip };
    private static readonly ProtoId<TagPrototype>[] StationSortTags = { SortTagStation };

    public const float SignatureAlpha = 1f;
    public const float MinSignatureSaturation = 0.8f;
    public const float MinSignatureValue = 0.65f;
    public const float MaxSignatureValue = 0.9f;

    [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
    public IFFFlags Flags = IFFFlags.None;

    [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
    public IFFDesignation Designation = IFFDesignation.Ship;

    [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
    public List<ProtoId<TagPrototype>> SortTags = new();

    /// <summary>
    /// Color for this to show up on IFF.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
    public Color Color = IFFColor;

    public static Color NormalizeSignatureColor(Color color)
    {
        var hsv = Color.ToHsv(color);
        var fallbackHue = Color.ToHsv(IFFColor).X;

        if (hsv.Y < MinSignatureSaturation)
        {
            hsv.X = fallbackHue;
            hsv.Y = MinSignatureSaturation;
        }

        hsv.Y = Math.Clamp(hsv.Y, MinSignatureSaturation, 1f);
        hsv.Z = Math.Clamp(hsv.Z, MinSignatureValue, MaxSignatureValue);
        hsv.W = SignatureAlpha;

        return Color.FromHsv(hsv);
    }

    public IReadOnlyList<ProtoId<TagPrototype>> GetSortTags()
    {
        if (SortTags.Count > 0)
            return SortTags;

        return Designation == IFFDesignation.Station ? StationSortTags : ShipSortTags;
    }

    public bool HasSortTag(ProtoId<TagPrototype> tag)
    {
        foreach (var sortTag in GetSortTags())
        {
            if (sortTag == tag)
                return true;
        }

        return false;
    }
}

public enum IFFDesignation : byte
{
    Ship,
    Station,
}

public enum IFFSortMode : byte
{
    None,
    Ship,
    Station,
}

[Flags]
public enum IFFFlags : byte
{
    None = 0,

    /// <summary>
    /// Should the label for this grid be hidden at all ranges.
    /// </summary>
    HideLabel,

    /// <summary>
    /// Should the grid hide entirely (AKA full stealth).
    /// Will also hide the label if that is not set.
    /// </summary>
    Hide,

    // TODO: Need one that hides its outline, just replace it with a bunch of triangles or lines or something.
}
