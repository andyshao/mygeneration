del ".\build.log"
del ".\installbuild_mygen.log"
del ".\installbuild_mymeta.log"
del ".\installbuild_doodads.log"
del ".\installbuild_cst2mygen.log"

if "%DEVENV%"=="" set DEVENV="%PROGRAMFILES%\Microsoft Visual Studio 8\Common7\IDE\VCSExpress.exe"  /out ".\build.log" /rebuild release

rem "..\mygeneration\MyGeneration\PluginResources\UpdateDLLs.bat"

%DEVENV% "..\plugins\MyMetaPlugins.sln"
%DEVENV% "..\plugins\ZeusPlugins.sln"
%DEVENV% "..\mygeneration\Zeus.sln"
rem %DEVENV% "..\codesmith2mygen\CodeSmith2MyGeneration\CodeSmith2MyGeneration.sln"
set DEVENV=

if "%MAKENSIS%"=="" set MAKENSIS=%PROGRAMFILES%\NSIS\makensis.exe
"%MAKENSIS%" ".\mygeneration.nsi" > ".\installbuild_mygen.log"
"%MAKENSIS%"  ".\mymeta.nsi" > ".\installbuild_mymeta.log"
"%MAKENSIS%"  ".\doodads.nsi" > ".\installbuild_doodads.log"
"%MAKENSIS%"  ".\cst2mygen.nsi" > ".\installbuild_cst2mygen.log"
set MAKENSIS=
s
