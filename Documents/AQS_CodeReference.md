# A Quokka Story -- Code Reference

**Purpose:** Script inventory and API reference for AQS. Names + responsibilities, not full API surface (grep the source for that).
**Last Updated:** 2026-05-11
**Version:** 2.0

## Revision History

| Date | Version | Change |
|------|---------|--------|
| 2026-04-01 | 1.0 | Initial pre-migration stub. Listed AQS.Core + AQS.Player + Test only. |
| 2026-05-11 | 2.0 | Refreshed with current scripts on disk -- adds full `AQS.Joey` namespace (9 scripts) and `ClimbStamina`. Reorganized as namespace tree. |

---

## AQS.Core

| Script | Type | Purpose |
|--------|------|---------|
| `GameEvent.cs` | SO | Vanilla SO event channel. Defines `GameEvent`, generic `GameEvent<T>`, `IGameEventListener<T>`, and the concrete `GameEventInt` / `GameEventFloat` typed channels. Raise + subscribe pattern (NOT SOAP). |
| `GameEventListener.cs` | MonoBehaviour | Subscribes a UnityEvent response to a `GameEvent` on enable; unsubscribes on disable. |

## AQS.Player

| Script | Type | Purpose |
|--------|------|---------|
| `QuokkaController.cs` | MonoBehaviour | Original custom hop controller. Mostly superseded by Malbers AC; retained for reference / fallback. |
| `QuokkaInputHandler.cs` | MonoBehaviour | Wires Input System actions to Mom (polling-based, not callback). |
| `ClimbStamina.cs` | sealed MonoBehaviour | Drains Malbers Stamina stat while climbing; forces fall when empty. |

## AQS.Joey

State machine for autonomous companion AI. Joeys follow Mom in an emergent chain, get pouched on proximity, and can be launched as mortar-arc projectiles. See `AQS_Status.md` for design decisions.

| Script | Type | Purpose |
|--------|------|---------|
| `JoeyTypes.cs` | enums | `JoeyState { FollowingInLine, InPouch, Aiming, Launched, Depleted }` + `JoeyRole`. |
| `JoeyDefinition.cs` | SO | Per-archetype data: role, base stats, ability ref, sprite, audio stem assignment. Assets: `Joey_Normal`, `Joey_Lead`. |
| `AbilityDefinition.cs` | SO | Per-ability data (cost, cooldown, drawback). Asset: `Ability_BowlingBall`. |
| `JoeyController.cs` | sealed MonoBehaviour | State machine front-door. Owns the FSM, exposes the public API consumed by `PouchManager`, `JoeyLauncher`, `JoeyBrain`. |
| `JoeyEnergy.cs` | sealed (POCO) | Energy pool with state-based regen (30/s in pouch, 10/s in line, 0 launched). Fires `OnEnergyChanged` for HUD + music. |
| `JoeyBrain.cs` | sealed MonoBehaviour | Autonomous FSM with chain-follow ordering (closest Joey follows Mom; each next follows the one ahead). |
| `JoeyGroundFollower.cs` | sealed MonoBehaviour | Physics-based hop movement along Z, edge/wall detection via raycasts, Mom-dodge placeholder. |
| `PouchManager.cs` | sealed MonoBehaviour | Proximity OverlapSphere scanner. Q/E cycles nearest/farthest pouchable Joey. No owned list -- proximity is the source of truth. |
| `JoeyLauncher.cs` | sealed MonoBehaviour | Hold-to-aim (slow-mo), release-to-fire mortar arc along Z, auto-recall after 3s. |

## AQS.Test (debug + diagnostics, not production)

| Script | Purpose |
|--------|---------|
| `TestModeActivation.cs` | Tests Malbers AC `Mode_TryActivate` via keypress (T). |
| `TestAnimDebug.cs` | Logs animator state changes, layer tags, transitions. |
| `RabbitACDebug.cs` | Logs AC state changes + movement stop timing on rabbit prefab. |
| `SnakeInputDebug.cs` | Logs full input chain (NIS -> PlayerInput -> MInputLink -> MAnimal). |
| `WeaponDebug.cs` | Logs `MWeaponManager` state; forces `MainAttack` via T. |
| `ProjectileTracker.cs` | Tracks projectile trajectory for aim-direction debugging. |

---

## Pending (Planned, Not Yet Created)

| Namespace | Scripts Needed |
|-----------|----------------|
| `AQS.Enemy` | `EnemyController`, `EnemyDefinition` (SO), `EnemySpawner` |
| `AQS.Audio` | `MusicManager` (stem management), `BiomeMusicData` (SO) |
| `AQS.UI` | `GameplayHUD`, `PouchUI`, `MenuController` |
| `AQS.Progression` | `SaveManager`, `CollectibleTracker`, `GateController` |

---

**End of Document**
