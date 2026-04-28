using Content.Shared.Containers.ItemSlots;
using Content.Shared.Invoices.Systems;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Invoices.Components;

[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedInvoicePrinterConsoleSystem))]
public sealed partial class InvoicePrinterConsoleComponent : Component
{
    public static string PrivilegedIdCardSlotId = "InvoicePrinter-privilegedId";

    [DataField]
    public ItemSlot PrivilegedIdSlot = new();

    [DataField]
    public bool StationMode = true;

    /// <summary>
    /// The sound made when printing occurs
    /// </summary>
    [DataField]
    public SoundSpecifier PrintSound = new SoundCollectionSpecifier("PrinterPrint");

    [DataField]
    public int SelectedStation = 0;

    [DataField]
    public bool InvoiceMode = true;

}
[Serializable, NetSerializable]
public sealed class PrintInvoice : BoundUserInterfaceMessage
{

    public string InvoiceReason = "";
    public int InvoiceCost = 0;
    public string InvoiceTitle = "";

    public PrintInvoice(string invoiceReason, int invoiceCost, string invoiceTitle)
    {
        InvoiceReason = invoiceReason;
        InvoiceCost = invoiceCost;
        InvoiceTitle = invoiceTitle;
    }
}

[Serializable, NetSerializable]
public sealed class ChangeInvoiceMode : BoundUserInterfaceMessage
{
}
[Serializable, NetSerializable]
public sealed class ChangeInvoicePayslipMode : BoundUserInterfaceMessage
{
}

[Serializable, NetSerializable]
public sealed class InvoicePrinterStationSelectMessage : BoundUserInterfaceMessage
{
    public int Target;
    public InvoicePrinterStationSelectMessage(int target)
    {
        Target = target;
    }

}


[Serializable, NetSerializable]
public sealed class InvoicePrinterConsoleBoundUserInterfaceState : BoundUserInterfaceState
{
    public bool IdPresent = false;
    public string? TargetName = null;
    public string? IdName = null;
    public bool StationMode = true;
    public int TaxRate = 0;
    public int TaxingStation;

    public string TaxingName;

    public Dictionary<int, string> FormattedStations;

    public int SelectedStation;

    public string SelectedName;
    public bool InvoiceMode;
    public InvoicePrinterConsoleBoundUserInterfaceState(bool idpresent, string? idname, string? targetname, bool stationMode, int taxRate, int taxingStation, string taxingName, int selectedStation, Dictionary<int, string> formattedStations, string selectedName, bool invoiceMode)
    {
        IdPresent = idpresent;
        TargetName = targetname;
        IdName = idname;
        StationMode = stationMode;
        TaxRate = taxRate;
        TaxingStation = taxingStation;
        TaxingName = taxingName;
        FormattedStations = formattedStations;
        SelectedStation = selectedStation;
        SelectedName = selectedName;
        InvoiceMode = invoiceMode;
    }
}

[Serializable, NetSerializable]
public enum InvoicePrinterConsoleUiKey : byte
{
    Key,
}
