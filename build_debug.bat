dotnet restore HaloLive.Authentication.Service.sln
dotnet publish src/ProjectVindictive.Service.Authentication/ProjectVindictive.Service.Authentication.csproj -c debug
dotnet publish src/ProjectVindictive.Service.UserContentManagement/ProjectVindictive.Service.UserContentManagement.csproj -c debug

if not exist "build" mkdir build

if not exist "build\auth" mkdir build\auth
xcopy src\ProjectVindictive.Service.Authentication\bin\Debug\netcoreapp1.1\publish build\auth /s /y

if not exist "build\auth\Certs" mkdir build\auth\Certs
if not exist "build\auth\Config" mkdir build\auth\Config

if not exist "build\ucm" mkdir build\ucm
xcopy src\ProjectVindictive.Service.UserContentManagement\bin\Debug\netcoreapp1.1\publish build\ucm /s /y

if not exist "build\ucm\Certs" mkdir build\ucm\Certs
if not exist "build\ucm\Config" mkdir build\ucm\Config

PAUSE