<h1 align="center">Unity Spline Collider</h1>
<p align="center">
  <img src="Documentation~/Images/SplineCollider Preview.svg" width="256">
</p>

Spline Collider is a Unity editor and runtime tool that generates optimized,
segmented trigger and collision colliders along splines, with configurable
sampling, post-processing, and unified physics event handling.

It is designed to provide reliable physics interaction for spline-based paths,
rails, tracks, and volumes while maintaining good performance and editor
workflow.

---

## Features

- Automatic generation of capsule or box colliders along splines
- Distance-based and count-based sampling modes
- Post-processing for segment merging and subdivision
- Aggregated trigger and collision detection
- Unified enter, stay, and exit callbacks
- C# event-based API for programmers
- Optional UnityEvents bridge for designers
- Custom inspector with bake/clear workflow
- Editor undo support
- Works in edit mode and at runtime
- Compatible with Unity 2022.3 LTS and later

---

## Requirements

- Unity 2022.3 LTS or later
- com.unity.splines 2.4.0+
- com.unity.mathematics 1.2.0+

---

## Installation

### Using Unity Package Manager (Git URL)

1. Open Unity
2. Open **Window → Package Manager**
3. Click **+ → Add package from git URL**
4. Enter:
```bash
https://github.com/Isako-Sakuraba/Unity-Spline-Collider.git
```
---

## Quick Start

### 1. Create a spline

Create a spline using Unity’s Splines package:

**GameObject → Spline → (Any Spline)**

Edit the spline shape as needed.

---

### 2. Add SplineCollider

Add the `SplineCollider` component to the same GameObject:

**Add Component → Spline Collider → Spline Collider**


---

### 3. Configure sampling

Choose how colliders are generated:

- **By Distance**  
  Generates segments at fixed spacing

- **By Segment Count**  
  Generates a fixed number of segments

Adjust spacing/count as needed.

---

### 4. Configure post-processing

Optional post-processing improves collider quality:

- **Merge Shallow Bends**  
  Removes unnecessary segments on straight sections

- **Subdivide Sharp Bends**  
  Adds extra segments on tight curves

These options help reduce collider count while preserving accuracy.

---

### 5. Bake colliders

Click **Bake** in the inspector to generate segment colliders.

Use **Clear** to remove them.

Baked colliders are stored as child GameObjects under the configured root.

---

## Physics Interaction

Spline Collider aggregates physics callbacks from all segment colliders and
exposes unified events.

This ensures that a collider interacting with multiple segments is treated as
a single logical contact.

---

## C# Event API

You can subscribe to events directly:

```csharp
public class Example : MonoBehaviour
{
    [SerializeField] private SplineCollider splineCollider;

    private void OnEnable()
    {
        splineCollider.OnTriggerEnter += HandleEnter;
        splineCollider.OnTriggerExit += HandleExit;
    }

    private void OnDisable()
    {
        splineCollider.OnTriggerEnter -= HandleEnter;
        splineCollider.OnTriggerExit -= HandleExit;
    }

    private void HandleEnter(Collider other)
    {
        Debug.Log("Entered spline");
    }

    private void HandleExit(Collider other)
    {
        Debug.Log("Exited spline");
    }
}
```

### Available Events

**Trigger Events**
- `OnTriggerEnter`
- `OnTriggerStay`
- `OnTriggerExit`

**Collision Events**
- `OnCollisionEnter`
- `OnCollisionStay`
- `OnCollisionExit`

All events are aggregated across segments.

---

## Unity Events Support

For designer-friendly workflows, add `SplineColliderUnityEvents` to the same GameObject.

This exposes UnityEvents in the inspector that mirror the C# events.

Example:

**Add Component → Spline Collider → Spline Collider Unity Events**

You can then hook up responses without writing code.

---

## Baked State Queries
Spline Collider provides two ways to check if colliders are baked:

**Authoritative (Hierarchy-Based)**

`HasBakedColliders`
- Checks the scene hierarchy
- Accurate
- Slower
- Not recommended for per-frame use

**Cached (Internal Cache Based)**

`HasBakedCollidersCache`
- Uses internal cache
- Fast
- Cleared on domain reload
- Cached in `OnEnable`
- Recommended for runtime logic

---

## Performance Notes

- Collider count directly affects physics performance
- Prefer post-processing when using small spacing
- Avoid very high segment counts
- Use cached baked-state queries in hot paths
- Avoid frequent rebakes at runtime

For long splines, enabling merge and subdivision is strongly recommended.

---

## Runtime Baking
- Baking can be performed at runtime, but note:
- Baked objects created in Play Mode are discarded when exiting Play Mode
- For persistent baking, bake in Edit Mode
- Runtime baking is best suited for procedurally generated splines

---

## Limitations
- Does not merge contacts across different rigidbodies
- Contact data is aggregated per collider, not per segment
- Extremely dense splines may require manual optimization

---

## License

This project is licensed under the MIT License.

See the **LICENSE** file for details.

---

## Contributing

Contributions are welcome.

**Recommended workflow:**
- Fork the repository
- Create a feature branch
- Make changes
- Test locally
- Submit a pull request
- Please keep changes focused and well-documented.

---

## Credits

Developed by Isako Sakuraba.

Built on top of Unity's Splines package.
