using Robust.Shared.Serialization;
namespace Content.Shared.Cargo.BUI;

[NetSerializable, Serializable]
public sealed class CargoPalletConsoleInterfaceState : BoundUserInterfaceState
{
    /// <summary>
    /// estimated apraised value of all the entities on top of pallets on the same grid as the console
    /// </summary>
    public int Appraisal;

    /// <summary>
    /// number of entities on top of pallets on the same grid as the console
    /// </summary>
    public int Count;

    /// <summary>
    /// are the buttons enabled
    /// </summary>
    public bool Enabled;

    public CargoSaleMode CashMode;

    public int Tax;

    public int TaxingStation;

    public string TaxingName;

    public Dictionary<int, string> FormattedStations;

    public int SelectedStation;

    public string SelectedName;



    public CargoPalletConsoleInterfaceState(int appraisal, int count, bool enabled, CargoSaleMode cashmode, int tax, int taxingStation, string taxingName, Dictionary<int, string> formattedStations, int selectedFaction, string selectedName)
    {
        Appraisal = appraisal;
        Count = count;
        Enabled = enabled;
        CashMode = cashmode;
        Tax = tax;
        TaxingStation = taxingStation;
        FormattedStations = formattedStations;
        SelectedStation = selectedFaction;
        TaxingName = taxingName;
        SelectedName = selectedName;
    }
}

public enum CargoSaleMode : byte
{
    Deposit,
    Cash,
    Payslip
}
