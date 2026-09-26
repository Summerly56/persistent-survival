using System.Numerics;
using Content.Server.Stack;
using Content.Server.Stunnable;
using Content.Shared.ActionBlocker;
using Content.Shared.CombatMode;
using Content.Shared.Damage.Systems;
using Content.Shared.Explosion;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Input;
using Content.Shared.Movement.Pulling.Components;
using Content.Shared.Movement.Pulling.Systems;
using Content.Shared.Stacks;
using Content.Shared.Standing;
using Content.Shared.Throwing;
using Robust.Shared.GameStates;
using Robust.Shared.Input.Binding;
using Robust.Shared.Map;
using Robust.Shared.Physics.Components;
using Robust.Shared.Player;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server.Hands.Systems
{
    public sealed partial class HandsSystem : SharedHandsSystem
    {
        // Starlight: Moved dependencies to shared since nothing in here needs them anymore.

        public override void Initialize()
        {
            base.Initialize();

            // Starlight: Moved disarmed event listener to shared for prediction.

            SubscribeLocalEvent<HandsComponent, ComponentGetState>(GetComponentState);

            SubscribeLocalEvent<HandsComponent, BeforeExplodeEvent>(OnExploded);

            // Starlight begin: Moved DropHandItemsEvent, physics query assignment, and input command binding to shared for prediction.
        }

        public override void Shutdown()
        {
            base.Shutdown();

            CommandBinds.Unregister<HandsSystem>();
        }

        private void GetComponentState(EntityUid uid, HandsComponent hands, ref ComponentGetState args)
        {
            args.State = new HandsComponentState(hands);
        }


        private void OnExploded(Entity<HandsComponent> ent, ref BeforeExplodeEvent args)
        {
            if (ent.Comp.DisableExplosionRecursion)
                return;

            foreach (var held in EnumerateHeld(ent.AsNullable()))
            {
                args.Contents.Add(held);
            }
        }

        // Starlight: Moved OnDisarmed to shared for prediction.
        #region interactions

        // Starlight: Moved HandleThrowItem, OnDropHandItems, and ThrowHeldItem to shared for prediction.

        #endregion
    }
}
