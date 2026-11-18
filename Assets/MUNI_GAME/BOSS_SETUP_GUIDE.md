# MUNI GAME — Three Wizard Boss Setup

## Reusable components added
- `MuniBossHealth.cs`: wraps `MuniHealth`, tracks health percentage and raises UnityEvents when the boss changes phase or is defeated.
- `MuniWizardBossAI.cs`: basic chase and three configurable attack patterns: Melee, Dash, Pulse.

## Suggested three-boss configuration
These are suggested starting values for playtesting, not final balance.

| Wizard | Attack pattern | Base damage | Cooldown | Attack radius |
|---|---|---:|---:|---:|
| Wizard 1 | Melee | 10 | 2.0 s | 1.5 |
| Wizard 2 | Dash | 12 | 2.4 s | 1.4 |
| Wizard 3 | Pulse | 15 | 3.0 s | 1.8 |

## Unity Inspector setup
1. Create each wizard boss root with a Collider.
2. Add `MuniHealth`, set Max Health (suggested: 150 / 200 / 250).
3. Add `MuniBossHealth` and configure the boss name and phase thresholds.
4. Add `MuniWizardBossAI`; assign the PlayerRoot Transform to Target.
5. Assign the Player layer in Target Layers. Do not leave the default all-layers mask for final gameplay.
6. Configure each boss's attack pattern and values using the table above.
7. Configure the `On Phase Changed` UnityEvent to trigger the relevant animation, VFX, audio, or UI changes when those assets exist.
8. Configure `On Defeated` to play the boss defeat sequence or disable the boss after the required animation.

## Phase behavior
- Phase 1: above the second-phase threshold.
- Phase 2: at or below the second-phase threshold.
- Phase 3: at or below the final-phase threshold.
- Boss attack damage scales by phase number (x1, x2, x3) in this prototype.

## Current limitations
- Attack patterns are gameplay prototypes: Dash is a short forward movement followed by a sphere hit; Pulse is a larger sphere hit; Melee is a smaller sphere hit.
- No animation, projectile, pathfinding, boss health bar UI, audio, VFX, or cinematic sequence is included yet.
- The AI uses direct transform movement and can pass through obstacles. Add collision-aware movement or NavMesh pathfinding later.
- Set the target layer mask carefully; test in Unity before considering this playable.
- Scripts have not yet been compiled or tested in Unity Editor.
