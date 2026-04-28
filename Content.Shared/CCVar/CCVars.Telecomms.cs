using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    public static readonly CVarDef<bool> TCommsUseNetwork =
        CVarDef.Create("tcomms.use_network", true, CVar.SERVERONLY);

}
