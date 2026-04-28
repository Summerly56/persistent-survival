using Content.Shared.AcceptDeath;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;
using static Robust.Client.UserInterface.Controls.BaseButton;

namespace Content.Client.AcceptDeath;

[UsedImplicitly]
public sealed class AcceptDeathBoundUserInterface : BoundUserInterface
{

    [ViewVariables]
    private AcceptDeathMenu? _menu;

    public AcceptDeathBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _menu = this.CreateWindow<AcceptDeathMenu>();
        _menu.Owner = this;
        _menu.AcceptDeathButton.OnPressed += OnAcceptDeath;
        _menu.SOSButton.OnPressed += OnSOS;
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);
        if (_menu == null) return;
        if (state is not AcceptDeathUpdateState cState)
            return;
        _menu.UpdateState(cState);


    }

    public void OnAcceptDeath(ButtonEventArgs args)
    {
        SendMessage(new AcceptDeathFinalizeMessage());
    }

    public void OnSOS(ButtonEventArgs args)
    {
        SendMessage(new AcceptDeathSOSMessage());
    }
}
