# Aviation Career Economy - Claude Code Richtlinien

## PROJEKT-KONTEXT

WPF-Desktop-Anwendung mit .NET/C#, EF Core und MVVM-Architektur.
Airline-Simulation mit MSFS 2024 Integration.
**Namespace:** `Ace.App` | **Datenbank:** `ace.db`

---

## CODE-QUALITÄT

- **Dependency Injection ist Pflicht** - KEINE `.Instance`-Aufrufe außer in `ServiceConfiguration`
- **Null Toleranz** für Warnings und Errors
- **Keine hardcoded Werte** - Konstanten oder Konfiguration verwenden
- **Kein toter Code** - Kein auskommentierter Code, keine unbenutzten Methoden/Variablen
- **Keine Kommentare** - Selbsterklärender Code braucht keine Kommentare
- **Proaktives Logging** - Ausreichend loggen für Problemanalyse statt aufwändigem Debuggen
- **DRY (Don't Repeat Yourself)** - Keine Code-Duplikation, gemeinsame Logik extrahieren

---

## ARCHITEKTUR

| Komponente | Pattern/Lifetime |
|------------|------------------|
| DB-Operationen | Repository Pattern |
| Services | Interface + Singleton |
| DbContext/Repositories | Scoped |
| ViewModels/Views | Transient |
| Datenbank-Änderungen | EF Core Migrations (KEIN manuelles SQL) |

- Interfaces nur wenn nötig (Testbarkeit, mehrere Implementierungen)
- Sinnvolle Ordnerstruktur (Models, Views, ViewModels, Services, Repositories, Infrastructure)
- Klare Namenskonventionen

---

## UI/UX KONSISTENZ

**OBERSTE REGEL: Einheitliches UI und UX in der gesamten App!**

- **Gleiche Komponenten, gleiches Aussehen** - Buttons, Panels, Cards, Listen etc. müssen überall identisch aussehen
- **Zentrale Helper-Klassen nutzen** - z.B. `AirportSymbolHelper` für Flughafen-Darstellung auf allen Karten
- **Gleiche Farbcodierung überall** - Wenn Flughäfen nach Runway-Länge gefärbt werden, dann auf ALLEN Karten gleich
- **Gleiche Interaktionsmuster** - Rechtsklick-Kontextmenüs, Hover-Effekte, Klick-Verhalten einheitlich
- **StaticResource/DynamicResource nutzen** - Keine Inline-Styles, alle Styles aus Theme-Dateien
- **Neue UI-Elemente** - Vor Implementierung prüfen, ob ähnliche Elemente bereits existieren und wiederverwenden

---

## ERROR HANDLING

- **NIEMALS** leere catch-Blocks
- **ALLE** Exceptions müssen geloggt werden (`_logger.Error()` oder `_logger.Debug()`)

---

## TESTING

- Kritische Geschäftslogik testen (Services, komplexe ViewModels)
- **Testabdeckung idealerweise 100%** für neuen Code (soweit sinnvoll)
- Unit Tests für Services/ViewModels/Repositories
- **NIEMALS in produktive Datenbanken schreiben** - In-Memory oder Test-DB verwenden

---

## DOKUMENTATION

| Datei | Inhalt |
|-------|--------|
| `README.md` | Installation, Requirements, technische Details, Projektstruktur, Lizenzen |
| `Manual.md` | Gameplay, Features, Anleitungen, Tipps - alles was der User wissen muss |
| `CHANGELOG.md` | Versionshistorie mit Änderungen |

- **README schlank halten** - Keine Gameplay-Erklärungen, keine Feature-Details
- **Manual ist das Handbuch** - Alle Gameplay-relevanten Infos gehören hierhin
- Bei neuen Features: Manual aktualisieren, README nur bei technischen Änderungen

---

## BUILD-PROZESS (Vor Commit)

1. Dokumentation aktualisieren (README, CHANGELOG, Manual etc.)
2. Build ausführen → 0 Warnings, 0 Errors
3. Alle Tests ausführen → Kein Test darf fehlschlagen
4. App starten und beenden ohne Exceptions

---

## BERECHTIGUNGEN

- Alle Projektdateien dürfen ohne Nachfrage manipuliert werden
- Alle Bash-Commands sind erlaubt ohne Nachfrage
- Neue Dateien dürfen angelegt werden ohne Nachfrage
- Dateien löschen ist erlaubt ohne Nachfrage
- Web-Recherchen sind erlaubt ohne Nachfrage
- Temporär erstellte Dateien nach Gebrauch wieder löschen

### KRITISCHE AUSNAHMEN

- **NIEMALS Spielstände/Datenbank löschen ohne Nachfrage** - Migration ist IMMER vorzuziehen
- **NIEMALS `Savegames/` oder `*.db` Dateien löschen** - User-Daten sind unersetzbar
- Bei DB-Schema-Änderungen: `AddColumnIfNotExists()` in DatabaseInitializer verwenden

### TEMPLATE-DATENBANK

- **Default-Werte ändern:** Bei Änderungen an Standardwerten IMMER sowohl den Code ALS AUCH die `Data/template.db` aktualisieren
- Neue Savegames basieren auf der template.db - vergessene Änderungen dort führen zu falschen Defaults
- Bei neuen Settings-Feldern: Auch in template.db den korrekten Default-Wert setzen

---

## ARBEITSWEISE

- Immer anzeigen, wenn ein Hintergrund-Agent noch läuft
- Bei Unklarheiten: Architektur hinterfragen - ist sie noch geeignet?

---

## DEBUGGING & PROBLEMLÖSUNG

**OBERSTE REGEL: Logs zuerst lesen statt zu raten!**

- **Proaktiv LoggingService nutzen** - Bei Problemen IMMER zuerst die Logs prüfen (`Logs/*.log`)
- **Logging hinzufügen** - Wenn nötige Log-Ausgaben fehlen, diese ZUERST hinzufügen, App starten, Logs analysieren
- **Keine teuren Raterunden** - Nicht blind Code ändern und hoffen dass es funktioniert
- **Systematisch debuggen**:
  1. Logs lesen und analysieren
  2. Falls keine hilfreichen Logs: Logging hinzufügen
  3. Problem reproduzieren
  4. Logs erneut analysieren
  5. Erst dann gezielt fixen

### KEIN RATEMODUS

**VERBOTEN: Blindes Ausprobieren von Lösungen ohne Diagnose!**

- **STOPP bei Unsicherheit** - Wenn unklar ist warum etwas nicht funktioniert: NICHT einfach Code ändern
- **Keine Vermutungs-Ketten** - Nicht "vielleicht ist es X" → fix X → "vielleicht ist es Y" → fix Y → ...
- **Erst verstehen, dann fixen** - Das Problem muss VERSTANDEN sein bevor eine Lösung implementiert wird
- **Diagnose-Tools nutzen** - Logs, Debugger, Tests - alles was hilft das Problem zu verstehen
- **Bei Wiederholung: STOPP** - Wenn der gleiche Fix-Versuch mehrfach fehlschlägt, anderen Ansatz wählen
- **Fragen statt raten** - Wenn Informationen fehlen, den User fragen statt zu spekulieren
