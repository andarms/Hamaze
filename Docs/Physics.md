## ✅ Lightweight Physics System – TODO List

### 📦 Base Infrastructure

- [x] Create `Vector2 Position` and `Vector2 Velocity` for all movable entities.
- [x] Implement a `ColliderComponent` (AABB or circle).
- [ ] Create tilemap-based world collision (solid tiles).

---

### 🏃🏽‍♂️ Movement and Collision

- [x] Implement continuous player movement with `velocity * dt`.
- [x] Add basic **AABB collision detection** against solid tiles.
- [x] Add **collision resolution** to prevent passing through walls.
- [x] Implement sliding along walls (optional).

---

### 🧱 Obstacles and Entities

- [ ] Tag collidable entities (walls, enemies, objects).
- [ ] Add collision checks between player and other entities.
- [ ] Support one-way or pass-through areas (optional).

---

### 💥 Combat Interaction

- [x] Create `HitboxComponent` and `HurtboxComponent`.
- [x] Implement hitbox-overlap detection (e.g., sword swing vs enemy).
- [x] Connect to damage system (trigger damage and effects on hit).

---

### 🌀 Knockback / Pushback

- [ ] Add `KnockbackComponent` with direction and duration.
- [ ] Apply temporary velocity during knockback.
- [ ] Decay knockback over time (e.g., linear or easing).

---

### 🏹 Projectiles

- [ ] Implement `ProjectileComponent` with velocity and damage.
- [ ] Move projectile using velocity each frame.
- [ ] Check for collision with enemies and world.
- [ ] Destroy or deactivate projectile on impact.

---

### 🎒 Pickup / Throw (Optional)

- [ ] Detect pickup input near liftable object.
- [ ] Attach object to player while held.
- [ ] On throw, apply velocity in facing direction.
- [ ] Apply movement, collision, and impact logic to thrown object.

---

### ⬇️ Gravity / Falling (Optional)

- [ ] Add `GravityComponent` (apply vertical acceleration).
- [ ] Mark fall zones or holes in the map.
- [ ] Trigger fall state and reset position or health.

---

### 📦 Pushable Objects (Optional)

- [ ] Create `PushableComponent`.
- [ ] Detect directional push input from player.
- [ ] Move object in push direction with collision resolution.
- [ ] Trigger events (e.g., pressure plates).

---

### 🧪 Debugging Tools

- [ ] Visualize colliders and hitboxes in debug mode.
- [ ] Log collision and hit events for testing.
- [ ] Add toggle for slow-motion (to test physics frame-by-frame).

---

## ⏱️ Prioritization Suggestions

**Minimum Viable:**

- Movement
- Collision
- Combat hit detection

**Next:**

- Projectiles
- Knockback

**Polish:**

- Pushable objects
- Pickup/throw
- Gravity/falling
