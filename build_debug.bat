dotnet restore HaloLive.Authentication.Service.sln
dotnet publish src/ProjectVindictive.Service.Authentication/ProjectVindictive.Service.Authentication.csproj -c debug

if not exist "build" mkdir build
xcopy src\ProjectVindictive.Service.Authentication\bin\Debug\netcoreapp1.1\publish build /s /y

if not exist "build\Certs" mkdir build\Certs
if not exist "build\Config" mkdir build\Config

PAUSE