# MUNI GAME — Demo 01 Scene Setup

## Current decision
- Do not use the Ellen character as the MUNI hero.
- Do not add a primitive placeholder character.
- Integrate the custom MUNI hero only after its model is ready.
- Keep the original `Assets/NaughtyCharacter` files unchanged.

## Existing starter-project assets verified
- Scene: `Assets/NaughtyCharacter/_Scenes/Demo.unity`
- Camera rig prefab: `Assets/NaughtyCharacter/Prefabs/CameraRig.prefab`
- Input prefab: `Assets/NaughtyCharacter/Prefabs/PlayerInput.prefab`
- Spring arm prefab: `Assets/NaughtyCharacter/Prefabs/SpringArm.prefab`
- Existing character prefab (reference only; not selected for MUNI): `Assets/NaughtyCharacter/Prefabs/Ellen.prefab`
- Controller definition: `Assets/NaughtyCharacter/Defs/PlayerController.asset`

The camera rig prefab contains a CameraRig, Pivot, SpringArm and Main Camera. The PlayerInput prefab uses Unity's Input System and has Move, Look and Jump events wired to NaughtyCharacter.PlayerInputComponent. The starter project is based on Unity 2021.3.8f1.

## Demo 01 target hierarchy (planned)
This is the intended logical organization. It is not yet a serialized Unity scene.

```text
MUNI_Demo01
├── Environment
│   ├── Ground
│   ├── VillageBlockout
│   └── TestObstacles
├── PlayerRoot                 (movement/controller root; no visual model yet)
│   ├── NormalVisualSlot       (empty; custom model will be attached later)
│   ├── DivineVisualSlot       (empty; custom divine model/visual will be attached later)
│   └── WeaponSocket           (empty; spear will be attached later)
├── Enemy
├── NPC
├── CameraRig                 (use the verified starter prefab)
├── PlayerInput               (use the verified starter prefab)
└── Directional Light
```

## Safe Unity Editor procedure (when Unity is installed)
1. Open the existing project with its matching Unity version, 2021.3.8f1, if available.
2. Open `Assets/NaughtyCharacter/_Scenes/Demo.unity`.
3. Use **File → Save As** and save a separate copy at `Assets/MUNI_GAME/Scenes/MUNI_Demo01.unity`. Never overwrite the original Demo scene.
4. Inspect the copied scene's existing player/controller/camera references before removing or replacing anything. Keep the working starter controller and camera wiring intact.
5. Organize new MUNI objects under the planned hierarchy. Leave both visual slots and the weapon socket empty until the custom model and spear asset are ready.
6. Add the MUNI gameplay scripts only after checking their serialized fields and required components in the Inspector:
   - Player root: `MuniHealth`, `MuniSpearCombat`, `MuniTransformation`.
   - Enemy: `MuniHealth`, `MuniEnemyHealth`.
7. Assign references only when the corresponding objects/components exist. Do not leave required Inspector references silently unassigned.
8. Save the new scene and test in Play Mode. Verify movement, camera follow, jump, enemy damage/defeat, and transformation separately.

## Important implementation notes
- The camera rig prefab is not sufficient by itself: its follow target/controller references must be checked in the copied scene.
- The PlayerInput prefab is tied to the starter project's Input System setup. Preserve its existing event wiring unless the controller integration is deliberately changed and tested.
- `MuniTransformation` currently provides a visual GameObject toggle only. It does not yet implement divine stats, abilities, animation, VFX or SFX.
- `MuniSpearCombat` requires a correctly configured target layer/collider setup and a valid attack call from input/UI. This must be tested in Unity.
- Do not claim this scene is playable until it has been opened, compiled and tested in Unity Editor.

## Current status
Planning and repository asset inspection only. No `.unity` scene has been created, and no Unity compilation or Play Mode test has been performed. Unity Editor installation is currently blocked by the available disk space; resolve storage first before attempting the editor setup.
