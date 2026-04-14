# Tower Defense Game

This project is a simple tower defense game built with C#, .NET 8, and MonoGame. The player protects the gate by placing towers on fixed build spots, earning coins from defeated enemies, and upgrading towers during the wave.

## Requirements

- .NET 8 SDK
- A desktop environment that can run MonoGame DesktopGL

## Run the game

Clone the repository:

```bash
gh repo clone DongKhanhBi24/Tower-Defense-Game
```

Move into the project folder:

```bash
cd Tower-Defense-Game/TowerDefenseGame
```

Run the game:

```bash
dotnet run
```

Notes:

- The repository root is `Tower-Defense-Game`, but the actual .NET project is inside the `TowerDefenseGame` folder.
- On the first run, `dotnet` may restore MonoGame tools and NuGet packages automatically.

## Basic Gameplay Flow

The game currently follows this flow:

1. Open the game and start from the main menu.
2. Press the `Start` button to enter the loading screen.
3. After loading finishes, the game switches to the main HUD gameplay screen.
4. Enemies begin spawning and walk along the path loaded from `path.txt`.
5. Build towers on valid tower spots to stop enemies before they reach the gate.
6. If the gate health reaches 0, the player loses.
7. If all configured waves are cleared, the player wins.

## Current Tower Defense Logic

This README is based on the current code in the project.

### Starting state

- The player starts with `1000` coins.
- The gate starts with `10` health.
- Enemy movement follows the coordinates in `TowerDefenseGame/path.txt`.
- The map has `8` fixed valid tower spots defined in `Map.cs`.

### Building towers

- Click an empty build spot to open the build selection UI.
- From there, you can build one of two tower types:
  - `Magic Tower` costs `100` coins.
  - `Barrack Tower` costs `75` coins.
- If you click outside the build buttons, build mode is cancelled.

### Tower types

#### Magic Tower

- Attacks enemies from range.
- Targets the first alive enemy within `250` units.
- Creates projectiles automatically.
- Attack speed improves with each upgrade:
  - Level 1: `1.2s` cooldown
  - Level 2: `1.0s`
  - Level 3: `0.8s`
  - Level 4: `0.6s`
- Levels 1 to 3 use the basic projectile.
- Level 4 uses an animated projectile with stronger damage.

#### Barrack Tower

- Spawns melee warriors to block and fight enemies.
- Levels 1 to 3 spawn `3` warriors.
- Level 4 spawns `4` elite warriors.
- If all warriors die, the tower respawns them after `5` seconds.
- Upgrading the tower replaces the current warriors with stronger ones.

### Upgrading towers

- Click an existing tower to select it.
- Click the upgrade icon above the tower to upgrade it.
- Each upgrade costs `125` coins.
- Towers can be upgraded up to level `4`.

### Enemy and wave logic

- Enemies are spawned from a queue.
- The current build spawns one enemy every `2` seconds.
- Right now, only wave 1 is enabled by default:
  - `5` Normal enemies
- The code already includes `Fast`, `Tank`, and `Mixed` wave types, but they are commented out in `WaveManager.cs`.
- If an enemy reaches the end of the path:
  - the gate loses `1` health
  - that enemy is removed

### Coins and rewards

- Killing enemies rewards the player with coins:
  - Normal enemy: `20`
  - Fast enemy: `30`
  - Tank enemy: `60`
- Coins are used for building and upgrading towers.

## Controls

- `Mouse left click`: interact with UI, select build spots, build towers, and upgrade towers
- `P`: pause or resume the game
- `Esc`: resume the game from the pause screen

## Project Structure

```text
Tower-Defense-Game/
├── README.md
└── TowerDefenseGame/
    ├── Program.cs
    ├── TowerDefenseGame.csproj
    ├── path.txt
    ├── Code/
    └── Content/
```

## Future Improvements

Some features are already partially prepared in the code and could be expanded later:

- enable more waves in `WaveManager.cs`
- balance tower costs and enemy rewards
- add more enemy types and maps
- improve UI instructions for new players
