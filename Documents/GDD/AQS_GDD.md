# A Quokka Story -- Game Design Document

**Developer:** TecVooDoo LLC / Rune (Stephen Brandon)
**Genre:** 2.5D Metroidvania Platformer
**Platform:** PC (primary), Android, iOS, Consoles (stretch)
**Engine:** Unity 6 (URP)
**Art Style:** Hyper-cute stylized 3D with 2D presentation
**Tone:** Heartfelt, humorous, conservation-themed
**Inspiration:** Hollow Knight, Celeste, Ori series, Kirby
**Document Version:** v2.0 (reformatted from v1 Dec 2025)
**Last Updated:** March 12, 2026

---

## Table of Contents

### Core Document
1. [Vision](#1-vision)
2. [Core Loop](#2-core-loop)
3. [Player Character](#3-player-character)
4. [Joey System](#4-joey-system)
5. [Enemy Design](#5-enemy-design)
6. [Music System](#6-music-system)
7. [Level Design and Progression](#7-level-design-and-progression)
8. [Narrative](#8-narrative)
9. [Visual Style](#9-visual-style)
10. [UI Layout](#10-ui-layout)
11. [Scope Control](#11-scope-control)
12. [Target Audience](#12-target-audience)
13. [Design Decisions](#13-design-decisions)
14. [Development Phases](#14-development-phases)

### Appendices
A. [Joey Roster Quick Reference](#appendix-a-joey-roster-quick-reference)
B. [Enemy Tables by Biome](#appendix-b-enemy-tables-by-biome)
C. [Stem Assignments by Biome](#appendix-c-stem-assignments-by-biome)
D. [Collectibles](#appendix-d-collectibles)

---

## 1. Vision

### High Concept

*"A mother's love knows no bounds -- even if it means launching her children at enemies."*

A 2.5D Metroidvania-style adventure platformer where players control a mother Quokka on a mission to rescue her scattered Joeys from poachers. The game's unique selling point is its **Joey-launching combat mechanic** combined with an **adaptive stem-based music system** where each character on screen contributes an instrument layer to the soundtrack.

### Design Pillars

1. **Joey-Launching Combat** -- risk/reward mechanics with meaningful drawbacks per Joey
2. **Adaptive Music** -- the soundtrack is alive; every character and enemy adds a layer
3. **Maternal Bond** -- a mother's unbreakable determination drives the story
4. **Exploration with Purpose** -- Metroidvania ability-gating tied to Joey rescues
5. **Conservation Message** -- wildlife trafficking awareness woven into gameplay

### What It Is / What It Isn't

**IS:** A family-friendly platformer with surprising depth, unique combat, and a living soundtrack.
**ISN'T:** A grimdark metroidvania, a precision platformer, or a collectathon.

---

## 2. Core Loop

1. **Explore** environments to find Joeys and map pieces
2. **Launch** Joeys to solve traversal puzzles and defeat enemies
3. **Rescue** Joeys to unlock new abilities and music layers
4. **Progress** through increasingly complex levels
5. **Unite** the family and liberate captured animals

---

## 3. Player Character: Quokka Mom

### Movement

- **Walk/Run:** Standard 2.5D platforming movement
- **Jump:** Single jump with variable height based on button hold
- **Pouch Dash:** Short-range dash that protects Joeys (unlocked mid-game)
- **Climb:** Can scale certain surfaces and vines (with stamina system)
- **Crouch:** Lower profile, access small passages

### Base Stats

| Stat | Value |
|------|-------|
| Health | 5 hearts (upgradeable, +3 max) |
| Movement Speed | Moderate (affected by equipped Joey) |
| Jump Height | Standard platformer range |
| Climb Stamina | Limited duration before forced release |

### Joey Launch Mechanic

**Core System:**
- **Aim:** Hold launch button to display targeting reticle
- **Power:** Hold longer for farther throw (max 2 seconds)
- **Launch:** Release to send Joey
- **Recall:** Joey returns after ability execution or manual recall
- **Cooldown:** Based on Joey's energy level

**Launch States:**
1. **In Pouch (Resting)** -- Joey regenerates energy at 3x rate, contributes full volume to music mix
2. **Aiming** -- Time slows slightly (15-20%), trajectory arc displayed
3. **Launched (Active)** -- Joey's instrument accents loudly, unique ability animation
4. **Depleted (Cooling)** -- Joey's instrument fades to low volume, passive regeneration

### Pouch Management

- **Active Slot:** Currently equipped Joey (Q/E to cycle)
- **Reserve Slots:** Up to 5 additional Joeys (quick-swap via number keys)
- **Rescued But Not Carried:** Stored safely, can swap at save points

### Energy System

| Parameter | Value |
|-----------|-------|
| Max Energy | 100 per Joey (upgradeable, +25 x3) |
| Ability Cost | 25-75 depending on power |
| In-Pouch Regen | 30 energy/second |
| Out-of-Pouch Regen | 10 energy/second |
| Music Volume Link | Energy level = instrument volume (0-100%) |

---

## 4. Joey System

### Design Principles

- Every ability has a drawback to prevent dominant strategies
- Equipped Joey affects Mom's stats to encourage swapping
- Energy management creates rhythm and prevents spam
- Personality shines through animations and sound design
- Synergies exist but are discovered, not required

### Joey Roster

#### 1. Normal Joey
- **Role:** Narrative anchor, map reader
- **Ability:** None (pure character)
- **Passive:** Can read torn map pieces to reveal secrets
- **Drawback:** Fragile -- if Normal Joey dies, "true ending" is locked
- **Instrument:** Clapsticks
- **Rescue:** Tutorial (Cargo Plane)

#### 2. Lead Joey
- **Role:** Tank, heavy hitter
- **Ability:** Bowling Ball Launch -- crashes through enemies and obstacles
- **Passive Buff:** Mom gains +30% defense, knockback immunity
- **Drawback:** Slows Mom's movement by 25%
- **Instrument:** Didgeridoo (deep bass drone)
- **Rescue:** Tutorial (Cargo Plane)

#### 3. Ballet Joey
- **Role:** Mobility, freeze control
- **Ability:** Pirouette Freeze -- spins and freezes enemies in AOE
- **Passive Buff:** Mom gains +40% movement speed and jump height
- **Drawback:** Fragile -- takes double damage when equipped
- **Instrument:** Bells and Violin (elegant, chiming melodies)
- **Rescue:** Level 1 (Swamp)
- **Special:** Stunned flying birds fall to ground, creating bait for predators

#### 4. Ninja Joey
- **Role:** DPS, stealth potential
- **Ability:** Banjo Shuriken -- bounces between enemies, tail blade slashes
- **Passive Buff:** Mom gains shadow step (brief invincibility during dash)
- **Drawback:** Causes exhaustion in other Joeys (25% slower energy regen)
- **Instrument:** Banjo (plucky, rhythmic strumming)
- **Rescue:** Level 2 (Suburb)

#### 5. Helium Joey
- **Role:** Traversal specialist, crowd control
- **Ability:** Inflate -- latches onto enemies/objects and inflates them until they float or pop
- **Passive Buff:** Mom's fall speed reduced by 40%
- **Drawback:** Reduced air control while equipped
- **Instrument:** Flute (airy, ascending notes)
- **Rescue:** Level 3 (Swamp Revisit)
- **Limit:** Cannot inflate large enemies (Black Bear, Wolverine, Wild Boar)

#### 6. GI Joey
- **Role:** Heavy damage dealer, area control
- **Ability:** Bazooka Blast -- explosive projectile that damages multiple enemies
- **Passive Buff:** Mom gains Gatling Gun (hold for rapid-fire)
- **Drawback:** Explosion damages Mom if too close (backblast)
- **Instrument:** Snare and Hand Drum (military intensity)
- **Rescue:** Level 4 (City/Industrial Zone)

#### 7. Kazoo Joey (Kickstarter Backer Choice)
- **Role:** Easter egg character, comedy relief
- **Ability:** TBD (named and designed by high-tier backer)
- **Instrument:** Kazoo (buzzy, comedic)
- **Rescue:** Final Zone (Airstrip HQ)

---

## 5. Enemy Design

### Design Philosophy

1. **Every Enemy Contributes to the Music** -- each enemy type adds a unique instrument layer
2. **Telegraphed Attacks** -- all attacks have clear visual tells
3. **Joey-Specific Weaknesses** -- enemies encourage Joey swapping
4. **Environmental Integration** -- enemies use terrain to advantage
5. **Escalation Pattern** -- early enemies teach, later enemies remix

See [Appendix B](#appendix-b-enemy-tables-by-biome) for full enemy tables.

### Boss: Poacher Leader (Multi-Phase)

- **Phase 1:** Ranged weapons, calls guards
- **Phase 2:** Uses stolen Joey abilities (mock versions)
- **Phase 3:** Desperate melee combat
- Requires all Joey abilities across phases
- Music becomes dissonant and chaotic during fight

---

## 6. Music System

### Philosophy

Music is the **heart of the game's identity**. The soundtrack is constructed from layered audio stems that dynamically fade in/out based on active Joeys, spawned enemies, and combat energy.

### Audio Source

Stems sourced from Nine Volt Audio's professional loop libraries (30,000 loops organized by key and tempo). Managed by Master Audio 2024.

### Instrument Assignments

| Character | Instrument | Always Present? |
|-----------|------------|-----------------|
| Mom | Frame Drum | Yes |
| Normal Joey | Clapsticks | Yes (post-tutorial) |
| Lead Joey | Didgeridoo | Yes (post-tutorial) |
| Helium Joey | Flute | After rescue |
| GI Joey | Snare/Hand Drum | After rescue |
| Ninja Joey | Banjo | After rescue |
| Ballet Joey | Bells/Violin | After rescue |
| Kazoo Joey | Kazoo | After rescue |

### Dynamic Mixing

| State | Volume |
|-------|--------|
| Mom | Always 100% |
| Joeys (In Pouch) | Volume = Energy Level (0-100%) |
| Joeys (Launched) | 150% (accent) |
| Joeys (Depleted) | 10-20% |
| Enemies | 60-80% |

### Instrument Conflict Resolution

**Problem:** Ninja Joey and Gator both use banjo.
**Solution:**
- Before Ninja Joey rescue: Gator plays full banjo
- After Ninja Joey rescue: Gator switches to banjo accents only
- Ninja Joey maintains primary banjo melody

See [Appendix C](#appendix-c-stem-assignments-by-biome) for full stem tables.

---

## 7. Level Design and Progression

### World Structure

```
Tutorial: Cargo Plane (Linear, 5-10 min)
    | Rescue: Normal Joey, Lead Joey
Level 1: Swamp (Semi-Open, 15-20 min)
    | Rescue: Ballet Joey
Level 2: Suburb (Hub-based, 20-25 min)
    | Rescue: Ninja Joey
Level 3: Swamp Revisit (New areas, 15-20 min)
    | Rescue: Helium Joey
Level 4: City/Industrial (Vertical, 25-30 min)
    | Rescue: GI Joey
Final Zone: Airstrip HQ (Gauntlet + Boss, 30-40 min)
    | Rescue: Kazoo Joey
```

### Progression Gates

- **Ability Gates:** Specific Joey abilities unlock areas
  - Lead Joey: Break heavy barriers
  - Helium Joey: Reach high platforms
  - Ballet Joey: Create ice platforms, freeze enemies as stepping stones
  - Ninja Joey: Cut vines and ropes
  - GI Joey: Burn brush, destroy armored obstacles
- **Map Gates:** Finding map pieces reveals paths
- **Story Gates:** Narrative triggers unlock biomes

---

## 8. Narrative

### Story Summary

A family of Quokkas is being illegally transported by poachers when their cargo plane crashes. Mom escapes with two Joeys, but the others are scattered across different biomes. She must brave dangerous environments to reunite her family and stop the poachers.

### Themes

- **Maternal Love:** A mother's unbreakable bond
- **Conservation:** Highlighting wildlife trafficking
- **Resilience:** Overcoming adversity through determination
- **Found Family:** All rescued animals become a community

### Endings

- **Standard Ending:** All Joeys rescued, animals freed, poachers defeated
- **True Ending:** All Joeys + all Animal Tokens collected, Normal Joey survives

### Narrative Delivery

- Minimal cutscenes (intro, midpoint, finale)
- Environmental storytelling (poacher camps, caged animals)
- Joey dialogue bubbles and reactions
- Found audio logs (optional backstory)

---

## 9. Visual Style

### Art Direction

- Thick outlines (toon shader) for readability
- Vibrant, saturated colors per biome
- Exaggerated proportions (big eyes, small bodies)
- Expressive animations for personality

### Biome Color Palettes

| Biome | Primary Colors | Mood |
|-------|----------------|------|
| Cargo Plane | Grays, oranges (sunset) | Urgent, chaotic |
| Swamp | Greens, teals, bioluminescent blues | Mysterious, eerie |
| Suburb | Pastels, faded colors | Nostalgic, abandoned |
| City | Cool grays, neon accents | Industrial, oppressive |
| Airstrip | Military greens, harsh whites | Sterile, threatening |

### Character Design

- **Mom:** Warm browns, maternal proportions, visible pouch
- **Joeys:** Color-coded by ability (Lead=silver, Helium=light blue, GI=olive green, etc.)
- **Enemies:** Desaturated, aggressive poses, red accents
- **Poachers:** Faceless/obscured to avoid humanizing

---

## 10. UI Layout

### Gameplay HUD

```
+------------------------------------------+
|  [Hearts]        [Energy Bar]    [Joey]  |
|                                          |
|            [GAME AREA]                   |
|                                          |
+------------------------------------------+
```

- Health (hearts)
- Active Joey portrait
- Energy bar (tied to music volume)
- Minimal -- focus on gameplay

### Accessibility

- Colorblind modes
- Remappable controls
- Text scaling
- Gore toggle (stylized blood optional)

---

## 11. Scope Control

### MVP (Minimum Viable Product)

- Tutorial + 2 levels (Swamp, Suburb)
- 4 Joeys (Normal, Lead, Ballet, Ninja)
- 3 enemy types per level
- Stem-based music (3-4 instruments)
- Basic save/load

### Nice to Have (Post-MVP)

- All 7 Joeys
- All 5 levels
- Full music system (8+ instruments)
- Photo mode
- Speedrun timer
- Online leaderboards

---

## 12. Target Audience

### Primary
- Age: 10-35 years old
- Interests: Platformers, Metroidvanias, cute characters, unique mechanics
- Comparable Games: Hollow Knight, Celeste, Ori series, Kirby

### Secondary
- Families (E10+ rating)
- Music enthusiasts (adaptive soundtrack)
- Animal lovers (conservation themes)

### Platforms
1. **PC (Steam)** -- Primary target
2. **Android** -- Second platform
3. **iOS** -- Third platform
4. **Consoles** -- Stretch goal (Nintendo Switch ideal)

---

## 13. Design Decisions

| # | Decision | Rationale |
|---|----------|-----------|
| 1 | Joey-launching combat with risk/reward drawbacks | Core innovation; every ability has a cost to prevent dominant strategies |
| 2 | Stem-based adaptive music (Nine Volt Audio + Master Audio 2024) | Music as identity; characters and enemies contribute layers |
| 3 | Collision-based ground detection (not raycasts) | Works better with 2.5D slopes and moving platforms |
| 4 | Vanilla SO architecture + GameEvent system | Standard across all TecVooDoo projects |
| 5 | Single difficulty, fair challenge | True ending for mastery, not punishment |
| 6 | Stylized/cartoonish gore with toggle option | Family-friendly default, optional for older players |
| 7 | Normal Joey death locks true ending | Creates emotional stakes without game-over penalty |
| 8 | Energy system tied to music volume | Gameplay and audio feedback are unified |

---

## 14. Development Phases

### Phase 1: Foundation (Redo -- all lost to crash)

| Item | Status |
|------|--------|
| Project setup (Unity 6, URP, GitHub) | TODO |
| Core player movement (walk, jump, crouch) | TODO |
| Climbing system with stamina | TODO |
| Input System integration | TODO |
| Ground detection (collision-based) | TODO |
| Joey prefab (base visuals) | TODO |
| JoeyDefinition/AbilityDefinition SOs | TODO |
| MVP launch mechanic | TODO |

### Phase 2: Systems Expansion

| Item | Status |
|------|--------|
| All 7 Joey abilities | TODO |
| Energy/cooldown system | TODO |
| Pouch management UI | TODO |
| Enemy AI (2-3 types) | TODO |
| Combat feedback (VFX, SFX) | TODO |
| Music Manager (stem layering) | TODO |

### Phase 3: Level Creation

| Item | Status |
|------|--------|
| Swamp level complete | TODO |
| Suburb level complete | TODO |
| Art pass on environments | TODO |
| Collectibles implementation | TODO |
| Map system | TODO |
| Save/load system | TODO |

### Phase 4: Content Complete

| Item | Status |
|------|--------|
| City level complete | TODO |
| Swamp revisit complete | TODO |
| Airstrip HQ complete | TODO |
| Boss fight | TODO |
| All music layers | TODO |
| Narrative cutscenes | TODO |

### Phase 5: Polish and Testing

| Item | Status |
|------|--------|
| Playtesting (internal + external) | TODO |
| Bug fixing and optimization | TODO |
| Balancing | TODO |
| Audio mixing | TODO |
| Accessibility features | TODO |

### Phase 6: Launch

| Item | Status |
|------|--------|
| PC (Steam) release | TODO |
| Android/iOS ports | TODO |
| Console ports (stretch goal) | TODO |

---

## Appendix A: Joey Roster Quick Reference

| Joey | Role | Ability | Passive Buff | Drawback | Instrument |
|------|------|---------|-------------|----------|------------|
| Normal | Narrative | Map reading | -- | Fragile (death locks true ending) | Clapsticks |
| Lead | Tank | Bowling Ball | +30% defense, knockback immunity | -25% move speed | Didgeridoo |
| Ballet | Mobility | Pirouette Freeze (AOE) | +40% speed and jump | Double damage taken | Bells/Violin |
| Ninja | DPS | Banjo Shuriken (bouncing) | Shadow step (dash invincibility) | -25% other Joey regen | Banjo |
| Helium | Traversal | Inflate (float/pop) | -40% fall speed | Reduced air control | Flute |
| GI | Heavy DPS | Bazooka Blast (AOE) | Gatling Gun (rapid-fire) | Backblast self-damage | Snare/Hand Drum |
| Kazoo | Easter egg | TBD (backer choice) | TBD | TBD | Kazoo |

---

## Appendix B: Enemy Tables by Biome

### Swamp

| Enemy | Behavior | Weaknesses | Instrument |
|-------|----------|------------|------------|
| Snake | Slithers, lunges | Lead, GI, Ballet | Slide Whistle |
| Gator | Guards water, bites, tail swipe | Helium, Ninja | Banjo/Accents |
| Python | Constricts, slow, high HP | GI (too large for Helium) | Bass Clarinet |
| Feral Cat | Quick pounces, erratic | Ballet, Ninja | Meowing vocals |
| Panther | Stalks, ambush attacks | Ninja (detect), GI (flush out) | Low Percussion |
| Bobcat | Territorial, circling swipes | Lead, Ballet | Growling Brass |

### Suburb

| Enemy | Behavior | Weaknesses | Instrument |
|-------|----------|------------|------------|
| Hawk | Swoops, grabs Joeys | GI (anti-air), Ballet (freeze) | Hurdy Gurdy |
| Domestic Dog | Patrols, chases, barks alert | Lead, Helium | Barking Percussion |
| Raccoon | Steals items, sets traps | Ninja, GI | Accordion |
| Wild Boar | Charges (high damage) | Ballet (freeze), Ninja | Heavy Percussion |
| Wolverine (Mini-Boss) | Aggressive, multi-hit combos | GI + combos | Distorted Guitar |

### City

| Enemy | Behavior | Weaknesses | Instrument |
|-------|----------|------------|------------|
| Security Drone | Patrols, triggers alarms | GI (destroy), Ninja (stealth) | Electronic Synth |
| Poacher Guard | Armed, coordinated, uses cover | Ninja (takedowns), combos | Heavy Percussion |
| Black Bear (Rare) | Territorial, massive damage | Avoid or GI (forced) | Deep Bass Drum |

---

## Appendix C: Stem Assignments by Biome

### Swamp (E minor, 105 BPM)

| Stem | Source | Trigger |
|------|--------|---------|
| Ambient | Nature sounds | Always on |
| Bass | Mom -- upright bass | Always on |
| Light drums | Energy 0-30% | Energy level |
| Full drums | Energy 70-100% | Energy level |
| Banjo | Ninja Joey OR Gator | Entity present |
| Harmonica | GI Joey | Entity present |
| Slide guitar | Snake enemies | Entity present |
| Fiddle | Ballet Joey | Entity present |
| Washboard | Helium Joey | Entity present |

### Suburb (C major, 115 BPM)

| Stem | Source |
|------|--------|
| Retro synth pad | Ambient |
| Electric bass | Mom |
| Electronic drums | Energy-based |
| Synth lead | Ninja Joey |
| Chiptune accent | GI Joey |
| Bell synth | Ballet Joey |

### City (D minor, 125 BPM)

| Stem | Source |
|------|--------|
| Industrial ambient | Machinery |
| Heavy bass | Mom |
| Industrial percussion | Energy-based |
| Distorted guitar | GI Joey |
| Dark synth | Ninja Joey |
| String stabs | Ballet Joey |

### Airstrip (E minor, 140 BPM)

| Stem | Source |
|------|--------|
| Military ambient | Alarms, radio |
| Orchestral bass | Mom |
| Epic drums | Energy-based |
| Brass stabs | GI Joey |
| Full ensemble | All Joeys + Kazoo |

---

## Appendix D: Collectibles

| Collectible | Total | Purpose |
|-------------|-------|---------|
| Map Pieces | 5 | Required for HQ location |
| Animal Tokens | Variable | Affect ending scenes |
| Pouch Charms | Variable | Cosmetic customization |
| Gnomes | 12 | Unlock secret music room |
| Joey Skins | 7 | Cosmetic only |
| Health Upgrades | 3 | +1 heart each |
| Energy Upgrades | 3 | +25 energy capacity |

---

**End of Game Design Document**
