# A Quokka Story -- Project Status

**Project:** A Quokka Story (2.5D Metroidvania Platformer)
**Developer:** TecVooDoo LLC / Rune (Stephen Brandon)
**Unity Version:** 6000.3.11f1 (Unity 6, URP)
**Project Path:** `E:\Unity\AQuokkaStory`
**AQS Root:** `Assets/_AQS/`
**Last Updated:** April 1, 2026 (Session 15 -- Migration from Sandbox)

> **NOTE:** Original project lost to crash (pre-GitHub backup era). Resurrected from archived docs. Code rebuilt from scratch in Sandbox incubator (Sessions 0-14). Migrated to standalone project Session 15.

> **ARCHIVE RULE:** This doc holds only the current state and last ~2 sessions. When adding a new session, move older entries to `AQS_StatusArchive.md` (newest first at top of archive). This keeps the status doc fast to read while preserving full history.

**Reference doc:** `AQS_DevReference.md` -- architecture, standards, AI rules. Read on demand.

---

## Current State

**Phase:** Sprint 1 -- Core Feel. Migrating from Sandbox to standalone project. Raccoon belly weapon fires mortar-arc bolts, stance-gated. 2.5D greybox level with Cinemachine follow camera. Malbers zones working (Swim, Climb, LedgeGrab). Recipe in `AQS_MalbersRecipe.md`.

**Session 15 (Apr 1, 2026) -- Migration from Sandbox:**
- Standalone project created at `E:\Unity\AQuokkaStory`
- Minimum packages being installed (new default label approach -- not every project needs every package)
- Docs migrated from `E:\Unity\Sandbox\Documents\AQuokkaStory\`
- GitHub repo initialized
- Claude config created
- Asset export from Sandbox pending (unitypackage of `Assets/_Sandbox/_AQS/`)

**Pre-migration state (Session 14, from Sandbox):**
- Raccoon_Weapon_Test with LockAxis, mortar weapon (Force=15, AimAngle=35), stance-gated firing
- GreyBox_TestLevel with 11 children: platforms, water pool, climb walls, ledge platform
- CM_2_5D_Follow virtual camera (offset 4,2,0 WorldSpace, rotation 10,270,0)
- All zones working: Swim (auto-trigger), Climb (tag+physmat+layer), LedgeGrab (top-of-climb)
- 10 C# scripts, 3 animator controllers, 7 state SOs, 9 prefabs, 3 scenes

**Next (Session 16 -- Post-Migration Verification):**
- Import AQS assets from Sandbox export (unitypackage)
- Verify scenes load, prefabs resolve, scripts compile
- Fix any broken serialized references (scene objects, prefab refs)
- Continue Sprint 1 wrap-up:
  - Climbing stamina -- wire Stamina stat to Climb state
  - Mortar tuning -- adjust Force/AimAngle with level geometry
  - Stand stance toggle vs hold decision
  - Remove remaining CustomPatch debug logs from MShootable.cs
  - Clean up: delete Raccoon_Fresh_Test from scene

**Sprint 2 prep (after Sprint 1 wrap-up):**
- Raccoon cub prefab as Joey prototype -- follow mom like lemmings
- Replace bolt projectile with cub launch mechanic
- JoeyDefinition/AbilityDefinition ScriptableObjects
- Toolkit for Ballistics trajectory visualization for launch arc

**What survived the crash:**
- Full GDD (multiple versions, latest Dec 2025)
- Architecture plan (namespaces, file structure, system design)
- Design decisions document (rationale for key choices)
- Concept art (Quokka Mom + 7 Joey character sheets, front/back/side)
- Fundraiser campaign plan
- Project starter document

**What was lost:**
- All Unity project files, scenes, prefabs
- All scripts (PlayerController, ClimbController, GroundDetector were DONE)
- All asset imports and configurations
- GitHub repo (did not exist yet)

---

## Phase 1 TODO (Foundation -- Redo)

These items were completed before the crash and need to be rebuilt:

| Item | Old Status | New Status |
|------|-----------|------------|
| Project setup (Unity 6, URP, GitHub) | Was DONE | DONE |
| Core player movement (hop, jump) | Was DONE | IN PROGRESS -- Malbers AC with LockAxis 2.5D, Cinemachine follow camera, mortar weapon working |
| Climbing system with stamina | Was DONE | TODO |
| Input System integration | Was DONE | DONE -- AQS_InputActions asset + QuokkaInputHandler |
| Ground detection (collision-based) | Was DONE | DONE -- collision enter/stay/exit with normal check |
| Joey prefab (base visuals) | Was DONE | TODO |
| JoeyDefinition/AbilityDefinition SOs | Was IN PROGRESS | TODO |
| MVP launch mechanic | Was TODO | TODO |
| Package installation + configuration | Was DONE | DONE -- Sprint 1 packages installed |
| Asset eval for new candidates | N/A | DONE -- 30 relevant assets identified |
| GameEvent/GameEventListener system | Was DONE | DONE -- AQS.Core namespace |
| Placeholder character (Scorch) | N/A | DONE -- imported with anims, no scripts |

## Key Decisions (Session 0)

| Decision | Choice | Rationale |
|----------|--------|-----------|
| Platformer framework | Custom (NOT Corgi Engine) | Quokka movement is unique -- hop-based, not walk. Need full control over Joey stat modifiers. |
| Placeholder character | Scorch from HOK | Can climb trees, stand on hind legs, closest to quokka movement until custom model |
| Destruction | RayFire (ENTRY-168) | Already evaluated, MCP tools built (ENTRY-267) |
| Music system | Handled by AudioProject (PATWA) | Adaptive music system being built as drop-in for any project. Not Sandbox scope. |
| Quokka models | TBD -- son might model later | No good marsupial animation assets exist. Very specialized movement (fast/slow hop, bipedal stance, hand use). |

## Asset Audit (from Sandbox AssetLog -- 315 entries)

Searched all evaluated assets for AQS relevance. Results by gameplay need:

### Sprint 1-2: Install Now (Confirmed + Ready)

| Entry | Asset | Purpose | Sprint |
|-------|-------|---------|--------|
| 208 | Toolkit for Ballistics 2026 (Heathen) | Joey launch trajectory arc + projectile physics | 1 |
| 195 | Toon Kit 2 (OccaSoftware) | Cel shading -- "thick outlines, vibrant colors" art direction | 1 |
| 191 | Outlines Post-Process (OccaSoftware) | Full-scene toon outlines | 1 |
| 200 | Outline Objects (OccaSoftware) | Per-mesh outlines for interactables | 1 |
| 269 | All In 1 Shader Nodes | Glow, hit flash, hologram nodes for ASE/Shader Graph | 1 |
| 271 | Rope Toolkit | Vine swinging, rope puzzles, bridge physics | 2 |

### Sprint 3-4: Install When Needed

| Entry | Asset | Purpose | Sprint |
|-------|-------|---------|--------|
| 136 | MegaFiers 2 (Chris West) | Helium Joey inflate (FFD, morph targets) | 3 |
| 153 | Deform (Beans) | Squash/stretch for platforming feel | 3 |
| 231 | SensorToolkit2 (Micosmo) | Enemy vision cones, range detection, LOS (has 2D variants) | 4 |
| 229 | Behavior Designer Pro (Opsive) | Enemy behavior trees (patrol, chase, attack) | 4 |
| 164 | A* Pathfinding Pro (Aron Granberg) | GridGraph for 2D enemy navigation | 4 |
| 036 | Low Poly Animated Animals (Polyperfect) | Placeholder enemy models (snake, hawk, dog, cat) | 4 |
| 216 | Ragdoll Animator 2 (FImpossible) | Joey impact, enemy death ragdoll blending | 4 |

### Sprint 5+: Polish and Environment

| Entry | Asset | Purpose | Sprint |
|-------|-------|---------|--------|
| 182 | Buto (OccaSoftware) | Volumetric fog -- swamp atmosphere | 5 |
| 185 | LSPP God Rays (OccaSoftware) | Light shafts through swamp canopy | 5 |
| 202 | VFX Library (OccaSoftware) | Fireflies, wisps, swamp bubbles | 5 |
| 156 | Advanced Dissolve (Amazing Assets) | Enemy death dissolves, biome transitions | 5 |
| 170 | Ghost Effect Shader (OccaSoftware) | Ghost/spirit visual effects | 5 |
| 265 | Weather Maker (Digital Ruby) | Rain, fog, lightning for swamp mood | 5+ |
| 245 | FS Grappling Hook (Fantacode) | Metroidvania grapple traversal | 5+ |
| 169 | Inventory Framework 2 | UI Toolkit inventory for Joey/collectible management | 5+ |
| 214 | Dialogue System for Unity | Joey dialogue bubbles, found audio logs | 5+ |

### Still Need Evaluation

| Need | Status | Notes |
|------|--------|-------|
| AA Map and Minimap | NOT EVALUATED | Metroidvania exploration -- needs formal eval |
| Smart Lighting 2D | NOT EVALUATED | Swamp bioluminescence -- needs formal eval |

---

## Development Plan

### Sprint 1: Core Feel
- Install Tier 1 packages (defaults + confirmed AQS stack)
- Import Scorch from HOK as placeholder character
- Prototype Mom movement: hop-based locomotion (fast hop, slow hop), bipedal stance, climb with stamina
- Gray-box test level with platforms, walls, climbable surfaces
- Get platforming feel right before adding Joey mechanics

### Sprint 2: Joey Launch MVP
- Implement aim + trajectory arc + launch + recall
- One Joey (Lead) with bowling ball ability
- Basic hit detection and feedback (Feel + Damage Numbers Pro)
- RayFire for breakable barriers (Lead Joey smashes through)
- Trajectory display: Toolkit for Ballistics 2026 (ENTRY-208) + ALINE

### Sprint 3: Joey Variety
- Add 2-3 more Joeys (Ballet freeze, Helium inflate, Ninja shuriken)
- Inflate mechanic prototype (start with DOTween scale tween, eval MegaFiers 2 ENTRY-136 or Deform ENTRY-153 if needed)
- Pouch management (swap active Joey, equipped Joey modifies Mom stats)
- Rope Toolkit (ENTRY-271) for vine swinging traversal

### Sprint 4: Enemies
- 2-3 enemy types with basic AI (patrol, chase, attack)
- Joey-specific weaknesses working
- Placeholder animal models: Low Poly Animated Animals (ENTRY-036)
- AI stack: SensorToolkit2 (ENTRY-231) + Behavior Designer Pro (ENTRY-229) + A* Pathfinding Pro (ENTRY-164)
- Ragdoll Animator 2 (ENTRY-216) for enemy death reactions

### Sprint 5: Vertical Slice
- One playable level (Tutorial or Swamp)
- All core systems integrated

---

## Known Issues

| Issue | Severity | Status | Notes |
|-------|----------|--------|-------|
| Mortar arc too high/far for 2.5D | Low | TUNING | Force=15, AimAngle=35. Needs adjustment with level geometry. |
| Holster ID mismatch warning | Low | OPEN | "Default Holster does not exist on Holster ID list" on Play. Cosmetic, doesn't affect firing. |
| LockNextTarget Vector2 type warning | Low | OPEN | Input action type mismatch. Cosmetic. |
| Rabbit mouth weapon fires into ground | Low | TUNING | Same UseCamera issue as raccoon. Apply UseCamera=false fix when revisiting rabbit. |
| Remaining CustomPatch debug logs in MShootable.cs | Low | TODO | Remove MShootable-TRACE Debug.Log lines (temporary debugging). |
| GDD may need design updates | Medium | TODO | User to review after format cleanup |

---

## Reference Documents

| Document | Path |
|----------|------|
| Dev Reference | `Documents\AQS_DevReference.md` |
| Malbers Recipe | `Documents\AQS_MalbersRecipe.md` |
| GDD | `Documents\GDD\AQS_GDD.md` |
| Concept Art | `Documents\Archives\Quokka Mom and Joeys Concept Art\` |
| Archived Docs (pre-crash) | `Documents\Archives\` |
| Migration Manifest | `Documents\AQS_Migration_Manifest.md` |
| Sandbox Asset Log | `E:\Unity\Sandbox\Documents\Sandbox_AssetLog.md` |

---

**End of Document**
