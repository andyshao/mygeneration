rem set MAKENSIS=C:\Programme\SDK\installer\NSIS\makensis.exe
rem set DEVENV="%WINDIR%\Microsoft.NET\Framework\v2.0.50727\msbuild" /T:Rebuild /p:Configuration=Release /p:OutputPath=D:\copy\rel_installer /p:DefineConstants="IGNORE_VISTA:IGNORE_SQLCE:ENTERPRISE" /p:RegisterForComInterop=false 
set DEVENV="%WINDIR%\Microsoft.NET\Framework\v2.0.50727\msbuild" /T:Rebuild /p:Configuration=Release /v:m


del ".\build.log"

if "%DEVENV%"=="" set DEVENV=%PROGRAMFILES%\Microsoft Visual Studio 8\Common7\IDE\devenv.exe  /out ".\build.log" /rebuild release

rem main app and all its dependencies
%DEVENV% "..\MyGeneration_k3b\MyGeneration_k3b.sln" > .\build2.log

rem plugins

rem %DEVENV% "..\plugins\MyMetaSqlCePlugin\MyMetaSqlCePlugin_k3b.csproj" >> .\build2.log

%DEVENV% "..\plugins\MyMetaTextFilePlugin\MyMetaTextFilePlugin_k3b.csproj" >> .\build2.log

%DEVENV% "..\plugins\MyMetaXsd3bPlugin\MyMetaXsd3bPlugin_k3b.csproj" >> .\build2.log

rem %DEVENV% "..\plugins\MyMetaVistaDB3xPlugin\MyMetaVistaDB3xPlugin_k3b.csproj" >> .\build2.log


set DEVENV=

if "%MAKENSIS%"=="" set MAKENSIS=%PROGRAMFILES%\NSIS\makensis.exe
"%MAKENSIS%" ".\mygeneration_k3b.nsi" > ".\installbuild_mygen.log"
rem "%MAKENSIS%"  ".\mymeta.nsi" > ".\installbuild_mymeta.log"
rem "%MAKENSIS%"  ".\doodads.nsi" > ".\installbuild_doodads.log"
set MAKENSIS=


rem C:\Programme\SDK\installer\NSIS\makensis.exe ".\mygeneration_k3b.nsi" > ".\installbuild_mygen.log"
