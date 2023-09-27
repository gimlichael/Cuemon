Invoke-WebRequest 'https://dot.net/v1/dotnet-install.ps1' -OutFile 'dotnet-install.ps1'; ./dotnet-install.ps1 -Version 8.0.100-rc.2.23469.4 -InstallDir 'C:\Program Files\dotnet'
$pathVariableName = 'Path'
$dotnetRootVariableName = 'DOTNET_ROOT'
$dotnetRootPath = 'C:\Program Files\dotnet'
$machinePath = $Env:Path + ';' + $dotnetRootPath + ';' + $Env:USERPROFILE + '\.dotnet\tools\'
[System.Environment]::SetEnvironmentVariable($dotnetRootVariableName, $dotnetRootPath, [System.EnvironmentVariableTarget]::Machine)
[System.Environment]::SetEnvironmentVariable($pathVariableName, $machinePath, [System.EnvironmentVariableTarget]::Machine)
