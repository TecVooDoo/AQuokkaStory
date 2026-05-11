# A Quokka Story -- Claude Instructions

## Project

- **Game:** A Quokka Story -- 2.5D Metroidvania Platformer
- **Engine:** Unity 6 (6000.3.15f1 on this recovery pass), URP
- **Path:** `E:\Unity\AQuokkaStory`
- **AQS root:** `Assets/_AQS/`
- **Branch:** `master` (not `main`). Remote: `https://github.com/TecVooDoo/AQuokkaStory`
- **Developer:** TecVooDoo LLC / Rune (Stephen Brandon)

## Docs -- Read in This Order

| Question | Read This |
|----------|-----------|
| Where are we at? Session bookends? | `Documents/AQS_Status.md` (primary) + `Canonical/UniversalWorkflow.md` |
| AQS-specific architecture + deltas? | `Documents/AQS_DevReference.md` |
| Script API? | `Documents/AQS_CodeReference.md` |
| Malbers AC recipe? | `Documents/AQS_MalbersRecipe.md` |
| Game design? | `Documents/GDD/AQS_GDD.md` (user's doc -- update only when asked) |
| Asset evals? | `E:\Unity\Sandbox\Documents\Sandbox_AssetLog.md` (Sandbox is the only source) |

## Canonical Layer (single source of truth, read on demand)

- **Coding standards:** `E:\Unity\Sandbox\Documents\Canonical\TecVooDoo_CodingStandards.md`
- **Workflow (bookends, commit ownership, refactor philosophy, gotchas, user preferences):** `E:\Unity\Sandbox\Documents\Canonical\UniversalWorkflow.md`
- **MCP install + upgrade recipes:** `E:\Unity\Sandbox\Documents\Canonical\MCP_ConnectionBrief.md`

Per the canonical commit-ownership rule, this AQS session may **edit** any file but only **commits + pushes** from `E:\Unity\AQuokkaStory`. Canonical edits stay in the Sandbox working tree for a Sandbox session to push.

## AQS-Specific Rules (everything else is in the canonicals)

- **2.5D axis convention:** Z = lateral movement, X = depth (locked), Y = up. Camera looks down -X. Freeze X on Joeys, Mom uses Malbers `LockAxis`.
- **3D physics for 2.5D**, never `Rigidbody2D`. Unity 6 API -- `rb.linearVelocity` not `velocity`.
- **Collision-based ground detection** -- never raycast.
- **GDD is user-owned** -- update only when asked.
- **Stem music is core identity** -- every character/enemy needs an instrument assignment.
- **Joey abilities always have drawbacks** -- no dominant strategies.
- **Build Malbers AC from a working prefab** -- never from a blank GameObject.
- **Asset evals live in Sandbox** -- don't create eval docs here.
- **Never assume APIs** -- read source before writing code. Verify every method/property name.

## MCP

- Unity MCP via `com.ivanmurzak.unity.mcp` (OpenUPM). AQS HTTP port: **25675**. Transport: `streamableHttp`.
- Custom tools via `com.tecvoodoo.mcp-tools` (local UPM at `E:/Unity/DefaultUnityPackages/com.tecvoodoo.mcp-tools`).
- `.claude/mcp.json` may go stale after Reconfigure -- hand-edit to match `.mcp.json` if needed.
- `script-execute` is the Roslyn power tool. C# `<>` gets HTML-encoded -- use `typeof()` casts.
- MCP disconnects during domain reload -- wait for auto-reconnect.
- Recovery upgrade target on this pass: MCP 0.66.1 -> 0.72.0. Follow the recipe in `Canonical/MCP_ConnectionBrief.md`.

## Critical Gotchas (project-specific)

- **Animancer Pro** -- install BEFORE 3D art assets (FBX catalog scan).
- **Master Audio 2024** -- install Addressables FIRST.
- **DOTween Pro** -- install TextMesh Pro FIRST.
- **Odin Inspector** -- never remove once installed.
- **UPM Git URLs don't work** on this machine -- clone repo, use `file:` reference.
- **Malbers MShootable.cs** has 3 custom patches (operator precedence, zero-delay, CG hierarchy). Re-apply after Malbers updates. Markers: `//CustomPatch:`.
- **Random ambiguity** -- `using Unity.Mathematics;` + `using UnityEngine;` => CS0104. Fix: `using Random = UnityEngine.Random;`.
