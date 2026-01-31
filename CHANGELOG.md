# Changelog

## [Build 2] - 2026-01-31

### Added
- Added native Windows 11 Snap Layout support for the custom title bar (hover maximize button for layout options)
- Added "New" badge (blue) for modern aircraft and "Classic" badge (gold) for oldtimers in Aircraft Market
- Added favorites filter toggle button in Aircraft Market toolbar (star icon to show favorites only)
- Added IsFavorite property to aircraft catalog with database persistence
- Added hardware input service interface

### Changed
- Dashboard gauges redesigned from circular to tall rounded rectangles (instrument-style)
- Aircraft Market sorting no longer pins favorites to top; favorites are now filtered via dedicated toggle
- Aircraft Market filter performance optimized (single collection replacement instead of item-by-item updates)

### Removed
- Removed custom snap layout popup in favor of native Windows 11 snap layouts

---

## [Build 1] - Unreleased

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
- Flight map routes now display as great circle arcs instead of straight lines for a realistic 3D-like appearance
- Thinner route lines across all maps for cleaner visuals
- Improved button styling consistency in Settings view
- AchievementService now uses IFBORepository for FBO achievement tracking
- Expanded oldtimer/classic aircraft classification with comprehensive pattern list (96 aircraft marked)

