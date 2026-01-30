# Changelog

## [Unreleased]

### Added
- Added "Sell to afford" badge in Aircraft Market showing when selling fleet aircraft would cover the purchase price
- Added Profit/h sorting option in Aircraft Market (ascending and descending)
- Added ROI sorting option in Aircraft Market (Profit/h relative to purchase price)
- Added dark-themed tooltips for all Statistics charts (replaces unreadable default yellow tooltips)
- Added screenshots to README for GitHub presentation
- Added Curtiss C-46 aircraft image and marked as oldtimer
- Added C-46/C-47 to oldtimer pattern list
- Added `GetPlayerPilot()` to PilotRepository for player-specific queries
- Added FBO count tracking in achievement refresh

### Fixed
- Fixed Antonov-2 displaying wrong aircraft image (was showing AN-225 instead of AN-2)
- Fixed terrain map not loading (switched from defunct Stamen Terrain to OpenTopoMap)
- Fixed achievement progress not updating correctly (flights, flight hours now recalculated at startup)
- Fixed aircraft images not matching catalog entries (renamed ~130 images to match title-based lookup)
- Fixed flight hours achievement tracking to use player pilot hours instead of sum of all pilots
- Fixed duplicate aircraft catalog entries (L39 Common, C188 agtruck)
- Fixed achievement save logic to always persist progress updates
- Fixed Dashboard Flight Info panel overlapping (FLIGHT INFO text visible behind route badges)
- Fixed Cessna C188 Agtruck not showing aircraft image (wrong filename in database)
- Fixed Curtiss C-46 cruise speed (corrected from 175 to 150 kts)

### Changed
- Improved button styling consistency in Settings view
- AchievementService now uses IFBORepository for FBO achievement tracking
- Expanded oldtimer/classic aircraft classification with comprehensive pattern list (96 aircraft marked)

