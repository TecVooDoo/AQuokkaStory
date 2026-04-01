# A Quokka Story -- Code Reference

**Purpose:** Script inventory and API reference for AQS.
**Last Updated:** April 1, 2026 (Session 15 -- Migration, pre-import)

> **NOTE:** Scripts are being migrated from Sandbox. This reference will be populated after Phase 2 import.

---

## Script Inventory (Pre-Migration from Sandbox)

### AQS.Core

| Script | Purpose |
|--------|---------|
| `GameEvent.cs` | ScriptableObject event channel (raise/listen pattern) |
| `GameEventListener.cs` | MonoBehaviour that subscribes to GameEvent and invokes UnityEvent |

### AQS.Player

| Script | Purpose |
|--------|---------|
| `QuokkaController.cs` | Player movement (original custom controller, mostly superseded by Malbers AC) |
| `QuokkaInputHandler.cs` | Wires Input System to controller (polling-based, not callbacks) |

### Test Scripts

| Script | Purpose |
|--------|---------|
| `TestModeActivation.cs` | Tests Malbers AC Mode_TryActivate via keypress (T key) |
| `TestAnimDebug.cs` | Logs animator state changes, layer tags, transitions |
| `RabbitACDebug.cs` | Logs AC state changes, movement stop timing |
| `SnakeInputDebug.cs` | Logs input chain (NIS -> PlayerInput -> MInputLink -> MAnimal) |
| `WeaponDebug.cs` | Logs MWeaponManager state, force MainAttack via T key |
| `ProjectileTracker.cs` | Tracks projectile trajectory for debugging aim direction |

---

## Pending (Not Yet Created)

| System | Scripts Needed |
|--------|---------------|
| Joey | JoeyController, JoeyDefinition (SO), AbilityDefinition (SO), PouchManager |
| Enemy | EnemyController, EnemyDefinition (SO), EnemySpawner |
| Audio | MusicManager (stem management), BiomeMusicData (SO) |
| UI | GameplayHUD, PouchUI, MenuController |
| Progression | SaveManager, CollectibleTracker, GateController |

---

**End of Document**
