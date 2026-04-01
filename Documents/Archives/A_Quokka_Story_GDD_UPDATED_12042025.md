# **A Quokka Story**

## **Game Design Document (Master)**

**Version:** 3.0  
**Last Updated:** December 4, 2025  
**Developer:** TecVooDoo  
**Engine:** Unity 6.2.2 URP

---

## **Table of Contents**

1. [Executive Summary](#executive-summary)  
2. [Game Overview](#game-overview)  
3. [Core Pillars](#core-pillars)  
4. [Gameplay Mechanics](#gameplay-mechanics)  
5. [Joey System](#joey-system)  
6. [Enemy Design](#enemy-design)  
7. [Combat & Abilities](#combat--abilities)  
8. [Music System](#music-system)  
9. [Level Design & Progression](#level-design--progression)  
10. [Narrative](#narrative)  
11. [Technical Architecture](#technical-architecture)  
12. [Art & Visual Style](#art--visual-style)  
13. [Target Audience & Platforms](#target-audience--platforms)  
14. [Asset Integration](#asset-integration)  
15. [Development Status](#development-status)  
16. [Development Roadmap](#development-roadmap)

---

## **Executive Summary**

**A Quokka Story** is a 2.5D Metroidvania-style adventure platformer where players control a mother Quokka on a mission to rescue her scattered Joeys from poachers. The game's unique selling point is its **Joey-launching combat mechanic** combined with an **adaptive stem-based music system** where each character on screen contributes an instrument layer to the soundtrack.

**Key Differentiators:**

* Launch Joeys from mom's pouch as weapons, tools, and puzzle solutions  
* Each Joey has unique abilities with meaningful drawbacks  
* Stem-based adaptive music that dynamically responds to gameplay  
* Emotional narrative centered on family reunion and animal liberation  
* Modular, SOLID-principle architecture designed for reusability

**Target Release:** PC (primary), followed by Android, iOS, and consoles  
**Genre:** 2.5D Metroidvania, Puzzle-Platformer  
**ESRB Rating:** E10+ (Everyone 10+)

---

## **Game Overview**

### **High Concept**

*"A mother's love knows no bounds - even if it means launching her children at enemies."*

### **Elevator Pitch**

Players guide a Quokka mom through dangerous biomes (swamp, suburbs, city, airstrip) to rescue her Joeys who were scattered after a cargo plane crash. Each rescued Joey adds new abilities and musical layers, building toward a climactic showdown at the poachers' headquarters.

### **Core Loop**

1. **Explore** environments to find Joeys and map pieces  
2. **Launch** Joeys to solve traversal puzzles and defeat enemies  
3. **Rescue** Joeys to unlock new abilities and music layers  
4. **Progress** through increasingly complex levels  
5. **Unite** the family and liberate captured animals

---

## **Core Pillars**

### **1. Family & Emotional Connection**

* Every Joey has personality and purpose  
* Players feel protective of the Joey family  
* Narrative centers on reunion and protection  
* "True ending" requires keeping Normal Joey safe (no deaths)

### **2. Creative Problem Solving**

* Multiple solutions to obstacles using different Joey combinations  
* Abilities designed to interact with environment in unique ways  
* Puzzles reward experimentation and creativity  
* No "correct" path - player expression encouraged

### **3. Risk vs. Reward Combat**

* Every Joey ability has meaningful drawbacks  
* Energy management affects music mix and combat effectiveness  
* Players must strategize which Joey to use and when  
* Spamming abilities reduces music layers (discourages button mashing)

### **4. Living Soundtrack**

* Music evolves organically with player actions  
* Each entity on screen contributes to the musical ensemble  
* Combat creates musical accents and flourishes  
* Players "conduct" the soundtrack through gameplay choices

### **5. Whimsical Meets Perilous**

* Cute art style contrasts with serious themes (poaching, family separation)  
* Lighthearted tone balanced with emotional weight  
* Characters are endearing but not saccharine  
* Accessible to all ages but resonates with adults

---

## **Gameplay Mechanics**

### **Player Character: Quokka Mom**

#### **Movement**

* **Walk/Run:** Standard 2.5D platforming movement  
* **Jump:** Single jump with variable height based on button hold  
* **Pouch Dash:** Short-range dash that protects Joeys (unlocked mid-game)  
* **Climb:** Can scale certain surfaces and vines (with stamina system)
* **Crouch:** Lower profile, access small passages

#### **Mom's Stats (Base)**

* **Health:** 5 hearts (can be upgraded)  
* **Movement Speed:** Moderate (affected by equipped Joey)  
* **Jump Height:** Standard platformer range
* **Climb Stamina:** Limited duration before forced release

### **Joey Launch Mechanic**

#### **Core System**

* **Aim:** Hold launch button to display targeting reticle  
* **Power:** Hold longer for farther throw (max 2 seconds)  
* **Launch:** Release to send Joey  
* **Recall:** Joey returns after ability execution or manual recall  
* **Cooldown:** Based on Joey's energy level

#### **Launch States**

1. **In Pouch (Resting)**  
   * Joey regenerates energy at 3x rate  
   * Contributes full volume to music mix  
   * Available for launch  
2. **Aiming**  
   * Time slows slightly (15-20%)  
   * Trajectory arc displayed  
   * Energy consumption preview shown  
3. **Launched (Active)**  
   * Joey's instrument **accents loudly** over mix  
   * Unique attack/ability animation  
   * Physics-based projectile motion  
4. **Depleted (Cooling)**  
   * Joey's instrument fades to very low volume  
   * Passive energy regeneration  
   * Faster regen when back in pouch

### **Pouch Management System**

**Pouch Slots:**

* **Active Slot:** Currently equipped Joey (Q/E to cycle)  
* **Reserve Slots:** Up to 5 additional Joeys (quick-swap via number keys)  
* **Rescued But Not Carried:** Stored safely, can swap at save points

**Energy System:**

* Each Joey has **100 energy points**  
* Abilities cost **25-75 energy** depending on power  
* In-pouch regen: **30 energy/second**  
* Out-of-pouch regen: **10 energy/second**  
* Energy level determines music volume for that Joey's instrument

### **Progression Gates**

* **Ability Gates:** Certain areas require specific Joey abilities  
  * Helium Joey: Reach high platforms  
  * Lead Joey: Break heavy barriers  
  * GI Joey: Destroy reinforced doors, burn brush and armored enemies  
  * Ninja Joey: Cut through vines and ropes  
  * Ballet Joey: Navigate ice/slippery surfaces, freeze enemies into stepping stones  
* **Map Gates:** Finding map pieces reveals new areas and paths to subsequent levels  
* **Story Gates:** Narrative triggers unlock new biomes

### **Backtracking & Secrets**

* **Backtracking with new abilities** opens shortcuts and hidden routes  
* Earlier levels contain secrets only accessible with later-game Joeys  
* **Secret animal rescues** require returning to previous biomes  
* **"Full Ensemble" music versions** unlock after rescuing all Joeys in an area  
* Hidden routes provide faster paths during speedruns or replays

---

## **Joey System**

### **Joey Roster (Final)**

#### **1. Normal Joey**

**Role:** Narrative anchor, map reader  
**Ability:** None (pure character)  
**Passive:** Can read torn map pieces to reveal secrets  
**Drawback:** Fragile - if Normal Joey dies, "true ending" is locked  
**Instrument:** Clapsticks  
**Rescue Location:** Tutorial (Cargo Plane)  
**Personality:** Brave but inexperienced, looks up to Mom

#### **2. Lead Joey**

**Role:** Tank, heavy hitter  
**Ability:** Bowling Ball Launch - crashes through enemies and obstacles  
**Passive Buff (Equipped):** Mom gains +30% defense, knockback immunity  
**Drawback:** Slows Mom's movement by 25%  
**Instrument:** Didgeridoo (deep bass drone)  
**Rescue Location:** Tutorial (Cargo Plane)  
**Personality:** Protective older sibling, serious demeanor

#### **3. Helium Joey**

**Role:** Traversal specialist, crowd control  
**Ability:** Inflate - latches onto enemies/objects and inflates them until they float or pop  
**Passive Buff (Equipped):** Mom's fall speed reduced by 40%  
**Drawback:** Reduced air control while Helium Joey is equipped  
**Instrument:** Flute (airy, ascending notes)  
**Rescue Location:** Level 3 (Swamp Revisit)  
**Personality:** Lighthearted, easily distracted, giggly  
**Size Limits:** Cannot inflate large enemies (Black Bear, Wolverine, Wild Boar)

#### **4. GI Joey** *(Replaces Fireball Joey)*

**Role:** Heavy damage dealer, area control  
**Ability:** Bazooka Blast - explosive projectile that damages multiple enemies in front  
**Passive Buff (Equipped):** Mom gains Gatling Gun (hold button for rapid-fire, consumes energy)  
**Drawback:** Explosion damages Mom if she's too close (backblast)  
**Instrument:** Snare and Hand Drum (military intensity, sharp rhythmic)  
**Rescue Location:** Level 4 (City/Industrial Zone)  
**Personality:** Disciplined, tactical, uses military jargon

#### **5. Ninja Joey**

**Role:** DPS, stealth potential  
**Ability:** Banjo Shuriken - bounces between enemies, tail blade slashes on contact  
**Passive Buff (Equipped):** Mom gains shadow step (brief invincibility during dash)  
**Drawback:** Causes exhaustion in other Joeys (25% slower energy regen for non-Ninja Joeys)  
**Instrument:** Banjo (plucky, rhythmic strumming)  
**Rescue Location:** Level 2 (Suburb)  
**Personality:** Cool, collected, speaks in haiku

#### **6. Ballet Joey**

**Role:** Mobility, freeze control  
**Ability:** Pirouette Freeze - spins and freezes enemies in AOE  
**Passive Buff (Equipped):** Mom gains +40% movement speed and jump height  
**Drawback:** Fragile - takes double damage when equipped  
**Instrument:** Bells and Violin (elegant, chiming melodies)  
**Rescue Location:** Level 1 (Swamp)  
**Personality:** Graceful, perfectionist, dramatic  
**Special Mechanic:** Stunned flying birds fall to ground, creating bait for predators

#### **7. Kazoo Joey** *(Kickstarter Backer Choice)*

**Role:** Easter egg character, comedy relief  
**Ability:** TBD (named and designed by high-tier Kickstarter backer)  
**Passive Buff (Equipped):** TBD based on backer choice  
**Drawback:** TBD based on ability balance  
**Instrument:** Kazoo (buzzy, comedic)  
**Rescue Location:** Final Zone (Airstrip HQ)  
**Personality:** Quirky, unpredictable

### **Joey Design Principles**

* **Every ability has a drawback** to prevent dominant strategies  
* **Equipped Joey affects Mom's stats** to encourage swapping  
* **Energy management** creates rhythm and prevents spam  
* **Personality shines through** animations and voice/sound design  
* **Synergies exist** but are discovered, not required

---

## **Enemy Design**

### **Design Philosophy**

**Core Principles:**

1. **Every Enemy Contributes to the Music**
   * Each enemy type adds a unique instrument layer to the adaptive soundtrack  
   * Combat creates musical tension through layering  

2. **Telegraphed Attacks**
   * All enemy attacks have clear visual tells (wind-up animations, color changes)  
   * Maintains fairness and readability  

3. **Joey-Specific Weaknesses**
   * Enemies encourage Joey swapping and strategic thinking  
   * No "brute force" solutions  

4. **Environmental Integration**
   * Enemies use terrain features and hazards to their advantage  
   * Creates dynamic encounters  

5. **Escalation Pattern**
   * Early enemies teach mechanics  
   * Later enemies combine and remix those mechanics

### **Swamp Biome Enemies**

#### **Snake**

* **Behavior:** Slithers on ground, lunges at Mom with telegraphed strike  
* **Attack Pattern:** Quick lunge (0.5s windup), moderate damage  
* **Weaknesses:** Lead Joey (crush), GI Joey (explosive), Ballet Joey (freeze, then shatter)  
* **Instrument:** Slide Whistle (each snake has unique procedurally-generated melody)  
* **Stun Duration:** 8-10 seconds (Ballet Joey)  
* **Predator Behavior:** Ignores bird bait

#### **Gator**

* **Behavior:** Guards water areas, strong bite attack, can perform tail swipe  
* **Attack Pattern:** Bite (1s windup, high damage), Tail swipe (knockback, medium damage)  
* **Weaknesses:** Helium Joey (inflate and float away), Ninja Joey (banjo shuriken bounces)  
* **Instrument:** Banjo (before Ninja Joey rescue), Banjo Accents (after Ninja Joey rescued)  
* **Stun Duration:** 5-7 seconds (Ballet Joey)  
* **Predator Behavior:** **Attracted to bird bait**

#### **Python (Larger Variant)**

* **Behavior:** Constricts if Mom gets too close, slow but high HP  
* **Attack Pattern:** Grab attempt (slow), constrict (damage over time if caught)  
* **Weaknesses:** GI Joey (high damage needed), Helium Joey cannot inflate (too large)  
* **Instrument:** Bass Clarinet (low, ominous droning)  
* **Stun Duration:** 3-5 seconds (Ballet Joey - large enemy)  
* **Predator Behavior:** Ignores bird bait

#### **Feral Cat**

* **Behavior:** Quick, erratic movement, pounce attacks  
* **Attack Pattern:** Rapid pounces, low damage but frequent  
* **Weaknesses:** Ballet Joey (freeze mid-pounce), Ninja Joey (stealth approach)  
* **Instrument:** Meowing vocal samples (chaotic, layered)  
* **Stun Duration:** 8-10 seconds (Ballet Joey - small enemy)  
* **Predator Behavior:** **Attracted to bird bait**

#### **Panther (Stealth Predator)**

* **Behavior:** Stalks from shadows, ambush attacks  
* **Attack Pattern:** Pounce from hiding (high burst damage)  
* **Weaknesses:** Ninja Joey (can detect hidden panthers), GI Joey (area damage flushes them out)  
* **Instrument:** Low Percussion (heartbeat rhythm, tension-building)  
* **Stun Duration:** 5-7 seconds (Ballet Joey)  
* **Predator Behavior:** **Attracted to bird bait**

#### **Bobcat**

* **Behavior:** Territorial, circles before attacking  
* **Attack Pattern:** Lunging swipes, medium damage  
* **Weaknesses:** Lead Joey (heavy impact), Ballet Joey (freeze during circle)  
* **Instrument:** Growling Brass (aggressive, staccato)  
* **Stun Duration:** 5-7 seconds (Ballet Joey)  
* **Predator Behavior:** **Attracted to bird bait**

### **Suburb Biome Enemies**

#### **Hawk (Flying Enemy)**

* **Behavior:** Swoops from above, attempts to grab Joeys  
* **Attack Pattern:** Dive attack (telegraphed shadow on ground), Joey grab attempt  
* **Weaknesses:** GI Joey (anti-air explosive), Ballet Joey (freeze mid-flight, falls to ground stunned)  
* **Instrument:** Hurdy Gurdy (droning, ominous)  
* **Stun Duration (grounded):** 10-15 seconds (disoriented from fall)  

#### **Domestic Dog**

* **Behavior:** Patrols yards, chases moving targets (Mom), barks to alert others  
* **Attack Pattern:** Chase and bite, moderate damage  
* **Weaknesses:** Lead Joey (intimidated by size), Helium Joey (inflate and float away)  
* **Instrument:** Barking Percussion (rhythmic, alarm-like)  
* **Stun Duration:** 5-7 seconds (Ballet Joey)  
* **Predator Behavior:** **Ignores bird bait** - focuses on moving targets

#### **Raccoon**

* **Behavior:** Steals items from Mom, sets simple traps, uses tools  
* **Attack Pattern:** Hit-and-run scratches, trap placement  
* **Weaknesses:** Ninja Joey (stealth approach), GI Joey (destroys traps)  
* **Instrument:** Accordion (mischievous, playful melody)  
* **Stun Duration:** 5-7 seconds (Ballet Joey)  
* **Predator Behavior:** **Attracted to bird bait** - raccoons investigate anything

#### **Wild Boar**

* **Behavior:** Charges in straight lines, high damage but predictable  
* **Attack Pattern:** Charge (long telegraph, devastating if hits)  
* **Weaknesses:** Ballet Joey (freeze before charge), Ninja Joey (dodge and counter)  
* **Instrument:** Heavy Percussion (tribal drums, threatening)  
* **Helium Joey:** **Cannot inflate** - too heavy/large  
* **Stun Duration:** 5-7 seconds (Ballet Joey)  
* **Predator Behavior:** **Attracted to bird bait** - omnivore

#### **Wolverine (Mini-Boss)**

* **Behavior:** Aggressive, relentless, high HP  
* **Attack Pattern:** Multi-hit combos, grab attempts  
* **Weaknesses:** GI Joey (sustained damage), Multiple Joey combos required  
* **Instrument:** Distorted Guitar (aggressive, metal-like)  
* **Helium Joey:** **Cannot inflate** - too large and aggressive  
* **Stun Duration:** 3-5 seconds (Ballet Joey - tough enemy)

### **City Biome Enemies**

#### **Security Drone**

* **Behavior:** Patrols set paths, alarm triggers reinforcements  
* **Attack Pattern:** Laser targeting (telegraph), energy blast  
* **Weaknesses:** GI Joey (brute force destruction), Ninja Joey (stealth disable)  
* **Instrument:** Electronic Beeps/Synth Bass (mechanical, pulsing)  
* **Stun Duration:** N/A (mechanical - destroyed instead)

#### **Poacher Guard (Human Enemy)**

* **Behavior:** Armed patrols, coordinated attacks, use cover  
* **Attack Pattern:** Net gun (capture attempt), taser (stun), coordinated flanking  
* **Weaknesses:** Ninja Joey (stealth takedowns), Ballet Joey + GI Joey combo  
* **Instrument:** Heavy Percussion (drum kit, military cadence)  
* **Resistances:** High resistance to single Joey attacks, requires strategy  
* **Stun Duration:** 3-5 seconds (Ballet Joey - well-trained)

#### **Black Bear (Rare/Roaming)**

* **Behavior:** Territorial, massive damage but slow  
* **Attack Pattern:** Powerful swipes, ground slam  
* **Weaknesses:** Avoid combat (run away or stealth past), GI Joey (if forced to fight)  
* **Instrument:** Deep Bass Drum (thunderous, intimidating)  
* **Helium Joey:** **Cannot inflate** - far too large  
* **Stun Duration:** 3 seconds (Ballet Joey - massive enemy)

### **Airstrip HQ Enemies**

#### **Poacher Leader (Final Boss)**

* **Behavior:** Multi-phase fight, uses captured animals as shields  
* **Attack Pattern:**
  * Phase 1: Ranged weapons, calls guards
  * Phase 2: Uses stolen Joey abilities (mock versions)
  * Phase 3: Desperate melee combat
* **Weaknesses:** Phase-specific, requires all Joey abilities  
* **Instrument:** All instruments layer together (musical chaos representing control)  
* **Special:** Music becomes dissonant and aggressive during boss fight

### **Enemy AI Implementation**

**Recommended Asset:** SensorToolkit 2 (owned)

* Sight cone detection for patrol enemies
* Hearing detection for alert systems
* Integration with behavior systems
* Performance-optimized for multiple enemies

---

## **Combat & Abilities**

### **Combat Flow**

1. **Enemy Appears** -> Instrument layer added to music  
2. **Player Launches Joey** -> Joey's instrument accents loudly  
3. **Ability Connects** -> Visual/audio feedback, enemy takes damage  
4. **Enemy Defeated** -> Instrument layer removed, victory stinger  
5. **Joey Returns to Pouch** -> Energy regenerates, volume restored

### **Joey-Enemy Matchup Matrix**

| Enemy | Best Counter | Alternative | Avoid Using |
|-------|--------------|-------------|-------------|
| Snake | Lead/GI Joey | Ballet Joey | Helium (ineffective) |
| Gator | Helium/Ninja Joey | GI Joey | Normal (too weak) |
| Hawk | GI/Ballet Joey | Ninja Joey | Lead (can't reach air) |
| Dog | Lead/Helium Joey | Ballet Joey | Normal (too aggressive) |
| Raccoon | Ninja Joey | GI Joey | Lead (too slow) |
| Boar | Ballet/Ninja Joey | GI Joey | Helium (can't inflate) |
| Wolverine | GI Joey + combos | All Joeys | Single Joey spam |
| Drone | GI/Ninja Joey | Multiple hits | Ballet (ineffective) |
| Guards | Ninja Joey | GI + Ballet combo | Direct assault |
| Bear | Stealth past | GI Joey (forced) | Direct combat |

---

## **Music System**

### **Philosophy**

The music system is the **heart of the game's identity**. Music is constructed from **layered audio stems** that dynamically fade in/out based on active Joeys, spawned enemies, and combat energy. Each biome has 8-10 instrument stems that seamlessly loop at a consistent tempo and key, creating adaptive music that responds to gameplay in real-time.

### **Audio Source**

Stems are sourced from **Nine Volt Audio's professional loop libraries** (30,000 loops organized by key and tempo). This approach ensures high-quality sound while maintaining performance across all platforms (PC, consoles, mobile).

### **Technical Foundation**

* **BiomeMusicData ScriptableObject:** Defines stem assignments per biome  
* **MusicManager Service:** Coordinates layers, manages mix, handles fades  
* **Audio Mixer Groups:** Separate mix groups for Joey layers, enemy layers, energy-based drums  
* **Event-Driven:** Music responds to Joey launch/recall, enemy spawn/death, energy changes
* **Implementation:** Master Audio 2024 (owned) for professional audio management

### **Instrument Assignments**

| Character | Instrument | Always Present? | Notes |
|-----------|------------|-----------------|-------|
| Mom | Frame Drum | Yes | Heartbeat of the game |
| Normal Joey | Clapsticks | Yes (post-tutorial) | Rhythmic foundation |
| Lead Joey | Didgeridoo | Yes (post-tutorial) | Bass drone |
| Helium Joey | Flute | After rescue | Airy, melodic |
| GI Joey | Snare/Hand Drum | After rescue | Military intensity |
| Ninja Joey | Banjo | After rescue | Rhythmic strumming |
| Ballet Joey | Bells/Violin | After rescue | Elegant chiming |
| Kazoo Joey | Kazoo | After rescue | Comedic accent |
| Snake | Slide Whistle | While alive | Unique melody per snake |
| Gator | Banjo/Accents | While alive | See conflict resolution |
| Hawk | Hurdy Gurdy | While alive | Droning tension |

### **Dynamic Behaviors**

#### **Volume Mixing**

Each entity's instrument volume is determined by:

* **Mom:** Always 100% volume  
* **Joeys (In Pouch):** Volume = Energy Level (0-100%)  
* **Joeys (Launched):** 150% volume (accent)  
* **Joeys (Cooling):** 10-20% volume  
* **Enemies:** 60-80% volume (balanced with Joeys)

#### **Instrument Conflict Resolution**

**Problem:** Ninja Joey and Gator both use banjo  
**Solution:**
* **Before Ninja Joey rescue:** Gator plays full banjo part  
* **After Ninja Joey rescue:** Gator switches to banjo **accents only** (string plucks, hammer-ons)  
* Ninja Joey maintains primary banjo melody

#### **Stem Organization Per Biome**

Each biome has stems at the **same key and tempo** for perfect synchronization:

**Swamp Biome** (E minor, 105 BPM):
* Ambient layer (nature sounds, always on)  
* Bass stem (Mom - upright bass)  
* Light drums (low energy 0-30%)  
* Full drums (high energy 70-100%)  
* Banjo (Ninja Joey OR Gator)  
* Harmonica (GI Joey)  
* Slide guitar (Snake enemies)  
* Fiddle (Ballet Joey)  
* Washboard percussion (Helium Joey)

**Suburb Biome** (C major, 115 BPM):
* Retro synth pad (ambient)  
* Electric bass (Mom)  
* Electronic drums (energy-based)  
* Synth lead (Ninja Joey)  
* Chiptune accent (GI Joey)  
* Bell synth (Ballet Joey)

**City Biome** (D minor, 125 BPM):
* Industrial ambient (machinery)  
* Heavy bass (Mom)  
* Industrial percussion (energy-based)  
* Distorted guitar (GI Joey)  
* Dark synth (Ninja Joey)  
* String stabs (Ballet Joey)

**Airstrip HQ** (E minor, 140 BPM):
* Military ambient (alarms, radio chatter)  
* Orchestral bass (Mom)  
* Epic drums (energy-based)  
* Brass stabs (GI Joey)  
* Full ensemble chaos (all Joeys + Kazoo)

---

## **Level Design & Progression**

### **World Structure**

```
Tutorial: Cargo Plane (Linear)
    |
Level 1: Swamp (Semi-Open)
    |
Level 2: Suburb (Hub-based)
    |
Level 3: Swamp Revisit (New areas unlocked)
    |
Level 4: City/Industrial (Vertical progression)
    |
Final Zone: Airstrip HQ (Gauntlet + Boss)
```

### **Level Breakdown**

#### **Tutorial: Cargo Plane Crash**

**Objectives:**
* Teach basic movement (walk, jump, climb)  
* Introduce Joey launch mechanic  
* Rescue Normal Joey and Lead Joey  
* Establish narrative (poachers, family separation)

**Music:** Frame Drum + Clapsticks + Didgeridoo (baseline)
**Length:** 5-10 minutes
**Gates:** None (linear tutorial)

---

#### **Level 1: Swamp**

**Environment:** Murky water, twisted trees, bioluminescent plants  
**Rescue:** Ballet Joey  
**New Mechanic:** Freeze ability for crossing water  
**Enemies:** Snakes, Gators, Feral Cats  
**Map Piece:** 1/5 found

**Collectibles:**
* Map Pieces: First piece found here  
* Animal Tokens: Hidden throughout level (affect ending)  
* Pouch Charms: Optional cosmetic collectibles

**Progression Gates:**
* Water crossing requires Helium Joey OR Ballet Joey freeze platforms  
* Heavy logs require Lead Joey

**Secrets:**
* Hidden alcove with health upgrade (requires Ballet Joey speed)  
* Musical room where standing still adds ambient layers

**Length:** 15-20 minutes

---

#### **Level 2: Suburb**

**Environment:** Abandoned neighborhood, houses, backyards  
**Rescue:** Ninja Joey  
**New Mechanic:** Stealth sections (avoid alerting hawks), vine cutting  
**Enemies:** Hawks, Domestic Dogs, Raccoons  
**Map Piece:** 2/5 found

**Progression Gates:**
* Vine walls require Ninja Joey  
* Rooftop traversal requires Helium Joey

**Secrets:**
* Secret basement with energy capacity upgrade  
* Gnome collectibles (Easter egg hunt)

**Length:** 20-25 minutes

---

#### **Level 3: Swamp Revisit**

**Environment:** Deeper swamp areas now accessible  
**Rescue:** Helium Joey  
**New Mechanic:** Floating platform puzzles, enemy inflation  
**Enemies:** Pythons, Panthers, Bobcats, more Snakes  
**Map Piece:** 3/5 found (reveals path to City)

**Progression Gates:**
* High platforms require Helium Joey  
* Underwater sections require Lead Joey weight

**Secrets:**
* All Gnomes from Level 2 unlock secret swamp shrine  
* Hidden Joey cosmetic skin

**Length:** 15-20 minutes

---

#### **Level 4: City/Industrial**

**Environment:** Urban decay, factories, warehouses  
**Rescue:** GI Joey  
**New Mechanic:** Explosive environmental destruction  
**Enemies:** Security Drones, Poacher Guards, Wild Boar, Wolverine  
**Map Piece:** 4/5 found (reveals path to Poachers' HQ)

**Progression Gates:**
* Reinforced doors require GI Joey  
* Vertical factory requires Helium Joey + Ballet Joey combo

**Secrets:**
* Factory music room (all instruments create unique composition)  
* Developer commentary logs

**Length:** 25-30 minutes

---

#### **Final Zone: Airstrip HQ**

**Environment:** Military base, hangars, control tower  
**Rescue:** Kazoo Joey  
**Boss Fight:** Poacher Leader + Guards (multi-phase)  
**Map Piece:** 5/5 found (reveals full map)

**Progression:**
1. Infiltration (stealth with Ninja Joey)  
2. Free captured animals (puzzle gauntlet)  
3. Rescue Kazoo Joey  
4. Boss fight (requires all Joey abilities)  
5. Escape sequence (timed platforming)

**Endings:**
* **Standard Ending:** All Joeys rescued, animals freed, poachers defeated  
* **True Ending:** All Joeys + all Animal Tokens collected, Normal Joey survives

**Length:** 30-40 minutes

---

### **Collectibles & Secrets**

| Collectible | Total | Purpose |
|-------------|-------|---------|
| Map Pieces | 5 | Required for revealing HQ location |
| Animal Tokens | Variable | Affect ending scenes and soundtrack |
| Pouch Charms | Variable | Cosmetic customization |
| Gnomes | 12 | Unlock secret music composition room |
| Joey Skins | 7 | Cosmetic only |
| Health Upgrades | 3 | +1 heart each |
| Energy Upgrades | 3 | +25 energy capacity |

---

## **Narrative**

### **Story Summary**

A family of Quokkas is being illegally transported by poachers when their cargo plane crashes. Mom escapes with two of her Joeys, but the others are scattered across different biomes. She must brave dangerous environments and hostile enemies to reunite her family and stop the poachers for good.

### **Themes**

* **Maternal Love:** A mother's unbreakable bond with her children  
* **Conservation:** Highlighting the real-world issue of wildlife trafficking  
* **Resilience:** Overcoming adversity through determination and cleverness  
* **Found Family:** Not just biological - all the rescued animals become a community

### **Characters**

| Character | Role | Personality | Arc |
|-----------|------|-------------|-----|
| Quokka Mom | Protagonist | Brave, resourceful, protective | Learns to trust her Joeys |
| Normal Joey | Map reader | Curious, earnest, naive | Grows from scared to courageous |
| Lead Joey | Protector | Serious, responsible | Learns when to be gentle |
| Helium Joey | Comic relief | Giggly, lighthearted | Proves their worth |
| GI Joey | Tactician | Disciplined, military jargon | Balances precision with warmth |
| Ninja Joey | Loner | Cool, speaks in haiku | Opens up to family |
| Ballet Joey | Perfectionist | Graceful, dramatic | Learns imperfection is okay |
| Kazoo Joey | Wildcard | TBD (Kickstarter backer) | TBD |
| Poacher Leader | Antagonist | Ruthless, pragmatic | Defeated and arrested |

### **Narrative Delivery**

* **Minimal cutscenes:** Intro (plane crash), midpoint (map completion), finale (liberation)  
* **Environmental storytelling:** Poacher camps, caged animals, propaganda posters  
* **Joey reactions:** Dialogue bubbles and animations convey personality  
* **Found audio logs:** Poacher communications reveal backstory (optional)

---

## **Technical Architecture**

### **Core Systems**

#### **1. Joey Management System**

```
JoeyDefinition (ScriptableObject)
+-- Stats (energy, cooldown, health)
+-- AbilityDefinition reference
+-- AudioClip reference (instrument stem)
+-- Material/Color settings
+-- Visual prefab reference

JoeyController (MonoBehaviour)
+-- Launch mechanics
+-- Energy tracking
+-- State machine (InPouch, Aiming, Launched, Depleted)
+-- Events (OnLaunched, OnRecalled, OnEnergyChanged)

PouchManager (Singleton/Service)
+-- Active Joey tracking
+-- Quick-swap logic
+-- UI updates
```

#### **2. Event System (ScriptableObject Event Channels)**

```
GameEventSO (ScriptableObject)
+-- OnJoeyRescued -> Unlock abilities, add music layer
+-- OnJoeyLaunched -> Music accent, UI update
+-- OnJoeyRecalled -> Restore volume
+-- OnEnemySpawned -> Add music layer
+-- OnEnemyDefeated -> Remove music layer
+-- OnNormalJoeyDeath -> Lock true ending flag
```

#### **3. Music System Architecture**

```
BiomeMusicData (ScriptableObject)
+-- BPM and loop settings
+-- Key signature
+-- Ambient/bass stems
+-- Energy-based drum stems
+-- Joey instrument stems array
+-- Enemy instrument stems array

MusicManager (Service)
+-- Tracks active music entities
+-- Manages Audio Mixer parameters
+-- Handles fade-in/fade-out
+-- Responds to game events
```

#### **4. Performance Optimizations**

* **Object Pooling:** Joeys, projectiles, particle effects (EasyPooling v2025)  
* **Occlusion Culling:** 2.5D layers culled based on camera view  
* **LOD System:** Background elements simplified when distant  
* **Audio Source Pooling:** Max 12 simultaneous instruments  
* **Async Operations:** UniTask for non-blocking operations

### **Code Quality Standards**

**SOLID Principles:**
* Single Responsibility: Each system has one clear purpose  
* Open/Closed: Extend via ScriptableObjects, not code rewrites  
* Liskov Substitution: All Joeys implement IJoeyBehavior  
* Interface Segregation: IJoeyAbility, IMusicEntity, etc.  
* Dependency Inversion: Systems depend on abstractions

**SOAP Architecture:**
* All configs as ScriptableObjects  
* Data-driven design  
* Event-based decoupling

**Performance First:**
* Cache all references  
* No allocations in gameplay loops  
* Profile early, optimize where needed
* Explicit types (no `var` keyword)

---

## **Art & Visual Style**

### **Art Direction**

**Target Aesthetic:** Hyper-cute stylized 3D with 2D presentation

**Key Features:**
* **Thick outlines** (toon shader) for readability  
* **Vibrant, saturated colors** for each biome  
* **Exaggerated proportions** (big eyes, small bodies)  
* **Expressive animations** for character personality

### **Biome Color Palettes**

| Biome | Primary Colors | Mood |
|-------|----------------|------|
| Cargo Plane | Grays, oranges (sunset) | Urgent, chaotic |
| Swamp | Greens, teals, bioluminescent blues | Mysterious, eerie |
| Suburb | Pastels, faded colors | Nostalgic, abandoned |
| City | Cool grays, neon accents | Industrial, oppressive |
| Airstrip | Military greens, harsh whites | Sterile, threatening |

### **Character Design**

* **Mom:** Warm browns, maternal proportions, pouch clearly visible  
* **Joeys:** Color-coded by ability (Lead=silver, Helium=light blue, GI=olive green, etc.)  
* **Enemies:** Desaturated, aggressive poses, red accent details  
* **Poachers:** Faceless/obscured to avoid humanizing them

### **Animation Priorities**

1. Joey launch/recall (core mechanic)  
2. Mom's movement (8-directional, smooth)  
3. Enemy attack patterns (telegraphed, readable)  
4. Environmental interactions (vines cut, platforms break)

### **UI/UX Design**

* **Minimalist HUD:** Health (hearts), active Joey portrait, energy bar  
* **Diegetic elements:** Map pieces appear in Mom's hand when viewing map  
* **Clear feedback:** Color flash on damage, audio stinger on success  
* **Accessibility:** Colorblind modes, remappable controls, text scaling

---

## **Target Audience & Platforms**

### **Primary Audience**

* **Age:** 10-35 years old  
* **Interests:** Platformers, Metroidvanias, cute characters, unique mechanics  
* **Comparable Games:** Hollow Knight, Celeste, Ori series, Kirby  
* **Motivations:** Creative gameplay, emotional stories, musical innovation

### **Secondary Audience**

* **Families:** Parents playing with children (E10+ rating)  
* **Music enthusiasts:** Drawn to adaptive music system  
* **Animal lovers:** Conservation themes resonate

### **Platform Priority**

1. **PC (Steam)** - Primary development target  
   * Hardware: Moderate specs (GTX 1050 equivalent or better)  
   * Controller + keyboard/mouse support  
2. **Android** - Second platform  
   * Touch controls adapted from mouse/controller  
   * Optimized audio for mobile performance  
3. **iOS** - Third platform  
   * Same as Android optimizations  
4. **Consoles (Nintendo Switch, PlayStation, Xbox)** - Stretch goal  
   * Post-launch, pending funding and success

### **Not Supported**

* **WebGL:** Music system and asset complexity too demanding

---

## **Asset Integration**

### **Recommended Asset Stack**

Based on December 2025 asset inventory review (2,094+ assets owned):

#### **Core Framework**
| Asset | Purpose | Priority |
|-------|---------|----------|
| **Corgi Engine** | Metroidvania framework OR reference | HIGH |
| **SOAP** | ScriptableObject architecture | HIGH |
| **Odin Inspector** | Better inspector UX | HIGH |

#### **Animation & Physics**
| Asset | Purpose | Priority |
|-------|---------|----------|
| **Animancer Pro v8** | State machine replacement | HIGH |
| **Dynamic Bone** | Quokka/joey physics | HIGH |
| **Ragdoll Animator 2** | Joey impact reactions | MEDIUM |
| **Toolkit for Verlet Motion** | Joey tether effects | LOW |

#### **Enemy AI**
| Asset | Purpose | Priority |
|-------|---------|----------|
| **SensorToolkit 2** | Sight/hearing detection | HIGH |
| **Blaze AI Engine** | Visual behavior editor | MEDIUM |

#### **Metroidvania Systems**
| Asset | Purpose | Priority |
|-------|---------|----------|
| **AA Map and Minimap System** | Exploration tracking | HIGH |
| **Dialogue System for Unity** | NPC interactions | MEDIUM |

#### **Visuals & Audio**
| Asset | Purpose | Priority |
|-------|---------|----------|
| **All In 1 Sprite Shader** | Visual effects | HIGH |
| **Smart Lighting 2D** | Atmosphere | MEDIUM |
| **Feel** | Game juice | HIGH |
| **Master Audio 2024** | Adaptive music | HIGH |
| **Stylized Nature MEGA PACK** | 2.5D backgrounds | MEDIUM |

#### **Utilities**
| Asset | Purpose | Priority |
|-------|---------|----------|
| **DOTween Pro** | Tweening | HIGH |
| **Easy Save** | Save system | HIGH |
| **Graphy** | Performance monitor | MEDIUM |
| **UniTask** | Async/await (GitHub) | HIGH |
| **EasyPooling v2025** | Object pooling | HIGH |

---

## **Development Status**

### **Current State (December 2025)**

**Completed:**
* [X] Project setup (Unity 6.2.2 URP, GitHub version control)  
* [X] Core player movement (walking, jumping, crouching)
* [X] Climbing system with stamina limitations
* [X] Input System integrated (New Input System)  
* [X] Joey prefab created (base visuals)  
* [X] Audio system decision made (stem-based)  
* [X] Nine Volt Audio bundle acquired (30,000 loops)
* [X] Ground detection converted to collision-based
* [X] Asset inventory reviewed and recommendations updated

**In Progress:**
* [ ] JoeyDefinition and AbilityDefinition ScriptableObjects (Sprint 1)
* [ ] Joey runtime controllers (Sprint 1)
* [ ] MVP launch mechanic (Sprint 1)
* [ ] Material and color systems for joeys (Sprint 1)
* [ ] Audio event wiring (Sprint 1)

**Not Started:**
* [ ] Full Joey ability implementations  
* [ ] Enemy AI  
* [ ] Music Manager system  
* [ ] Level art and assets  
* [ ] Combat system  
* [ ] Save/load system
* [ ] Map/minimap system

### **Technical Setup**

| Component | Version/Status |
|-----------|----------------|
| Unity | 6.2.2 URP |
| Cinemachine | 3.0 |
| Input System | New Input System |
| Version Control | GitHub |
| Architecture | SOAP + Event Channels |

### **Technical Debt & Decisions Needed**

* [ ] Finalize Corgi Engine evaluation (use as framework vs reference)
* [ ] Organize Nine Volt Audio stems by key/tempo for Swamp biome  
* [ ] Build first BiomeMusicData asset for Swamp  
* [ ] Implement MusicManager with fade system  
* [ ] Establish animation pipeline

---

## **Development Roadmap**

### **Phase 1: Foundation (Current)**

* [X] Project setup (Unity, version control)  
* [X] Core player movement  
* [X] Basic climbing system
* [X] Audio system decision (stems)  
* [ ] ScriptableObject architecture (Joey definitions, abilities)  
* [ ] Event system implementation  
* [ ] MVP joey launch mechanic
* [ ] Tutorial level whitebox

**Milestone:** Playable tutorial with 2 Joeys and basic music

---

### **Phase 2: Systems Expansion**

* [ ] All 7 Joey abilities implemented  
* [ ] Energy/cooldown system  
* [ ] Pouch management UI  
* [ ] Enemy AI (2-3 enemy types)  
* [ ] Combat feedback (VFX, SFX)  
* [ ] Music Manager (3+ instruments layering from stems)

**Milestone:** Full combat loop functional with music integration

---

### **Phase 3: Level Creation**

* [ ] Swamp level (Level 1) complete  
* [ ] Suburb level (Level 2) complete  
* [ ] Art pass on all environments  
* [ ] Collectibles implementation  
* [ ] Map system  
* [ ] Save/load system

**Milestone:** 2 full levels playable start to finish

---

### **Phase 4: Content Complete**

* [ ] City level (Level 4) complete  
* [ ] Swamp revisit (Level 3) complete  
* [ ] Airstrip HQ (Final) complete  
* [ ] Boss fight implementation  
* [ ] All music layers complete (stem-based)  
* [ ] Narrative cutscenes  
* [ ] All collectibles placed

**Milestone:** Full game playable, alpha testing begins

---

### **Phase 5: Polish & Testing**

* [ ] Playtesting (internal + external)  
* [ ] Bug fixing and optimization  
* [ ] Balancing (difficulty, energy costs, enemy placement)  
* [ ] Audio mixing and mastering  
* [ ] Accessibility features  
* [ ] Achievements/trophies  
* [ ] Localization (if funded)

**Milestone:** Release Candidate 1

---

### **Phase 6: Launch & Post-Launch**

* [ ] PC (Steam) release  
* [ ] Marketing push  
* [ ] Community management  
* [ ] Patch support  
* [ ] Android/iOS ports (if funded)  
* [ ] Console ports (stretch goal)  
* [ ] DLC planning (if successful)

**Milestone:** v1.0 Live

---

## **Success Metrics**

### **Critical Success Factors**

1. **Joey launch feels satisfying** - core mechanic must be tight and responsive  
2. **Music integration is noticeable** - players recognize and appreciate the dynamic soundtrack  
3. **Joeys have personality** - players care about rescuing them  
4. **Difficulty curve is fair** - challenging but not frustrating  
5. **Replayability exists** - true ending, speedruns, different Joey strategies

### **Key Performance Indicators (KPIs)**

**Development Phase:**
* Playtest feedback scores (7+/10 average)  
* Bug count trending downward  
* Performance (60 FPS on target hardware)

**Launch Phase:**
* Steam reviews (75%+ positive)  
* Sales (10,000 units in first month)  
* Media coverage (5+ major outlets)

**Long-term:**
* Active player retention (30% at 3 months)  
* Community engagement (Discord, fan art, speedruns)  
* Platform expansion viability

---

## **Document Control**

**Created:** October 4, 2025  
**Last Updated:** December 4, 2025  
**Version:** 3.0  
**Author:** TecVooDoo Development Team  
**Status:** Living Document

**Changes in v3.0:**
* **Updated Unity version** to 6.2.2 URP
* **Added Asset Integration section** with recommended asset stack from inventory review
* **Added Corgi Engine** as framework/reference option
* **Updated Development Status** to reflect current progress (climbing, ground detection)
* **Added technical decisions** (collision-based ground detection, GitHub migration)
* **Integrated asset recommendations** (SensorToolkit 2, AA Map System, Dynamic Bone, etc.)
* **Updated Technical Architecture** with performance optimization notes
* **Added code quality standards** (explicit types, no `var` keyword)

**Changes in v2.0:**
* Consolidated all project documents into single master GDD  
* Updated Music System to reflect stem-based approach using Nine Volt Audio  
* Added comprehensive Enemy Design section with all biome enemies  
* Added Ballet Joey bird bait mechanic and predator behavior systems  
* Updated Joey roster (GI Joey replaces Fireball Joey)  
* Added enemy stun durations and size limits for Helium Joey

**Next Review Date:** After Phase 1 completion (Joey MVP)

---

*This GDD is a living document and will evolve as development progresses and playtesting reveals insights. All systems are designed with SOLID principles and performance-first architecture to ensure scalability and reusability.*
