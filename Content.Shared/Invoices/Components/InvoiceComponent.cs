using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Invoices.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class InvoiceComponent : Component
{
    [DataField]
    public DateTime? PaidTime;
    [DataField]
    public int InvoiceCost = 0;
    [DataField]
    public string InvoiceReason = "";

    [DataField]
    public int? TargetStation = null;
    [DataField]
    public string? TargetPerson = null;

    [DataField]
    public bool Paid = false;

    [DataField]
    public string PaidBy = "";

    [DataField]
    public int TaxOwner = 0;

    [DataField]
    public bool PayslipMode = false;

    [DataField]
    public SoundSpecifier PaySuccessSound = new SoundPathSpecifier("/Audio/Effects/kaching.ogg");
    [DataField]
    public SoundSpecifier ErrorSound = new SoundPathSpecifier("/Audio/Effects/Cargo/buzz_sigh.ogg");
}


[Serializable, NetSerializable]
public sealed class InvoiceBoundUserInterfaceState : BoundUserInterfaceState
{
    public Dictionary<int, string> PossibleStations = new();
    public int InvoiceCost;
    public string InvoiceReason;
    public string PaidTo;
    public bool Paid;
    public string PaidBy;
    public string UserName;
    public bool PayslipMode;
    public DateTime? PaidTime;

    public InvoiceBoundUserInterfaceState(Dictionary<int, string> possiblestations, int invoicecost, string invoicereason, string paidTo, string paidBy, bool paid, string userName, bool payslipMode, DateTime? paidTime)
    {
        PossibleStations = possiblestations;
        InvoiceCost = invoicecost;
        InvoiceReason = invoicereason;
        PaidTo = paidTo;
        PaidBy = paidBy;
        Paid = paid;
        UserName = userName;
        PayslipMode = payslipMode;
        PaidTime = paidTime;
    }
}

[Serializable, NetSerializable]
public sealed class PayInvoicePersonal : BoundUserInterfaceMessage
{
    public PayInvoicePersonal()
    {
    }
}

[Serializable, NetSerializable]
public sealed class PayInvoice : BoundUserInterfaceMessage
{
    public int Station;
    public PayInvoice(int station)
    {
        Station = station;
    }
}


[Serializable, NetSerializable]
public enum InvoiceUiKey : byte
{
    Key,
}
