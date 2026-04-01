# A Quokka Story -- Migration Manifest

**Purpose:** Step-by-step plan for migrating AQS from Sandbox subproject to standalone Unity project.
**Source:** `E:\Unity\Sandbox` -- subproject at `Assets/_Sandbox/_AQS/`
**Target:** `E:\Unity\AQuokkaStory` -- standalone Unity 6 (6000.3.11f1) URP project
**Status:** MIGRATION COMPLETE
**Created:** April 1, 2026

---

## Overview

A Quokka Story was developed as a subproject inside Sandbox (Sessions 0-14). The standalone project at `E:\Unity\AQuokkaStory` is being set up for active development. Migration is happening earlier than planned (Sprint 1 vs Sprint 5) because Sandbox lag from eval bloat is impacting dev velocity.

Migration breaks into four phases:
1. Set up the standalone project (packages, UPM, settings) -- **COMPLETE**
2. Export + import custom AQS assets -- **COMPLETE**
3. Import third-party assets (Malbers, Polyperfect, etc.) -- **COMPLETE (CustomPatch pending)**
4. Verify, fix broken references, confirm scene plays -- **COMPLETE**

---

## Phase 1 -- Standalone Project Setup

User is installing minimum packages manually. New approach: only install what this project actually needs (not the old "default" label set).

### UPM / Registry Packages (manifest.json)

| Package | Status | Notes |
|---------|--------|-------|
| Input System | Done | Already in manifest |
| URP | Done | Project created with URP |
| Cinemachine | Done | Via characters-animation feature |
| AI Navigation | Done | 2.0.12 |
| ProBuilder | Done | Via worldbuilding feature |
| Splines | Done | Via worldbuilding feature |
| Timeline | Done | Already in manifest |
| Addressables | Done | 2.9.1 |
| TextMesh Pro | Done | Via ugui |

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
| UniTask (`com.cysharp.unitask`) | Done | 2.5.10 |
| MCP for Unity (`com.ivanmurzak.unity.mcp`) | Done | 0.63.3 |
| MCP Animation (`com.ivanmurzak.unity.mcp.animation`) | Done | 1.1.22 |
| MCP ProBuilder (`com.ivanmurzak.unity.mcp.probuilder`) | Done | 1.0.61 |

**DO NOT install:** `com.ivanmurzak.unity.mcp.particlesystem` -- CS0117 errors.

### Local File Packages

| Package | Status | Notes |
|---------|--------|-------|
| com.tecvoodoo.utilities | Done | `file:../../DefaultUnityPackages/com.tecvoodoo.utilities` |
| com.tecvoodoo.mcp-tools | Done | `file:../../DefaultUnityPackages/com.tecvoodoo.mcp-tools` |
| com.tecvoodoo.games | Done | `file:../../DefaultUnityPackages/com.tecvoodoo.games` |

### Asset Store Packages -- Default Set (new standard)

| Package | Status | Notes |
|---------|--------|-------|
| Odin Inspector and Serializer 4.0.1.4 | Done | Never remove once installed |
| DOTween Pro 1.0.410 | Done | |
| Easy Save 3.5.25 | Done | Save/load |
| Master Audio 2024 1.0.4 | Done | |
| ALINE 1.7.9 | Done | Debug visualization |
| vHierarchy 2 (2.1.8), vFolders 2 (2.1.14), vFavorites 2 (2.0.14) | Done | Editor QoL |
| Asset Inventory 4 (4.1.1) | Done | Asset management |
| Flexalon Pro 4.4.0 | Done | 3D & UI Layouts |
| Text Animator 3.5.0 | Done | UI Toolkit + TMP |
| Wingman 1.3.0 | Done | Inspector tool |
| Ultimate Preview Window 1.3.2 | Done | Editor preview |
| Audio Preview Tool 1.1.0 | Done | Audio preview |
| Markdown for Unity 1.0.0 | Done | Markdown rendering |

### Asset Store Packages -- AQS-Specific (install in this order)

**Animancer MUST be first before any FBX art:**

| # | Package | Status | Notes |
|---|---------|--------|-------|
| 1 | Animancer Pro v8 (8.3.0) | Done | ENTRY-012 |
| 2 | Feel | Done | ENTRY-015 |
| 3 | Damage Numbers Pro | Done | Hit feedback |
| 4 | EasyPooling 2025 | Done | Object pooling |
| 5 | Boing Kit | Done | Ear/tail physics |

### MCP Configuration -- Done

Both config files created:
- `.vscode/mcp.json` -- VS Code format, points to Library exe
- `.claude/mcp.json` -- Claude CLI format, `mcpServers` key pointing to Library exe

---

## Phase 2 -- Custom AQS Asset Export

### What to Export from Sandbox

Export `Assets/_Sandbox/_AQS/` as a `.unitypackage` from Sandbox.
**Include everything** -- Unity carries .meta files preserving GUIDs.

Export path: `E:\Unity\Sandbox\Exports\AQS_Export_20260401.unitypackage`

**What was imported (trimmed from original plan -- only working raccoon setup):**

| Category | Key Items |
|----------|-----------|
| C# Scripts (10) | Core (GameEvent, GameEventListener), Player (QuokkaController, QuokkaInputHandler), Test (6 debug scripts) |
| Animator Controllers (3) | Raccoon AC v2 AQS, AC_Rabbit_Test, AC_Snake_Test |
| State SOs (7) | Rabbit Idle/Locomotion/Fall/JumpBasic, Snake Idle/Locomotion/Fall |
| Prefabs (8) | Raccoon_Weapon_Test, 5 PolyPerfect test animals, 2 weapon prefabs (Belly/Mouth) |
| Scene (1) | BlankTest |
| Input Actions (1) | AQS_InputActions.inputactions |

**Not imported (intentionally dropped):**
- Scenes: AQS_Greybox, 2.5dMalbersAQS (older test scenes)
- Animations: Rac_Blink, Rac_Semi, Rac_Closed
- Materials: Magic Black, RaccoonPA Black
- Prefab: Scorch (HOK placeholder -- no longer needed)

### Import Result

Imported directly to `Assets/_AQS/` (no folder rename needed).
All scripts compile with 0 errors. Only console output is MCP reconnection noise (expected during setup).

---

## Phase 3 -- Third-Party Asset Import

Install from Asset Store cache (no export needed):

| Pack | Status | Notes |
|------|--------|-------|
| Horse Animset Pro 4.5.1 (Malbers) | Done | Includes Animal Controller. Primary movement system. |
| Poly Art: Raccoon 4.0 (Malbers) | Done | Raccoon + Raccoon Cub prefabs. |
| Low Poly Animated Animals 4.1.1 (Polyperfect) | Done | 162 prefabs, 68 species. Enemy/test placeholders. |

### CustomPatch Files

These Malbers source files have custom patches that need to be re-applied after importing:

| File | Patch | Notes |
|------|-------|-------|
| MShootable.cs line 478 | Operator precedence fix | `//CustomPatch:` marker |
| MShootable.cs line 522 | Same fix for MainAttack_Released | `//CustomPatch:` marker |
| MShootable.cs line 565-567 | Direct ReleaseProjectile() for zero delay | `//CustomPatch:` marker |
| MShootable.cs multiple lines | Temp debug logs | REMOVED -- cleaned up during migration |
| Ammo Pistol.asset | Set value to -1 (infinite) | Malbers default is 32 |
| Ammo Pistol in Chamber.asset | Set value to -1 (infinite) | Malbers default is 0 -- weapon won't fire without this |

---

## Phase 4 -- Verification

### Checklist

- [x] Project compiles with 0 errors
- [x] All scripts resolve their dependencies
- [x] BlankTest scene loads (primary greybox level)
- [x] Raccoon_Weapon_Test prefab loads without missing references
- [x] Malbers AC state SOs assigned correctly (8 states, 5 modes, 6 stances)
- [x] Cinemachine CM_2_5D_Follow camera tracks player (re-wired CM Main Target)
- [x] Mortar weapon fires
- [x] Swim zone triggers
- [x] Climb zone triggers (tag=Climb confirmed)
- [ ] LedgeGrab triggers at top of climb -- not tested
- [ ] No console errors during play -- MCP reconnection noise only

### Re-wiring Done

- CM_2_5D_Follow tracking target -> CM Main Target (re-assigned, camera distance slightly farther than before)

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
| Apr 1, 2026 | Phase 1 | User installing minimum packages | Done |
| Apr 1, 2026 | Infra | Docs migrated from Sandbox | Done |
| Apr 1, 2026 | Infra | .gitignore created | Done |
| Apr 1, 2026 | Infra | Claude config files created | Done |
| Apr 1, 2026 | Phase 1 | manifest.json: OpenUPM registry, UniTask, MCP packages, mcp-tools | Done |
| Apr 1, 2026 | Phase 1 | MCP configs (.vscode/mcp.json, .claude/mcp.json) | Done |
| Apr 1, 2026 | Phase 1 | Default Asset Store set installed (13 packages) | Done |
| Apr 1, 2026 | Phase 1 | AQS-specific Asset Store packages (Animancer, Feel, DNP, EasyPooling, Boing Kit) | Done |
| Apr 1, 2026 | Phase 2 | Custom AQS assets imported to Assets/_AQS/ (trimmed set -- raccoon + scripts) | Done |
| Apr 1, 2026 | Phase 2 | 0 compile errors confirmed | Done |
| Apr 1, 2026 | Phase 3 | Malbers Horse Animset Pro + Poly Art Raccoon + Polyperfect Animals installed | Done |
| | Phase 2 | Custom AQS assets exported from Sandbox | Pending |
| | Phase 2 | Assets imported to Assets/_AQS/ | Pending |
| | Phase 3 | Malbers + Polyperfect imported from Asset Store cache | Pending |
| Apr 1, 2026 | Phase 4 | BlankTest scene verified -- weapon, swim, climb all working | Done |
| Apr 1, 2026 | Phase 4 | CM_2_5D_Follow re-wired to CM Main Target | Done |
| Apr 1, 2026 | Phase 3 | CustomPatches verified intact, temp debug logs removed (15 lines) | Done |

---

**End of Document**
