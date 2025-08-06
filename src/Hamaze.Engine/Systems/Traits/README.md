### ✅ Trait Naming Conventions

| Prefix       | Purpose / Meaning                                                | Example Trait       | Use When...                                                   |
| ------------ | ---------------------------------------------------------------- | ------------------- | ------------------------------------------------------------- |
| **`Is`**     | Describes a **state**, **quality**, or **boolean-like property** | `IsSolid`           | The object has a passive or static property (e.g., physics).  |
|              |                                                                  | `IsPickupable`      | The object _can_ be picked up (identity/flag).                |
|              |                                                                  | `IsDestructible`    | The object can be destroyed (breakable).                      |
| **`Has`**    | Indicates a **contained system**, **feature**, or **component**  | `HasInventory`      | The object owns or manages a system or container.             |
|              |                                                                  | `HasHealth`         | The object tracks an internal state like HP or mana.          |
|              |                                                                  | `HasAI`             | The object uses a specific subsystem.                         |
| **`Can`**    | Expresses a **capability**, **action**, or **permission**        | `CanAttack`         | The object is _able_ to perform a dynamic action.             |
|              |                                                                  | `CanJump`           | The ability is context-sensitive or requires checks.          |
|              |                                                                  | `CanTrade`          | The object may perform this behavior when triggered.          |
| **`Should`** | Suggests a **behavioral hint**, **goal**, or **AI directive**    | `ShouldPatrol`      | The object _ought_ to do this when idle.                      |
|              |                                                                  | `ShouldAvoidPlayer` | The object has a preferred behavior under certain conditions. |
|              |                                                                  | `ShouldChaseTarget` | Used to guide AI or conditional systems.                      |
| **Noun**     | Defines an **identity**, **role**, or **type**                   | `Enemy`             | The object _is_ a type/category (used in tagging systems).    |
|              |                                                                  | `Vendor`            | The object plays a role in the game world.                    |
|              |                                                                  | `Projectile`        | System recognizes it by this role.                            |

---

### 🧠 Use Guidelines

- 🔵 **`Is`** → Static or boolean-like properties
- 🟢 **`Has`** → Ownership of a system or component
- 🟡 **`Can`** → Capabilities, dynamic actions
- 🟠 **`Should`** → AI or behavioral guidance (soft rules)
- 🟣 **Noun** → Identity or classification (tags/roles)
