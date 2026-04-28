using Content.Shared.Access.Components;
using Content.Shared.Containers.ItemSlots;
using JetBrains.Annotations;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Prototypes;

namespace Content.Shared.Access.Systems
{
    [UsedImplicitly]
    public abstract class SharedIdPrinterConsoleSystem : EntitySystem
    {

        public const string Sawmill = "idconsole";

        public override void Initialize()
        {
            base.Initialize();
        }

        [Serializable, NetSerializable]
        private sealed class IdPrinterConsoleComponentState : ComponentState
        {

            public IdPrinterConsoleComponentState()
            {
            }
        }
    }
}
