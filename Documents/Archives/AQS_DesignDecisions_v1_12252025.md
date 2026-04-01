# A Quokka Story - Design Decisions and Lessons Learned

**Version:** 1
**Date Created:** December 25, 2025
**Last Updated:** December 25, 2025
**Developer:** TecVooDoo LLC

---

## Purpose

This document captures design decisions, technical insights, and lessons learned during development. It serves as a historical record and reference for future development.

---

## Version History

| Version | Date | Summary |
|---------|------|---------|
| 1.0 | Dec 25, 2025 | Document restructured to match SHT format, consolidated from previous versions |

---

## Project Origin

**Original Concept:** A mini marsupial from Australia must rescue joeys in the Florida Everglades - even if she has to sacrifice a few to survive.

**Core Pitch:** "A mother's love knows no bounds - even if it means launching her children at enemies."

**Key Feature:** Joey-launching combat combined with adaptive stem-based music where each character contributes an instrument layer.

---

## Design Decisions

### Combat: Joey Launching with Risk/Reward

**Decision:** Every Joey ability has a meaningful drawback to prevent dominant strategies.

**Rationale:**
- Prevents "one Joey for everything" gameplay
- Forces strategic Joey selection
- Creates player expression through combo discovery
- Energy system prevents spam

**Implementation:**
| Joey | Benefit | Drawback |
|------|---------|----------|
| Lead | +30% defense | -25% movement speed |
| Helium | -40% fall speed | Reduced air control |
| GI | Gatling gun passive | Backblast damages Mom |
| Ninja | Shadow step invincibility | -25% energy regen for others |
| Ballet | +40% speed and jump | Takes double damage |

### Music: Stem-Based Adaptive System

**Decision:** Use pre-recorded stem loops instead of real-time procedural synthesis.

**Rationale:**
- Hardware constraints (i5-8300H development laptop)
- Simpler implementation with Master Audio 2024
- Professional quality from Nine Volt Audio (30,000 loops)
- Same adaptive feel with fraction of complexity

**Implementation:**
- Each biome has 8-10 stems at matching key/tempo
- Entities contribute their instrument when present
- Energy level controls volume (low energy = quiet instrument)
- Discourages button mashing (spam = quieter music)

### Ground Detection: Collision-Based

**Decision:** Use OnCollisionEnter/Exit with "Ground" tag instead of raycasts.

**Rationale:**
- Raycasts unreliable on moving platforms
- Edge cases with sloped surfaces
- False negatives on certain collider types
- Collision-based is simpler and 100% reliable

### Framework: Custom with SOAP (Study Corgi Engine)

**Decision:** Build custom architecture using SOAP pattern, but study Corgi Engine for Metroidvania patterns.

**Rationale:**
- Full architectural control
- Matches TecVooDoo coding preferences
- Corgi Engine provides excellent reference for ability gating, save points, room transitions
- Can adopt specific patterns without full framework dependency

---

## Lessons Learned from Previous Projects

These lessons are carried forward from TecVooDoo development:

### Code Architecture

**1. Extract Controllers Early**
- Don't let MonoBehaviours grow past 500 lines
- Extract to plain C# classes before it becomes painful

**2. Event Timing Matters**
- Update state BEFORE firing events
- Handlers often check state during event processing
```csharp
// CORRECT:
_currentState = NewState;
OnStateChanged?.Invoke();

// WRONG:
OnStateChanged?.Invoke();
_currentState = NewState;
```

**3. Initialize UI Explicitly**
- Don't rely on default Unity states
- Call Hide/Show explicitly on startup
- Set all UI to known state before gameplay begins

### Development Process

**4. Step-by-Step Verification**
- Complete one step, verify it works, then proceed
- Don't rush ahead with multiple changes
- Errors compound when steps are skipped

**5. ASCII Encoding Only**
- Avoid UTF-8 in scripts
- No smart quotes, em-dashes, or special characters
- Prevents encoding-related bugs

**6. Validate After MCP Edits**
- Always run `validate_script` after changes
- Check Unity console for errors
- Don't assume edits succeeded

### UI/UX

**7. Guard Batch Operations**
- Hide/reset UI before AND after batch operations
- Prevents visual glitches from event cascades

**8. Boolean Flags for Persistent State**
- Don't rely on show/hide state
- Use explicit boolean flags for important states

**9. Test Edge Cases Early**
- Playtest unusual scenarios before polishing
- Edge cases found late are expensive to fix

---

## Technical Decisions

### Audio System

**Decision:** Stem-based adaptive music using Master Audio 2024 + Nine Volt Audio loops.

**Rationale:**
- Original plan (procedural LightSynth) was CPU-intensive
- Hardware constraints on development laptop
- Stem approach achieves similar adaptive feel
- Professional quality, simpler implementation

### Version Control

**Decision:** GitHub instead of Unity Version Control.

**Rationale:**
- Plastic SCM had frequent sync issues
- Conflicts with large assets
- Slow operations
- GitHub provides cleaner workflow

### Character Controller

**Decision:** Custom with SOAP, not Corgi Engine framework.

**Rationale:**
- More control over Joey integration
- Matches architectural preferences
- Can still study Corgi Engine patterns
- SOAP provides data-driven flexibility

### Coding Standards

**Decision:** Explicit types only (no `var` keyword).

**Rationale:**
- Clearer code readability
- Easier to understand at a glance
- Consistent with team preferences
- Reduces cognitive load during reviews

---

## Scope Control

### MVP Definition

**Must Have (Phase 1-2):**
- Tutorial + 2 levels (Swamp, Suburb)
- 4 Joeys (Normal, Lead, Ballet, Ninja)
- 3 enemy types per level
- Basic stem music (3-4 instruments)
- Save/load at checkpoints

**Nice to Have (Phase 3+):**
- All 7 Joeys
- All 5 levels
- Full music system (8+ instruments)
- Photo mode
- Speedrun features

### Scope Creep Prevention

- No new features until MVP is playable
- Document feature requests, don't implement immediately
- Playtest before polish
- "Good enough" beats "perfect"
- Innovation focuses on core mechanics (Joey combat, adaptive music)

---

## Open Questions

These decisions are pending and will be resolved during development:

| Question | Options | Decision |
|----------|---------|----------|
| Corgi Engine usage | Full framework / Reference only | Study patterns, build custom |
| Kazoo Joey ability | TBD (Kickstarter backer choice) | TBD |
| Boss fight music | All instruments chaotic / Structured phases | TBD |
| Difficulty options | Single / Multiple | TBD |
| Max simultaneous stems | 8 / 12 / 16 | Test on target hardware |

---

## Bug Fix History

| Date | Issue | Fix |
|------|-------|-----|
| Dec 2025 | Raycast ground detection unreliable on platforms | Switched to collision-based with "Ground" tag |

---

## Implementation Status

### Completed

- Project setup (Unity 6.2.2 URP, GitHub)
- Core player movement (walk, jump, crouch)
- Climbing system with stamina
- Input System integration (New Input System)
- Ground detection (collision-based)
- Joey prefab (base visuals)
- Audio decision (stem-based approach)
- Nine Volt Audio bundle acquired (30,000 loops)
- Asset inventory review and recommendations

### In Progress

- JoeyDefinition ScriptableObject
- AbilityDefinition ScriptableObject
- Joey launch mechanic MVP
- Material/color system for Joeys

### Pending/Needs Adjustment

- Corgi Engine evaluation (study vs adopt)
- Stem organization by key/tempo for Swamp biome
- Animation pipeline establishment
- MusicManager with fade system

---

## Playtest Feedback

*No playtest sessions yet - will be updated after Phase 1 MVP.*

---

## Future Considerations

### The Counterintuitive Discovery: Corgi Engine (Dec 2025)

Asset inventory review revealed Corgi Engine was owned but overlooked. This complete Metroidvania framework could accelerate development significantly.

**Corgi Engine Features Relevant to AQS:**
- Room-based progression
- Ability gating system
- Save point system
- Health/damage system
- 2.5D support
- Integrates with Feel (already own)

**Current Decision:** Study Corgi Engine patterns as reference. May adopt specific systems without full framework dependency.

### Instrument Conflict Resolution

**Problem:** Ninja Joey and Gator both use banjo.

**Solution:**
- Before Ninja Joey rescue: Gator plays full banjo
- After Ninja Joey rescue: Gator switches to banjo accents only
- Ninja Joey maintains primary banjo melody

### Ballet Joey Bird Bait Mechanic

When Ballet Joey stuns a flying bird (Hawk), it falls to ground. Certain predators investigate:

**Attracted to bird bait:**
- Gators, Panthers, Bobcats, Feral Cats
- Wild Boar, Raccoons

**Ignore bird bait:**
- Domestic Dogs (focus on moving targets)
- Snakes, Black Bears

**Design Insight:** Creates emergent gameplay - stun hawk to distract panther, sneak past.

### Helium Joey Size Limits

Cannot inflate large enemies:
- Wild Boar (too heavy)
- Wolverine (too aggressive)
- Black Bear (too large)
- Pythons (too large)

Prevents Helium Joey from trivializing encounters.

### Platform Expansion (Post-Launch)

- Steam release (primary)
- Mobile (Android, iOS)
- Console (Nintendo Switch ideal for party game nature)

---

## Asset Discoveries

### December 2025 Asset Inventory Review

Review of 2,094+ Unity assets revealed several overlooked tools:

| Asset | Purpose | Status |
|-------|---------|--------|
| Corgi Engine | Metroidvania framework/reference | NOW PRIORITIZED |
| SensorToolkit 2 | Enemy AI perception | Added to stack |
| AA Map and Minimap System | Exploration tracking | Added to stack |
| Dynamic Bone | Quokka/joey physics | Added to stack |
| Ragdoll Animator 2 | Joey impact reactions | Added to stack |

**Lesson:** Regularly review owned assets against project needs.

---

**End of Design Decisions Document**
