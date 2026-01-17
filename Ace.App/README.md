# Aviation Career Economy - Airline Simulation for Microsoft Flight Simulator

WPF .NET 9 airline management simulation integrated with Microsoft Flight Simulator 2024.

## Implemented Features

### Core System
- **SimConnect Integration**: Automatic connection to MSFS 2024 with flight tracking
- **SQLite Database**: Persistent storage of all data (flights, finances, aircraft, settings)
- **Database Initialization**: Automatic initialization with starting pilot, license, aircraft and capital
- **Logging System**: Comprehensive logging of all operations
- **Settings Management**: Persistent app settings including window position
- **Airport Database**: Integration with Little Navmap for airport data

### Navigation & UI
- **Loading Window**: Startup screen with initialization status and error handling
- **Home**: Main menu with real-time SimConnect status display
- **Flight Plan**: Flight planning with aircraft selection, ICAO validation, distance/time calculation
- **Hangar**: Fleet management with detailed aircraft information
- **Aircraft Market**: Purchase aircraft with realistic pricing and balance validation
- **Flight Statistics**: Overview of all completed flights
- **Bank & Finance**: Transaction management and account balance
- **Pilot**: Pilot management with flight time tracking and license management
- **Settings**: App configuration including SimConnect settings

### Flight Tracking
- **Flight Plan Requirement**: Flight recorder only starts with valid, activated flight plan
- **Automatic Flight Tracking**: Takeoff/landing detection via SimConnect
- **Flight Records**: Storage of departure, arrival, duration, distance, landing rate
- **Automatic Earnings**: Automatic calculation and credit of flight earnings
- **Airport Detection**: Automatic detection of nearest airports during takeoff/landing
- **Status Display**: Color-coded flight recorder status (No Flight Plan/Ready/Recording)

### Aircraft Management
- **Aircraft Catalog**: Automatic scanning of all installed MSFS 2024 aircraft on startup
- **MSFS Aircraft Database**: Persistent storage of all detected aircraft with prices
- **Realistic Pricing**: Intelligent price calculation for 60+ aircraft types (Cessna, Airbus, Boeing, etc.)
- **Aircraft Market**: Purchase new aircraft with balance validation and automatic registration
- **Hangar System**: Management of owned aircraft fleet with status filtering and search
- **Aircraft Data**: Tracking of flight hours, maintenance, value, performance data
- **Pilot Assignment**: Assign pilots to aircraft for daily earnings generation
- **Starting Aircraft**: Cessna 172 Skyhawk SP (G1000) as initial aircraft

### Maintenance System
- **Category-Based Checks**: Different maintenance schedules for GA and commercial aircraft
- **GA Aircraft**: 50-Hour Check, 100-Hour Check, Annual Inspection, Engine Overhaul (TBO)
- **Commercial Aircraft**: A-Check, B-Check, C-Check (Heavy Maintenance), D-Check (Overhaul)
- **Realistic Costs**: From €450 (50h check) to €3.5M (D-Check)
- **Maintenance Duration**: From 4 hours to 42 days depending on check type
- **Auto-Grounding**: Aircraft with overdue checks are automatically grounded
- **Check Hierarchy**: Higher checks include all lower checks (e.g., Annual includes 50h/100h)
- **Maintenance Scheduling**: Schedule maintenance from aircraft detail view
- **Status Indicators**: Visual badges showing maintenance status in hangar view

### Daily Earnings System
- **Passive Income**: Aircraft with assigned pilots generate daily earnings
- **Earnings Calculation**: Based on aircraft capacity and cruise speed
- **Flight Hours Tracking**: Automatic accumulation of flight hours
- **Maintenance Exclusion**: Aircraft in maintenance do not generate earnings

### Finance System
- **Transaction Tracking**: Complete transaction system with income/expenses
- **Automatic Earnings**: Automatic credit after each flight
- **Balance Management**: Persistent account balance with real-time updates
- **Purchase Transactions**: Automatic transaction creation on aircraft purchase
- **Flight-Linked Transactions**: Linking of transactions with flights
- **Balance Validation**: Purchase protection with insufficient balance
- **Loan System**: Take loans with realistic interest rates and monthly payments

### FBO (Fixed Base Operator) System
- **FBO Rental**: Rent FBOs at airports (Local, Regional, International types)
- **Terminal Configuration**: Configurable terminal sizes (None, Small, Medium, Large)
- **FBO Services**: Refueling, Hangar, Catering, Ground Handling, De-Icing services
- **Cost Management**: Monthly rent and service costs calculation
- **FBO Details**: Detailed view with service configuration and cost overview

## Technology Stack
- **.NET 9** WPF Application
- **Entity Framework Core** with SQLite
- **SimConnect SDK** for MSFS integration
- **JSON** for configuration and MSFS manifest parsing
- **Little Navmap** Airport Database integration

## Prerequisites
- .NET 9 SDK
- Microsoft Flight Simulator 2024
- (Optional) Little Navmap for extended airport data

## Build & Run
```powershell
cd .\Ace.App
dotnet build
dotnet run
```

## Database Structure
The SQLite database (`Savegames/Current/ace.db`) contains:
- **Aircraft**: Aircraft fleet with all details
- **MsfsAircraft**: Master list of all detected MSFS aircraft with new prices
- **AircraftCatalog**: Market prices and availability for aircraft purchase
- **FBOs**: Fixed Base Operators with services and terminal configuration
- **Flights**: Flight records
- **Transactions**: Financial transactions
- **Pilots**: Pilot information with flight time tracking
- **Licenses**: Pilot licenses
- **Settings**: App settings

### Initial Game World
On first startup, a game world is automatically initialized:
- Pilot "John Doe" with 2000h flight experience
- PPLA license (issued 5 years ago by EASA)
- Cessna 172 Skyhawk SP (G1000) in hangar
- €100,000 starting capital

### Project Structure

```
Ace.App/
├── Data/               # Database context
├── DevTools/           # Development utilities (CheckDb, FixAircraftSpecs)
├── Infrastructure/     # DI configuration (ServiceConfiguration, ServiceLocator)
├── Interfaces/         # Service interfaces
├── Models/             # Data models (Aircraft, Pilot, FBO, etc.)
├── Repositories/       # Data access repositories
├── Services/           # Business logic services
├── ViewModels/         # MVVM ViewModels
├── Views/
│   ├── Aircraft/       # Hangar, Market, AircraftDetail, MaintenanceSchedule
│   ├── Core/           # Dashboard, Flightplan, Settings, SaveLoad, LoadingWindow
│   ├── Dialogs/        # ConfirmDialog, MessageDialog, InfoDialog, EditDialogs
│   ├── FBO/            # FBOView, FBODetailView
│   ├── Finance/        # BankView, StatisticsView
│   ├── Menus/          # TabControl wrappers (Home, Dashboard, FBO, Pilot, Settings, SaveLoad)
│   └── Pilots/         # PersonnelView, PilotDetailView
└── Themes/             # XAML themes (DarkTheme)
```

## Features in Development
- Extended aircraft performance data
- Crew management
- Weather integration for flight planning
- Competition system
