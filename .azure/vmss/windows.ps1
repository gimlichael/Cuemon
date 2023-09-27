Invoke-WebRequest 'https://dot.net/v1/dotnet-install.ps1' -OutFile 'dotnet-install.ps1'; ./dotnet-install.ps1 -Version 8.0.100-rc.2.23469.4 -InstallDir 'C:\Program Files\dotnet'
$envVariable = "Path"
$machinePath = $Env:Path + '";C:\Program Files\dotnet;' + $Env:USERPROFILE + '\.dotnet\tools\"'
[System.Environment]::SetEnvironmentVariable($envVariable, $machinePath, [System.EnvironmentVariableTarget]::Machine)
