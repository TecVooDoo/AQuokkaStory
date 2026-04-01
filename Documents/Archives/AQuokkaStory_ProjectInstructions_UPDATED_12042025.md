# A Quokka Story - Claude Project Instructions

**Project:** A Quokka Story  
**Developer:** TecVooDoo LLC  
**Designer:** Rune (Stephen Brandon)  
**Unity Version:** 6.2.2 URP  
**Date Created:** December 4, 2025  
**Last Updated:** December 4, 2025  

---

## Core Development Philosophy

### Step-by-Step Verification Protocol

**CRITICAL: Never rush ahead with multiple steps**

- Provide ONE step at a time
- Wait for user confirmation via text OR screenshot before proceeding
- User will verify each step is complete before moving forward
- If a step fails, troubleshoot that specific step before continuing
- Assume nothing - verify everything

### File Encoding Rule

**CRITICAL: ASCII Only**

- All scripts and text files MUST use ASCII encoding
- Do NOT use UTF-8 or other encodings
- Avoid special characters, smart quotes, em-dashes, etc.
- Use standard apostrophes (') not curly quotes
- Use regular hyphens (-) not em-dashes

### File Naming Convention

**When creating updated documentation or files:**

Format: `OriginalFileName_UPDATED_MMDDYYYY.ext`

**Examples:**
- `GDD_Master_UPDATED_12042025.md`
- `DesignDecisions_UPDATED_12042025.md`

### Code Style Preferences

**CRITICAL: Explicit over implicit**

- **NO `var` keyword** - Always use explicit type declarations
- Clear, readable code always wins over clever code
- Consistent naming conventions (PascalCase for public, camelCase for private)
- Avoid God classes/managers - keep classes focused

### Code Editing Preference

**For manual copy/paste (when providing code to user):**
- Provide COMPLETE file replacements
- Easier than hunting for specific lines
- Reduces errors from partial edits

**For MCP direct edits (Claude editing via tools):**
- Use `script_apply_edits` for method-level changes
- Use `apply_text_edits` for precise line/column edits
- Use `validate_script` to check for errors
- Direct MCP edits are preferred when connection is stable

---

## Unity MCP Tools

### Overview

Unity MCP allows Claude to directly interact with the Unity Editor. The project uses:
- **MCP Package:** Unity MCP for Claude
- **Transport:** Stdio (required for Claude Desktop)
- **Unity Version:** 6.2.2 URP

### Script Operations

| Tool | Purpose | When to Use |
|------|---------|-------------|
| `manage_script` (read) | Read script contents | Legacy - still works |
| `get_sha` | Get SHA256 hash + file size | Precondition checks before edits |
| `validate_script` | Check syntax/structure | Before and after edits |
| `create_script` | Create new C# scripts | New files (also overwrites existing) |
| `delete_script` | Remove scripts | By URI or Assets-relative path |

### Script Editing Tools (Preferred)

**`script_apply_edits`** - Structured C# edits with safer boundaries:
- `replace_method` - Replace entire method body
- `insert_method` - Add new method (position: start/end/after/before)
- `delete_method` - Remove a method
- `replace_class` / `delete_class` - Class-level operations
- `anchor_insert` / `anchor_delete` / `anchor_replace` - Pattern-based edits

**`apply_text_edits`** - Precise text edits:
- Line/column coordinate-based changes
- Atomic multi-edit batches
- Precondition SHA256 hashes for safety

### Asset and Scene Operations

| Tool | Purpose |
|------|---------|
| `manage_asset` | Import, create, modify, delete, search assets |
| `manage_scene` | Load, save, create scenes, get hierarchy |
| `manage_gameobject` | Create, modify, find GameObjects and components |
| `manage_prefabs` | Open/close prefab stage, create from GameObject |
| `manage_editor` | Play/pause/stop, tags, layers, editor state |

### Console and Testing

| Tool | Purpose |
|------|---------|
| `read_console` | Get Unity console messages (errors, warnings, logs) |
| `run_tests` | Execute EditMode or PlayMode tests |

### Script Path Format

Always use Assets-relative paths:
```
Assets/Scripts/Joey/JoeyDefinition.cs
Assets/Scripts/Player/PlayerController.cs
Assets/Scripts/Audio/MusicManager.cs
```

### MCP Best Practices

1. **Read before editing:** Use `manage_script` read or view the file before making changes
2. **Validate after edits:** Run `validate_script` to catch syntax errors immediately
3. **Use structured edits:** Prefer `script_apply_edits` over raw text edits for methods
4. **Check console:** Use `read_console` after changes to catch runtime issues
5. **SHA256 preconditions:** For critical edits, use `get_sha` to ensure file hasn't changed

### Troubleshooting MCP Connection

If MCP tools fail:
1. Check Unity Editor is open with the correct project
2. Verify MCP window shows "Session Active" (Window > MCP for Unity)
3. Check terminal window running the server is still open
4. Restart: Stop server, restart Unity, start server again

---

## Project Architecture

### Folder Structure

```
Assets/
├── Art/
│   ├── Sprites/
│   ├── Animations/
│   └── Materials/
├── Audio/
│   ├── Music/
│   │   └── Stems/           # Nine Volt Audio stems by biome
│   └── SFX/
├── Prefabs/
│   ├── Joey/
│   ├── Enemies/
│   └── Environment/
├── Scenes/
│   ├── Tutorial/
│   ├── Swamp/
│   ├── Suburb/
│   ├── City/
│   └── Airstrip/
├── Scripts/
│   ├── Core/                # Managers, events, utilities
│   ├── Joey/                # Joey system scripts
│   ├── Player/              # Mom/player controller
│   ├── Enemy/               # Enemy AI and behaviors
│   ├── Audio/               # Music manager, stem handling
│   ├── UI/                  # HUD, menus, pouch UI
│   └── Progression/         # Save/load, collectibles, gates
├── ScriptableObjects/
│   ├── Joey/                # JoeyDefinition, AbilityDefinition
│   ├── Enemy/               # EnemyDefinition
│   ├── Audio/               # BiomeMusicData
│   ├── Events/              # GameEventSO channels
│   └── Variables/           # Runtime variable SOs
└── Settings/
    └── Input/               # Input System actions
```

### Core Systems Architecture

**SOAP (ScriptableObject Architecture Pattern):**
- All game data as ScriptableObjects
- Event channels for decoupled communication
- Runtime sets for object tracking
- Variables as ScriptableObjects

**Key ScriptableObjects:**
- `JoeyDefinition` - Stats, abilities, audio reference
- `AbilityDefinition` - Ability parameters, costs, cooldowns
- `EnemyDefinition` - AI settings, weaknesses, audio
- `BiomeMusicData` - Stem assignments, BPM, key
- `GameEventSO` - Event channels for system communication

---

## Asset Integration

### Recommended Assets for A Quokka Story

Based on the December 2025 asset inventory review:

#### Core Framework (Choose One)
| Asset | Purpose | Priority |
|-------|---------|----------|
| **Corgi Engine** | Metroidvania framework OR reference | HIGH |
| **Flexible 2D Character Controller** | Alternative: lightweight custom | OWNED |

> **Decision:** Study Corgi Engine patterns even if building custom. It provides Metroidvania-native systems (ability gating, save points, room transitions).

#### Animation & Physics
| Asset | Purpose | Priority |
|-------|---------|----------|
| **Animancer Pro v8** | State machine replacement for animation | HIGH |
| **Dynamic Bone** | Quokka ear/tail physics, joey bounce | HIGH |
| **Ragdoll Animator 2** | Joey impact reactions | MEDIUM |
| **Toolkit for Verlet Motion** | Joey tether/rope effects | LOW |

#### Systems
| Asset | Purpose | Priority |
|-------|---------|----------|
| **SOAP** | ScriptableObject architecture | HIGH |
| **Odin Inspector** | Better inspector UX | HIGH |
| **Easy Save** | Save system | HIGH |
| **Dialogue System for Unity** | NPC interactions, tutorials | MEDIUM |
| **GASify (Free)** | Gameplay Ability System structure | MEDIUM |

#### Enemy AI
| Asset | Purpose | Priority |
|-------|---------|----------|
| **SensorToolkit 2** | Enemy sight cones, hearing | HIGH |
| **Blaze AI Engine** | Visual behavior editor | MEDIUM |

#### Metroidvania Features
| Asset | Purpose | Priority |
|-------|---------|----------|
| **AA Map and Minimap System** | Fog of war, room discovery | HIGH |

#### Visuals & Audio
| Asset | Purpose | Priority |
|-------|---------|----------|
| **All In 1 Sprite Shader** | Visual effects | HIGH |
| **Smart Lighting 2D** | Atmospheric lighting | MEDIUM |
| **Feel** | Game juice, feedback | HIGH |
| **Master Audio 2024** | Adaptive music stems | HIGH |
| **Stylized Nature MEGA PACK** | 2.5D backgrounds | MEDIUM |

#### Utilities
| Asset | Purpose | Priority |
|-------|---------|----------|
| **DOTween Pro** | Tweening, animations | HIGH |
| **Graphy** | Performance monitoring | MEDIUM |
| **UniTask** | Async/await (GitHub package) | HIGH |

### GitHub Packages (Not in Asset Store)

Install via Package Manager -> Add from Git URL:

```
UniTask: https://github.com/Cysharp/UniTask.git
Unity-Event-Bus: https://github.com/git-amend/Unity-Event-Bus.git
Unity-Improved-Timers: https://github.com/git-amend/Unity-Improved-Timers.git
```

---

## Development Guidelines

### SOLID Principles (Mandatory)

- **S** - Single Responsibility: Each class has one reason to change
- **O** - Open/Closed: Use inheritance and interfaces, extend via ScriptableObjects
- **L** - Liskov Substitution: All Joeys implement IJoeyBehavior
- **I** - Interface Segregation: IJoeyAbility, IMusicEntity, etc.
- **D** - Dependency Inversion: Depend on abstractions, not concrete classes

### Performance-First Development

**In Gameplay Loops (Update, FixedUpdate):**
- NO allocations (`new`, string concatenation, LINQ)
- Cache all component references in Awake/Start
- Use object pooling for frequently spawned objects (joeys, projectiles, VFX)

**Profiling:**
- Use Graphy for FPS monitoring during development
- Profile early on target hardware (i5-8300H, 12GB RAM, 4GB GPU)
- Smart Lighting 2D can be expensive - test early

### Input System

Using Unity's new Input System with InputAction syntax for Unity 6.2:

```csharp
// Correct Unity 6.2 InputAction syntax
private InputAction moveAction;
private InputAction jumpAction;
private InputAction launchAction;

private void Awake()
{
    moveAction = InputSystem.actions.FindAction("Move");
    jumpAction = InputSystem.actions.FindAction("Jump");
    launchAction = InputSystem.actions.FindAction("Launch");
}

private void OnEnable()
{
    moveAction.Enable();
    jumpAction.Enable();
    launchAction.Enable();
}
```

### Ground Detection

**Use collision-based detection, NOT raycasts:**
- Tag ground objects with "Ground" tag
- Use OnCollisionEnter/Exit for reliable detection
- Raycast-based detection was found to be unreliable on platform objects

### Cinemachine 3.0

Unity 6.2 uses Cinemachine 3.0 with updated API:
- `CinemachineCamera` instead of `CinemachineVirtualCamera`
- Component-based configuration
- New follow and aim modes

---

## Current Development Status

### Phase 1: Foundation (Current)

**Completed:**
- [X] Project setup (Unity 6.2.2 URP, GitHub version control)
- [X] Core player movement (walking, jumping, crouching)
- [X] Climbing system with stamina
- [X] Input System integrated
- [X] Joey prefab created (base visuals)
- [X] Audio decision made (stem-based adaptive music)
- [X] Nine Volt Audio bundle acquired (30,000 loops)
- [X] Ground detection converted to collision-based

**In Progress:**
- [ ] JoeyDefinition and AbilityDefinition ScriptableObjects
- [ ] Joey runtime controllers
- [ ] MVP launch mechanic
- [ ] Material and color systems for joeys
- [ ] Audio event wiring

**Not Started:**
- [ ] Full Joey ability implementations
- [ ] Enemy AI
- [ ] Music Manager system
- [ ] Level art and assets
- [ ] Combat system
- [ ] Save/load system
- [ ] Map/minimap system

### Technical Setup

| Component | Version/Status |
|-----------|----------------|
| Unity | 6.2.2 URP |
| Cinemachine | 3.0 |
| Input System | New Input System |
| Version Control | GitHub |
| Architecture | SOAP + Event Channels |

---

## Key Technical Decisions

### Joey System Architecture

```
JoeyDefinition (ScriptableObject)
├── Stats (energy, cooldown, health)
├── AbilityDefinition reference
├── AudioClip reference (instrument stem)
├── Material/Color settings
└── Visual prefab reference

JoeyController (MonoBehaviour)
├── Launch mechanics
├── Energy tracking
├── State machine (InPouch, Aiming, Launched, Depleted)
└── Events (OnLaunched, OnRecalled, OnEnergyChanged)

PouchManager (Singleton/Service)
├── Active Joey tracking
├── Quick-swap logic (Q/E to cycle, number keys for direct)
└── UI updates via events
```

### Music System Architecture

```
BiomeMusicData (ScriptableObject)
├── BPM and loop settings (bars per loop)
├── Key signature
├── Ambient/bass stems (always playing)
├── Energy-based drum stems
├── Joey instrument stems array
└── Enemy instrument stems array

MusicManager (Service)
├── Tracks all active music entities
├── Manages Audio Mixer volume parameters
├── Handles fade-in/fade-out (1-2 second transitions)
└── Responds to game events
```

### Event Channels

Key events for decoupled communication:
- `OnJoeyRescued` - Unlock abilities, add music layer
- `OnJoeyLaunched` - Music accent, UI update
- `OnJoeyRecalled` - Restore volume
- `OnEnemySpawned` - Add music layer
- `OnEnemyDefeated` - Remove music layer
- `OnEnergyChanged` - Adjust drum layers, UI update
- `OnNormalJoeyDeath` - Lock true ending flag

---

## Hardware Constraints

**Development Machine (NitroDad Laptop):**
- Intel i5-8300H
- 12 GB RAM
- 4 GB GPU
- 238 GB SSD (176 GB used)
- Windows 11 Home 24H2

**Implications:**
- Profile Smart Lighting 2D early - can be expensive
- Object pooling essential for joeys and projectiles
- Avoid real-time procedural audio - use stem-based approach
- Keep max simultaneous audio sources to ~12
- Test on target hardware regularly

---

## Key Reminders

**Always:**
- [X] Wait for user verification before proceeding
- [X] Provide COMPLETE file replacements when giving code manually
- [X] Use MCP tools for direct edits when connection is stable
- [X] Use ASCII encoding only
- [X] Use explicit types (no `var` keyword)
- [X] Look up current Unity 6.2 documentation
- [X] Use Odin Inspector for data structures
- [X] Use DOTween Pro for animations
- [X] Follow SOLID principles
- [X] Use SOAP architecture with event channels
- [X] Use collision-based ground detection (not raycasts)
- [X] Validate scripts after MCP edits
- [X] Profile early on target hardware
- [X] Study Corgi Engine for Metroidvania patterns

**Never:**
- [ ] Rush ahead with multiple steps
- [ ] Use `var` keyword
- [ ] Use incremental edits when providing code for manual copy/paste
- [ ] Use UTF-8 or special characters
- [ ] Assume old Unity knowledge is current
- [ ] Use deprecated APIs
- [ ] Create allocations in Update loops
- [ ] Use raycast-based ground detection
- [ ] Implement real-time procedural audio synthesis

---

## Scope Control

**The GDD is the authoritative feature set.**

- Any feature not in the GDD requires strong justification
- Challenge scope creep - demand reasons for additions
- Focus on shipping a complete, polished experience
- Innovation should focus on core mechanics (joey combat, adaptive music)
- Secondary systems can use simpler approaches

---

## Questions Protocol

1. **Clarify the context** - What are you trying to accomplish?
2. **Look up current information** - Search Unity 6.2 docs
3. **Check asset inventory** - Do we own something that solves this?
4. **Provide step-by-step solution** - One step at a time
5. **Explain the reasoning** - Why this approach?

---

**End of Project Instructions**

These instructions should be followed for every conversation in this project.
