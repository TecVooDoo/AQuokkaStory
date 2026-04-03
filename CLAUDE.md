# A Quokka Story -- Claude Instructions

## Project

- **Game:** A Quokka Story -- 2.5D Metroidvania Platformer
- **Engine:** Unity 6 (6000.3.11f1), URP
- **Path:** `E:\Unity\AQuokkaStory`
- **Root:** `Assets/_AQS/`
- **Developer:** TecVooDoo LLC / Rune (Stephen Brandon)

## Docs -- Read These First

| Question | Read This |
|----------|-----------|
| Where are we at? | `Documents/AQS_Status.md` |
| Architecture, standards? | `Documents/AQS_DevReference.md` |
| Script API? | `Documents/AQS_CodeReference.md` |
| Malbers AC recipe? | `Documents/AQS_MalbersRecipe.md` |
| Game design? | `Documents/GDD/AQS_GDD.md` (user's doc -- update only when asked) |
| Migration plan? | `Documents/AQS_Migration_Manifest.md` |
| Asset evals? | `E:\Unity\Sandbox\Documents\Sandbox_AssetLog.md` (Sandbox is single source) |

## Coding Standards

- **No `var`** -- explicit types always
- **No per-frame allocations/LINQ** -- cache, pool, reuse
- **ASCII only** in docs and identifiers
- **sealed** on MonoBehaviours unless inheritance intended
- **Prefer async/await (UniTask)** over coroutines
- **Prefer interfaces and generics** -- decouple systems, reduce duplication
- **Vanilla SO architecture** -- GameEvent/GameEventListener for events (NOT SOAP)
- **Malbers Animal Controller** for player movement (MAnimal + LockAxis for 2.5D)
- **3D physics for 2.5D** -- Rigidbody + CapsuleCollider. **Z = lateral movement, X = depth (locked), Y = up.** Camera looks down -X. Freeze X position on Joeys, Mom uses LockAxis.
- **Collision-based ground detection** -- NOT raycasts
- **Unity 6 API** -- `rb.linearVelocity` not `velocity`
- **Extract by responsibility** not line count
- **Production-quality test code** even during prototyping

## Key Rules

- **GDD is user's doc** -- update only when asked
- **Stem music is core identity** -- every character/enemy needs an instrument assignment
- **Joey abilities always have drawbacks** -- no dominant strategies
- **Build Malbers AC from prefab** -- never build MAnimal from scratch on blank GameObjects
- **Asset evals live in Sandbox** -- don't create eval docs here
- **NEVER assume APIs** -- read actual source before writing code. Verify every method/property name.

## MCP

- Unity MCP via `com.ivanmurzak.unity.mcp` (OpenUPM). Port in `.claude/mcp.json` -- update after MCP install.
- Custom tools via `com.tecvoodoo.mcp-tools` (local UPM package)
- `script-execute` is the power tool (Roslyn). C# `<>` gets HTML-encoded -- use `typeof()` casts.
- MCP disconnects during domain reload -- wait for auto-reconnect.

## Critical Gotchas

- **Animancer Pro** -- install BEFORE 3D art assets (FBX catalog scan)
- **Master Audio 2024** -- install Addressables FIRST
- **DOTween Pro** -- install TextMesh Pro FIRST
- **Odin Inspector** -- never remove from installed project
- **UPM Git URLs don't work** on this machine -- clone repo, use `file:` reference
- **Malbers MShootable.cs** has 3 custom patches (operator precedence, zero-delay, CG hierarchy). Re-apply after Malbers updates. Markers: `//CustomPatch:`
- **Random ambiguity** -- `using Unity.Mathematics;` + `using UnityEngine;` = CS0104. Fix: `using Random = UnityEngine.Random;`
