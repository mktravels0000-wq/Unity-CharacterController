# MUNI GAME — Magic Binding Setup

## Confirmed gameplay rule

- Villain magic binds Muni with visible green ropes.
- Muni is completely unable to move, run, attack, transform, or use controls while bound.
- The binding lasts 10 seconds, then releases automatically.
- Ordinary villain attacks do not reduce Muni's health. The magic binding is a temporary control effect, not health damage.

## Script

Add `MuniMagicBinding.cs` to the Muni player root.

### Inspector setup

1. Set **Binding Duration** to `10`.
2. Assign the Muni root's NaughtyCharacter **Character** component to **Muni Character**.
3. Assign the root **CharacterController** to **Character Controller**.
4. Add player action components that must also stop (for example `MuniDemoActionInput`) to **Additional Controls To Disable**. Do not add this binding component itself.
5. Create or import a green-rope binding visual prefab that wraps around the character. Assign it to **Green Rope Visual Prefab**.
6. Optionally assign a child transform to **Rope Visual Anchor**. Otherwise the visual is instantiated at the player root.
7. Optionally assign the character Animator and create a bool parameter named `IsBound` in its Animator Controller. Use that parameter to play the tied-up pose.

### Connect the villain spell

On the villain's magic-attack event (UnityEvent / Animation Event), call:

`MuniMagicBinding.ApplyBinding()`

The villain AI must invoke this only when its magic spell successfully hits Muni. The script does not invent spell range, projectile visuals, or hit detection.

### Optional early release

Call `MuniMagicBinding.ReleaseBinding()` if a mission event explicitly frees Muni before the timer ends.

## Important implementation notes

- The 10-second timer uses scaled game time. It pauses if the game is paused with `Time.timeScale = 0`.
- Recasting while Muni is already bound restarts the 10-second timer.
- The green rope prefab is a required art asset for the visible rope effect; this script does not generate rope geometry.
- This is additive code under `Assets/MUNI_GAME`. Existing NaughtyCharacter starter files are not changed.
- Unity compilation and Play Mode testing are still pending. Do not treat this as tested until opened and tested in the matching Unity editor.
