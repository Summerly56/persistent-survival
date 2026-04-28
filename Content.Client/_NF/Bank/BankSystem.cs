using Content.Shared._NF.Bank;
using Content.Shared._NF.Bank.Components;
using Content.Shared.GameTicking;
using Robust.Client.Player;
using Robust.Shared.Player;

namespace Content.Client.Bank;

// Shared is abstract.
public sealed partial class BankSystem : SharedBankSystem
{

    public MoneyAccountsComponent? GetMoneyAccountsComponent()
    {

        var personalAccountQuery = AllEntityQuery<MoneyAccountsComponent>();
        while (personalAccountQuery.MoveNext(out var uid, out var comp))
        {
            return comp;
        }
        return null;
    }
    public bool TryGetBalance(EntityUid ent, out int balance)
    {
        balance = 0;
        var component = GetMoneyAccountsComponent();
        if (component == null)
        {
            return false;
        }
        MoneyAccountsComponent? accounts = component;
        var accName = Name(ent);
        if (!accounts!.TryGetAccount(accName, out var account))
        {
            return false;
        }

        balance = account!.Balance;
        return true;
    }

}
