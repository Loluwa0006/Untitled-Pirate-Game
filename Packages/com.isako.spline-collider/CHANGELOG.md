## [1.0.0] - 2026-02-09

### Added
- Initial release of the Spline Collider package.
- Automatic generation of segmented capsule colliders along splines.
- Support for distance-based and count-based sampling modes.
- Post-processing system for segment merging and subdivision.
- Aggregated trigger and collision handling across spline segments.
- Unified physics callbacks for enter, stay, and exit events.
- C# event-based API (`Action<Collider>`, `Action<Collision>`).
- Optional UnityEvents bridge component for designer-friendly workflows.
- Custom inspector with validation and bake/clear actions.
- Editor-time undo support for baking and clearing.
- Internal proxy system for forwarding physics callbacks.
- Cached and hierarchy-based baked-state detection.
- Support for Unity 2022.3 LTS and later.
- Dependency on `com.unity.splines` and `com.unity.mathematics`.

### Changed
- N/A

### Fixed
- N/A

### Removed
- N/A
