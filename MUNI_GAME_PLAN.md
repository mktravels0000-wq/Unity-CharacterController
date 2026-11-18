# MUNI GAME – Muniyan: The Divine Guardian

## 1. Project Overview

- **Working title:** MUNI GAME – Muniyan: The Divine Guardian
- **Genre:** Third-person action-adventure with Tamil village-folklore and supernatural elements
- **Target platforms:** Android and Windows PC
- **Languages:** Tamil and English
- **Camera:** GTA V-style third-person camera
- **Visual direction:** Stylized, low-poly realism; readable environments and characters rather than expensive high-detail assets
- **Target build size:** Approximately 300 MB is an aspiration, not a guaranteed limit. Actual size will depend on models, textures, audio, voice acting, and platform-specific builds.

This document records the agreed concept and separates the full-game plan from the first playable prototype. Features listed for the full game are not assumed to be part of Demo 01.

## 2. Story Premise

The hero is a poor young man with a rowdy lifestyle, but he has a good heart and is intelligent. Three fictional wizards arrive intending to steal the treasure kept at the village temple.

The hero stands against them. When the danger becomes too great, the grace of Muniyan enters him and he transforms into Muniyan, the divine guardian. The transformation is the central supernatural turning point of the story.

## 3. World and Map

The full-game world is planned as one compact, connected map containing:

- Village
- Temple
- Palm grove
- Forest
- Rocky hill

The player can travel on foot or ride a horse. The map should feel connected while remaining small enough to support the project's performance and build-size goals.

## 4. Wizard Disguise and Discovery Mechanic

The three wizards can conceal themselves by taking forms associated with the environment, including a tree, rock/stone, or wind.

- The player investigates clues in the map to identify suspicious locations.
- At a suspected location, the hero drives his spear into the ground.
- The spear triggers an energy effect that reveals the disguised wizard.

This clue-finding and spear-based reveal mechanic is a core planned gameplay feature. Its complete implementation is outside the minimum Demo 01 scope unless added later.

## 5. Combat and Weapons

Planned weapons:

- Spear
- Trident
- Sickle
- Whip

The divine form is intended to have combo attacks. Combat should support the story's progression from the young hero's ordinary abilities to Muniyan's divine power.

## 6. Boss Encounters

The full game is planned to include three major boss fights, with one boss encounter for each wizard.

## 7. Health and Optional Pickups

- Goat meat and chicken meat can restore health.
- Optional world pickups discussed include sarakku, cigarette, and palm toddy.

These are planned game-world items; their exact effects, availability, presentation, and ratings/platform requirements remain to be designed. No specific gameplay benefit is assumed here beyond the stated health-restoration role for goat and chicken meat.

## 8. Mission Structure

The intended full-game length is approximately 6–8 missions. Mission order, objectives, dialogue, and final progression will be specified in a later mission-design document.

## 9. First Playable Demo 01 — Scope

Demo 01 is a small technical prototype, not the complete game.

### Included in Demo 01

1. One young hero character.
2. A small village test area.
3. GTA V-style third-person camera prototype.
4. Walk, run, and jump.
5. One NPC.
6. One basic spear.
7. One basic enemy.
8. An initial Muniyan transformation prototype.
9. Basic control support planned for both target platforms:
   - **Windows PC:** keyboard and mouse.
   - **Android:** touch controls.

### Not required for Demo 01

The full compact map, horse riding, all four weapons, complete combo system, all three wizard disguises, clue investigation, all three boss fights, the full 6–8 mission campaign, and final audio/visual polish are later milestones. They should not be treated as already implemented.

## 10. Platform and Control Approach

The project targets Android and Windows PC. Gameplay systems should be shared where practical, with platform-specific input:

- **PC:** keyboard and mouse.
- **Android:** on-screen touch controls.

The camera and movement should be designed for third-person play on both platforms. Performance and control feel must be tested separately on each target.

## 11. Technical Starting Point

The GitHub repository is based on the Unity Third Person Character Controller starter project.

- **Repository:** `mktravels0000-wq/Unity-CharacterController`
- **Current project editor version recorded in ProjectSettings:** Unity 2021.3.8f1
- **Existing starter controls documented in its README:** WASD movement, mouse camera, and Space to jump.
- **Existing starter project files and its MIT license are to be preserved.**

New MUNI GAME work should be added in clearly separated project folders where possible. Existing starter scripts should be inspected before modification; avoid overwriting them without a specific need. Any reuse or distribution of starter-project code must retain the applicable MIT copyright and license notice.

## 12. Performance and Build-Size Strategy

The approximate 300 MB target is a design goal only, not a promise. To work toward a compact build:

- Keep the world compact.
- Prefer optimized low-poly models.
- Use appropriately sized and reused textures.
- Compress textures and audio where suitable.
- Keep voice/dialogue audio limited and compressed.
- Avoid unnecessary duplicate assets.
- Profile Android and Windows builds independently.

Actual build size and performance can only be confirmed after building and testing.

## 13. Development Constraints and Staged Workflow

Current development constraints recorded during planning:

- Development laptop: Windows 11 Home, AMD Ryzen 3 7320U with Radeon Graphics, 8 GB RAM.
- Unity is not currently installed.
- Available free disk space is currently less than 10 GB in total (C: has less than 10 GB free; D: has approximately 6 GB free, as last reported).
- No external hard drive or USB pen drive is available.
- Daily internet data is limited; the preferred download budget is approximately 1–2 GB.

Because Unity Editor and platform modules require substantial storage and downloads, do not begin a full Unity installation until storage and download requirements have been checked and sufficient space is available. Do not assume the current free space is enough.

### Suggested staged workflow

1. Maintain the game design, task list, and small source files in GitHub.
2. Inspect the starter controller and its package/input setup before adding code.
3. Prepare Demo 01 scripts and scene instructions in small, reviewable changes.
4. Check available disk space and Unity installation requirements before downloading the editor or Android modules.
5. Once Unity can be installed, open the project in the recorded compatible editor version and test the prototype.
6. Add Android and Windows input/build support, then profile each platform.
7. Expand from Demo 01 toward the full map, mechanics, bosses, and missions.

## 14. Open Design Decisions

The following details have not yet been finalized and should be confirmed before implementation:

- Hero name, appearance, clothing, and detailed backstory.
- The three wizards' names, appearances, powers, and individual boss mechanics.
- Exact mission titles, objectives, and sequence.
- Horse handling and movement details.
- Weapon damage, attack timing, and combo definitions.
- Exact effects and rules for optional pickups.
- Final art, animation, music, voice, and sound design.
- Minimum supported Android devices and PC performance targets.
- Final build-size target after actual platform builds.

---

**Status:** Planning baseline. This file records the agreed direction; it does not claim that the game or Demo 01 has been implemented or tested.
