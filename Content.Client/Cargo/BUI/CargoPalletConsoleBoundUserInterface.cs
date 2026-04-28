using Content.Client.Cargo.UI;
using Content.Shared.Cargo.BUI;
using Content.Shared.Cargo.Events;
using Content.Shared.Radio.Components;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.Cargo.BUI;

public sealed class CargoPalletConsoleBoundUserInterface : BoundUserInterface
{
    [ViewVariables]
    private CargoPalletMenu? _menu;

    public CargoPalletConsoleBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _menu = this.CreateWindow<CargoPalletMenu>();
        _menu.AppraiseRequested += OnAppraisal;
        _menu.SellRequested += OnSell;
        _menu.ChangeMoneyMode += OnChangeMoneyMode;
        _menu.PossibleStations.OnItemSelected += OnStationSelected;
    }
    private void OnStationSelected(OptionButton.ItemSelectedEventArgs args)
    {
        SendMessage(new CargoPalletStationSelectMessage(args.Id));

    }

    private void OnAppraisal()
    {
        SendMessage(new CargoPalletAppraiseMessage());
    }

    private void OnSell()
    {
        SendMessage(new CargoPalletSellMessage());
    }

    private void OnChangeMoneyMode()
    {
        SendMessage(new CargoPalletChangeMoneyMode());
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not CargoPalletConsoleInterfaceState palletState)
            return;
        _menu?.SetStation(palletState.SelectedName, palletState.TaxingStation, palletState.SelectedStation, palletState.FormattedStations, palletState.CashMode);
        _menu?.SetEnabled(palletState.Enabled);
        _menu?.SetAppraisal(palletState.Appraisal, palletState.Tax, palletState.CashMode, palletState.TaxingName);
        _menu?.SetCount(palletState.Count);
        _menu?.SetMoneyMode(palletState.CashMode);
    }
}
