Invoke-WebRequest 'https://dot.net/v1/dotnet-install.ps1' -OutFile 'dotnet-install.ps1'; ./dotnet-install.ps1 -Channel LTS -InstallDir 'C:\Program Files\dotnet'
$env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User")
If (!(Test-Path $Profile.CurrentUserAllHosts)) {New-Item -Path $Profile.CurrentUserAllHosts -Force}
Add-Content -Path $Profile.CurrentUserAllHosts -Value '$env:Path += \";C:\Program Files\dotnet;%USERPROFILE%\.dotnet\tools\"'