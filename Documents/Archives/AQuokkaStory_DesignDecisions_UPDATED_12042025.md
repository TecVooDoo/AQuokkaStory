# A Quokka Story - Design Decisions and Insights

**Version:** 1.0  
**Date Created:** December 4, 2025  
**Last Updated:** December 4, 2025  
**Developer:** TecVooDoo LLC  
**Designer:** Rune (Stephen Brandon)  

---

## Recent Changes (December 2025)

### Asset Inventory Review

A comprehensive review of the 2,094+ Unity assets revealed several overlooked tools highly relevant to A Quokka Story:

| Asset | Purpose | Status |
|-------|---------|--------|
| **Corgi Engine** | Metroidvania framework - was missing from recommendations | NOW PRIORITIZED |
| **SensorToolkit 2** | Enemy AI perception (sight, hearing) | Added to stack |
| **AA Map and Minimap System** | Metroidvania exploration tracking | Added to stack |
| **Dynamic Bone** | Quokka/joey physics (ears, tails) | Added to stack |
| **Ragdoll Animator 2** | Joey impact reactions | Added to stack |
| **Toolkit for Verlet Motion** | Joey tether/rope effects | Added to stack |
| **GASify** | Gameplay Ability System structure | Added to stack |

### Technical Decisions

| Decision | Choice | Rationale |
|----------|--------|-----------|
| Ground detection | Collision-based (not raycasts) | Raycasts unreliable on platform objects |
| Audio system | Stem-based (not procedural synthesis) | Hardware constraints, simpler implementation |
| Version control | GitHub (not Unity Version Control) | Sync issues with UVC |
| Character controller | Custom with SOAP (study Corgi Engine) | More control, matches architecture preferences |

---

## The Joey System Design

### Core Innovation: Risk vs Reward

Every Joey ability has a meaningful **drawback** to prevent dominant strategies:

| Joey | Ability | Benefit | Drawback |
|------|---------|---------|----------|
| Normal | Map reading | Reveals secrets | Fragile - death locks true ending |
| Lead | Bowling ball crash | +30% defense, knockback immunity | -25% movement speed |
| Helium | Inflate enemies | -40% fall speed | Reduced air control |
| GI | Bazooka blast | Gatling gun passive | Backblast damages Mom |
| Ninja | Banjo shuriken | Shadow step invincibility | -25% energy regen for other Joeys |
| Ballet | Pirouette freeze | +40% speed and jump | Takes double damage |
| Kazoo | TBD (Kickstarter) | TBD | TBD |

### Energy System Economics

The energy system creates strategic depth:

- **Max Energy:** 100 points per Joey
- **Ability Costs:** 25-75 energy depending on power
- **In-Pouch Regen:** 30 energy/second (3x faster)
- **Out-of-Pouch Regen:** 10 energy/second
- **Music Volume:** Tied to energy level (low energy = quiet instrument)

**Design Insight:** This creates natural gameplay rhythm - launch joey, ability activates, joey returns to pouch to recharge. Spamming abilities reduces music layers, which discourages button mashing.

### Pouch Management

- **Active Slot:** Currently equipped Joey (Q/E to cycle)
- **Reserve Slots:** Up to 5 additional Joeys (number keys)
- **At Save Points:** Swap stored Joeys not currently in pouch

**Design Insight:** Limited pouch slots force strategic choices about which Joeys to bring into different areas.

---

## The Counterintuitive Discovery: Corgi Engine

### Original Assumption

Build everything custom with SOAP architecture for maximum control.

### December 2025 Discovery

The asset inventory review revealed **Corgi Engine** (More Mountains) was owned but overlooked. This is a complete 2D/2.5D Metroidvania framework by the same developer as TopDown Engine and Feel.

### Why This Matters

| Corgi Engine Feature | A Quokka Story Need |
|---------------------|---------------------|
| Room-based progression | ✓ Metroidvania exploration |
| Ability gating system | ✓ Joey ability gates |
| Save point system | ✓ Save/load needed |
| Health/damage system | ✓ Combat system needed |
| Integrates with Feel | ✓ Already own Feel |
| 2.5D support | ✓ 2.5D presentation |

### Decision Framework

| Approach | Pros | Cons |
|----------|------|------|
| **Build on Corgi Engine** | Faster to playable, Metroidvania systems pre-built, proven quality | Less architectural control, may need workarounds |
| **Custom with SOAP** | Full control, matches preferences, cleaner joey integration | More work, reinventing solved problems |

**Current Decision:** Study Corgi Engine patterns as reference, evaluate whether to build on it or just learn from it. Either way, it's invaluable for understanding Metroidvania implementation.

---

## Music System Design

### Philosophy

Music is the **heart of the game's identity**. The soundtrack is constructed from layered audio stems that dynamically respond to:
- Active Joeys in pouch
- Joey energy levels
- Enemies on screen
- Combat intensity

### Original vs Revised Approach

| Aspect | Original (Abandoned) | Revised (Current) |
|--------|---------------------|-------------------|
| Audio Generation | Real-time procedural synthesis | Pre-recorded stem loops |
| Implementation | Custom LightSynth system | Master Audio 2024 + Audio Mixer |
| Performance | Heavy CPU load | Efficient playback |
| Quality | Variable | Professional (Nine Volt Audio) |

**Reason for Change:** Hardware constraints (i5-8300H laptop) and development complexity. Stem-based approach achieves similar adaptive feel with much simpler implementation.

### Stem Organization Per Biome

Each biome has 8-10 instrument stems at **matching key and tempo**:

**Swamp (E minor, 105 BPM):**
- Ambient (always on)
- Bass (Mom - upright bass)
- Light drums (low energy)
- Full drums (high energy)
- Banjo (Ninja Joey OR Gator)
- Harmonica (GI Joey)
- Slide guitar (Snake enemies)
- Fiddle (Ballet Joey)
- Washboard (Helium Joey)

### Instrument Conflict Resolution

**Problem:** Ninja Joey and Gator both use banjo.

**Solution:**
- Before Ninja Joey rescue: Gator plays full banjo
- After Ninja Joey rescue: Gator switches to banjo accents only
- Ninja Joey maintains primary banjo melody

---

## Enemy Design Insights

### Stun Duration Scaling (Ballet Joey)

Different enemy sizes require different stun durations for balance:

| Enemy Size | Stun Duration | Examples |
|------------|---------------|----------|
| Small | 8-10 seconds | Snakes, Feral Cats |
| Medium | 5-7 seconds | Raccoons, Dogs, Gators |
| Large | 3-5 seconds | Boar, Wolverine, Bears |
| Flying (grounded) | 10-15 seconds | Hawks (disoriented from fall) |

### Predator Bait Mechanic (Ballet Joey)

When Ballet Joey stuns a flying bird, it falls to ground. Certain predators investigate:

**Attracted to bird bait:**
- Gators, Panthers, Bobcats, Feral Cats
- Wild Boar (omnivore curiosity)
- Raccoons (investigate anything)

**Ignore bird bait:**
- Domestic Dogs (focus on moving targets)
- Snakes (not bird predators)
- Black Bears (not interested)

**Design Insight:** This creates emergent gameplay - stun a hawk to distract a panther, creating a window to sneak past.

### Helium Joey Size Limits

Helium Joey cannot inflate:
- Wild Boar (too heavy)
- Wolverine (too aggressive)
- Black Bear (far too large)
- Pythons (too large)

**Design Insight:** This prevents Helium Joey from trivializing encounters. Large enemies require different strategies.

---

## Level Design Principles

### Progression Structure

```
Tutorial: Cargo Plane (Linear, 5-10 min)
    ↓ Rescue: Normal Joey, Lead Joey
Level 1: Swamp (Semi-Open, 15-20 min)
    ↓ Rescue: Ballet Joey
Level 2: Suburb (Hub-based, 20-25 min)
    ↓ Rescue: Ninja Joey
Level 3: Swamp Revisit (New areas, 15-20 min)
    ↓ Rescue: Helium Joey
Level 4: City (Vertical, 25-30 min)
    ↓ Rescue: GI Joey
Final: Airstrip HQ (Gauntlet + Boss, 30-40 min)
    ↓ Rescue: Kazoo Joey
```

### Ability Gating Design

Each Joey unlocks specific traversal:
- **Lead Joey:** Break heavy barriers
- **Helium Joey:** Reach high platforms, float
- **Ballet Joey:** Create ice platforms, navigate slippery surfaces
- **Ninja Joey:** Cut vines and ropes
- **GI Joey:** Burn brush, destroy armored obstacles

**Design Insight:** Earlier levels contain secrets only accessible with later-game Joeys, encouraging backtracking.

### Map System

- 5 map pieces total (one per main level)
- Required for revealing Airstrip HQ location
- Normal Joey's passive ability reads torn map pieces
- Full map reveals shortcuts and secret areas

---

## Technical Architecture Decisions

### Why SOAP Architecture

**ScriptableObject Architecture Pattern (SOAP)** chosen because:
1. Data-driven design enables iteration without code changes
2. Event channels provide decoupled communication
3. Matches SOLID principles
4. Easy to extend via new ScriptableObjects
5. Inspector-friendly for non-programmers

### Why NOT Real-Time Procedural Audio

Original plan: Each entity generates audio in real-time using LightSynth.

**Problems:**
- CPU-intensive on target hardware
- Complex synchronization challenges
- Variable audio quality
- Long development time

**Solution:** Pre-recorded stems from Nine Volt Audio (30,000 loops), managed by Master Audio 2024. Same adaptive feel, fraction of the complexity.

### Why Collision-Based Ground Detection

Original: Raycast from player feet.

**Problems encountered:**
- Unreliable on moving platforms
- Edge cases with sloped surfaces
- False negatives on certain collider types

**Solution:** Tag ground objects with "Ground" tag, use OnCollisionEnter/Exit.

**Result:** 100% reliable ground detection.

### Why GitHub Over Unity Version Control

Originally used Unity Version Control (Plastic SCM).

**Problems:**
- Frequent sync issues
- Conflicts with large assets
- Slow operations

**Solution:** Migrated to GitHub.

**Result:** Cleaner workflow, better reliability.

---

## Open Questions / TODO

### Design Questions to Resolve

| Question | Context | Priority |
|----------|---------|----------|
| Build on Corgi Engine or custom? | Framework decision | HIGH |
| Kazoo Joey ability? | Kickstarter backer designs | LATER |
| Boss fight music design? | All instruments + chaos | MEDIUM |
| Difficulty options? | Accessibility vs challenge | MEDIUM |

### Technical Debt

| Item | Description | Priority |
|------|-------------|----------|
| Corgi Engine evaluation | Study patterns, decide on adoption | HIGH |
| Stem organization | Sort Nine Volt Audio by key/tempo | MEDIUM |
| Animation pipeline | Establish sprite workflow | MEDIUM |

### Features to Implement

| Feature | Priority | Dependencies |
|---------|----------|--------------|
| JoeyDefinition SO | HIGH | SOAP setup |
| AbilityDefinition SO | HIGH | JoeyDefinition |
| Joey launch mechanic MVP | HIGH | Input System |
| Energy system | HIGH | JoeyController |
| MusicManager | MEDIUM | BiomeMusicData |
| Enemy AI (basic) | MEDIUM | SensorToolkit 2 |
| Map/minimap | MEDIUM | AA Map System |

---

## Lessons Learned

### 1. Asset Inventory Matters

The December 2025 asset review revealed Corgi Engine was owned but overlooked. This single discovery could save months of development time.

**Action:** Regularly review owned assets against project needs.

### 2. Simpler Is Often Better

Procedural audio synthesis was ambitious but impractical. Stem-based adaptive music achieves similar results with far less complexity.

**Action:** Challenge assumptions about implementation complexity.

### 3. Hardware Constraints Are Real

Development laptop (i5-8300H, 4GB GPU) limits what's practical:
- No real-time procedural audio
- Profile Smart Lighting 2D
- Object pooling essential

**Action:** Test on target hardware early and often.

### 4. Collision > Raycast for Ground Detection

Raycast-based ground detection failed on platform objects. Collision-based detection is simpler and 100% reliable.

**Action:** Prefer simple, proven approaches over clever solutions.

### 5. Study Existing Frameworks

Even if not using Corgi Engine directly, studying its Metroidvania patterns saves time and prevents reinventing wheels.

**Action:** Always check what existing solutions do before building custom.

### 6. Scope Control Is Essential

The GDD defines the feature set. Any addition requires justification. Innovation should focus on core mechanics (joey combat, adaptive music), not every system.

**Action:** Challenge scope creep aggressively.

---

## Implementation Status

### Completed Systems

| System | Status | Notes |
|--------|--------|-------|
| Project setup | COMPLETE | Unity 6.2.2 URP, GitHub |
| Player movement (basic) | COMPLETE | Walk, jump, crouch |
| Climbing system | COMPLETE | With stamina |
| Input System integration | COMPLETE | New Input System |
| Ground detection | COMPLETE | Collision-based |
| Joey prefab (visual) | COMPLETE | Base placeholder |
| Audio decision | COMPLETE | Stem-based approach |

### In Progress

| Component | Status | Blockers |
|-----------|--------|----------|
| JoeyDefinition SO | Sprint 1 | None |
| AbilityDefinition SO | Sprint 1 | JoeyDefinition |
| Joey launch mechanic | Sprint 1 | SOs needed |

### Not Started

| Component | Priority | Notes |
|-----------|----------|-------|
| Full Joey abilities | Phase 2 | After MVP launch works |
| Enemy AI | Phase 2 | Will use SensorToolkit 2 |
| MusicManager | Phase 2 | After basic gameplay |
| Level art | Phase 3 | After mechanics proven |
| Combat feedback (VFX) | Phase 2 | Use Feel |
| Map system | Phase 3 | Use AA Map System |
| Save/load | Phase 3 | Use Easy Save |

---

## Sprint Planning

### Sprint 1: Joey Foundation (Current)

**Goal:** MVP joey launch mechanic with one ability

**Tasks:**
1. [ ] Create JoeyDefinition ScriptableObject
2. [ ] Create AbilityDefinition ScriptableObject
3. [ ] Build JoeyController with state machine
4. [ ] Implement basic launch mechanic (aim, power, release)
5. [ ] Set up material/color system for joey types
6. [ ] Wire audio events for launch/recall
7. [ ] Test with Lead Joey (bowling ball ability)

**Definition of Done:** Can launch Lead Joey, see it crash through test obstacle, have it return to pouch.

### Sprint 2: Energy & Music (Next)

**Goal:** Energy system affects gameplay and music

**Tasks:**
1. [ ] Implement energy tracking in JoeyController
2. [ ] Create BiomeMusicData ScriptableObject
3. [ ] Build basic MusicManager (stem loading, volume control)
4. [ ] Connect energy level to music volume
5. [ ] Test stem layering with 3-4 instruments

---

## Reference Materials

### In Google Drive
- A Quokka Story GDD Master (authoritative design document)
- Project Asset Recommendations v5 (asset inventory analysis)
- Progress summaries and sprint notes

### Corgi Engine Study Topics
- Room-based level architecture
- Ability unlock system
- Save point implementation
- Health/damage system
- 2.5D camera configuration

### Nine Volt Audio Organization
- Stems need sorting by key and tempo
- Target biome assignments documented in GDD
- Start with Swamp (E minor, 105 BPM)

---

**End of Design Decisions Document**

This is a living document updated as:
- New discoveries reveal insights
- Design questions are resolved
- Implementation uncovers new considerations
- Playtesting provides feedback
