; MyGenerationSqlTool 0.1 Beta Installation Script
;-----------------------------------------

; Set the compressions to lzma, which is always the best compression!
SetCompressor lzma 

; The name of the installer
Name "MyGeneration Sql Tool Plugin 0.1 Beta"

; The file to write
OutFile "mygen_plugin_sqltool010b.exe"

; Icon doesn't work for some reason
Icon ".\modern-install.ico"

XPStyle on

ShowInstDetails show

LicenseText "Liscence Agreement"
LicenseData "BSDLicense.rtf"


; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM SOFTWARE\MyGenerationSqlTool "Install_Dir"

; The text to prompt the user to enter a directory
ComponentText "This will install the MyGeneration SqlTool on your computer. Select which optional things you want installed."

; The text to prompt the user to enter a directory
;DirText "Choose an install directory for MyGenerationSqlTool."

; Install .Net Framework 2.0
Section "Detect .Net Framework 2.0"
  Call DotNet20Exists
  Pop $1
  IntCmp $1 0 SkipFramework
    MessageBox MB_OK|MB_ICONINFORMATION "You cannot run MyGenerationSqlTool without having the .Net Framework 2.0 installed. It is not included $\r$\nin the installer because the file is huge and most people already have it installed." IDOK
    ExecShell open http://www.microsoft.com/downloads/details.aspx?familyid=0856EACB-4362-4B0D-8EDD-AAB15C5E04F5&displaylang=en
    DetailPrint ".Net Framework 2.0 not installed... Aborting Installation."
    Abort
    Goto FrameworkDone
	SkipFramework:
		DetailPrint ".Net Framework 2.0 found... Continuing."
	FrameworkDone:
SectionEnd

; The stuff to install
Section "Install Files, Reg Entries, Uninstaller, & Shortcuts"

  ReadRegStr $0 HKLM Software\MyGeneration13 "Install_Dir"
  DetailPrint "MyGeneration is installed at: $0"
  StrCpy $INSTDIR $0

  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  CreateDirectory "$INSTDIR"

  File /oname=MyGeneration.UI.Plugins.SqlTool.dll ..\plugins\MyGeneration.UI.Plugins.SqlTool\bin\Release\MyGeneration.UI.Plugins.SqlTool.dll

  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\MyGenerationSqlTool "Install_Dir" "$INSTDIR"

  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\MyGenerationSqlTool" "DisplayName" "MyGenerationSqlTool Plugin (remove only)"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\MyGenerationSqlTool" "UninstallString" '"$INSTDIR\MyGenerationSqlToolUninstall.exe"'

  WriteUninstaller "MyGenerationSqlToolUninstall.exe"
  
  CreateDirectory "$SMPROGRAMS\MyGeneration 1.3"
  CreateShortCut "$SMPROGRAMS\MyGeneration 1.3\Uninstall MyGeneration SqlTool.lnk" "$INSTDIR\MyGenerationSqlToolUninstall.exe" "" "$INSTDIR\MyGenerationSqlToolUninstall.exe" 0

SectionEnd ; end the section

; uninstall stuff
UninstallText "This will uninstall the MyGeneration SqlTool plugin. Hit next to continue."
UninstallIcon ".\modern-uninstall.ico"

; special uninstall section.
Section "Uninstall"
    
  ; remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\MyGenerationSqlTool"
  DeleteRegKey HKLM SOFTWARE\MyGenerationSqlTool

  ; MUST REMOVE UNINSTALLER, too
  Delete $INSTDIR\MyGenerationSqlToolUninstall.exe
  Delete $INSTDIR\MyGeneration.UI.Plugins.SqlTool.dll
   
  Delete "$SMPROGRAMS\MyGeneration 1.3\Uninstall MyGeneration SqlTool.lnk"
SectionEnd

; detects Microsoft .Net Framework 2.0
Function DotNet20Exists

	ClearErrors
	ReadRegStr $1 HKLM "SOFTWARE\Microsoft\.NETFramework\policy\v2.0" "50727"
	IfErrors MDNFNotFound MDNFFound

	MDNFFound:
		Push 0
		Goto ExitFunction
		
	MDNFNotFound:
		Push 1
		Goto ExitFunction

	ExitFunction:

FunctionEnd

; eof