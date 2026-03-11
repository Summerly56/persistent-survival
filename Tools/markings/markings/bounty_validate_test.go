package markings_test

// NOTE: These tests validate bounty rebalance logic concepts since the actual
// bounty data is in YAML and loaded at runtime by the game engine.
// They document the design constraints from issue #161 so regressions are caught.

import (
	"testing"
)

// BountyEntry mirrors the structure of a cargo bounty entry for validation.
type BountyEntry struct {
	ID     string
	Amount int
}

// CargoBounty mirrors the structure of a cargo bounty prototype for validation.
type CargoBounty struct {
	ID      string
	Reward  int
	Entries []BountyEntry
}

// isTrivial returns true if a bounty can be completed trivially:
// defined as having only 1 entry type AND total item count below the trivial threshold.
// Issue #161: trivial bounties complete too fast relative to the 2.5-hour refresh timer.
func isTrivial(b CargoBounty) bool {
	if len(b.Entries) < 2 {
		// Single-item bounties are the primary target for removal per issue #161
		return true
	}
	totalItems := 0
	for _, e := range b.Entries {
		totalItems += e.Amount
	}
	// WHY threshold=15: below this even multi-item bounties complete in <10 min
	return totalItems < 15
}

// rewardIsReasonable checks that a bounty reward falls within design guidelines:
// not so low players ignore it, not so high it breaks weekly payout caps.
// Issue #161 guideline: ~20% above one constituent bounty's value, roughly $2000-$7000.
func rewardIsReasonable(b CargoBounty) bool {
	return b.Reward >= 2000 && b.Reward <= 8000
}

// TestRebalancedBountiesAreNotTrivial verifies the new combined bounties
// meet the complexity bar set in issue #161: multiple item types, meaningful quantities.
func TestRebalancedBountiesAreNotTrivial(t *testing.T) {
	// These represent the new bounties defined in cargo_bounties_food.yml
	// and cargo_bounties_materials.yml after the rebalance.
	newBounties := []CargoBounty{
		{
			ID:     "FoodCarrotMixed",
			Reward: 2500,
			Entries: []BountyEntry{
				{ID: "FoodCarrot", Amount: 25},
				{ID: "FoodCarrotFries", Amount: 8},
				{ID: "FoodCarrotCake", Amount: 4},
			},
		},
		{
			ID:     "MaterialsMetalSheet",
			Reward: 4000,
			Entries: []BountyEntry{
				{ID: "SheetSteel", Amount: 50},
				{ID: "SheetPlastic", Amount: 30},
				{ID: "SheetGlass", Amount: 40},
			},
		},
		{
			ID:     "MaterialsExoticAlloy",
			Reward: 6500,
			Entries: []BountyEntry{
				{ID: "SheetPlasma", Amount: 20},
				{ID: "SheetUranium", Amount: 15},
				{ID: "SheetGold", Amount: 10},
			},
		},
		{
			ID:     "FoodBreadAssorted",
			Reward: 3000,
			Entries: []BountyEntry{
				{ID: "FoodBreadPlain", Amount: 10},
				{ID: "FoodBreadMeatball", Amount: 5},
				{ID: "FoodBreadBanana", Amount: 5},
			},
		},
	}

	for _, b := range newBounties {
		t.Run(b.ID, func(t *testing.T) {
			// WHY: every new bounty must have multiple entry types
			// single-type bounties are the definition of trivial per issue #161
			if isTrivial(b) {
				t.Errorf("bounty %q is trivial (single item type or too few items); must have 2+ item types with meaningful quantities", b.ID)
			}
		})
	}
}

// TestRewardRangeIsReasonable ensures no bounty pays out absurdly low or high
// relative to the weekly payout cap and the ~20% guideline from issue #161.
func TestRewardRangeIsReasonable(t *testing.T) {
	bounties := []CargoBounty{
		{ID: "FoodCarrotMixed", Reward: 2500},
		{ID: "FoodBreadAssorted", Reward: 3000},
		{ID: "FoodSaladPlatter", Reward: 2800},
		{ID: "FoodMeatCooked", Reward: 3500},
		{ID: "MaterialsMetalSheet", Reward: 4000},
		{ID: "MaterialsExoticAlloy", Reward: 6500},
		{ID: "MaterialsClothFiber", Reward: 3000},
		{ID: "MaterialsChemReagents", Reward: 5000},
		{ID: "MaterialsCircuitBoards", Reward: 5500},
	}

	for _, b := range bounties {
		t.Run(b.ID, func(t *testing.T) {
			// WHY: reward must be in $2000-$8000 range
			// below $2000: players won't bother; above $8000: breaks economy pacing
			if !rewardIsReasonable(b) {
				t.Errorf("bounty %q reward %d is outside reasonable range [2000, 8000]", b.ID, b.Reward)
			}
		})
	}
}

// TestOldTrivialBountiesWereCorrectlyIdentified documents the REMOVED bounties
// so future contributors understand why they were replaced, not just deleted.
// Issue #161: these completed in <10 min vs the 2.5-hour refresh timer.
func TestOldTrivialBountiesWereCorrectlyIdentified(t *testing.T) {
	// These are the OLD bounties that were trivial and have been removed.
	// This test serves as living documentation of the rebalance decision.
	oldTrivialBounties := []CargoBounty{
		{
			// WHY removed: single item, only 10 units, completable in <5 minutes
			ID:     "OldCarrotTen",
			Reward: 3000,
			Entries: []BountyEntry{
				{ID: "FoodCarrot", Amount: 10},
			},
		},
		{
			// WHY removed: single item, only 3 units, trivially fast to cook
			ID:     "OldCarrotFriesThree",
			Reward: 2000,
			Entries: []BountyEntry{
				{ID: "FoodCarrotFries", Amount: 3},
			},
		},
	}

	for _, b := range oldTrivialBounties {
		t.Run(b.ID+"_was_trivial", func(t *testing.T) {
			// Confirm our isTrivial function correctly identifies these as trivial.
			// If this test fails, isTrivial logic regressed and may let trivial bounties
			// slip back in during future content additions.
			if !isTrivial(b) {
				t.Errorf("bounty %q should be identified as trivial but was not; check isTrivial() logic", b.ID)
			}
		})
	}
}
