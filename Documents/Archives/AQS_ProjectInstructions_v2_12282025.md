# A Quokka Story - Project Instructions

**Project:** A Quokka Story
**Developer:** TecVooDoo LLC
**Designer:** Rune (Stephen Brandon)
**Unity Version:** 6.2.2 URP
**Project Path:** E:\Unity\AQuokkaStory
**Document Version:** 2
**Last Updated:** December 28, 2025

---

## Shared Documentation

**This project follows TecVooDoo standards. Review these documents:**

| Document | Location | Purpose |
|----------|----------|---------|
| Core Protocols | `E:\TecVooDoo\Projects\Documents\CORE_DevelopmentProtocols.md` | Universal development rules |
| Unity Standards | `E:\TecVooDoo\Projects\Documents\Type\TYPE_Unity.md` | Unity-specific patterns and tools |

---

## Project Path

**The Unity project is located at:** `E:\Unity\AQuokkaStory`

Do NOT use worktree paths - always use the E: drive path for all file operations.

---

## Project-Specific Packages

Beyond the standard TYPE_Unity packages:

| Package | Version | Purpose |
|---------|---------|---------|
| SOAP | - | ScriptableObject architecture (event channels) |
| Master Audio 2024 | - | Adaptive stem-based music |
| Cinemachine | 3.0 | Camera system |

---

## Project Documents

| Document | Purpose |
|----------|---------|
| AQS_GDD | Game design and mechanics |
| AQS_Architecture | Script catalog, packages, code structure |
| AQS_DesignDecisions | History, lessons learned, version tracking |
| AQS_ProjectInstructions | Development protocols (this document) |

**Naming Convention:** `AQS_DocumentName_v#_MMDDYYYY.md`

All four documents share the same version number. Increment all when any document is updated.

---

## Project-Specific Rules

### Ground Detection

**Use collision-based detection, NOT raycasts:**
- Tag ground objects with "Ground" tag
- Use OnCollisionEnter/Exit
- Raycast-based was found unreliable on platforms

### Audio Approach

**Stem-based music (not procedural):**
- Use Master Audio 2024 for adaptive layering
- Max ~12 simultaneous audio sources (hardware constraint)
- No real-time procedural audio synthesis

---

## Hardware Constraints

**Development Machine (NitroDad Laptop):**
- Intel i5-8300H, 12 GB RAM, 4 GB GPU

**Implications:**
- Profile Smart Lighting 2D early - can be expensive
- Object pooling essential for Joeys, projectiles, VFX
- Test on target hardware regularly

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

## Scope Control

**The GDD is the authoritative feature set.**

- Any feature not in the GDD requires strong justification
- Challenge scope creep - demand reasons for additions
- Focus on shipping a complete, polished experience
- Innovation focuses on core mechanics (Joey combat, adaptive music)

---

## Project-Specific Lessons

- Collision > Raycast for ground detection reliability
- Stem-based audio vs procedural synthesis (simpler is better)
- Asset inventory matters - review owned assets against project needs

---

## Known Issues / TODO for Next Session

- (None currently)

---

**End of Project Instructions**

Review CORE_DevelopmentProtocols.md and TYPE_Unity.md for full development standards.
