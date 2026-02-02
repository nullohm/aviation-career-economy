# Aviation Career Economy Manual

> Build your airline empire in Microsoft Flight Simulator 2024

---

## Contents

| | | |
|:---|:---|:---|
| **[Getting Started](#getting-started)** | **[Aircraft Management](#aircraft-management)** | **[FBO](#fbo)** |
| [Prerequisites](#prerequisites) | [Hangar](#hangar) | [Map-based Overview](#map-based-overview) |
| [Game Start](#game-start) | [Status Color Coding](#status-color-coding) | [Distance Measure Tool](#distance-measure-tool) |
| | [Aircraft Market](#aircraft-market) | [FBO Types](#fbo-types) |
| | [Maintenance](#maintenance) | [Terminal Sizes](#terminal-sizes) |
| | | [Services](#services) |
| | | [Passive Income](#passive-income) |
| | | [Network Bonus](#network-bonus) |
| **[Flight Planning](#flight-planning)** | **[Scheduled Routes](#scheduled-routes)** | **[Pilot Management](#pilot-management)** |
| [Step-by-Step Guide](#step-by-step-guide) | [How It Works](#how-it-works) | [Rank System](#rank-system) |
| [Route Info Badge](#route-info-badge) | [Routes on the Map](#routes-on-the-map) | [Type Ratings](#type-ratings) |
| [Suggested Routes](#suggested-routes) | [Earnings Bonus](#earnings-bonus) | [Your Player Pilot](#your-player-pilot) |
| [Interactive Map](#interactive-map) | [Slot Limits](#slot-limits) | |
| [Range Circle](#range-circle) | [FBO Type Restrictions](#fbo-type-restrictions) | |
| [Airspace Overlay](#airspace-overlay) | [Aircraft Sizes & Terminal Compatibility](#aircraft-sizes--terminal-compatibility) | |
| [Map Attribution](#map-attribution) | [Troubleshooting: "No Aircraft"](#troubleshooting-no-aircraft-in-selection) | |
| [Completing a Flight](#completing-a-flight) | | |
| **[Finance System](#finance-system)** | **[SimConnect Integration](#simconnect-integration)** | **[Achievements](#achievements)** |
| [Revenue Formula](#revenue-formula) | [Flight Phases](#flight-phases) | [Categories](#categories) |
| [Cost Breakdown](#cost-breakdown) | [Dashboard Display](#dashboard-display) | [Tiers](#tiers) |
| [Loans](#loans) | [Anti-Cheat](#anti-cheat) | |
| [Monthly Billing](#monthly-billing) | | |
| **[Statistics](#statistics)** | **[Settings](#settings)** | **[Tips for Beginners](#tips-for-beginners)** |
| [Available Tabs](#available-tabs) | [Categories](#categories-1) | [Growth Benchmarks](#growth-benchmarks) |
| [Flight Map Legend](#flight-map-legend) | [Aircraft Load Factor](#aircraft-load-factor) | |
| **[Developer Tools](#developer-tools)** | **[Files and Customization](#files-and-customization)** | **[Sound System](#sound-system)** |
| [Funds](#funds) | [Important Paths](#important-paths) | [Sound Events](#sound-events) |
| [Simulation](#simulation) | [Little Navmap Setup](#little-navmap-setup) | [Custom Sounds](#custom-sounds) |
| | **[Pilot Customization](#pilot-customization)** | |
| | [Template System](#template-system) | |
| | [Pilot Fields](#pilot-fields) | |

---

## Getting Started

Welcome to Aviation Career Economy! This application is an airline simulation for Microsoft Flight Simulator 2024. Build your own airline, buy aircraft, hire pilots, and track your flights.

### Prerequisites

| Requirement | Status |
|-------------|--------|
| Little Navmap | **Required** for airport data (45,000+ airports) |
| MSFS 2024 | Optional - app works standalone with test aircraft |
| Save System | Automatic - progress saved in local database |

### Game Start

You start with **€0 and no aircraft**. This is intentional - you build your airline from scratch!

**First Steps:**

| Step | Action |
|:----:|--------|
| 1 | Go to **Settings → Game → Developer Tools** |
| 2 | Use **Capital Injection** to add starting funds (e.g., €100,000) |
| 3 | Go to **Market** and buy your first aircraft |
| 4 | Your first pilot is already available (yourself) |

> **Tip:** Start with a small aircraft like a Cessna 172 (~€50,000) to learn the basics, then expand!

---

## Flight Planning

Create flight plans to earn money through passenger or cargo transport. Each flight generates revenue based on distance, passenger count, and aircraft type.

### Step-by-Step Guide

| Step | Action |
|:----:|--------|
| 1 | Select an aircraft from your hangar |
| 2 | Enter departure ICAO (e.g., `EDDF` for Frankfurt) |
| 3 | Enter arrival ICAO (e.g., `EDDM` for Munich) |
| 4 | Choose passenger or cargo mission |
| 5 | Click **Create Flight Plan** |

### Route Info Badge

The toolbar shows live route information:

| Info | Description |
|------|-------------|
| Distance | Route length in NM |
| Course | Bearing in degrees |
| ETA | Estimated flight time |
| Revenue | Expected earnings |

### Suggested Routes

The "Suggested Routes" tab shows routes from your departure airport to your FBOs. Click on a route to automatically fill in the ICAO codes.

### Interactive Map

The full-screen map shows all airports with unified color coding:

| Color | Airport Type |
|-------|--------------|
| Yellow | Your FBOs (with label) |
| Purple | Large airports (> 8000 ft runway) |
| Dark Blue | Medium-large airports (6000-8000 ft) |
| Blue | Medium airports (4000-6000 ft) |
| Light Blue | Small airports (2000-4000 ft) |
| Very Light | Grass strips (< 2000 ft) |

**Right-click** on an airport opens a context menu to set as departure or arrival, plus "Airport Info" for details.

### Range Circle

When a departure airport is selected, a blue circle shows the maximum range of the selected aircraft. The circle uses geodesic (great circle) calculation for accurate distance representation.

### Airspace Overlay

Toggle airspace visualization to see controlled airspace zones. Can be enabled/disabled in **Settings → General**.

### Map Attribution

| Map Style | Attribution |
|-----------|-------------|
| Street | © OpenStreetMap contributors, © CartoDB |
| Terrain | © OpenStreetMap contributors, © OpenTopoMap |
| Satellite | © Esri, Maxar, Earthstar Geographics |

### Completing a Flight

| Mode | How to Complete |
|------|-----------------|
| **With MSFS** | Fly the route - automatically detected and completed |
| **Without MSFS** | Click "Complete Flight Plan" for manual completion |

---

## Aircraft Management

### Hangar

Your fleet is managed in the Hangar with a modern two-column layout.

**Toolbar:** Status filter, search field, stats badge (aircraft count, fleet value)

**Details Panel Shows:**
- Size, Location, Flight Hours, Range, Value, Passengers
- Pilot and FBO assignments
- Actions: Details, Maintenance, Sell, Delete

### Status Color Coding

| Status | Color | Description |
|--------|-------|-------------|
| Available | 🟢 Green | Ready for flights |
| InFlight | 🔵 Blue | Currently in use |
| Maintenance | 🟠 Orange | Under maintenance |
| Stationed | 🟣 Purple | Stationed at FBO |
| Grounded | 🔴 Red | Out of service |

### Aircraft Market

Buy new aircraft in the Market. The **Estimated Daily Profit** shows how much this aircraft would earn per day when operated.

| Feature | Description |
|---------|-------------|
| Search | Filter by name, manufacturer, or type |
| Favorites | Star icon to mark favorites, toggle button to show favorites only |
| Sorting | Sort by name, passengers, cargo, price, profit/h, or ROI (profit/h relative to purchase price) |
| Badges | **New** (blue) for modern aircraft, **Classic** (gold) for oldtimers |
| Prices | Realistic used prices (2024/2025 market data) |
| Affordability | "Can't afford" badge when over budget, "Sell to afford" badge when selling fleet aircraft would cover the price |

### Maintenance

Aircraft require regular maintenance. Intervals and costs differ by category:

**GA Aircraft (Light Aircraft)**

| Check | Cost | Duration |
|-------|------|----------|
| 50h Check | €450 | 4 hours |
| 100h Check | €1,200 | 8 hours |
| Annual Inspection | €3,500 | 2 days |
| Engine TBO | €25,000 | 14 days |

**Commercial Aircraft**

| Check | Cost | Duration |
|-------|------|----------|
| A-Check | €15,000 | 1 day |
| B-Check | €50,000 | 3 days |
| C-Check (Heavy) | €500,000 | 14 days |
| D-Check (Overhaul) | €3,500,000 | 42 days |

---

## FBO

Fixed Base Operators (FBOs) are your bases at airports. Rent FBOs to station aircraft and generate passive income.

### Map-based Overview

| Element | Display |
|---------|---------|
| Your FBOs | Colored rectangles |
| Other Airports | Circles (size/color by runway) |
| Legend | Symbol meanings |
| Scale | Distance in NM |

### Distance Measure Tool

1. Click the ruler icon in the toolbar
2. Click on the map to set start point
3. Click again to set end point
4. Panel shows distance (NM) and bearing (°)
5. Click "Clear" to reset

### FBO Types

| Type | Monthly Rent | Route Slots |
|------|--------------|-------------|
| Local | €500 | 2 |
| Regional | €1,500 | 5 |
| International | €5,000 | 10 |

### Terminal Sizes

| Size | Aircraft Types |
|------|---------------|
| Small | Single-engine propellers (C172, Piper) |
| Medium | Twin-engine, small jets (King Air, Citation) |
| MediumLarge | Regional aircraft (ATR, E-Jets) |
| Large | Narrow-body jets (A320, B737) |
| VeryLarge | Wide-body aircraft (A350, B777, B747) |

### Services

| Service | Description |
|---------|-------------|
| Refueling | Fuel service |
| Hangar | Protected storage |
| Catering | In-flight catering |
| Ground Handling | Ground support |
| De-Icing | Winter operations |

### Passive Income

> **Setup:** FBO → Aircraft → Pilot = Daily Revenue

1. Rent an FBO at an airport
2. Station an aircraft at the FBO
3. Assign a pilot to the aircraft

Revenue depends on aircraft size, passenger capacity, and route length.

### Network Bonus

| Bonus | Condition |
|-------|-----------|
| +20% | Both airports are your FBOs |

**Tip:** Open FBOs at strategically important airports that can be well connected!

---

## Scheduled Routes

Create scheduled connections between your FBOs for automated flight operations with bonus revenue.

### How It Works

| Step | Action |
|:----:|--------|
| 1 | Right-click FBO → "Create Route from..." |
| 2 | Select destination FBO |
| 3 | Assign compatible aircraft |
| 4 | Receive +30% Daily Earnings bonus |

### Routes on the Map

| Line Style | Meaning |
|------------|---------|
| Green solid | Both FBOs full, aircraft assigned |
| Yellow solid | Free slots, aircraft assigned |
| Dashed | No aircraft assigned |

### Earnings Bonus

| Bonus Type | Amount | Condition |
|------------|--------|-----------|
| Route Bonus | +30% | Aircraft assigned to scheduled route |
| Service Bonus | +30% | Manual flights on scheduled route |
| Network Bonus | +20% | Both airports are your FBOs |

**Bonuses stack for maximum earnings!**

### Slot Limits

**FBO Slots (by type):**

| FBO Type | Max Routes |
|----------|------------|
| Local | 2 |
| Regional | 5 |
| International | 10 |

**FBO Pair Limit:** Max 2 routes between two specific FBOs (adjustable)

### FBO Type Restrictions

| FBO Type | Route Range |
|----------|-------------|
| Local | Same country only |
| Regional | Same region (Europe, NA, etc.) |
| International | Worldwide |

### Aircraft Sizes & Terminal Compatibility

| Size | Passengers | Examples | Terminal |
|------|------------|----------|----------|
| Small | 1-4 | C172, DA40, SR22 | Small |
| Medium | 5-19 | Citation CJ4, PC-12 | Medium |
| MediumLarge | 20-100 | E190, ATR 72 | MediumLarge |
| Large | 101-350 | A320, B737, B787 | Large |
| VeryLarge | 351+ | A380, B747 | VeryLarge |

> **Important:** The system checks the SMALLER terminal of both FBOs!

### Troubleshooting: "No Aircraft" in Selection

| Issue | Solution |
|-------|----------|
| Terminal too small | Upgrade terminal at destination FBO |
| Wrong location | Aircraft must be at ORIGIN FBO |
| No pilot | Assign a pilot to the aircraft |
| Already assigned | Aircraft is on another route |
| Out of range | Route exceeds aircraft range |

---

## Pilot Management

Hire pilots to fly your aircraft. Pilots gain experience and advance in ranks.

### Rank System

| Rank | Hours Required | Salary Bonus |
|------|----------------|--------------|
| Junior | 0+ | Base |
| Senior | 500+ | +15% |
| Captain | 1,500+ | +30% |
| Senior Captain | 3,000+ | +50% |
| Chief Pilot | 5,000+ | +75% |

### Type Ratings

Pilots can only fly aircraft for which they have a type rating. More experienced pilots have more type ratings.

### Multi-Crew System

Aircraft can have multiple pilots assigned simultaneously. The Hangar detail panel shows a "Crew" section where you can assign and remove pilots.

**Crew Requirement** (Settings → Pilots): When enabled, aircraft need at least the minimum crew count (defined per aircraft type) to generate passive income. If crew is insufficient, the aircraft earns nothing.

**Multi-Crew Shift Operations** (Settings → Pilots): When enabled, assigning multiple crews to one aircraft simulates shift operations, increasing daily flight hours:

```
Effective Hours = min((Assigned Pilots / Required Crew) × Hours per Day, 24)
```

Example: A Boeing 737 (Crew: 2) with 4 pilots assigned and 8h/day setting → (4/2) × 8 = 16 hours/day.

The crew status badge in the aircraft list shows the current assignment (e.g., "Crew 2/2") with color coding: green when complete, orange when understaffed.

**Utilization Badge:** Each aircraft with assigned pilots shows a utilization percentage badge indicating how much of the 24-hour day is covered by flight operations. Color coded: green (90%+), blue (50-89%), orange (below 50%).

### Your Player Pilot

The first pilot is your player character (cannot be fired).

| Customization | Path |
|---------------|------|
| Pilot Data | `Savegames/Current/Pilots/Pilots.json` |
| Pilot Image | `Savegames/Current/Pilots/Pictures/myPilot.bmp` |

---

## Finance System

Manage your airline's finances carefully.

### Revenue Formula

```
Revenue = Passengers × Distance (NM) × Rate
```

| Aircraft Size | Rate per PAX/NM |
|---------------|-----------------|
| Small | €1.20 |
| Medium | €1.50 |
| MediumLarge | €1.70 |
| Large | €1.85 |
| VeryLarge | €2.00 |

### Cost Breakdown

| Cost Type | Calculation |
|-----------|-------------|
| Fuel | Consumption × Hours × Price/gal |
| Maintenance | Hourly rate × Flight hours |
| Insurance | % of aircraft value (annual) |
| Depreciation | Annual rate (daily deduction) |
| Crew | Monthly salary / Operating days |
| Landing Fees | By aircraft size (€50 - €1,500) |
| Catering | Per passenger |

### Loans

| Parameter | Value |
|-----------|-------|
| Interest Rate | 10% |
| Repayment | Any time |

### Monthly Billing

On the **1st of each month**:
- FBO rents and service costs
- Pilot salaries (base + rank bonus)

---

## SimConnect Integration

Connect to MSFS 2024 for automatic flight detection and real-time data.

### Flight Phases

| Phase | Detection |
|-------|-----------|
| Ready | On ground, engines off |
| Taxi | On ground, moving |
| Climbing | In air, positive climb rate |
| Cruise | Stable altitude |
| Descending | Negative climb rate |
| Landed | On ground after flight |
| At Destination | Arrived at destination |

### Dashboard Display

The dashboard features instrument-style gauges (tall, rounded rectangles) showing live flight data:

| Gauge | Information |
|-------|-------------|
| ALT | Current altitude (ft) |
| GS | Ground speed (kts) |
| HDG | Current heading (deg) |
| VS | Vertical speed (fpm) |
| AOA | Angle of Attack (stall warning) |
| FUEL | Fuel level (%) |
| FF | Fuel flow (GPH) |
| SIM | Simulation rate |

**AOA Warnings:**
- 🟡 Yellow: AoA > 12° (approaching stall)
- 🔴 Red: AoA > 15° or active stall warning

### Anti-Cheat

When simulation rate > 1x:
- Actual flight time is not counted
- Estimated time (Distance ÷ Cruise speed) is used instead

---

## Achievements

Track your progress with **43 achievements** in **7 categories**.

### Categories

| Category | Examples |
|----------|----------|
| Flights | First Flight → Mile High Club (1000) |
| Distance | Short Hop (100 NM) → World Traveler (100,000 NM) |
| Fleet | Size and value milestones |
| Finance | Balance and revenue goals |
| FBOs | Network building progress |
| Pilots | Team size and flight hours |
| Special | Landing quality (Butter, Perfect, Good) |

### Tiers

| Tier | Level |
|------|-------|
| 🥉 Bronze | Entry |
| 🥈 Silver | Intermediate |
| 🥇 Gold | Advanced |
| 💎 Platinum | Master |

---

## Statistics

The Statistics section provides comprehensive insights into your airline performance.

### Available Tabs

| Tab | Content |
|-----|---------|
| **Earnings** | Total earnings, revenue, costs, profit margin, daily chart |
| **Flights** | Flight history, distance, duration, landing rate |
| **Flight Map** | Routes and visited airports |
| **Fleet** | Fleet value, hours, maintenance, category distribution |
| **Finance** | Revenue, expenses, monthly profit trend |
| **Pilots** | Employees, salaries, assignments |

### Flight Map Legend

| Element | Meaning |
|---------|---------|
| Orange dashed curves | Completed flights (great circle arcs) |
| Green/Blue markers | Visited airports |
| Magenta route | Active flight plan |
| Red triangle | Your position in MSFS |

---

## Settings

Almost all economic parameters are adjustable.

### Categories

| Category | Parameters |
|----------|------------|
| **General** | Theme, Flight Plan options |
| **Economy** | Revenue rates, load factors, fuel costs, bonuses |
| **FBO** | Rents, terminal costs, service costs |
| **Aircraft** | Maintenance, insurance, depreciation |
| **Pilots** | Salary, training, rank system, crew requirements |
| **Game** | Achievement rewards, Developer Tools |

### Aircraft Load Factor

Control the average utilization of your aircraft capacity:

| Setting | Description | Default |
|---------|-------------|---------|
| **Passenger Load** | Percentage of seats filled | 100% |
| **Cargo Load** | Percentage of cargo capacity used | 100% |

Lower values simulate realistic airline operations (typically 80-85%). Revenue is reduced proportionally while operating costs remain unchanged.

---

## Tips for Beginners

| # | Tip |
|---|-----|
| 1 | **Inject capital first** - Use Developer Tools to add starting funds |
| 2 | **Buy a small aircraft** - Start with a Cessna 172 or similar (~€50k) |
| 3 | **Fly short routes** - Build experience and capital |
| 4 | **Save for first FBO** - Enables passive income |
| 5 | **Hire pilots** - Assigned pilots generate money |
| 6 | **Watch maintenance** - Skipped = grounded |
| 7 | **Go bigger** - Large aircraft are more efficient |

### Growth Benchmarks

*At ~5 hours playtime per week:*

| Time | Aircraft | Fleet Value |
|------|----------|-------------|
| 1 Year | 10-20 | ~€2B |
| 2 Years | 40-60 | ~€10B |
| 3 Years | 80-100 | ~€22B |

---

## Developer Tools

In **Settings → Game** you'll find developer tools for testing.

### Funds

| Action | Effect |
|--------|--------|
| Add | Capital injection |
| Remove | Capital withdrawal |

### Simulation

| Button | Effect |
|--------|--------|
| Simulate 1/3/7 Days | Calculate passive income |

---

## Files and Customization

### Important Paths

| File | Purpose |
|------|---------|
| `Savegames/Current/ace.db` | Database (all data) |
| `Savegames/Current/Images/Pilots/` | Pilot images |
| `Savegames/Current/Images/Aircraft/` | Custom aircraft images |

### Little Navmap Setup

1. Install Little Navmap (free)
2. **Scenery Library → Load Scenery Library**
3. Select Microsoft Flight Simulator 2024
4. Wait for scan completion

Database location:
```
%APPDATA%\ABarthel\little_navmap_db\little_navmap_msfs24.sqlite
```

---

## Pilot Customization

### Template System

| Type | Location | Purpose |
|------|----------|---------|
| Template | `Pilots/` | Read-only backup |
| User | `Savegames/Current/Pilots/` | Editable copy |

### Pilot Fields

| Field | Format |
|-------|--------|
| Name | Full name |
| CallSign | Radio callsign |
| FlyingTime | `Days.Hours:Minutes:Seconds` |
| FlyingDistance | NM |
| PicturePath | Relative path |
| BirthDate | ISO 8601 |
| EarnedLicenses | Array of IDs |
| EarnedTyperatings | Array of names |

**Supported Images:** BMP, JPG, PNG (file extension is auto-detected, so the database entry doesn't need to match the actual file extension)

> **Warning:** Never remove the first pilot entry!

---

## Sound System

### Settings

Located in **Settings → General**:
- **Sound Enabled:** On/Off toggle
- **Volume:** 0-100%

### Sound Events

| Event | Trigger |
|-------|---------|
| Flight Completed | Flight successfully finished |
| Achievement | New achievement unlocked |
| Top of Descent | Approaching descent point |
| Warning | Important warnings |
| Notification | General notifications |
| Button Click | UI interactions |

### Custom Sounds

Place files in `Data/Sounds/`:

| Event | Filename |
|-------|----------|
| Flight Completed | `flight_completed.mp3` / `.wav` |
| Achievement | `achievement.mp3` / `.wav` |
| Top of Descent | `top_of_descent.mp3` / `.wav` |
| Warning | `warning.mp3` / `.wav` |
| Notification | `notification.mp3` / `.wav` |
| Button Click | `click.mp3` / `.wav` |

**Supported formats:** MP3, WAV
