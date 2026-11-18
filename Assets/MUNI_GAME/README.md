# MUNI GAME — Demo 01

This folder contains additive prototype scripts for MUNI GAME. The existing `Assets/NaughtyCharacter` controller has not been modified.

## Current prototype scripts

- `Scripts/MuniHealth.cs` — reusable health component.
- `Scripts/MuniEnemyHealth.cs` — basic enemy health and defeat handling.
- `Scripts/MuniSpearCombat.cs` — short-range spear hit detection.
- `Scripts/MuniTransformation.cs` — switches between assigned normal and divine visual objects.

These are starter components only. They have not yet been compiled or play-tested in Unity. The current development machine does not have Unity installed and has insufficient free disk space for a safe Unity installation at this stage.

## Suggested Unity scene setup (after Unity is installed)

1. Open this project with the editor version recorded in `ProjectSettings/ProjectVersion.txt` (2021.3.8f1).
2. Create a separate scene named `MUNI_Demo01`; do not overwrite the starter demo scene.
3. Create a small ground plane and a few primitive village props.
4. Use the existing NaughtyCharacter player/controller setup for third-person movement and camera. Inspect its demo scene and input-action asset before duplicating or changing anything.
5. Add `MuniHealth` and `MuniSpearCombat` to the player. Assign the player's health component and tune the attack range/radius in the Inspector.
6. Add `MuniHealth` and `MuniEnemyHealth` to a basic enemy. Assign the health reference on `MuniEnemyHealth`.
7. Add `MuniTransformation` to the player. Assign two child visual GameObjects: one for the normal appearance and one for the divine appearance. Keep only the normal visual active at scene start.
8. For a temporary PC test, connect `MuniSpearCombat.Attack` and `MuniTransformation.ToggleTransformation` to UI Buttons or UnityEvents. Android touch UI can call these same public methods later.
9. Test in Play Mode, then fix any Inspector wiring or compile issues before building.

## Important

- The scripts do not create a player, enemy, scene, animations, UI, or mobile controls automatically.
- Spear combat uses an overlap sphere in front of the player; tune its position and collision layers in a real scene.
- Transformation only swaps visual GameObjects. It does not yet add divine abilities, animation, effects, or stat changes.
- Use original or properly licensed character art, audio, and other assets. Preserve the existing starter project's MIT license and copyright notice.
