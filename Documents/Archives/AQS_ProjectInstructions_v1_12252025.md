# A Quokka Story - Claude Project Instructions

**Project:** A Quokka Story
**Developer:** TecVooDoo LLC
**Designer:** Rune (Stephen Brandon)
**Unity Version:** 6.2.2 URP
**Project Path:** E:\Unity\AQuokkaStory
**Document Version:** 1
**Last Updated:** December 25, 2025

---

## IMPORTANT: Project Path

**The Unity project will be located at: `E:\Unity\AQuokkaStory`**

Do NOT use worktree paths - always use the E: drive path above for all file operations.

---

## Critical Development Protocols

### NEVER Assume Names Exist

**CRITICAL: Verify before referencing**

- NEVER assume file names, method names, or class names exist
- ALWAYS read/search for the actual names in the codebase first
- If a script might be named "JoeyController" or "JoeyManager", search to find the actual name
- Incorrect assumptions waste context and force complete rewrites

### Step-by-Step Verification Protocol

**CRITICAL: Never rush ahead with multiple steps**

- Provide ONE step at a time
- Wait for user confirmation via text OR screenshot before proceeding
- User will verify each step is complete before moving forward
- If a step fails, troubleshoot that specific step before continuing
- Assume nothing - verify everything

**Why this matters:** Jumping ahead when errors occur forces entire scripts to be redone, wasting context and causing chats to run out of room.

### Documentation Research Protocol

**CRITICAL: Use current documentation**

- ALWAYS fetch the most up-to-date documentation for Unity and packages before making recommendations
- User prefers waiting for accurate information over redoing work later
- Do not rely on potentially outdated knowledge - verify current APIs and patterns
- This applies to Unity 6.2.2, Cinemachine 3.0, all installed packages, and external libraries

---

## File Conventions

### Encoding Rule

**CRITICAL: ASCII Only**

- All scripts and text files MUST use ASCII encoding
- Do NOT use UTF-8 or other encodings
- Avoid special characters, smart quotes, em-dashes
- Use standard apostrophes (') not curly quotes
- Use regular hyphens (-) not em-dashes

### Core Document Naming Convention

**Format:** `AQS_DocumentName_v#_MMDDYYYY.md`

**Rules:**
- All four core documents share the SAME version number
- Increment version for ALL documents when ANY document is updated
- If a document has no changes, update the filename only (no content changes needed)
- Move old versions to `Documents/Archive/` folder

**Core Documents:**
- `AQS_ProjectInstructions_v#_MMDDYYYY.md`
- `AQS_GDD_v#_MMDDYYYY.md`
- `AQS_Architecture_v#_MMDDYYYY.md`
- `AQS_DesignDecisions_v#_MMDDYYYY.md`

**Example version bump:**
```
v1 -> v2 (all four files)
Old files moved to Documents/Archive/
```

---

## Code Editing Preferences

### Manual Copy/Paste (providing code to user)

- Provide COMPLETE file replacements
- Easier than hunting for specific lines
- Reduces errors from partial edits

### MCP Direct Edits (Claude editing via tools)

- Use `script_apply_edits` for method-level changes
- Use `apply_text_edits` for precise line/column edits
- Use `validate_script` to check for errors
- Direct MCP edits are preferred when connection is stable

---

## Unity MCP Tools Reference

### Script Operations

| Tool | Purpose |
|------|---------|
| `manage_script` (read) | Read script contents |
| `get_sha` | Get SHA256 hash + file size |
| `validate_script` | Check syntax/structure |
| `create_script` | Create new C# scripts |
| `delete_script` | Remove scripts |

### Script Editing (Preferred)

**`script_apply_edits`** - Structured C# edits:
- `replace_method` - Replace entire method body
- `insert_method` - Add new method
- `delete_method` - Remove a method
- `anchor_insert/delete/replace` - Pattern-based edits

**`apply_text_edits`** - Precise text edits:
- Line/column coordinate-based changes
- Atomic multi-edit batches

### Other Operations

| Tool | Purpose |
|------|---------|
| `manage_asset` | Import, create, modify, delete assets |
| `manage_scene` | Load, save, create scenes |
| `manage_gameobject` | Create, modify, find GameObjects |
| `manage_prefabs` | Open/close prefab stage |
| `manage_editor` | Play/pause/stop, tags, layers |
| `read_console` | Get Unity console messages |
| `run_tests` | Execute EditMode or PlayMode tests |

### MCP Best Practices

1. **Read before editing** - View the file before making changes
2. **Validate after edits** - Run `validate_script` to catch syntax errors
3. **Use structured edits** - Prefer `script_apply_edits` for methods
4. **Check console** - Use `read_console` after changes
5. **Avoid verbose output** - Don't display full read tool calls (causes lockup)
6. **Avoid hierarchy modifications** - MCP hierarchy changes can cause Unity lockups

---

## Coding Standards

### Required Frameworks

- **Odin Inspector** (4.0.1.2) - For data structures and Inspector UI
- **Odin Validator** (4.0.1.2) - For asset validation
- **DOTween Pro** (1.0.386) - For animations and juice
- **Feel** (5.9.1) - For MMFeedbacks, impacts, screen shake
- **UniTask** (2.5.10) - For async/await patterns
- **SOAP** - For ScriptableObject architecture
- **Master Audio 2024** - For adaptive stem-based music
- **New Input System** (1.16.0) - Use `InputSystem.actions.FindAction()`

### Architecture Patterns

**SOLID Principles - Required:**
- **S**ingle Responsibility - One reason to change per class
- **O**pen/Closed - Open for extension, closed for modification
- **L**iskov Substitution - Subtypes must be substitutable
- **I**nterface Segregation - Many specific interfaces over one general
- **D**ependency Inversion - Depend on abstractions, not concretions

**ScriptableObject Architecture Pattern (SOAP):**
- Use ScriptableObjects for data, configuration, and events
- Decouple systems through SO-based event channels
- Prefer SO events over direct references between managers

**Additional Patterns:**
- **Use Interfaces** - IJoeyAbility, IMusicEntity, IDamageable
- **Async over Coroutines** - Prefer UniTask async/await unless coroutine is trivial
- Keep scripts small and focused
- Extract controllers/services early to avoid large MonoBehaviours
- Use events for decoupled communication

### Code Style

**CRITICAL: Explicit types only**

- **NO `var` keyword** - Always use explicit type declarations
- Clear, readable code wins over clever code
- PascalCase for public, camelCase for private
- Avoid God classes - keep classes focused

### File Size Limits

**CRITICAL: 800 lines maximum per script**

- When a script approaches 800 lines, it is time to refactor
- Extract logic into separate classes, services, or controllers
- Do not wait until scripts are unmanageable

### Event Patterns

- Set state BEFORE firing events that handlers may check
- Initialize UI to known states (explicit Hide/Show calls)
- Guard against event re-triggering during batch operations

### Performance Rules

**In Gameplay Loops (Update, FixedUpdate):**
- NO allocations (`new`, string concatenation, LINQ)
- Cache all component references in Awake/Start
- Use object pooling for Joeys, projectiles, VFX

### Ground Detection

**Use collision-based detection, NOT raycasts:**
- Tag ground objects with "Ground" tag
- Use OnCollisionEnter/Exit
- Raycast-based was found unreliable on platforms

---

## Project Documents

| Document | Purpose |
|----------|---------|
| AQS_GDD | Game design and mechanics |
| AQS_Architecture | Script catalog, packages, code structure |
| AQS_DesignDecisions | History, lessons learned, version tracking |
| AQS_ProjectInstructions | Development protocols (this document) |

**Note:** User will ask Claude to review core docs at the start of each chat. Read and follow these instructions carefully.

---

## Development Status

### Phase 1: Foundation - IN PROGRESS

| Item | Status |
|------|--------|
| Project setup (Unity 6.2.2 URP, GitHub) | DONE |
| Core player movement (walk, jump, crouch) | DONE |
| Climbing system with stamina | DONE |
| Input System integration | DONE |
| Ground detection (collision-based) | DONE |
| Joey prefab (base visuals) | DONE |
| Audio decision (stem-based) | DONE |
| Nine Volt Audio acquired | DONE |
| JoeyDefinition/AbilityDefinition SOs | IN PROGRESS |
| MVP launch mechanic | TODO |

### Phase 2: Systems Expansion - TODO

| Item | Status |
|------|--------|
| All 7 Joey abilities | TODO |
| Energy/cooldown system | TODO |
| Pouch management UI | TODO |
| Enemy AI (2-3 types) | TODO |
| Combat feedback (VFX, SFX) | TODO |
| Music Manager (stem layering) | TODO |

### Phase 3: Level Creation - TODO

| Item | Status |
|------|--------|
| Swamp level complete | TODO |
| Suburb level complete | TODO |
| Map/minimap system | TODO |
| Save/load system | TODO |

---

## Hardware Constraints

**Development Machine (NitroDad Laptop):**
- Intel i5-8300H
- 12 GB RAM
- 4 GB GPU
- Windows 11 Home 24H2

**Implications:**
- Profile Smart Lighting 2D early - can be expensive
- Object pooling essential
- No real-time procedural audio - use stem-based
- Max ~12 simultaneous audio sources
- Test on target hardware regularly

---

## Quick Reference Checklist

**Always:**
- [ ] Verify file/method/class names exist before referencing
- [ ] Wait for user verification before proceeding
- [ ] Use ASCII encoding only
- [ ] Use explicit types (no `var` keyword)
- [ ] Use New Input System
- [ ] Validate scripts after MCP edits
- [ ] Set state BEFORE firing events
- [ ] Use interfaces for dependencies
- [ ] Prefer async/await (UniTask) over coroutines
- [ ] Follow SOLID principles
- [ ] Use SOAP architecture with event channels
- [ ] Use collision-based ground detection
- [ ] Fetch current documentation before recommendations
- [ ] Profile early on target hardware

**Never:**
- [ ] Assume names exist without checking
- [ ] Rush ahead with multiple steps
- [ ] Use UTF-8 or special characters
- [ ] Use `var` keyword
- [ ] Use legacy Input class
- [ ] Display full read tool calls
- [ ] Use MCP for hierarchy modifications
- [ ] Let scripts grow past 800 lines without refactoring
- [ ] Make recommendations based on potentially outdated docs
- [ ] Use raycast-based ground detection
- [ ] Implement real-time procedural audio synthesis
- [ ] Create allocations in Update loops

---

## Lessons Learned from Previous Projects

1. **Extract controllers early** - Don't let MonoBehaviours grow large
2. **Event timing matters** - Update state BEFORE firing events
3. **Initialize UI explicitly** - Don't rely on default states
4. **Validate positions** - Check before UI placement
5. **Guard batch operations** - Hide/reset UI before AND after
6. **Use boolean flags** - For state that persists across show/hide
7. **Test edge cases early** - Don't wait for playtest to find bugs
8. **Asset inventory matters** - Review owned assets against project needs
9. **Simpler is often better** - Stem-based audio vs procedural synthesis
10. **Collision > Raycast** - For ground detection reliability

---

## Scope Control

**The GDD is the authoritative feature set.**

- Any feature not in the GDD requires strong justification
- Challenge scope creep - demand reasons for additions
- Focus on shipping a complete, polished experience
- Innovation focuses on core mechanics (Joey combat, adaptive music)
- Secondary systems can use simpler approaches

---

**End of Project Instructions**

These instructions should be followed for every conversation in this project. User will ask Claude to review these docs at the start of each chat session.
