# Zoo World

A small 3D prototype of a “zoo world” featuring a food chain, physical collisions, and a simple UI statistics system.

## Implemented Features

- Top-down camera view.
- Animal spawning every **1–2 seconds**.
- Random animal movement across the field.
- Screen-bound reaction: when leaving the screen, an animal changes direction and returns to the visible area.
- Physical collisions between animals.
- Food chain:
  - `Prey + Prey` → bounce apart using physics.
  - `Predator + Prey` → prey dies and disappears.
  - `Predator + Predator` → one survives, the other dies.
- `Tasty!` message appears after “eating”.
- UI counters for killed `prey` and `predator`.
- Animals:
  - **Frog** — `Prey`, jumping movement.
  - **Snake** — `Predator`, linear movement.

## Architecture

The project is built with a focus on scalability (designed for adding a large number of new animal types).

- **DI**: `VContainer`
- **Configuration**: `ScriptableObject` (entity descriptions and gameplay parameters). Configurations are built around abstractions; using SO as data storage is mainly a time-saving decision.
- **Separation of responsibilities**:
  - `Movement` — ;)
  - `Behaviour` — orchestrates the entity’s overall behavior
  - `ContactsResolver` — collision rules
  - `Session` — stores session data and provides root-level game flow control
  - `SimpleUI` — simplified MVP

## How to Run

1. Open the project in Unity (version 6.3.8f1).
2. Open the scene: `Assets/Scenes/SampleScene.unity`.
3. Press Play.

## Where to Find the Core Logic

- [Entry Point](Assets/MyAssets/Scripts/Gameplay/GameplayScopeEnterPoint.cs)
- [Spawning](Assets/MyAssets/Scripts/Gameplay/Entities/Spawn/)
- [Collisions and Food Chain](Assets/MyAssets/Scripts/Gameplay/Entities/ContactsResolver/)
- [Movement](Assets/MyAssets/Scripts/Gameplay/Movement/)
- [Session / Statistics](Assets/MyAssets/Scripts/Gameplay/Session/)
- [UI](Assets/MyAssets/Scripts/Gameplay/SimpleUI/)
