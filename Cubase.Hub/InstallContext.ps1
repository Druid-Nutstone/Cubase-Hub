# ===== CONFIG =====
$menuText = "Edit Audio in Cubase Hub"
$exeName  = "Cubase.Hub.exe"
$modeParam = "editaudio"
$extensions = @(".flac", ".mp3", ".wav")

$exePath = Join-Path $PSScriptRoot $exeName
$iconPath = $exePath

if (!(Test-Path $exePath)) {
    Write-Host "ERROR: Could not find $exeName in $PSScriptRoot"
    exit
}

foreach ($ext in $extensions) {

    $baseKey = "Registry::HKEY_CLASSES_ROOT\SystemFileAssociations\$ext\shell\CubaseHubEdit"
    $commandKey = "$baseKey\command"

    New-Item -Path $baseKey -Force | Out-Null
    Set-ItemProperty -Path $baseKey -Name "(default)" -Value $menuText
    Set-ItemProperty -Path $baseKey -Name "Icon" -Value $iconPath

    New-Item -Path $commandKey -Force | Out-Null
    Set-ItemProperty -Path $commandKey -Name "(default)" -Value "`"$exePath`" $modeParam `"%1`""
}

Write-Host "Context menu registered successfully."