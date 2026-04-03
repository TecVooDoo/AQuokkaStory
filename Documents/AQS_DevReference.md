# A Quokka Story -- Dev Reference

**Purpose:** Architecture, coding standards, and AI rules for A Quokka Story. Read on demand -- primary doc is `AQS_Status.md`.
**Last Updated:** April 1, 2026 (Session 15 -- Migration to standalone)

---

## Project Overview

**Genre:** 2.5D Metroidvania Platformer
**Engine:** Unity 6 (6000.3.11f1), URP
**Project Path:** `E:\Unity\AQuokkaStory`
**AQS Root:** `Assets/_AQS/`

**Core Innovation:** Joey-launching combat mechanic + adaptive stem-based music where each character/enemy contributes an instrument layer to the soundtrack.

---

## Namespaces

| Namespace | Purpose | Status |
|-----------|---------|--------|
| `AQS.Core` | Game state, managers, events, utilities | Active |
| `AQS.Player` | Mom movement, input, stats | Active |
| `AQS.Joey` | Joey system, abilities, pouch management | Active |
| `AQS.Enemy` | Enemy AI, behaviors, spawning | Planned |
| `AQS.Audio` | Music manager, stem handling, SFX | Planned |
| `AQS.UI` | HUD, menus, pouch UI | Planned |
| `AQS.Progression` | Save/load, collectibles, gates, map | Planned |

---

## Folder Structure

```
Assets/_AQS/
|
+-- Scripts/
|   +-- Core/
|   +-- Player/
|   +-- Joey/
|   |   +-- Abilities/
|   +-- Enemy/
|   |   +-- Behaviors/
|   +-- Audio/
|   +-- UI/
|   +-- Progression/
|   +-- Test/
|
+-- Art/
|   +-- Characters/
|   +-- Environments/
|   +-- VFX/
|   +-- UI/
|
+-- Audio/
|   +-- Stems/
|   +-- SFX/
|
+-- Data/
|   +-- Joeys/          (JoeyDefinition SOs)
|   +-- Abilities/      (AbilityDefinition SOs)
|   +-- Enemies/        (EnemyDefinition SOs)
|   +-- Biomes/         (BiomeMusicData SOs)
|   +-- Events/         (GameEvent SOs)
|   +-- States/         (Malbers AC state SOs)
|
+-- Prefabs/
+-- Scenes/
+-- Animations/
```

---

## Dependencies (Planned)

### Confirmed (owned, evaluated, current standards)

| Package | Version | Purpose | Notes |
|---------|---------|---------|-------|
| Malbers Horse Animset Pro | 4.5.1 | Player movement (Animal Controller) | Primary movement system |
| Poly Art: Raccoon | 4.0 | Placeholder character + Raccoon Cub for Joey | Malbers AC prefabs included |
| Animancer Pro | v8 | Animation state management | For non-AC characters. Install BEFORE art assets |
| Master Audio 2024 | 1.0.3 | Adaptive stem-based music | Install Addressables FIRST |
| DOTween Pro | 1.0.410 | Tweening, juice effects | Install TextMesh Pro FIRST |
| Feel | 5.9.1 | Game feel (screen shake, feedback) | Install BEFORE 3D art assets |
| Easy Save 3 | 3.5.25 | Save/load system | |
| Odin Inspector | 4.0.1.4 | Enhanced inspector | Never remove once installed |
| Odin Validator | 4.0.1.4 | Asset validation | |
| BoingBones (Boing Kit) | 1.2.47 | Quokka ear/tail physics, joey bounce | Replaces Dynamic Bone |
| EasyPooling 2025 | (owned) | Object pooling | |
| UniTask | 2.5.10 | Async/await | OpenUPM |
| TecVooDoo Utilities | 1.0.0 | Timers, singletons, extensions | UPM local package |
| TecVooDoo MCP Tools | 1.0.0 | Custom MCP tools | UPM local package |
| TecVooDoo Games | 1.2.0 | Shared gameplay library | UPM local package |
| RayFire 2 | (owned) | Destruction (barriers, obstacles) | MCP tools built (ENTRY-267) |
| ALINE | 1.7.9 | Debug visualization | Trajectory arcs, AI detection cones |
| Damage Numbers Pro | 4.51 | Hit feedback | |

### AssetLog Picks (evaluated, approved, owned -- install by sprint)

Source: Sandbox AssetLog audit (315 entries searched). See AQS_Status.md for full list.

| Package | Entry | AQS Need | Sprint |
|---------|-------|----------|--------|
| Toolkit for Ballistics 2026 (Heathen) | 208 | Joey launch trajectory + projectile physics | 1 |
| Toon Kit 2 (OccaSoftware) | 195 | Cel shading art direction | 1 |
| Outlines Post-Process (OccaSoftware) | 191 | Full-scene toon outlines | 1 |
| Outline Objects (OccaSoftware) | 200 | Per-mesh interactable outlines | 1 |
| All In 1 Shader Nodes | 269 | Glow, hit flash, hologram for ASE/SG | 1 |
| Rope Toolkit | 271 | Vine swinging, rope puzzles | 2 |
| MegaFiers 2 (Chris West) | 136 | Helium Joey inflate (FFD, morph) | 3 |
| Deform (Beans) | 153 | Squash/stretch platforming feel | 3 |
| SensorToolkit2 (Micosmo) | 231 | Enemy detection (vision, range, LOS) | 4 |
| Behavior Designer Pro (Opsive) | 229 | Enemy behavior trees | 4 |
| A* Pathfinding Pro (Aron Granberg) | 164 | Enemy pathfinding (GridGraph 2D) | 4 |
| Low Poly Animated Animals (Polyperfect) | 036 | Placeholder enemy models | 4 |
| Ragdoll Animator 2 (FImpossible) | 216 | Joey impact, enemy death | 4 |

### Dropped (replaced by current standards)

| Old Package | Replacement | Reason |
|-------------|-------------|--------|
| SOAP (UrbanRobots) | Vanilla SO + GameEvent/GameEventListener | SOAP dropped across all TecVooDoo projects |
| Corgi Engine | Custom movement system | Quokka hop-based movement too unique for framework |
| Unity-Event-Bus (GitHub) | GameEvent/GameEventListener pattern | Custom event system standard |
| Unity-Improved-Timers (GitHub) | TecVooDoo Utilities Timer system | Consolidated into shared package |
| Dynamic Bone | BoingBones (Boing Kit) | Transform-based spring chains, better evaluated |
| Service Locator pattern | PersistentSingleton (TecVooDoo Utilities) | Simpler, proven pattern |

---

## Architecture

### System Overview

```
                      +-------------------+
                      |   GameManager     |
                      | (Game State Flow) |
                      +---------+---------+
                                |
      +-------------------------+-------------------------+
      |                         |                         |
+-----v-----------+    +-------v---------+    +-----------v-----+
| PlayerController|    |  PouchManager   |    |  MusicManager   |
| (Mom Movement)  |    | (Joey Manage)   |    | (Adaptive Stems)|
+-----+-----------+    +-------+---------+    +-----------+-----+
      |                        |                          |
+-----v-----------+    +-------v---------+    +-----------v-----+
| JoeyController  |    |  EnemyManager   |    |  AudioManager   |
| (Launch/Ability)|    |  (AI/Spawning)  |    |  (SFX/Music)    |
+-----------------+    +-----------------+    +-----------------+
```

### Event Architecture

Communication via GameEvent ScriptableObjects (NOT SOAP, NOT direct references):

```
PlayerController
    +--- OnPlayerDamaged ---> GameplayHUD (update hearts)
    +--- OnPlayerDied ---> GameManager (game over)

JoeyController
    +--- OnJoeyLaunched ---> MusicManager (accent volume)
    +--- OnJoeyLaunched ---> PouchUI (update display)
    +--- OnJoeyRecalled ---> MusicManager (restore volume)
    +--- OnEnergyChanged ---> MusicManager (adjust volume)
    +--- OnEnergyChanged ---> GameplayHUD (update bar)

PouchManager
    +--- OnJoeyRescued ---> MusicManager (add stem layer)
    +--- OnJoeyRescued ---> GameManager (unlock abilities)
    +--- OnActiveJoeyChanged ---> GameplayHUD (update portrait)

EnemyController
    +--- OnEnemySpawned ---> MusicManager (add layer)
    +--- OnEnemyDefeated ---> MusicManager (remove layer)

GameManager
    +--- OnStateChanged ---> UIController (show/hide panels)
    +--- OnBiomeChanged ---> MusicManager (transition stems)
```

### Design Patterns

1. **Vanilla ScriptableObject Architecture** -- all game data as SOs (JoeyDefinition, AbilityDefinition, EnemyDefinition, BiomeMusicData). GameEvent/GameEventListener for event channels.
2. **Interface Segregation** -- `IJoeyAbility`, `IMusicEntity`, `IDamageable`, `ISaveable`
3. **State Machine for Joeys** -- InPouch, Aiming, Launched, Depleted
4. **Object Pooling** -- EasyPooling 2025 for projectiles, particles, audio sources
5. **PersistentSingleton** -- TecVooDoo Utilities pattern for managers (GameManager, MusicManager, SaveManager)

### Physics Setup

| Layer | Purpose |
|-------|---------|
| Player | Mom character |
| Joey | Joey projectiles and entities |
| Enemy | All enemy types |
| Ground | Walkable surfaces |
| Climbable | Climbable surfaces |
| Hazard | Damage zones |
| Interactable | Doors, switches, collectibles |

---

## Coding Standards

Same as all TecVooDoo projects:

- **No `var`** -- explicit types always
- **No per-frame allocations/LINQ** -- cache, pool, reuse
- **ASCII only** in docs and identifiers
- **Vanilla SO architecture** -- GameEvent/GameEventListener for events (NOT SOAP)
- **Malbers Animal Controller** for player movement (MAnimal + LockAxis for 2.5D). Animancer Pro may still be used for non-AC characters/VFX.
- **Keep scripts focused** -- extract when a class has more than one clear responsibility. No hard line limit. A 3000-line class that does one thing well is fine.
- **Prefer interfaces and generics** -- decouple systems, reduce duplication
- **Collision-based ground detection** -- NOT raycasts (design decision from original project, works better with 2.5D slopes and moving platforms)
- **3D physics for 2.5D** -- use `Rigidbody` + `CapsuleCollider` + `Collision` (3D types), NOT `Rigidbody2D`. **Z is lateral movement, X is depth (locked), Y is up.** Camera looks down -X. Freeze X position + rotation on Joeys. Mom uses LockAxis.
- **Unity 6 Rigidbody API** -- use `rb.linearVelocity` (not `velocity`). No `gravityScale` on 3D Rigidbody -- use `AddForce` with `Physics.gravity` multiplier for fall acceleration.
- **sealed on MonoBehaviours** -- unless inheritance is specifically intended
- **Prefer async/await (UniTask)** -- over coroutines

---

## Audio Architecture

### Stem-Based Music System

Music is the heart of AQS identity. The soundtrack is layered audio stems that dynamically fade in/out based on active Joeys, spawned enemies, and combat energy.

**Source:** Nine Volt Audio professional loop libraries (30,000 loops by key/tempo). Managed by Master Audio 2024.

**Volume Rules:**
- Mom: Always 100%
- Joeys (In Pouch): Volume = Energy Level (0-100%)
- Joeys (Launched): 150% (accent)
- Joeys (Depleted): 10-20%
- Enemies: 60-80%

**Biome Keys:**
| Biome | Key | BPM |
|-------|-----|-----|
| Swamp | E minor | 105 |
| Suburb | C major | 115 |
| City | D minor | 125 |
| Airstrip | E minor | 140 |

**Max simultaneous stems:** 12 (performance constraint)

---

## Malbers AC Integration

Full step-by-step recipe in `AQS_MalbersRecipe.md`. Key rules summarized here:

- **states[Count-1] = startup state** -- Idle must always be last in the MAnimal states list. Malbers activates the last element at startup via `CleanStateStart()`, bypassing `TryActivate()`.
- **State ordering:** highest priority first, Idle (Priority=1) last.
- **JumpBasic GravityPower must be > 0** -- GravityPower=0 means no downward force during jump.
- **AnyState->Idles canTransitionToSelf=True** -- required for Fall->Idle re-entry.
- **Sub-State Machines only** -- do not use flat states at the animator root level.
- **Build from prefab** -- start from a working Malbers demo prefab and swap clips/model. Never build MAnimal from scratch on a blank GameObject.

---

## AI Rules

1. **Primary doc:** `AQS_Status.md` -- read first, always.
2. **Working directory:** `E:\Unity\AQuokkaStory`
3. **AQS root:** `Assets/_AQS/`
4. **GDD is user's doc** -- update only when asked.
5. **Collision-based ground detection** -- never suggest raycasts for ground checks.
6. **Stem music is core identity** -- every character/enemy must have an instrument assignment.
7. **Joey abilities always have drawbacks** -- no dominant strategies.
8. **All TecVooDoo coding standards apply** -- see Coding Standards section above.
9. **MCP tools available** -- use for scene setup, component configuration, testing.
10. **Asset evaluations live in Sandbox** -- reference `E:\Unity\Sandbox\Documents\Sandbox_AssetLog.md` for eval decisions.

---

**End of Document**
