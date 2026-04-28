using Content.Shared.Power.Generation.GasGenerator;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;

namespace Content.Client.Power.Generation.GasGenerator;

/// <summary>
/// Bound user interface for the gas generator.
/// Displays power output, efficiency, and fuel status.
/// </summary>
[UsedImplicitly]
public sealed class GasGeneratorBoundUserInterface : BoundUserInterface
{
    private GasGeneratorWindow? _window;

    public GasGeneratorBoundUserInterface(EntityUid entity, Enum uiKey) : base(entity, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();
        _window = this.CreateWindow<GasGeneratorWindow>();
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not GasGeneratorBoundUserInterfaceState gasState)
            return;

        _window?.UpdateState(gasState);
    }
}

/// <summary>
/// Window for the gas generator UI.
/// </summary>
public sealed class GasGeneratorWindow : BaseWindow
{
    private readonly Label _powerLabel;
    private readonly Label _efficiencyLabel;
    private readonly Label _consumptionLabel;
    private readonly Label _fuelTempLabel;
    private readonly Label _fuelPressureLabel;
    private readonly Label _poweredLabel;

    public GasGeneratorWindow()
    {
        var mainVBox = new BoxContainer
        {
            Orientation = BoxContainer.LayoutOrientation.Vertical
        };

        _powerLabel = new Label { Text = "Power Output: 0 W" };
        _efficiencyLabel = new Label { Text = "Efficiency: 0%" };
        _consumptionLabel = new Label { Text = "Fuel Consumption: 0.00 mol/s" };
        _fuelTempLabel = new Label { Text = "Fuel Temp: 0 K" };
        _fuelPressureLabel = new Label { Text = "Fuel Pressure: 0 kPa" };
        _poweredLabel = new Label { Text = "Powered: No" };

        mainVBox.AddChild(_powerLabel);
        mainVBox.AddChild(_efficiencyLabel);
        mainVBox.AddChild(_consumptionLabel);
        mainVBox.AddChild(_fuelTempLabel);
        mainVBox.AddChild(_fuelPressureLabel);
        mainVBox.AddChild(_poweredLabel);

        AddChild(mainVBox);
    }

    public void UpdateState(GasGeneratorBoundUserInterfaceState state)
    {
        _powerLabel.Text = $"Power Output: {state.CurrentPowerOutput:F0} W / {state.MaxPowerOutput:F0} W";
        _efficiencyLabel.Text = $"Efficiency: {state.CurrentEfficiency * 100:F1}%";
        _consumptionLabel.Text = $"Fuel Consumption: {state.CurrentConsumptionRate:F2} mol/s";
        _fuelTempLabel.Text = $"Fuel Temp: {state.FuelTemperature:F1} K";
        _fuelPressureLabel.Text = $"Fuel Pressure: {state.FuelPressure:F1} kPa";
        _poweredLabel.Text = $"Powered: {(state.Powered ? "Yes" : "No")}";
    }
}
