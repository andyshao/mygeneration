if "%PROGRAMFILES(X86)%"=="" goto :x86
goto :x64
:x86
	if "%DEVENV%"=="" set DEVENV="%PROGRAMFILES%\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe"  /out ".\plugins_build.log" /rebuild release
	if "%MAKENSIS%"=="" set MAKENSIS=%PROGRAMFILES%\NSIS\makensis.exe
	goto done
:x64
	if "%DEVENV%"=="" set DEVENV="%PROGRAMFILES(X86)%\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe"  /out ".\plugins_build.log" /rebuild release
	if "%MAKENSIS%"=="" set MAKENSIS=%PROGRAMFILES(X86)%\NSIS\makensis.exe
:done

%DEVENV% "..\..\plugins\MyMetaPlugins.sln"
%DEVENV% "..\..\plugins\ZeusPlugins.sln"
set DEVENV=
pause

COPY "..\..\plugins\ContextProcessor\bin\release\*.dll" .
COPY "..\..\plugins\Dnp.Utils\bin\release\*.dll" .
COPY "..\..\plugins\MyMetaSqlCePlugin\bin\release\*.dll" .
COPY "..\..\plugins\MyMetaTextFilePlugin\bin\release\*.dll" .
COPY "..\..\plugins\MyMetaXsd3bPlugin\bin\release\*.dll" .
COPY "..\..\plugins\MyMetaSybaseASEPlugin\bin\release\*.dll" .
COPY "..\..\plugins\MyMetaSybaseASAPlugin\bin\release\*.dll" .
COPY "..\..\plugins\MyMetaIngres2006Plugin\bin\release\*.dll" .
COPY "..\..\plugins\MyMetaEffiprozPlugin\bin\release\*.dll" .
COPY "..\..\plugins\MyMetaFoxProPlugin\bin\release\*.dll" .
COPY "..\..\plugins\TypeSerializer\bin\release\*.dll" .
COPY "..\..\plugins\SampleUIPlugin\bin\release\*.dll" .
COPY "..\..\plugins\MyGeneration.UI.Plugins.CodeSmith2MyGen\bin\release\*.dll" .
COPY "..\..\plugins\MyGeneration.UI.Plugins.SqlTool\bin\release\*.dll" .
rem COPY "..\..\..\plugins\MyMetaVistaDB3xPlugin\bin\release\*.dll" .
pause

DEL Dnp.Utils.dll
DEL MyGenUtility.dll
DEL MyMeta.dll
DEL PluginInterfaces.dll
DEL Zeus.dll
DEL MyGeneration.UI.Plugins.Sample.dll
pause
