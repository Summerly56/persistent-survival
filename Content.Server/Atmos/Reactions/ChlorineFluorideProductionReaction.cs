using Content.Server.Atmos.EntitySystems;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Reactions;
using JetBrains.Annotations;

namespace Content.Server.Atmos.Reactions
{
    /// <summary>
    /// ClF3 synthesis: Cl2 + 3 F2 → 2 ClF3
    /// Real-world ClF3 is produced industrially at ~250°C (523K) and is highly exothermic.
    /// Formation enthalpy is approximately -163.2 kJ/mol of ClF3.
    /// The reaction is prevented from running if a fire reaction already occurred this tick,
    /// avoiding instant recombination of decomposition products.
    /// </summary>
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class ChlorineFluorideProductionReaction : IGasReactionEffect
    {
        public ReactionResult React(GasMixture mixture, IGasMixtureHolder? holder, AtmosphereSystem atmosphereSystem, float heatScale)
        {
            // Prevent immediate recombination into ClF3 if a fire reaction has just occurred this tick.
            if (mixture.ReactionResults[(byte)GasReaction.Fire] != 0)
                return ReactionResult.NoReaction;

            var oldHeatCapacity = atmosphereSystem.GetHeatCapacity(mixture, true);
            var temperature = mixture.Temperature;
            var energyReleased = 0f;

            var nCl2 = mixture.GetMoles(Gas.Chlorine);
            var nF2 = mixture.GetMoles(Gas.Fluorine);

            // Stoichiometry: Cl2 + 3 F2 → 2 ClF3
            // Limiting reagent: min(Cl2/1, F2/3)
            var limiting = MathF.Min(nCl2 / 1f, nF2 / 3f);
            var extent = limiting / Atmospherics.ClF3ProductionRate;

            if (extent > Atmospherics.GasMinMoles)
            {
                mixture.AdjustMoles(Gas.Chlorine, -extent * 1f);
                mixture.AdjustMoles(Gas.Fluorine, -extent * 3f);
                mixture.AdjustMoles(Gas.ChlorineTrifluoride, extent * 2f);

                // Exothermic: -163.2 kJ/mol of ClF3 formed (releases heat).
                energyReleased = Atmospherics.ClF3FormationEnergy * extent * 2f;
                energyReleased /= heatScale;
            }

            if (MathF.Abs(energyReleased) > 0f)
            {
                var newHeatCapacity = atmosphereSystem.GetHeatCapacity(mixture, true);
                if (newHeatCapacity > Atmospherics.MinimumHeatCapacity)
                    mixture.Temperature = (temperature * oldHeatCapacity + energyReleased) / newHeatCapacity;
            }

            return extent > Atmospherics.GasMinMoles ? ReactionResult.Reacting : ReactionResult.NoReaction;
        }
    }
}
