# AQS Malbers AC Integration Recipe

**Purpose:** Step-by-step recipe for integrating a Polyperfect animal model with Malbers Animal Controller 4.5.1.
**Derived from:** Sessions 3-5 (Mar 2026), rabbit test build in `Assets/_Sandbox/_AQS/`.
**Last Updated:** March 15, 2026 (Sessions 6-7 -- JumpBasic values confirmed, Step 6 Weapons added)

---

## Context

This recipe was developed by working through the full integration on a Polyperfect rabbit
as a learning exercise. The goal was to understand the system well enough to resolve a
Bolt mode issue on the raccoon (Scorch placeholder). The recipe applies to any Polyperfect
animal being wired to Malbers AC for AQS use.

**Reference files (rabbit test build):**

| File | Purpose |
|------|---------|
| `Assets/_Sandbox/_AQS/Data/AC_Rabbit_Test.controller` | Animator Controller |
| `Assets/_Sandbox/_AQS/Data/States/Rabbit Idle.asset` | Idle state SO |
| `Assets/_Sandbox/_AQS/Data/States/Rabbit Locomotion.asset` | Locomotion state SO |
| `Assets/_Sandbox/_AQS/Data/States/Rabbit Fall.asset` | Fall state SO |
| `Assets/_Sandbox/_AQS/Data/States/Rabbit JumpBasic.asset` | JumpBasic state SO |
| `Assets/_Sandbox/_AQS/Scripts/Test/TestAnimDebug.cs` | Animator debug script |
| `Assets/_Sandbox/_AQS/Scripts/Test/RabbitACDebug.cs` | AC state debug script |

---

## Step 1: Create the Animator Controller

Create a new Animator Controller (not copy from raccoon -- start clean).

### Parameters

Add these parameters in this exact order (Malbers AC requires them by name):

| Name | Type |
|------|------|
| ModeOn | Trigger |
| StateOn | Trigger |
| Stance | Int |
| LastStance | Int |
| State | Int |
| LastState | Int |
| Vertical | Float |
| Horizontal | Float |
| StateStatus | Int |
| StateExitStatus | Int |
| Mode | Int |
| ModeStatus | Int |
| Grounded | Bool |
| Movement | Bool |
| SpeedMultiplier | Float |
| StateFloat | Float |
| Random | Int |
| LookAt | Float |

### Layers

- **States** layer: Override blending, weight=0 (AC drives it), iKPass=off
- **Modes** layer: Override blending, weight=1, iKPass=off. Add one state "Empty" tagged "Empty" with no motion. No transitions needed.

### Sub-State Machines (optional -- flat states also work)

In the States layer, you can either use Sub-State Machines per state group OR flat root
states. Both work with Malbers AC. The sub-SM pattern groups states visually; flat states
are simpler and avoid DefaultState misconfiguration. See Snake-Specific Notes section for
flat state gotchas. AnyState transitions are the same either way.

| Sub-SM Name | Internal State | Tag | Motion |
|-------------|---------------|-----|--------|
| Idles | Idle | Idle | Rabbit_Idle.anim |
| Locomotion | Locomotion | Locomotion | Rabbit_Locomotion_BlendTree (or clip) |
| Fall | Fall | Fall | Rabbit_Fall.anim |
| Jump | Jump | Jump | Rabbit_Jump_Up.anim |

For each sub-SM:
- Set the internal state as the `defaultState`
- Leave Entry transitions empty (defaultState handles routing)
- Add an Exit transition on the internal state: hasExitTime=true, normalizedTime=0.9, duration=0.1

### AnyState Transitions (root level)

Add one AnyState transition per sub-SM. Conditions: `StateOn (If)` + `State (Equals) [ID]`.

| Destination | State ID | canTransitionToSelf | hasExitTime | duration |
|-------------|----------|--------------------|-----------:|--------:|
| Idles sub-SM | 0 | **True** | false | 0 |
| Locomotion sub-SM | 1 | False | false | 0 |
| Fall sub-SM | 3 | False | false | 0 |
| Jump sub-SM | 2 | False | false | 0.1 (normalized) |

**Important:** Idles must have `canTransitionToSelf=True`. The AC fires StateOn+State=0
when returning from Fall to Idle. Without this, the Fall->Idle transition never completes.

---

## Step 2: Create State ScriptableObjects

Copy state SOs from the raccoon reference pack as a starting point:
`Assets/Malbers Animations/Animals Packs/01 Forest Pack/_Forest Pack (AC)/Raccoon AC/2 - Asset/States/`

Rename and move to `Assets/_Sandbox/_AQS/Data/States/`.

### Key settings per state

**Idle** (`StateIDName="Idle"`, Priority=1):
- General.Grounded=true, General.RootMotion=true, General.OrientToGround=true
- ResetLastState=true (important for Idle)
- CanExit=true (set in Activate)

**Locomotion** (`StateIDName="Locomotion"`, Priority=2):
- General.Grounded=true, General.RootMotion=true

**Fall** (`StateIDName="Fall"`, Priority=3):
- General.Grounded=false, General.Gravity=true
- Activates automatically when not grounded and not jumping

**JumpBasic** (`StateIDName="Jump"`, Priority=8):
- Input="Jump"
- General.Grounded=false, General.Gravity=false, General.RootMotion=false
- General.IgnoreLowerStates=true, General.Persistent=true
- See JumpBasic Profile section below

### StateID values (Malbers built-in)

| State | ID |
|-------|----|
| Idle | 0 |
| Locomotion | 1 |
| Jump | 2 |
| Fall | 3 |
| Fly | 6 |

The State integer in the animator must match these IDs exactly.

---

## Step 3: Configure the MAnimal States List

**Critical rule: `states[Count-1]` is always the startup state.**

Malbers uses `var StartStateIndex = states.Count - 1` (MAnimalLogic.cs:361). The last
element in the states list is activated at startup via `CleanStateStart()`, which calls
`Activate()` directly -- bypassing `TryActivate()`. Any state placed last becomes the
startup state regardless of priority or conditions.

**Required ordering:** Sort descending by priority with Idle LAST.

| Index | State | Priority |
|-------|-------|----------|
| [0] | JumpBasic | 8 |
| [1] | Fall | 3 |
| [2] | Locomotion | 2 |
| [3] | Idle | 1 | <- startup state (states[Count-1]) |

**What breaks if this is wrong:** If JumpBasic ends up last, it activates at startup.
Its `General.Modify()` applies `Grounded=false + IgnoreLowerStates=true`, deadlocking
all lower-priority states. The animal appears frozen and never exits Jump.

### Removing raccoon-specific states

When building from a raccoon reference, remove states that don't apply:

| State to Remove | Reason |
|-----------------|--------|
| Raccoon Death (ID=10) | No death state needed in test build |
| Raccoon LedgeGrab (ID=8) | No ledge grab |
| Raccoon Climb (ID=7) | No climb |
| Raccoon Swim (ID=4) | No swim |
| Raccoon Jump RM (ID=2) | Raccoon uses Root Motion Jump; rabbit uses Basic Jump |

---

## Step 4: JumpBasic Profile

The JumpBasic SO contains a `profiles` list (usually one "Default" profile). Key fields:

| Field | Value | Notes |
|-------|-------|-------|
| Height | 2.4 | Units of upward force per second during JumpTime. Tune to taste. |
| GravityPower | 1 | Multiplier for downward gravity during jump. **Must be > 0** or rabbit never falls. |
| JumpTime | 0.33s | Duration of the jump logic window before forcing Fall state. |
| StartGravityTime | 15 | Gravity accumulation frame offset at jump start. Higher = more arcade hang-at-peak. |
| AirControl | false | Disable mid-air direction change for basic jump. |
| WaitForAnimation | false | Activate jump physics immediately (don't wait for animator event). |

**GravityPower=0 is the default in the raccoon asset but it is wrong for BasicJump.**
The raccoon uses Jump RM (Root Motion Jump), not JumpBasic, so the raccoon's JumpBasic
asset values are untested defaults. Always set GravityPower >= 1 for a JumpBasic state.

### SleepFromState for JumpBasic

Set SleepFromState to Fall (ID=3) and Fly (ID=6). This prevents double-jumping while
already falling or flying. Do NOT include Idle or Locomotion -- those must allow jumping.

---

## Step 5: Wire Input

Malbers input is handled by `MInputLink [New Input System]` component on the animal.
The Jump input entry needs:
- Name: "Jump"
- Input Action Reference: `Gameplay/Jump`
- Interaction: Press

The JumpBasic SO's `Input` field must match this name exactly: "Jump".

---

## Known Limitations and Visual Notes

### Standing jump looks like Idle animation

**Behavior:** When jumping from a standing position, the rabbit appears to stay in the
Idle pose while moving up and down. The jump animation IS playing (confirmed by debug
logs showing `STATE CHANGE -> Jump`).

**Cause:** In-place animations start from a standing pose. Without forward momentum,
the visual difference between Idle and Jump_Up is subtle. The blend transition
(0.1 normalized) blends from Idle into Jump, further masking the pose change.
Running -> Jump is visually clear because the pose contrast is dramatic.

**Status:** Known limitation. Not a bug. For production, use a more exaggerated jump
animation or a separate Jump_Idle clip if available.

### Jump state transition timing

The full Idle->Jump->Fall->Idle cycle completes in under 100ms (3-5 frames at 60fps).
Debug timestamps showing `t=+0.000s` on all three transitions from a standing jump are
expected -- RabbitACDebug only tracks elapsed time since movement stop. A standing jump
never triggers movement detection, so its timer reads 0 throughout.

---

## Debug Scripts

Two test scripts live in `Assets/_Sandbox/_AQS/Scripts/Test/`:

**TestAnimDebug.cs** (attach to animal root):
- Tracks animator layer 0/1 tags, normalizedTime, transition state
- Logs all animator state changes (hash change detection)
- After movement input release, logs every 5 frames for 2 seconds
- Monitors: ACState, MovementDetected, State/Movement/Vertical params

**RabbitACDebug.cs** (attach to animal root):
- Logs detailed AC state on every state ID change
- Tracks movement stop -> Idle handoff timing
- Can force State_Force(0) after configurable delay for isolation testing
- Log every frame toggle via Inspector (`Log Every Frame`)

---

## Troubleshooting

| Symptom | Cause | Fix |
|---------|-------|-----|
| Animal starts in Jump state (ACState=2) | JumpBasic is last in states list | Reorder: Idle must be last |
| Jump activates but animal never falls | GravityPower=0 on JumpBasic profile | Set GravityPower=1 |
| Jump input does nothing from Idle | JumpBasic sleeping from current state | Check SleepFromState list |
| Animator stays in Idle during jump | AnyState->Jump sub-SM not wired | Check StateOn+State==2 conditions |
| All state changes show t=+0.000s | Animal never moved before jump | Expected -- RabbitACDebug timer only starts on movement stop |
| Fall->Idle never completes | Idles AnyState canTransitionToSelf=False | Must be True for re-entry from Fall |

---

## Step 6: Adding MWeaponManager (Mouth Weapon -- Work In Progress)

**Status:** Partial. Weapon equips to jaw anchor, projectile assigned. Firing blocked -- see blocker below.

### What Was Set Up (Sessions 6-7)

**Bone anchor:**
- `Mouth_WeaponPoint` -- child of `Jaw` bone, localPosition=(0,0,0)
- `Mouth_FirePoint` -- child of Mouth_WeaponPoint, localPosition=(0,0,0.15) [needs in-editor tuning for forward direction]

**Weapon prefab (`Assets/_Sandbox/_AQS/Prefabs/Weapons/Mouth_Weapon_Test.prefab`):**
- Copied from `Malbers Animations/Common/Prefabs/Weapons/Pistol.prefab`
- `MShootable.HasFireAnim = false` (bypasses fire animation/Mode for fire)
- `MShootable.releaseByAnimation = false`
- `MShootable.aimAction = Ignore`
- `MShootable.equipProjectile = OnAttackStart` (changed from OnEquip -- OnEquip doesn't re-equip per shot)
- `MShootable.m_Projectile = Wolf Lite/Bolt.prefab`
- `MShootable.m_Ammo = -1` (infinite ammo for testing)

**MWeaponManager on Rabbit_AC_Test root:**
- `m_IgnoreDraw = true`, `m_IgnoreStore = true`
- `UseHolsters = false`, `IgnoreHandOffset = true`
- `RightHandEquipPoint = LeftHandEquipPoint = Mouth_WeaponPoint`
- `m_MainAttack = "Action1"` (LMB / gamepad left trigger via Malbers Inputs asset)
- `startWeapon = Mouth_Weapon_Test` (auto-equips on Start)
- `m_CombatLayerPath = ""` (cleared -- no Combat2 overlay needed)
- `DrawWeaponModeID = StoreWeaponModeID = null`

**MAnimal Mode requirement:**
- MWeaponManager.EquipWeapon_AnimalController() calls `animal.Mode_Get(Weapon.WeaponType)` to find a matching Mode
- Weapon's `weaponType = Pistol (WeaponID)`. WeaponID inherits from ModeID.
- Added a Mode to MAnimal with `ID = Pistol` (the WeaponID SO at `Common/Scriptable Assets/IDs/Weapons/Pistol.asset`)
- Without this Mode, weapon is disabled at equip time (`Weapon.Active = false`)

**CG hierarchy must be enabled:**
- `Rabbit_AC_Test/CG` must be active. Mouth_WeaponPoint lives under `CG/.../Jaw`.
- If CG is disabled, everything under it has `activeInHierarchy=false`, and `ReleaseProjectile()` returns immediately.

### Malbers Source Bugs Found (Session 8)

Three bugs discovered during integration. All patched with `//CustomPatch:` comments.

**Bug 1: MShootable operator precedence (lines 478, 522)**
```
// BROKEN: (A && B) || C -- CanShootWithAimLimit blocks even when aim is Ignored
if (aimAction != AimingAction.Ignore && !IsAiming || !CanShootWithAimLimit) return;
// FIXED: A && (B || C) -- Ignore bypasses both aim checks
if (aimAction != AimingAction.Ignore && (!IsAiming || !CanShootWithAimLimit)) return;
```

**Bug 2: Delay_Action with delay=0 never fires callback (line 565)**
`this.Delay_Action(ref iReleaseDelay, 0, () => ReleaseProjectile())` -- callback never executes.
Fix: call `ReleaseProjectile()` directly when `ReleaseDelay <= 0`.

**Bug 3: CG hierarchy disabled breaks weapon activeInHierarchy**
Not a Malbers bug per se, but a setup gotcha: if the bone hierarchy parent is disabled, the weapon's `gameObject.activeInHierarchy` is false and `ReleaseProjectile()` bails at its first line.

### Troubleshooting

| Symptom | Cause | Fix |
|---------|-------|-----|
| "does not have a mode for the Equipped Weapon" | No Mode on MAnimal matching weapon's WeaponType | Add Mode with ID = weapon's WeaponID (e.g. Pistol) |
| Weapon fires nothing (CanAttack stays True) | `aimAction=Ignore` but `CanShootWithAimLimit=False` blocks | Apply operator precedence fix to MShootable.cs lines 478/522 |
| Weapon fires nothing (CanAttack goes False) | `Delay_Action` with delay=0 never calls `ReleaseProjectile` | Call `ReleaseProjectile()` directly when delay<=0 |
| Weapon fires nothing (activeInHierarchy=False) | CG or parent bone hierarchy disabled | Enable CG on the animal |
| "Holster does not exit on list" warning | UseHolsters=false but holster ID still referenced | Harmless -- ignore |
| Bolt fires into ground | Aim Point forward direction != world forward | Rotate Aim Point transform so +Z faces desired fire direction |
| LMB doesn't fire (T key works) | MInputLink ConnectInput routing issue | Under investigation -- use WeaponDebug T key workaround |

---

## Snake-Specific Notes (Sessions 9-10, Mar 2026) -- EXPERIMENT PARKED

Experiment to apply this recipe to Polyperfect Snake_Yellow2 (generic rig, 28-bone spine,
no legs) was parked after hitting an unresolved IsPending initialization bug. The snake
was removed from the scene. Key finding: **start from a working Malbers demo prefab and
swap clips/model, rather than building MAnimal from scratch on a blank GameObject.**

The IsPending bug: MAnimal's `CacheAnimatorState()` uses a fullPathHash comparison to
detect animator state changes. When building from scratch, the initialization sequence
can cause the hash cache to match before the first tag detection fires, leaving
`AnimStateTag=0` permanently. This prevents IsPending from ever clearing, which blocks
all state transitions. The rabbit (built differently) does not have this issue.

Original gotchas discovered (still valid for reference):

### No Jump state
Snakes don't jump. Only Idle (ID=0), Locomotion (ID=1), Fall (ID=3) are needed. States
list ordering: Fall[0], Locomotion[1], Idle[2] (Idle must remain last = startup state).

### Flat root states vs sub-state machines
The recipe's Step 1 describes sub-state machines (Idles, Locomotion, Fall sub-SMs). For
snakes (and possibly other models), **flat root states work equally well.** The AnyState
-> flat state transitions use the same StateOn+State conditions. If the sub-SM pattern
produces "Animator is not playing an AnimatorController" errors, rebuild with flat states
at root level.

Root cause of the sub-SM failure: if `DefaultState` gets set to point INTO a sub-SM
rather than the sub-SM's internal state, Unity can't resolve the starting state and throws
the error. Flat states avoid this ambiguity entirely.

### Mesh child Y=180 rotation
Polyperfect snake model has a mesh child named "snake" with LocalRotation Y=180°. This
makes the mesh face -Z (backward), while MAnimal expects +Z as forward. Symptom: snake
spins in place on any movement input, never translates forward.

**Fix:** Set the mesh child's localEulerAngles to (0,0,0) before running Play. Check any
Polyperfect mesh child's local rotation before wiring AC -- they aren't always zeroed.

### Copy-paste MAnimal contamination
If you copy the MAnimal component from an existing animal (e.g., rabbit) to start faster,
all state SOs and mode SOs transfer with it. You'll get:
- Rabbit/raccoon state ScriptableObjects in the states list instead of your new animal's
- Jump state (ID=2, Priority=8) if the source had it -- must remove for non-jumping animals
- Weapon modes (Pistol, etc.) if source had MWeaponManager set up

Always audit the states list and modes list after a component copy. Remove anything that
doesn't apply to the target animal.

### SnakeInputDebug script
`Assets/_Sandbox/_AQS/Scripts/Test/SnakeInputDebug.cs` -- equivalent of RabbitACDebug for
the snake. Logs NewInputSystem key state, OldInputSystem axes, PlayerInput Move value, and
MAnimal state changes. Press Tab (or configured dumpKey) for a full snapshot.

**Known issue:** In Unity Editor, New Input System only receives keyboard when Game View
has focus. If SnakeInputDebug shows W=False while pressing W, click inside Game View first.

### Troubleshooting (snake-specific)

| Symptom | Cause | Fix |
|---------|-------|-----|
| "Animator is not playing an AnimatorController" | DefaultState points into sub-SM | Rebuild with flat root states |
| Snake spins in place, no forward movement | Mesh child LocalRotation Y=180° | Zero the mesh child's localEulerAngles |
| Wrong animations (rabbit clips playing) | MAnimal copy-paste brought rabbit state SOs | Reassign state SOs to snake-specific assets |
| Jump state activates at startup | Jump state copied from rabbit, is last in list | Remove Jump state from states list |
| W=False in SnakeInputDebug | Game View doesn't have focus | Click inside Game View before pressing keys |

---

**End of Document**
