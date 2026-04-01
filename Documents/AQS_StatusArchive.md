# A Quokka Story -- Status Archive

**Project:** A Quokka Story (2.5D Metroidvania Platformer)

**Archive of older session entries from AQS_Status.md. Newest first.**

---

## Session 11 (Mar 28, 2026) -- Raccoon Belly Weapon Setup

Raccoon_Weapon_Test instantiated from Raccoon PA Player prefab. Belly weapon system wired:
- `Belly_WeaponPoint` on Spine1 bone, `Belly_FirePoint` child (0.15 forward offset)
- `MWeaponManager` added: IgnoreDraw/Store=true, equip points=Belly_WeaponPoint, startWeapon=Belly_Weapon_Test
- `Belly_Weapon_Test.prefab` created from Pistol.prefab: HasFireAnim=false, releaseByAnimation=false, aimAction=Ignore, Ammo=-1, Projectile=Bolt, equipProjectile=OnAttackStart
- Pistol mode (ID=300) added to MAnimal -- weapon equips successfully (gun cocking sound on Play)
- **LMB routing FIXED.** Three issues resolved: (1) `m_MainAttack` must be "Action1" not "MainAttack" to match MInputLink input name, (2) Attack1 mode added to `DisableModes` list so melee doesn't intercept, (3) `aimAction` must be `Ignore` (enum=2, not 0). Also fixed: `equipProjectile` must be `OnAttackStart` (flag=4, not 1=ByAnimation).
- **Bolt fires but direction needs tuning.** Projectile spawns and fires along Spine1 +Z axis. Bolt is invisible because it fires toward the tail (-Z world) or into the ground. Aim Point rotation and Belly_WeaponPoint position need in-editor tuning. ProjectileTracker debug script added for trajectory monitoring.

## Session 10 (Mar 21, 2026) -- Snake AC Experiment PARKED

Snake AC experiment parked after extensive debugging. Snake_AC_Test removed from scene. The snake (as a potential AQS enemy) doesn't need Malbers AC -- simple AI (Behavior Designer + Sensor Toolkit + A*) with a basic Animator Controller is sufficient for enemies. Malbers AC is reserved for the player character (quokka/raccoon).

**Key findings from snake experiment (Sessions 9-10):**
- Input chain works perfectly: NIS keyboard -> PlayerInput -> MInputLink -> MAnimal (MovDet=True, Axis values correct)
- **IsPending=True blocker:** MAnimal's `AnimStateTag` (internal tag hash tracker) never updated from 0 despite the animator correctly playing tagged states. Root cause: `CacheAnimatorState()` in `OnAnimalMove` compares `m_CurrentState.fullPathHash` against cached `AnimState.fullPathHash` -- if they match, AnimStateTag is not updated. On the snake, the initialization sequence caused the hashes to match before the first real tag detection could fire, leaving IsPending permanently stuck.
- This is likely a Malbers AC initialization timing issue specific to animals built from scratch (vs. using Malbers' pre-built prefabs which have correct runtime state).
- **Recommendation for future AC builds:** Start from a working Malbers demo prefab and swap the model/clips, rather than building MAnimal from scratch on a blank GameObject.

---

## Session 8 (Mar 16, 2026) -- COMPLETE

**Status:** Mouth weapon FIRES. Bolt projectile spawns from Jaw area, fires into ground (direction needs tuning). Three Malbers source bugs found and patched.

**Session 8 Work:**

*Weapon Fix -- Three Bugs Found:*
1. **AC Mode mismatch:** MAnimal had no Mode matching `weaponType=Pistol`. Added Pistol Mode to MAnimal (WeaponID inherits from ModeID). `EquipWeapon_AnimalController()` now finds WeaponMode.
2. **MShootable operator precedence bug (CustomPatch):** Line 477/520: `if (aimAction != Ignore && !IsAiming || !CanShootWithAimLimit)` evaluates as `(A && B) || C` not `A && (B || C)`. Even with `aimAction=Ignore`, `CanShootWithAimLimit` check still blocks. Fixed with parentheses. Affects `MainAttack_Start` and `MainAttack_Released`.
3. **`Delay_Action` with delay=0 never fires callback:** `FireAnim_ReleaseProjectile()` calls `this.Delay_Action(ref iReleaseDelay, 0, () => ReleaseProjectile())` but the callback never executes. Fixed by calling `ReleaseProjectile()` directly when `ReleaseDelay <= 0`.
4. **CG hierarchy disabled:** `Rabbit_AC_Test/CG` was `activeSelf=false` (from session 5 raccoon mesh hiding). Weapon parented under `CG/.../Jaw/Mouth_WeaponPoint` had `activeInHierarchy=false`. `ReleaseProjectile()` returns immediately on `!gameObject.activeInHierarchy`. Fixed by enabling CG.

*Debug Infrastructure:*
- Created `WeaponDebug.cs` on Rabbit_AC_Test -- logs MWeaponManager state at runtime, press T to force MainAttack(true)
- Added temporary `[MShootable-TRACE]` logs throughout MShootable.cs attack chain (MainAttack_Start, FireAnim_ReleaseProjectile, ReleaseProjectile, FireProjectile)
- All marked with `//CustomPatch: temp debug` for easy removal

*Also This Session:*
- Unity updated to 6000.3.11f1
- MAnimal namespace changed: `MalbersAnimations.MAnimal` -> `MalbersAnimations.Controller.MAnimal`
- Expanded adammyhre gist catalog: 60 gists found, 12 HIGH candidates for TVG
- TVG v1.2.0: added StateMachine (CRTP) + Processing (ProcessorChain) modules

**CustomPatches Applied to Malbers Source:**
- `MShootable.cs` line 478: operator precedence fix (marked `//CustomPatch:`)
- `MShootable.cs` line 522: same fix for MainAttack_Released
- `MShootable.cs` line 565-567: direct ReleaseProjectile() for zero delay (marked `//CustomPatch:`)
- `MShootable.cs` multiple lines: temp debug logs (marked `//CustomPatch: temp debug`)

**Next Session:**
- Tune Mouth_FirePoint / Aim Point rotation so bolt fires forward, not into ground
- Wire LMB input (Action1 via MInputLink -> MWeaponManager not connecting; T key bypass works)
- Remove temp debug logs from MShootable.cs
- Remove WeaponDebug.cs component (or keep for development)
- Hide pistol mesh on weapon (cosmetic)
- Save scene with CG enabled
- Update AQS_MalbersRecipe.md with weapon bugs/fixes

---

## Sessions 6-7 (Mar 15, 2026) -- COMPLETE

**Status:** Rabbit JumpBasic fixed and working. Full recipe documented. Malbers MWeaponManager integration started but blocked -- mouth weapon fires nothing until AC Mode is wired.

**Session 6-7 Work:**

*JumpBasic Fix (Rabbit):*
- Fixed `GravityPower=0` on `Rabbit JumpBasic.asset` -> set to 1. Rabbit now arcs and returns properly.
- Fixed `StartGravityTime=0` -> 15 (more arcade hang-at-peak).
- Tuned `Height` to 2.4 (user preference after testing 3.0 and 5.0).
- Running jump looks correct. Standing jump appears to stay in Idle pose -- expected visual limitation, not a bug. Confirmed working via `STATE CHANGE -> Jump` in AnimDebug logs.
- Diagnosed `t=+0.000s` on all standing jump transitions: RabbitACDebug timer only starts on movement stop; standing jump never triggers MovementDetected, so elapsed is always 0.

*Recipe Documentation:*
- Created `AQS_MalbersRecipe.md` -- full step-by-step Polyperfect-to-AC integration recipe.
  - 18 Animator parameters in exact order
  - Sub-SM setup with tags, motions, Entry/Exit transitions
  - AnyState rules including `canTransitionToSelf=True` on Idles
  - State SO key fields (Idle/Locomotion/Fall/JumpBasic)
  - JumpBasic profile values (Height=2.4, GravityPower=1, JumpTime=0.33s, StartGravityTime=15)
  - Critical rule: `states[Count-1]` = startup state (Idle must be last)
  - JumpBasic `SleepFromState` (Fall + Fly, NOT Idle/Locomotion)
  - Input wiring: MInputLink "Jump" -> Gameplay/Jump
  - Troubleshooting table covering 6 common failure modes
- Updated `AQS_DevReference.md` with Malbers AC Integration rules summary.

*Mouth Weapon Test (Rabbit):*
- Located rabbit Jaw bone: `Rabbit_AC_Test/CG/Pelvis/Spine/Spine1/Spine2/Neck/Head/Jaw`
- Created `Mouth_WeaponPoint` (child of Jaw, localPosition=0) and `Mouth_FirePoint` (child of WeaponPoint, localPosition=(0,0,0.15)) -- both in scene, saved.
- Copied `Malbers Animations/Common/Prefabs/Weapons/Pistol.prefab` to `Assets/_Sandbox/_AQS/Prefabs/Weapons/Mouth_Weapon_Test.prefab`.
- Configured MShootable on prefab via Roslyn reflection:
  - `HasFireAnim = false`
  - `releaseByAnimation = false`
  - `aimAction = Ignore`
  - `equipProjectile = OnEquip`
  - `m_Projectile = Wolf Lite/Bolt.prefab`
- Added `MWeaponManager` to `Rabbit_AC_Test` root:
  - `m_IgnoreDraw = true`, `m_IgnoreStore = true`
  - `UseHolsters = false`, `IgnoreHandOffset = true`
  - `RightHandEquipPoint = LeftHandEquipPoint = Mouth_WeaponPoint`
  - `m_MainAttack = "Action1"` (maps to LMB / gamepad left trigger)
  - `startWeapon = Mouth_Weapon_Test`
  - `m_CombatLayerPath` cleared, `DrawWeaponModeID`/`StoreWeaponModeID` nulled

**Runtime Result:**
- MWeaponManager initializes, loads weapon, equips to mouth anchor.
- Warning: `"The Animal Controller does not have a mode for the Equipped Weapon!!"` at `EquipWeapon_AnimalController()` (Logic.cs:1182).
- `WeaponMode` never gets set because no AC Mode exists for the weapon type.
- LMB fires Action1 input but nothing happens -- `MainAttack()` path requires `WeaponMode` to be non-null.

**Key Finding -- MWeaponManager + AC Mode Requirement:**
Even with `HasFireAnim=false` (which bypasses the fire animation), `EquipWeapon_AnimalController()` at line 1182 still requires an AC Mode to be assigned for the weapon at equip time. `WeaponMode` is set then. Without it, `ConnectInput()` -> `MainAttack()` -> fire chain is dead. The only bypass path would require calling `MainAttack()` directly, or adding a minimal AC Mode (Attack1) to the rabbit.

**Next Session:**
- Add a minimal Attack1 Mode to the rabbit's MAnimal (can be empty/no animation for now)
- Wire it to MWeaponManager as the weapon mode
- Test LMB -> projectile spawn
- OR: look for a `WeaponMode`-bypass path in MWeaponManager source (direct `TryFireProjectile()` call?)
- Tune Mouth_FirePoint offset once firing works

**Known Issues (Session 6-7):**
- `IOException: Sharing violation on ai-editor-logs.txt` on every domain reload -- MCP plugin tries to lock a log file that's already locked by another instance. Harmless, self-recovers. Not a Sandbox/AQS issue.

---

## Session 5 (Mar 14, 2026) -- COMPLETE

**Status:** Polyperfect rabbit running on Malbers AC. Attack2 Bolt mode activates via code but not via input.

**Session 5 Work:**

*Bolt Mode Transfer (Raccoon):*
- Fixed MCP connectivity issues (GitBook MCP server was blocking Unity MCP tools -- removed)
- ChatGPT identified missing piece: mode ability configuration (active, index, status) not matching wolf
- Fixed ability fields: Status=Charged, active=True, Index=1, UseConstant=True on all
- Added entry transition inside Secondary Attack sub-SM: `-> Attack Bolt [Mode Equals 2001]`
- TestModeActivation script proved Mode_TryActivate(2,1) returns TRUE via code (T key)
- Attack2 mode WORKS via direct code call but NOT via RMB input
- Compared wolf vs raccoon MInputLink: wolf uses Float interaction, names "Attack1"/"Attack2"; raccoon uses "Action1"/"Action2"
- Changed Action2 to Float interaction type
- Still blocked: input fires (Action2 Float True/False in debug) but MAnimal never translates to mode activation
- Root cause likely in how MInputLink routes button events to MAnimal mode activation internally

*Polyperfect Rabbit AC Integration (NEW):*
- Created BlankTest scene with Polyperfect rabbit_brown
- Analyzed rabbit clips: ALL in-place (no root motion), 7 clips (Idle, Jump_Walk, Run, LookOut, Jump_Up, Death x2)
- Used Raccoon PA as shell: unpacked, disabled raccoon mesh/CG, parented rabbit visual under shell
- Set avatar to rabbit's, UpdateMode=AnimatePhysics, ApplyRootMotion=false
- Built minimal AC_Rabbit_Test.controller: States layer (Idles/Locomotion/Fall sub-SMs), Modes layer (Empty)
- All 18 Malbers animator parameters added
- Added AnyState transitions: StateOn + State==0/1/3 for Idle/Locomotion/Fall
- Configured Ground speed set with position values for in-place movement (Walk=1.5, Trot=3, Run=5, Sprint=7, rotation=5)
- Created rabbit-specific State SOs (Rabbit Locomotion, Rabbit Idle, Rabbit Fall) with RootMotion=false
- Identified `lerpPosAnim` on MSpeed as the control for Vertical parameter decay rate (default=4, set to 20)
- Result: Rabbit idles, moves with locomotion animation, physically traverses scene

**Key AC Architecture Learnings (Session 5):**
- AC States are ScriptableObject assets (e.g. `Raccoon Locomotion.asset`) referenced by MAnimal, NOT components
- State SO has `General.RootMotion` flag -- must be false for in-place animation characters
- `lerpPosAnim` on each MSpeed entry controls Vertical animator parameter decay rate. Low values (4) = slow stop. High values (20) = snappy stop. This is the "locomotion exit delay" control.
- AC's Locomotion exit is gated by the Vertical parameter reaching 0, not by MovementDetected flag
- Mode_TryActivate(modeID, abilityIndex) works even when input routing doesn't -- useful for debugging
- AnyState transitions must target SUB-STATE MACHINES with entry transitions inside, not individual states
- Entry transitions inside sub-SMs use `Mode Equals [ModeID*1000+AbilityIndex]` conditions
- ModeBehaviour on animator states needs ModeID ScriptableObject reference set
- MInputLink "Find Buttons" populates button mappings from InputActionAsset

**Polyperfect-to-AC Integration Recipe:**
1. Use a working Malbers animal prefab as shell (provides MAnimal, Rigidbody, colliders, states)
2. Unpack and disable the shell's mesh/CG
3. Parent third-party model visual under shell, reset local transform
4. Set shell's Animator avatar to third-party model's avatar
5. Set Animator UpdateMode=AnimatePhysics, ApplyRootMotion=false (for in-place clips)
6. Build minimal Animator Controller with Malbers parameters and tagged states (Idle, Locomotion, Fall)
7. Add AnyState transitions with StateOn trigger + State parameter conditions
8. Create character-specific State SOs with RootMotion=false
9. Set position/rotation values on speed sets (in-place clips need non-zero position speeds)
10. Set lerpPosAnim to 20+ for snappy stop response
11. Add PlayerInput + configure MInputLink

**Recovered Design Docs:**
- Pre-crash design documents found at `E:\TecVooDoo\Projects\Games\2 Planning\A Quokka Story\Documents\AQSRessurection\`
- `a_quokka_story_design_bible.pdf` -- full design bible
- `a_quokka_story_first_playable_prototype_spec.pdf` -- prototype spec

**Carried Forward:** Jump added to rabbit (Sessions 6-7). lerpPosAnim=20 confirmed working. Raccoon Bolt mode deferred.

---

## Session 4 (Mar 14, 2026) -- COMPLETE

**Status:** Built MCP tools, analyzed demos, recovered Joey mechanics. AQS 2.5D scene created with raccoon.

**Session 4 Work:**
- Built 52 MCP tools across 9 asset groups (added Malbers AC 7 tools, Quest Forge 7 tools, Retarget Pro 3 tools)
- Analyzed all Malbers demo scenes: 2.5D Platformer, Forest, AI Brain, Callbacks, Mobile, PlayGround, Sandbox Utilities, Swap Characters, Replace States At Runtime, Animal Tracker, plus AI demo folder
- Recovered critical Joey design mechanics from user memory (pouch buff/debuff, companion train, Joey shuttling/ladder traversal puzzles)
- Recovered pre-crash design docs: Design Bible PDF + First Playable Prototype Spec PDF at `E:\TecVooDoo\Projects\Games\2 Planning\A Quokka Story\Documents\AQSRessurection\`
- Created AQS 2.5D scene (`Assets/_Sandbox/_AQS/Scenes/2.5dMalbersAQS.unity`) from Malbers demo
- Swapped wolf for Raccoon Forest prefab + LockAxis + PlayerInput (on character, not separate object)
- Movement, jump, swim, fall, climb, dodge, melee attack (LMB) all working
- Full wolf vs raccoon AC comparison: States (10 vs 8), Modes (9 vs 5), Stances (3 vs 6), Speeds (4 vs 5)
- Attempted Attack2 Bolt mode transfer: added animator states, ModeBehaviour, MAnimal mode entry, AnyState transitions
- Input confirmed reaching AC (`[Input Action2 - Float : True]`) but mode not activating
- Identified missing pieces incrementally: ModeBehaviour needed ModeID, AnyState transitions need SUB-SM targets (not STATE), Float input type needed for charged modes
- Added Malbers GitBook MCP server to `.claude/mcp.json` for documentation access

**Key AC Architecture Learnings:**
- Mode parameter formula: `ModeID * 1000 + AbilityIndex` (e.g. Attack2 Bolt = 2001)
- AnyState transitions target SUB-STATE MACHINES, not individual states (e.g. `AnyState -> SUB-SM: Attacks [ModeOn, Mode>1000, Mode<2000]`)
- ModeBehaviour on each mode animation state must have ModeID SO reference
- Exit transitions need: interrupt (`ModeStatus == -2`, no exit time) + default exit (has exit time, no conditions)
- MInputLink button "name" must match Mode "Input" field exactly for auto-connection
- Float interaction type sends continuous 0 values every frame when not pressed -- can cause issues
- Wolf's Attack1/2 buttons used Float interaction; raccoon's used Press (had to change)
- Animation tags are case-sensitive

**Key Joey Design Recovery (not in GDD):**
- Pouch holds ONE Joey. Others follow Mom as companion train (AnimalTracker pattern)
- Pouched Joey modifies Mom's stats: Lead=slow+tanky, Helium=high jump+less control, Ballet=fast+fragile
- Joey shuttling puzzles: Mom carries Joeys one-by-one to high platforms they can't reach alone
- Joey ladder: 3-4+ Joeys can pull each other up, reducing shuttling need
- More Joeys = more responsibility + easier traversal (Metroidvania gating)
- Maps to AC demos: Replace States (#9) for buff/debuff, AnimalTracker (#15) for following, Swap Characters (#10) for control transfer

**Unresolved: Bolt Mode Not Activating**
- Input reaches MInputLink and fires correctly (Action2 Float True/False)
- MAnimal has Attack2 mode with Bolt ability (Charged, Index 1)
- Animator Controller has Secondary Attack sub-SM with ModeBehaviour + ModeID
- AnyState transition targets SUB-SM with correct conditions (ModeOn, Mode>2000, Mode<3000)
- Still no mode activation -- may need additional animator parameter setup, entry transitions within sub-SM, or undiscovered AC requirement
- Next step: study Malbers docs via GitBook MCP, or ask Malbers Discord

**Next Session:**
- Resolve Bolt mode activation (complete the proof-of-concept)
- If resolved: transfer remaining wolf modes to raccoon
- If blocked: ask Malbers Discord for cross-character mode transfer guidance
- Clean up test objects (cube, pick up item)
- Update docs based on outcome
- Verify GitBook MCP server connectivity

---

## Session 3 (Mar 14, 2026) -- COMPLETE

**Status:** Malbers AC pivot. Installed full ecosystem, analyzed 2.5D demo, confirmed raccoon as player character.

**Session 3 Work:**
- Installed Horse Animset Pro 4.5.1 (includes Animal Controller 4.5.1) -- safe mode reimport required after CS0115 errors (Common folder version mismatch)
- Fixed Malbers bug: Lootbag.cs `[SerializeField]` on class -> `[System.Serializable]`
- Installed Poly Art: Raccoon 4.0 (full Malbers raccoon pack with AC prefabs + Raccoon Cub)
- Installed polyperfect Low Poly Animated Animals 4.1.1 (162 prefabs, 68 species for enemy placeholders)
- Installed Quest Forge 1.0 (quest/dialogue/minimap system)
- Re-evaluated 9 Malbers assets + 1 polyperfect asset for Sandbox AssetLog (entries 028, 029, 030, 031, 032, 001, 294, 295, 296, 297, 036)
- Analyzed Malbers 2.5D Platformer demo scene (Wolf Lite: 15 components including MAnimal, LockAxis, MInputLink)
- Analyzed Raccoon PA Player prefab: 13 components, near-identical to Wolf Lite. Missing LockAxis (2.5D constraint) and MWeaponManager
- Decision: copy 2.5D demo scene as AQS base, swap Wolf Lite for Raccoon PA Player, add LockAxis
- Decision: keep demo environment/interactables (Zones, Water, Platform) and adapt rather than rebuild

**Key Findings:**
- LockAxis component is how AC does 2.5D -- solves our facing bug without FreezeRotation hacks
- Raccoon PA Player is a near drop-in for Wolf Lite (same AC stack minus LockAxis and MWeaponManager)
- Raccoon PA Cub prefab available for Joey placeholders -- matches adult raccoon style
- AC's 2.5D demo has interactable zones (Eat zones with Meat items), water, stair-tagged rocks, sandbox boundary colliders
- Malbers Common folder version mismatch causes CS0115 on reimport -- delete Common folder and reimport from scratch per Malbers docs

**Next Session:**
- Copy 2.5D demo scene to `_AQS/Scenes/`
- Swap Wolf Lite for Raccoon PA Player
- Add LockAxis component to raccoon
- Test movement, facing, jump, climb
- Evaluate whether custom QuokkaController/QuokkaInputHandler are still needed or fully replaced by AC
- MCP candidate evals for Malbers ecosystem (deferred from this session)

---

## Session 2 (Mar 13, 2026) -- COMPLETE

**Status:** Greybox scene built, player wired, movement partially working. Facing bug unresolved.

**Session 2 Work:**
- Created AQS_Greybox scene via MCP with greybox platforms, walls, climbable surface, boundary walls
- Wired QuokkaMom GameObject: Rigidbody, CapsuleCollider, PlayerInput, QuokkaController, QuokkaInputHandler, Scorch model child
- Set up layers: Ground=6, Player=7, Climbable=9 (layer 8 was already "posteffects")
- Fixed player floating (Y=1 -> Y=0), PlayerInput Unity Event wiring (rewrote to self-wire via C# API)
- Recreated AQS_InputActions via Unity API (hand-written GUIDs caused NullReferenceException)
- Fixed 3D model facing: Scorch's +Z forward needs Y=90 rotation for 2.5D side-scrolling; Flip() uses Y rotation (90/-90)
- Rewrote QuokkaInputHandler to polling-based (moveAction.ReadValue in Update) -- composite binding callbacks had unreliable timing
- Updated QuokkaController input API: SetMoveInput(float), OnJumpPressed(), OnJumpReleased()
- Attempted model child rotation for facing (modelTransform instead of root transform) -- still snaps back
- Installed Poly Art: Raccoon from Malbers for full animation set + Raccoon Cub model for Joey placeholders

**Unresolved: Facing Bug**
- Symptom: model tries to turn opposite direction but snaps back immediately
- Root transform rotation conflicts with Rigidbody FreezeRotation constraint
- Moved rotation to model child transform -- still snaps back (possibly Animator or model hierarchy issue)
- Decision: installing Malbers Animal Controller (via Horse Animset Pro) to evaluate as movement solution

**Key Gotchas Discovered:**
- Composite bindings (WASD) have unreliable callback timing -- poll with ReadValue in Update instead
- FreezeRotation on Rigidbody fights transform.rotation changes every physics step -- rotate visual child instead
- InputActions asset: hand-written JSON with fabricated GUIDs causes NullReferenceException -- must use Unity API to generate proper GUIDs
- AssetDatabase.LoadAssetAtPath argument order: (path, type) not (type, path)

**Next Session:**
- Install Malbers Horse Animset Pro (which includes Animal Controller)
- Discuss approach: use Malbers AC for movement vs continue custom controller
- Resolve facing bug
- Test hop movement + jump feel
- Begin climb system if time permits

---

## Session 1 (Mar 12, 2026) -- COMPLETE

**Status:** Sprint 1 foundation. Scripts compiling, Scorch imported, GitHub repo created.

**Session 1 Work:**
- Created GameEvent/GameEventListener system (AQS.Core) adapted from HOK
- Created QuokkaController (AQS.Player) -- hop-based movement, jump with coyote time + buffer + cut, fall gravity, collision-based ground detection
- Created QuokkaInputHandler (AQS.Player) -- wires Input System to controller
- Created AQS_InputActions.inputactions -- Gameplay map with Move/Jump/LaunchJoey, keyboard + gamepad
- Imported Scorch from HOK as placeholder character (models, materials, animations -- no scripts)
- Created .gitignore with third-party asset exclusions (207 tracked files, 27.8MB)
- Initial commit + push to https://github.com/TecVooDoo/Sandbox
- Fixed compile errors: `Collision2D.GetContacts(List<>)` overload doesn't work in Unity 6 -- use `collision.contactCount` + `collision.GetContact(i)` instead
- Fixed physics type: 2.5D uses 3D physics (Rigidbody, CapsuleCollider, Collision) with Z locked, NOT 2D physics (Rigidbody2D)
- Boing Kit vs All In 1 Springs comparison: Boing Kit wins for AQS (BoingBones for ear/tail, AnimationBlend works with Animancer)

**Key Gotchas Discovered:**
- Unity 6 `Rigidbody` uses `linearVelocity` (not `velocity`) -- confirmed by linter
- Unity 6 `Collision2D.GetContacts(List<ContactPoint2D>)` silently fails to compile -- use index-based access
- 2.5D with 3D models = 3D physics + `RigidbodyConstraints.FreezePositionZ | FreezeRotation`
- 3D `Rigidbody` has no `gravityScale` -- use `AddForce` with `Physics.gravity` multiplier instead

**Next Session:**
- Create AQS_Greybox scene with ProBuilder platforms, walls, climbable surfaces
- Wire up player GameObject (Rigidbody, CapsuleCollider, PlayerInput, QuokkaController, QuokkaInputHandler)
- Set up Ground layer
- Test hop movement + jump feel
- Begin climb system if time permits

---

## Session 0.5 (Mar 12, 2026) -- COMPLETE

**Status:** Asset audit. Searched Sandbox AssetLog (293 entries) for AQS-relevant assets.

**Session 0.5 Work:**
- Searched AssetLog across 10 categories: mesh deformation, projectile/trajectory, enemy AI, destruction, platformer, animal models, ragdoll, lighting, shaders, map/minimap
- Identified ~30 relevant assets with sprint-aligned install schedule
- Key finding: Toolkit for Ballistics 2026 + ALINE eliminates need for separate line renderer asset
- Key finding: Toon Kit 2 + Outlines Post-Process matches GDD art direction ("thick outlines, vibrant colors")
- Key finding: SensorToolkit2 + Behavior Designer Pro + A* Pathfinding Pro = complete AI stack
- Key finding: AA Map and Minimap, Smart Lighting 2D still need formal evaluation
- Updated AQS_Status.md with full asset audit results organized by sprint
- Updated AQS_DevReference.md with AssetLog-sourced recommendations in dependency tables

---

## Session 0 (Mar 12, 2026) -- COMPLETE

**Status:** Project resurrection. Docs recovered from Google Drive, reformatted to current doc system.

**Session 0 Work:**
- Retrieved archived docs from `E:\Unity\Sandbox\Documents\AQuokkaStory\Documents\Archives\`
- Created AQS_Status.md, AQS_DevReference.md, GDD/AQS_GDD.md in current format
- Updated architecture: SOAP dropped (vanilla SO + GameEvent/GameEventListener), Dynamic Bone replaced by BoingBones, Unity-Event-Bus/Improved-Timers replaced by TecVooDoo Utilities, Service Locator replaced by PersistentSingleton
- Created `Assets/_Sandbox/_AQS/` working folder in Sandbox
- Flagged assets needing re-evaluation for current ecosystem
- Decided: custom movement (not Corgi Engine), Scorch from HOK as placeholder, RayFire for destruction
- Created 5-sprint development plan for Sandbox gameplay prototyping
