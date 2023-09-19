Invoke-WebRequest 'https://dot.net/v1/dotnet-install.ps1' -OutFile 'dotnet-install.ps1'; ./dotnet-install.ps1 -Channel LTS
setx /M PATH "%PATH%;%USERPROFILE%\.dotnet\tools"