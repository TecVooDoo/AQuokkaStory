# A Quokka Story -- Migration Manifest

**Purpose:** Step-by-step plan for migrating AQS from Sandbox subproject to standalone Unity project.
**Source:** `E:\Unity\Sandbox` -- subproject at `Assets/_Sandbox/_AQS/`
**Target:** `E:\Unity\AQuokkaStory` -- standalone Unity 6 (6000.3.11f1) URP project
**Status:** Phase 1 IN PROGRESS
**Created:** April 1, 2026

---

## Overview

A Quokka Story was developed as a subproject inside Sandbox (Sessions 0-14). The standalone project at `E:\Unity\AQuokkaStory` is being set up for active development. Migration is happening earlier than planned (Sprint 1 vs Sprint 5) because Sandbox lag from eval bloat is impacting dev velocity.

Migration breaks into four phases:
1. Set up the standalone project (packages, UPM, settings) -- **IN PROGRESS**
2. Export + import custom AQS assets -- **PENDING**
3. Import third-party assets (Malbers, Polyperfect, etc.) -- **PENDING**
4. Verify, fix broken references, confirm scene plays -- **PENDING**

---

## Phase 1 -- Standalone Project Setup

User is installing minimum packages manually. New approach: only install what this project actually needs (not the old "default" label set).

### UPM / Registry Packages (manifest.json)

| Package | Status | Notes |
|---------|--------|-------|
| Input System | Done | Already in manifest |
| URP | Done | Project created with URP |
| Cinemachine | TBD | Required for CM_2_5D_Follow camera |
| AI Navigation | TBD | May not need until Sprint 4 (enemy AI) |
| ProBuilder | TBD | Required for greybox level geometry |
| Splines | TBD | Used by 2.5D Terrain if installed |
| Timeline | Done | Already in manifest |
| Addressables | TBD | Required BEFORE Master Audio 2024 |
| TextMesh Pro | TBD | Required BEFORE DOTween Pro |

### OpenUPM Scoped Registry

Add to manifest.json:
```json
"scopedRegistries": [
  {
    "name": "package.openupm.com",
    "url": "https://package.openupm.com",
    "scopes": [
      "com.cysharp.unitask",
      "com.ivanmurzak",
      "extensions.unity",
      "org.nuget.com.ivanmurzak",
      "org.nuget.microsoft",
      "org.nuget.system",
      "org.nuget.r3"
    ]
  }
]
```

### OpenUPM Packages

| Package | Status | Notes |
|---------|--------|-------|
| UniTask (`com.cysharp.unitask`) | TBD | async/await |
| MCP for Unity (`com.ivanmurzak.unity.mcp`) | TBD | AI bridge |
| MCP Animation (`com.ivanmurzak.unity.mcp.animation`) | TBD | Animation tools |
| MCP ProBuilder (`com.ivanmurzak.unity.mcp.probuilder`) | TBD | ProBuilder tools |

**DO NOT install:** `com.ivanmurzak.unity.mcp.particlesystem` -- CS0117 errors.

### Local File Packages

| Package | Status | Notes |
|---------|--------|-------|
| com.tecvoodoo.utilities | Done | `file:../../DefaultUnityPackages/com.tecvoodoo.utilities` |
| com.tecvoodoo.mcp-tools | TBD | `file:../../DefaultUnityPackages/com.tecvoodoo.mcp-tools` |
| com.tecvoodoo.games | Done | `file:../../DefaultUnityPackages/com.tecvoodoo.games` |

### Asset Store Packages (install in this order)

**Animancer MUST be first before any FBX art:**

| # | Package | Status | Notes |
|---|---------|--------|-------|
| 1 | Animancer Pro v8 | TBD | ENTRY-012. Install BEFORE any 3D art |
| 2 | Odin Inspector + Validator | TBD | Never remove once installed |
| 3 | DOTween Pro | TBD | Requires TextMesh Pro |
| 4 | Easy Save 3 | TBD | Save/load |
| 5 | Feel | TBD | ENTRY-015. Install BEFORE 3D art |
| 6 | Master Audio 2024 | TBD | Requires Addressables |
| 7 | Damage Numbers Pro | TBD | Hit feedback |
| 8 | ALINE | TBD | Debug visualization |
| 9 | EasyPooling 2025 | TBD | Object pooling |
| 10 | Boing Kit | TBD | Ear/tail physics |
| 11 | vHierarchy 2, vFolders 2, vFavorites 2 | TBD | Editor QoL |
| 12 | Asset Inventory 4 | TBD | Asset management |

### MCP Configuration

Both config files needed:
- `.vscode/mcp.json` -- VS Code format, points to Library exe
- `.claude/mcp.json` -- Claude CLI format, `mcpServers` key pointing to Library exe
- Port: check AI Game Developer panel in Unity Editor after MCP install

---

## Phase 2 -- Custom AQS Asset Export

### What to Export from Sandbox

Export `Assets/_Sandbox/_AQS/` as a `.unitypackage` from Sandbox.
**Include everything** -- Unity carries .meta files preserving GUIDs.

Export path: `E:\Unity\Sandbox\Exports\AQS_Export_20260401.unitypackage`

**Contents being exported:**

| Category | Key Items |
|----------|-----------|
| C# Scripts (10) | Core (GameEvent, GameEventListener), Player (QuokkaController, QuokkaInputHandler), Test (6 debug scripts) |
| Animator Controllers (3) | Raccoon AC v2 AQS, AC_Rabbit_Test, AC_Snake_Test |
| State SOs (7) | Rabbit Idle/Locomotion/Fall/JumpBasic, Snake Idle/Locomotion/Fall |
| Prefabs (9) | Scorch, 6 PolyPerfect test animals, 2 weapon prefabs (Belly/Mouth) |
| Scenes (3) | BlankTest, AQS_Greybox, 2.5dMalbersAQS |
| Input Actions (1) | AQS_InputActions.inputactions |
| Animations (3) | Rac_Blink, Rac_Semi, Rac_Closed |
| Materials (2) | Magic Black, RaccoonPA Black |

### Import Target in Standalone

Import to: `Assets/_AQS/` (drop the `_Sandbox` prefix)

### Post-Import Folder Rename

The unitypackage will import with the original path `Assets/_Sandbox/_AQS/`. After import:
1. Move contents from `Assets/_Sandbox/_AQS/` to `Assets/_AQS/`
2. Delete empty `Assets/_Sandbox/` folder
3. Update any hardcoded path references in scripts (if any)

### Post-Import Fixes

- Scripts should compile if all packages are installed (Phase 1)
- Malbers AC state SOs reference Malbers internal assets by GUID -- resolve after Malbers pack import (Phase 3)
- Weapon prefabs reference Malbers Bolt.prefab -- resolve after Malbers import
- Scene objects reference each other by instance ID -- should survive if imported together

---

## Phase 3 -- Third-Party Asset Import

Install from Asset Store cache (no export needed):

| Pack | Status | Notes |
|------|--------|-------|
| Horse Animset Pro 4.5.1 (Malbers) | TBD | Includes Animal Controller. Primary movement system. |
| Poly Art: Raccoon 4.0 (Malbers) | TBD | Raccoon + Raccoon Cub prefabs. |
| Low Poly Animated Animals 4.1.1 (Polyperfect) | TBD | 162 prefabs, 68 species. Enemy/test placeholders. |

### CustomPatch Files

These Malbers source files have custom patches that need to be re-applied after importing:

| File | Patch | Notes |
|------|-------|-------|
| MShootable.cs line 478 | Operator precedence fix | `//CustomPatch:` marker |
| MShootable.cs line 522 | Same fix for MainAttack_Released | `//CustomPatch:` marker |
| MShootable.cs line 565-567 | Direct ReleaseProjectile() for zero delay | `//CustomPatch:` marker |
| MShootable.cs multiple lines | Temp debug logs | `//CustomPatch: temp debug` -- REMOVE these |

---

## Phase 4 -- Verification

### Checklist

- [ ] Project compiles with 0 errors
- [ ] All scripts resolve their dependencies
- [ ] BlankTest scene loads (primary greybox level)
- [ ] Raccoon_Weapon_Test prefab loads without missing references
- [ ] Weapon prefabs (Belly/Mouth) have correct projectile refs
- [ ] Malbers AC state SOs assigned correctly
- [ ] Cinemachine CM_2_5D_Follow camera tracks player
- [ ] LockAxis constrains to 2.5D
- [ ] Mortar weapon fires (stance-gated, arc trajectory)
- [ ] Swim zone triggers
- [ ] Climb zone triggers (tag + physmat + layer)
- [ ] LedgeGrab triggers at top of climb
- [ ] No console errors during play

### Known Re-wiring Required (Inspector)

Scene objects reference each other -- these may need manual re-assignment:

**BlankTest scene:**
- CM_2_5D_Follow tracking target -> Raccoon_Weapon_Test transform
- Water_Volume collider layer -> Water (layer 4)
- Climb walls: tag=Climb + Climbable physmat + BoxCollider on Default layer

---

## File Size Estimate

| Component | Estimated Size |
|-----------|---------------|
| Custom AQS scripts/assets | ~3 MB |
| Malbers Horse Animset Pro + Raccoon | ~400 MB |
| Polyperfect Low Poly Animals | ~200 MB |
| Third-party packages (new installs) | ~2 GB |
| **Total standalone project** | **~3 GB** |

---

## Migration Log

| Date | Phase | Action | Status |
|------|-------|--------|--------|
| Apr 1, 2026 | Planning | Manifest created | Done |
| Apr 1, 2026 | Phase 1 | Standalone project created (6000.3.11f1 URP) | Done |
| Apr 1, 2026 | Phase 1 | User installing minimum packages | In Progress |
| Apr 1, 2026 | Infra | Docs migrated from Sandbox | Done |
| Apr 1, 2026 | Infra | .gitignore created | Done |
| Apr 1, 2026 | Infra | Claude config files created | Done |
| | Phase 1 | manifest.json: TecVooDoo packages, MCP, OpenUPM | Pending |
| | Phase 1 | MCP configs created (.vscode/mcp.json, .claude/mcp.json) | Pending |
| | Phase 1 | Asset Store packages installed (Animancer first) | Pending |
| | Phase 2 | Custom AQS assets exported from Sandbox | Pending |
| | Phase 2 | Assets imported to Assets/_AQS/ | Pending |
| | Phase 3 | Malbers + Polyperfect imported from Asset Store cache | Pending |
| | Phase 3 | CustomPatch re-applied to MShootable.cs | Pending |
| | Phase 4 | Compile, scene load, playtest verification | Pending |

---

**End of Document**
