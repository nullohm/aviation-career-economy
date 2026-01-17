# Aviation Career Economy

A flight management simulation application for Microsoft Flight Simulator 2024, built with WPF and .NET.

## Installation

### Download
1. Download the latest release from [Releases](../../releases)
2. Extract `aviation-career-economy-vX.X.X-win-x64.zip` to any folder
3. Run `Ace.App.exe`

**Requirements:** Windows 10/11 (64-bit) - No .NET installation required!

### Little Navmap Integration
Little Navmap is **required** for airport data (45,000+ airports):
1. Install [Little Navmap](https://albar965.github.io/littlenavmap.html)
2. Load MSFS 2024 scenery library
3. Aviation Career Economy will automatically detect the database

**Note**: Without Little Navmap, a warning dialog will be shown at startup and airport features will be unavailable.

## Features

- **SimConnect Integration** - Real-time connection to MSFS 2024 with flight tracking
- **Flight Planning** - Interactive map-based flight planner with route suggestions
- **Aircraft Management** - Fleet management with maintenance system
- **Aircraft Market** - Browse and purchase aircraft from MSFS 2024 catalog
- **FBO Management** - Rent and operate Fixed Base Operators at airports
- **Scheduled Routes** - Automated FBO-to-FBO connections with bonus revenue
- **Personnel Management** - Hire pilots with rank progression system
- **Finance System** - Revenue, costs, loans, and daily passive income
- **Statistics Dashboard** - Flight history, earnings charts, and fleet analytics
- **Achievements** - 43 achievements across 7 categories
- **Multi-Theme Support** - Dark and Light mode

See the in-app **User Manual** for detailed gameplay instructions.

## Technical Details

### Architecture
- **Framework**: .NET 9.0 with WPF
- **Database**: Entity Framework Core with SQLite
- **Data Persistence**: Automatic save via SQLite
- **Design Pattern**: MVVM with services layer

### Project Structure

```
aviation-career-economy/
├── Logs/               # Application logs
├── Data/               # Static data files
│   └── Sounds/         # Sound files (optional)
├── Pilots/             # Pilot template data
│   ├── Pilots.json
│   └── Pictures/
├── Savegames/          # Savegame folder
│   └── Current/        # Active savegame
│       ├── ace.db
│       ├── Images/     # Custom aircraft images
│       └── Pilots/     # User pilot data
├── Ace.App/
│   ├── Views/          # WPF Views
│   ├── ViewModels/     # MVVM ViewModels
│   ├── Models/         # Data models
│   ├── Services/       # Business logic
│   └── Data/           # Database context and repositories
└── Ace.App.Tests/      # Unit tests
```

## Development

### Build
```bash
dotnet build
```

### Run Tests
```bash
dotnet test
```

### Run with Coverage
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

### Third-Party Components

#### NuGet Packages
| Package | License | Description |
|---------|---------|-------------|
| Mapsui.Wpf | MIT | Interactive map control for WPF |
| Microsoft.EntityFrameworkCore.Sqlite | MIT | SQLite database provider |
| Microsoft.Extensions.DependencyInjection | MIT | Dependency injection framework |
| OxyPlot.Wpf | MIT | Charting and plotting library |
| MdXaml | MIT | Markdown rendering for WPF |
| xunit | Apache 2.0 | Unit testing framework |
| FluentAssertions | Apache 2.0 | Fluent assertion library |
| Moq | BSD | Mocking library for tests |

#### External Dependencies
| Component | License | Description |
|-----------|---------|-------------|
| SimConnect | MSFS SDK License | Microsoft Flight Simulator SDK |
| Little Navmap Database | GPL v3 | Airport and navigation data by Alexander Barthel |

#### Map Tile Providers
| Provider | License | Attribution |
|----------|---------|-------------|
| CartoDB | CC BY 3.0 | © OpenStreetMap contributors, © CartoDB |
| OpenTopoMap | CC BY-SA 3.0 | © OpenStreetMap contributors, © OpenTopoMap |
| Esri World Imagery | Esri ToU | © Esri, Maxar, Earthstar Geographics |

### Acknowledgments

Special thanks to:
- **Alexander Barthel** ([Little Navmap](https://albar965.github.io/littlenavmap.html)) - Airport database
- **OpenStreetMap contributors** - Map data
- **Mapsui Team** - Excellent WPF map control

---

**Aviation Career Economy** - Your Flight Management Companion for MSFS 2024
