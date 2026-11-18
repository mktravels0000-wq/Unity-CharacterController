# MUNI GAME — Villain Combat, Run & Hide AI

## Intended behaviour

Villains can fight Muni, retreat, and hide. Their ordinary attacks are theatrical/gameplay attacks only: they do **not** reduce Muni's health. Their special green-rope spell can call Muni's existing 10-second magic binding.

## Script

Add `MuniVillainTacticsAI.cs` and `MuniHealth.cs` to each villain root. Assign the Muni player Transform to **Muni Target**. Muni must have `MuniMagicBinding.cs` for the spell behaviour.

Create empty GameObjects at practical hiding locations (behind a tree, boulder, hut corner, etc.) and assign them to **Hide Points**. These are destination markers; build the actual cover geometry separately.

## Three suggested villain configurations

| Villain | Style | Attack cooldown | Binding chance per attack |
|---|---|---:|---:|
| Villain 1 | AggressiveFighter | 1.8 sec | 0 (not used) |
| Villain 2 | HitAndRun | 2.2 sec | 0.10 |
| Villain 3 | SpellCaster | 2.8 sec | 0.35 |

These are initial tuning suggestions, not final difficulty balance. Adjust health, speed, detection range and flee threshold per villain.

## Unity Inspector setup

1. Assign the Muni root to **Muni Target**.
2. Add 2–4 hide-point transforms per villain, placed behind real cover.
3. Configure **Style**, **Chase Speed**, **Flee Speed**, **Melee Range**, and **Attack Cooldown**.
4. For attack animation, connect the **On Attack** UnityEvent to the villain Animator trigger or attack method.
5. For spell animation/effects, connect **On Binding Cast** to the spell animation/VFX. The script calls `MuniMagicBinding.ApplyBinding()` when its binding chance succeeds.
6. Make sure Muni's `MuniHealth` Inspector has **Damage Immune** enabled. The AI itself never calls `TakeDamage` on Muni.
7. Ensure the Muni green-rope visual prefab is assigned to `MuniMagicBinding`.

## Current limitations

- This is a lightweight direct-movement prototype, not NavMesh pathfinding. Villains can walk through obstacles unless collision-aware movement/pathfinding is added.
- Hide points do not provide automatic line-of-sight checks; they are manually placed retreat destinations.
- The script supplies AI state and UnityEvents, not character models, animation clips, sound, spell projectiles, or VFX assets.
- Muni's normal health damage immunity must be enabled in the Inspector on the player component. The magic binding is a separate status effect.
- The script has not been compiled or Play Mode tested in Unity yet.
- Existing `Assets/NaughtyCharacter` starter files are not modified.
