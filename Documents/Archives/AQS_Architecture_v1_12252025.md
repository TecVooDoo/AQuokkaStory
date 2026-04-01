# A Quokka Story - Architecture Document

**Version:** 1
**Date Created:** December 25, 2025
**Last Updated:** December 25, 2025
**Developer:** TecVooDoo LLC
**Unity Version:** 6.2.2 URP
**Total Scripts:** TBD (Pre-development)

---

## Installed Packages

### Asset Store Packages

| Package | Version | Purpose |
|---------|---------|---------|
| Animancer Pro | v8 | State machine replacement for animation |
| Corgi Engine | (owned) | Metroidvania framework/reference |
| DOTween Pro | 1.0.386 | Animation, tweening, juice effects |
| Dynamic Bone | (owned) | Quokka ear/tail physics, joey bounce |
| Easy Save | (owned) | Save/load system |
| Feel | 5.9.1 | MMFeedbacks for impacts, screen shake, game feel |
| Master Audio 2024 | (owned) | Adaptive stem-based music |
| Odin Inspector and Serializer | 4.0.1.2 | Enhanced inspector, serialization |
| Odin Validator | 4.0.1.2 | Asset validation, error checking |
| Ragdoll Animator 2 | (owned) | Joey impact reactions |
| SensorToolkit 2 | (owned) | Enemy AI sight/hearing detection |
| SOAP | (owned) | ScriptableObject Architecture Pattern |
| All In 1 Sprite Shader | (owned) | Visual effects |
| AA Map and Minimap System | (owned) | Metroidvania exploration tracking |
| Smart Lighting 2D | (owned) | Atmospheric lighting |
| EasyPooling v2025 | (owned) | Object pooling |

### Third-Party Packages (GitHub)

| Package | Version | Purpose |
|---------|---------|---------|
| UniTask | 2.5.10 | Async/await, cancellation tokens |
| Unity-Event-Bus | (git) | Event system |
| Unity-Improved-Timers | (git) | Timer utilities |

### Key Unity Packages

| Package | Version | Purpose |
|---------|---------|---------|
| Input System | 1.16.0 | New input handling |
| TextMesh Pro | (included) | UI text rendering |
| Cinemachine | 3.0 | Camera control, follow, zoom |
| 2D Animation | (included) | Sprite animation |
| Universal Render Pipeline | 17.x | 2D/2.5D URP rendering |

---

## System Overview

```
                          +-------------------+
                          |   GameManager     |
                          | (Game State Flow) |
                          +---------+---------+
                                    |
          +-------------------------+-------------------------+
          |                         |                         |
+---------v---------+    +----------v----------+    +---------v---------+
|  PlayerController |    |    PouchManager     |    |   MusicManager    |
|  (Mom Movement)   |    |  (Joey Management)  |    | (Adaptive Stems)  |
+---------+---------+    +----------+----------+    +---------+---------+
          |                         |                         |
+---------v---------+    +----------v----------+    +---------v---------+
|  JoeyController   |    |    EnemyManager     |    |   AudioManager    |
| (Launch/Ability)  |    |   (AI/Spawning)     |    |    (SFX/Music)    |
+-------------------+    +---------------------+    +-------------------+
```

---

## Namespaces

| Namespace | Purpose |
|-----------|---------|
| `AQS.Core` | Game state, managers, events, utilities |
| `AQS.Player` | Mom movement, input, stats |
| `AQS.Joey` | Joey system, abilities, pouch management |
| `AQS.Enemy` | Enemy AI, behaviors, spawning |
| `AQS.Audio` | Music manager, stem handling, SFX |
| `AQS.UI` | HUD, menus, pouch UI |
| `AQS.Progression` | Save/load, collectibles, gates, map |

---

## File Structure

```
Assets/AQS/Scripts/
|
+-- Core/
|   |-- GameManager.cs              (PLANNED - Game state machine)
|   |-- GameEventSO.cs              (PLANNED - Event channel base)
|   |-- GameSettingsSO.cs           (PLANNED - Global config)
|   +-- ServiceLocator.cs           (PLANNED - Service access)
|
+-- Player/
|   |-- PlayerController.cs         [IN PROGRESS] Mom movement, input
|   |-- PlayerStats.cs              (PLANNED - Health, upgrades)
|   |-- ClimbController.cs          [DONE] Climbing with stamina
|   +-- GroundDetector.cs           [DONE] Collision-based detection
|
+-- Joey/
|   |-- JoeyDefinition.cs           (PLANNED - SO for joey stats)
|   |-- AbilityDefinition.cs        (PLANNED - SO for ability params)
|   |-- JoeyController.cs           (PLANNED - Launch, energy, states)
|   |-- PouchManager.cs             (PLANNED - Active joey tracking)
|   |-- IJoeyAbility.cs             (PLANNED - Ability interface)
|   +-- Abilities/
|       |-- LeadJoeyAbility.cs      (PLANNED - Bowling ball)
|       |-- HeliumJoeyAbility.cs    (PLANNED - Inflate)
|       |-- GIJoeyAbility.cs        (PLANNED - Bazooka)
|       |-- NinjaJoeyAbility.cs     (PLANNED - Shuriken)
|       |-- BalletJoeyAbility.cs    (PLANNED - Freeze)
|       +-- KazooJoeyAbility.cs     (PLANNED - TBD)
|
+-- Enemy/
|   |-- EnemyDefinition.cs          (PLANNED - SO for enemy config)
|   |-- EnemyController.cs          (PLANNED - Base enemy behavior)
|   |-- EnemySpawner.cs             (PLANNED - Spawn management)
|   +-- Behaviors/
|       |-- PatrolBehavior.cs       (PLANNED)
|       |-- ChaseBehavior.cs        (PLANNED)
|       +-- AttackBehavior.cs       (PLANNED)
|
+-- Audio/
|   |-- BiomeMusicData.cs           (PLANNED - SO for stem config)
|   |-- MusicManager.cs             (PLANNED - Stem layering)
|   |-- MusicEntity.cs              (PLANNED - Interface for music contributors)
|   +-- SFXManager.cs               (PLANNED - Sound effects)
|
+-- UI/
|   |-- GameplayHUD.cs              (PLANNED - Health, energy, joey)
|   |-- PouchUI.cs                  (PLANNED - Joey selection)
|   |-- MapUI.cs                    (PLANNED - Minimap, full map)
|   +-- MenuController.cs           (PLANNED - Main menu, pause)
|
+-- Progression/
|   |-- SaveManager.cs              (PLANNED - Easy Save integration)
|   |-- CollectibleManager.cs       (PLANNED - Tokens, charms, gnomes)
|   |-- AbilityGate.cs              (PLANNED - Joey-gated areas)
|   +-- MapPieceTracker.cs          (PLANNED - Map collection)
```

---

## Key Components

### GameManager

**Purpose:** Controls overall game flow and state.

**States:**
- `MainMenu`
- `Playing`
- `Paused`
- `Cutscene`
- `GameOver`

**Responsibilities:**
- Track current level/biome
- Manage game state transitions
- Coordinate save/load triggers

### PlayerController (Mom)

**Purpose:** Handles player movement and input.

**Movement:**
- Walk/Run (2.5D platforming)
- Jump (variable height)
- Crouch (smaller hitbox)
- Climb (with stamina system)
- Pouch Dash (unlocked mid-game)

**Key Properties:**
- `health` - 5 hearts (upgradeable)
- `moveSpeed` - Affected by equipped Joey
- `climbStamina` - Limited duration

### JoeyController

**Purpose:** Individual Joey behavior after launch.

**States:**
- `InPouch` (resting, 3x energy regen)
- `Aiming` (time slow, trajectory arc)
- `Launched` (active ability)
- `Depleted` (cooling, low music volume)

**Key Properties:**
- `energy` - 100 max, costs 25-75 per ability
- `cooldown` - Based on ability
- `ownerJoeyDefinition` - Reference to SO config

**Energy System:**
- In-pouch regen: 30 energy/second
- Out-of-pouch regen: 10 energy/second
- Energy level determines music volume

### PouchManager

**Purpose:** Manages Joey inventory and swapping.

**Slots:**
- Active Slot (currently equipped)
- Reserve Slots (up to 5 Joeys)
- Stored (at save points only)

**Methods:**
- `CycleJoey(int direction)` - Q/E cycling
- `SelectJoey(int index)` - Number key direct
- `LaunchActiveJoey()` - Initiate launch
- `RecallActiveJoey()` - Manual recall

### MusicManager

**Purpose:** Coordinates adaptive stem-based music.

**Key Properties:**
- `currentBiomeData` - BiomeMusicData SO
- `activeMusicEntities` - List of contributors
- `masterMixer` - Audio Mixer reference

**Volume Rules:**
- Mom: Always 100%
- Joeys (In Pouch): Volume = Energy Level
- Joeys (Launched): 150% (accent)
- Joeys (Depleted): 10-20%
- Enemies: 60-80%

**Methods:**
- `RegisterEntity(IMusicEntity)` - Add to mix
- `UnregisterEntity(IMusicEntity)` - Remove from mix
- `SetEntityVolume(IMusicEntity, float)` - Adjust level
- `TransitionToBiome(BiomeMusicData)` - Crossfade stems

---

## Event Architecture

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
    +--- OnNormalJoeyDeath ---> GameManager (lock true ending)

EnemyController
    +--- OnEnemySpawned ---> MusicManager (add layer)
    +--- OnEnemyDefeated ---> MusicManager (remove layer)
    +--- OnEnemyDefeated ---> ScoreManager (if applicable)

GameManager
    +--- OnStateChanged ---> UIController (show/hide panels)
    +--- OnBiomeChanged ---> MusicManager (transition stems)
```

---

## Design Patterns

### 1. SOAP (ScriptableObject Architecture Pattern)
All game data as ScriptableObjects:
- `JoeyDefinition` - Joey stats, abilities, audio
- `AbilityDefinition` - Ability parameters
- `EnemyDefinition` - Enemy config
- `BiomeMusicData` - Stem assignments
- `GameEventSO` - Event channels

### 2. Event-Driven Communication
Use event channels for decoupling. No direct references between managers.

### 3. Interface Segregation
- `IJoeyAbility` - Ability execution contract
- `IMusicEntity` - Music contribution contract
- `IDamageable` - Damage handling contract
- `ISaveable` - Save/load contract

### 4. State Machine for Joeys
JoeyController uses explicit states:

```csharp
public enum JoeyState
{
    InPouch,
    Aiming,
    Launched,
    Depleted
}
```

### 5. Object Pooling
Pool frequently spawned objects:
- Joey projectiles
- Enemy projectiles
- Particle effects
- Audio sources

### 6. Service Locator
Central access to core services:
- MusicManager
- SaveManager
- InputManager

---

## Physics Setup

### Layers

| Layer | Purpose |
|-------|---------|
| Default | General objects, UI |
| Player | Mom character |
| Joey | Joey projectiles and entities |
| Enemy | All enemy types |
| Ground | Walkable surfaces |
| Climbable | Climbable surfaces |
| Hazard | Damage zones |
| Interactable | Doors, switches, collectibles |

### Collision Matrix

| | Player | Joey | Enemy | Ground | Hazard |
|---|---|---|---|---|---|
| Player | No | No | Yes | Yes | Yes |
| Joey | No | No | Yes | Yes | No |
| Enemy | Yes | Yes | No | Yes | No |
| Ground | Yes | Yes | Yes | No | No |
| Hazard | Yes | No | No | No | No |

### Scene Hierarchy

```
Level_Swamp
  - Environment
    - Background (parallax layers)
    - Midground (platforms, terrain)
    - Foreground (decorative)
  - Gameplay
    - Player (Mom + Joeys)
    - Enemies (spawned at runtime)
    - Collectibles
    - Hazards
  - Audio
    - MusicManager
    - AmbientSources
  - UI
    - GameplayHUD (Canvas)
    - PauseMenu (Canvas)
  - Cameras
    - MainCamera
    - CinemachineCamera
```

---

## Audio Architecture

### Stem Organization Per Biome

**Swamp (E minor, 105 BPM):**
| Stem | Source | Trigger |
|------|--------|---------|
| Ambient | Nature sounds | Always on |
| Bass | Mom - upright bass | Always on |
| Light drums | Energy 0-30% | Energy level |
| Full drums | Energy 70-100% | Energy level |
| Banjo | Ninja Joey OR Gator | Entity present |
| Harmonica | GI Joey | Entity present |
| Slide guitar | Snake enemies | Entity present |
| Fiddle | Ballet Joey | Entity present |
| Washboard | Helium Joey | Entity present |

**Suburb (C major, 115 BPM):**
- Retro synth pad (ambient)
- Electric bass (Mom)
- Electronic drums (energy-based)
- Synth lead (Ninja Joey)
- Chiptune accent (GI Joey)
- Bell synth (Ballet Joey)

**City (D minor, 125 BPM):**
- Industrial ambient (machinery)
- Heavy bass (Mom)
- Industrial percussion (energy-based)
- Distorted guitar (GI Joey)
- Dark synth (Ninja Joey)
- String stabs (Ballet Joey)

**Airstrip (E minor, 140 BPM):**
- Military ambient (alarms, radio)
- Orchestral bass (Mom)
- Epic drums (energy-based)
- Brass stabs (GI Joey)
- Full ensemble (all Joeys + Kazoo)

### Instrument Conflict Resolution

**Problem:** Ninja Joey and Gator both use banjo.

**Solution:**
- Before Ninja Joey rescue: Gator plays full banjo
- After Ninja Joey rescue: Gator switches to banjo accents only
- Ninja Joey maintains primary banjo melody

---

## Data Persistence

### Easy Save Integration

**Saved Data:**
- Player progress (rescued Joeys, unlocked abilities)
- Collectibles (map pieces, tokens, charms)
- Settings (audio, controls)
- Current checkpoint

**Save Points:**
- Specific locations in each level
- Auto-save on Joey rescue
- Auto-save on level transition

### PlayerPrefs (Settings Only)

- Audio volumes
- Control preferences
- Accessibility options

---

## Performance Considerations

### Target Hardware

**Minimum (Development Laptop):**
- Intel i5-8300H
- 12 GB RAM
- 4 GB GPU (GTX 1050 equivalent)

### Optimization Strategies

- **Object Pooling:** Joeys, projectiles, particles (EasyPooling v2025)
- **Audio Source Limit:** Max 12 simultaneous stems
- **Occlusion Culling:** 2.5D layers culled by camera
- **Sprite Atlasing:** Reduce draw calls
- **Async Loading:** UniTask for non-blocking operations

### Performance Concerns

- **Smart Lighting 2D:** Test early - can be expensive
- **Dynamic Bone:** Limit to visible characters
- **Particle Systems:** Cap per-system counts

---

## Testing Strategy

### Unit Tests (EditMode)
- Joey energy calculations
- Ability cost/cooldown logic
- Save/load data integrity
- Event channel firing

### Integration Tests (PlayMode)
- Full launch -> ability -> return cycle
- Music stem transitions
- Enemy AI behaviors
- Save point functionality

### Performance Tests
- 60 FPS target on minimum hardware
- Memory usage under stress
- Audio latency measurements

---

**End of Architecture Document**

*Note: This document will be updated as development progresses and actual scripts are created.*
