# PowerShell script to convert AircraftData.cs to JSON
# Run this once to generate the complete aircraft_data.json

$csFile = "e:\02_SW_Projects\aviation-career-economy\Ace.App\Data\AircraftData.cs"
$jsonFile = "e:\02_SW_Projects\aviation-career-economy\Planes\aircraft_data.json"

$content = Get-Content $csFile -Raw

# Extract all P() calls using regex
$pattern = 'P\((\d+),\s*"([^"]+)",\s*([\d.]+)m?,\s*(\d+),\s*(\d+),\s*([\d.]+),\s*([\d.]+),\s*([\d.]+),\s*([\d.]+)\)'
$matches = [regex]::Matches($content, $pattern)

$aircraft = @()
foreach ($match in $matches) {
    $aircraft += @{
        id = [int]$match.Groups[1].Value
        pattern = $match.Groups[2].Value
        basePrice = [decimal]$match.Groups[3].Value
        priority = [int]$match.Groups[4].Value
        passengers = [int]$match.Groups[5].Value
        cruiseSpeedKts = [double]$match.Groups[6].Value
        rangeNM = [double]$match.Groups[7].Value
        fuelCapacityGal = [double]$match.Groups[8].Value
        fuelBurnGph = [double]$match.Groups[9].Value
    }
}

$jsonData = @{
    version = "1.0"
    lastModified = (Get-Date).ToString("yyyy-MM-dd HH:mm:ss")
    aircraft = $aircraft
}

$json = $jsonData | ConvertTo-Json -Depth 3
$json | Out-File -FilePath $jsonFile -Encoding UTF8

Write-Host "Converted $($aircraft.Count) aircraft entries to $jsonFile"
