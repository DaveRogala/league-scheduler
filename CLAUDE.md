# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

All commands run from the `LeagueScheduler/` solution root (the directory containing the three `.csproj` files).

```bash
# Build
dotnet build

# Run the server (serves both API and Blazor WASM)
dotnet run --project LeagueScheduler/LeagueScheduler.csproj

# Run tests (none yet — add xUnit projects under LeagueScheduler/)
dotnet test
```

The server starts at `http://localhost:5227` (HTTP) or `https://localhost:7267` (HTTPS). The Blazor WASM client is served as static assets by the server project; there is no separate client dev server.

## Architecture

This is a Blazor Web App with WebAssembly interactivity on .NET 9, structured as three projects:

- **`LeagueScheduler`** — ASP.NET Core server. Hosts Blazor components server-side, serves the WASM bundle, and exposes a minimal API (`POST /api/scheduler/compute`).
- **`LeagueScheduler.Client`** — Blazor WebAssembly project. Runs in the browser. Contains the interactive UI pages and a typed `SchedulerClient` HTTP wrapper.
- **`LeagueScheduler.Shared`** — Class library with all DTOs shared between server and client. No dependencies on ASP.NET or WASM — safe to use on both sides.

### Scheduling domain

The core scheduling logic lives entirely in `LeagueScheduler/Services/SchedulerService.cs`. The algorithm:

1. Expands `LeagueDto.StartDate`..`EndDate` by `DaysOfWeek`, minus `NonPlayDates`, to produce eligible play dates.
2. Computes a target slot count per player from `PlayerDto.PreferencePercent` × total available slots.
3. Greedily fills each match by picking the player with the lowest `assignedCount / target` ratio, giving `OnCall` players last priority and applying a `NudgePreference` tiebreaker (±0.01 to the ratio).
4. After filling all matches, validates each player's assigned count against `FairnessTolerance` (default 1, configurable via `appsettings.json` → `Scheduling:DefaultFairnessTolerance`).
5. Persists the result as JSON to `Data/schedule_{id}.json` via `JsonScheduleRepository`.

`ScheduleOptions` (bound from the `Scheduling` config section) controls `DefaultFairnessTolerance` and `DataFolder`.

### Key DTOs (all records, in `LeagueScheduler.Shared/Models/`)

| Type | Purpose |
|---|---|
| `LeagueDto` | League definition: courts, match type, date range, non-play dates, pre-planned events |
| `PlayerDto` | Player: `PreferencePercent` (0–1), `Role` (Regular/AsNeeded/OnCall), `NudgePreference`, `UnavailableDates` |
| `ScheduleRequestDto` | API request: `League` + `Players` + optional `FairnessTolerance` override |
| `ScheduleResultDto` | API response: `Matches`, `AssignedCounts`, `TargetCounts`, `FairnessToleranceUsed`, `Conflicts` |
| `MatchDto` | Single match: `Date`, `Court`, `PlayerIds` (2 for singles, 4 for doubles) |
| `PrePlannedEventDto` | A court block or fixed match that the scheduler must honor (currently stored but not yet consumed by the algorithm) |

### UI

MudBlazor 8.x is used throughout. The only substantive page is `LeagueScheduler.Client/Pages/Scheduler.razor` — a minimal MVP that accepts raw `ScheduleRequestDto` JSON, calls the API via `SchedulerClient`, and displays results in a `MudTable`.

The server-side `MudBlazor.Services` registration and the client-side registration are done separately in their respective `Program.cs` files.
